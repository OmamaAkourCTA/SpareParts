using Microsoft.Extensions.Configuration;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Core.Helpers;
using SparePartsModule.Domain.Context;
using SparePartsModule.Domain.Models.Library;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Category;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Warehouses;
using Microsoft.EntityFrameworkCore;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Location;
using SparePartsModule.Infrastructure.ViewModels.Models;
using Microsoft.AspNetCore.SignalR;
using SparePartsModule.Infrastructure.ViewModels.Models.Users;

namespace SparePartsModule.Core.Library
{
    public class WarehousesService: IWarehousesService
    {
        private readonly IConfiguration _config;
        private readonly SparePartsModuleContext _context;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilties;
        List<string> errors = new List<string>();

        public WarehousesService(SparePartsModuleContext context, IConfiguration config, FileHelper fileHelper, UtilitiesHelper utilties)
        {

            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;
            _utilties = utilties;
        }
        public async ValueTask<ApiResponseModel> AddWarehouses(AddWarehousesModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var exists = _context.MasterSPGeneralWareHouses.Where(e => e.Cancelled == false && e.WarehouseName == model.WarehouseName).Any();
            if (exists)
            {

                throw new ManagerProcessException("000146");
            }



            var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.WarehouseType && e.LookupTypeID == (int)LookupTypes.WarehouseType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000147");
                }
            
          
               isValid = _context.MasterBusinessArea.Where(e => e.Cancelled == false && e.BusinessAreaID == model.WarehouseBA).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000145");
                }
            
            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            //if (model.WarehouseAbb != null)
            //{
            //    isValid = _utilties.EnglishChar(model.WarehouseAbb);
            //    if (!isValid)
            //    {
            //        throw new ManagerProcessException("000159");
            //    }
            //}
            var newId = ((int?)_context.MasterSPGeneralWareHouses.Max(inv => (int?)inv.WarehouseID) ?? 0) + 1;
            var item = new MasterSPGeneralWareHouse
            {
                WarehouseID = newId,
                WarehouseName = model.WarehouseName,
                WarehouseType = model.WarehouseType,
                WarehouseBA=model.WarehouseBA,
                WarehouseAbb=model.WarehouseAbb.ToUpper(),
  
                Cancelled = false,

                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            await _context.MasterSPGeneralWareHouses.AddAsync(item);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = _context.MasterSPGeneralWareHouses.Where(e => e.WarehouseID == newId
            ).Select(e => new
            {
                e.WarehouseID,
                e.WarehouseName,
                e.WarehouseAbb,

                WarehouseType = _context.MasterSPLookup.Where(x => x.LookupID == e.WarehouseType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                WarehouseBA = _context.MasterBusinessArea.Where(x => x.BusinessAreaID == e.WarehouseBA).Select(x => new { x.BusinessAreaID, x.BAName }).FirstOrDefault(),

                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,
                e.EnterTime,
                e.ModDate,
                e.ModTime,
                EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).Select(x => new { x.FullName, x.UserId }).FirstOrDefault(),
                ModUser = _context.Users.Where(x => x.UserId == e.ModUser).Select(x => new { x.FullName, x.UserId }).FirstOrDefault(),

            }).FirstOrDefault();

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditWarehouses(EditWarehousesModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var item = _context.MasterSPGeneralWareHouses.Where(e => e.Cancelled == false && e.WarehouseID == model.WarehouseID).FirstOrDefault();  
            if (item == null)
            {
                throw new ManagerProcessException("000148");
            }

            var exists = _context.MasterSPGeneralWareHouses.Where(e =>e.WarehouseID!=model.WarehouseID&& e.Cancelled == false && e.WarehouseName == model.WarehouseName).Any();
            if (exists)
            {

                throw new ManagerProcessException("000146");
            }



            var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.WarehouseType && e.LookupTypeID == (int)LookupTypes.WarehouseType).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000147");
            }


            isValid = _context.MasterBusinessArea.Where(e => e.Cancelled == false && e.BusinessAreaID == model.WarehouseBA).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000145");
            }

            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            if (model.WarehouseAbb != null)
            {
                //isValid = _utilties.EnglishChar(model.WarehouseAbb);
                //if (!isValid)
                //{
                //    throw new ManagerProcessException("000159");
                //}
                item.WarehouseAbb = model.WarehouseAbb.ToUpper();
            }

            item.WarehouseName = model.WarehouseName;
              item.WarehouseType = model.WarehouseType;
              item.WarehouseBA = model.WarehouseBA;
              item.Status = model.Status;
              item.ModUser = userId;
              item.ModDate = DateTime.Now;
              item.ModTime = DateTime.Now.TimeOfDay;
          
      
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteWarehouses(string WarehouseID, int userId)
        {
            var afftedRows = _context.MasterSPGeneralWareHouses.Where(e => ("," + WarehouseID + ",").Contains("," + e.WarehouseID + ","))
                      .ExecuteUpdate(e =>
                      e.SetProperty(x => x.Status, (int)Status.Deleted)
                      .SetProperty(x => x.CancelDate, DateTime.Now)
                      .SetProperty(x => x.Cancelled, true)
                      .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                      .SetProperty(x => x.ModUser, userId)

                      );
            _context.MasterSPGeneralWarehousesSub.Where(e => ("," + WarehouseID + ",").Contains("," + e.WarehouseID + ","))
                     .ExecuteUpdate(e =>
                     e.SetProperty(x => x.Status, (int)Status.Deleted)
                     .SetProperty(x => x.CancelDate, DateTime.Now)
                     .SetProperty(x => x.Cancelled, true)
                     .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                     .SetProperty(x => x.ModUser, userId)
                     );
            _context.MasterSPGeneralWarehousesZones.Where(e => ("," + WarehouseID + ",").Contains("," + e.WarehouseID + ","))
                 .ExecuteUpdate(e =>
                 e.SetProperty(x => x.Status, (int)Status.Deleted)
                 .SetProperty(x => x.CancelDate, DateTime.Now)
                 .SetProperty(x => x.Cancelled, true)
                 .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                 .SetProperty(x => x.ModUser, userId)
                 );
            var zoneIds = _context.MasterSPGeneralWarehousesZones
                .Where(e => ("," + WarehouseID + ",").Contains("," + e.WarehouseID + ",")).Select(e=>e.ZoneID).ToList();    
            _context.MasterSPGeneralWarehousesShelfs.Where(e => zoneIds.Contains( e.ZoneID ))
             .ExecuteUpdate(e =>
             e.SetProperty(x => x.Status, (int)Status.Deleted)
             .SetProperty(x => x.CancelDate, DateTime.Now)
             .SetProperty(x => x.Cancelled, true)
             .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
             .SetProperty(x => x.ModUser, userId)
             );
            var shelfIds = _context.MasterSPGeneralWarehousesShelfs.Where(e => zoneIds.Contains(e.ZoneID))
                .Select(e => e.ShelfID).ToList();

            _context.MasterSPGeneralWarehouseSection.Where(e => shelfIds.Contains(e.WarehouseShelfID))
             .ExecuteUpdate(e =>
             e.SetProperty(x => x.Status, (int)Status.Deleted)
             .SetProperty(x => x.CancelDate, DateTime.Now)
             .SetProperty(x => x.Cancelled, true)
             .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
             .SetProperty(x => x.ModUser, userId)
             );

            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000148");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");



        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetWarehouses(GetWarehousesModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralWareHouses.Where(e => e.Cancelled == false &&
            (model.WarehouseName == null || e.WarehouseName.Contains(model.WarehouseName)) &&
            (model.WarehouseType == null || e.WarehouseType==model.WarehouseType) &&
            (model.WarehouseBA == null || e.WarehouseBA == model.WarehouseBA) &&
             (model.Status == null || e.Status == model.Status) 
            ).Select(e => new
            {
                e.WarehouseID,
                e.WarehouseName,
                e.WarehouseAbb,

                WarehouseType = _context.MasterSPLookup.Where(x => x.LookupID == e.WarehouseType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                WarehouseBA = _context.MasterBusinessArea.Where(x => x.BusinessAreaID == e.WarehouseBA).Select(x => new { x.BusinessAreaID, x.BAName }).FirstOrDefault(),

                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,
                e.EnterTime,
                e.ModDate,
                e.ModTime,
                EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).Select(x => new {x.FullName,x.UserId}).FirstOrDefault(),
                ModUser = _context.Users.Where(x => x.UserId == e.ModUser).Select(x => new { x.FullName, x.UserId }).FirstOrDefault(),

            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.WarehouseID);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.WarehouseName);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.WarehouseName);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.WarehouseBA.BAName);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.WarehouseBA.BAName);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.WarehouseType.LookupName);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.WarehouseType.LookupName);
            }

            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.Status.LookupName);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.Status.LookupName);
            }
            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);
            return result;


        }
        public async ValueTask<ApiResponseModel> AddSubWarehouse(AddSubWarehouseModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (model.SubWarehouseAbb != null)
            {
               //var isValid = _utilties.EnglishChar(model.SubWarehouseAbb);
               // if (!isValid)
               // {
               //     throw new ManagerProcessException("000159");
               // }
               
            }
            var exists = _context.MasterSPGeneralWareHouses.Where(e => e.Cancelled == false && e.WarehouseID == model.WarehouseID).Any();
            if (!exists)
            {

                throw new ManagerProcessException("000148");
            }
             exists = _context.MasterSPGeneralWarehousesSub.Where(e => e.Cancelled == false && e.WarehouseID == model.WarehouseID&&
            e.SubWarehouseName==model.SubWarehouseName).Any();
            if (exists)
            {

                throw new ManagerProcessException("000149");
            }
            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }


            var newId = ((int?)_context.MasterSPGeneralWarehousesSub.Max(inv => (int?)inv.SubWarehouseID) ?? 0) + 1;
            var item = new MasterSPGeneralWarehousesSub
            {
                SubWarehouseID=newId,   
                WarehouseID = model.WarehouseID,
                SubWarehouseName=model.SubWarehouseName,
                SubWarehouseAbb=model.SubWarehouseAbb.ToUpper(),
                Comments = model.Comments,
                Cancelled = false,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            await _context.MasterSPGeneralWarehousesSub.AddAsync(item);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data= _context.MasterSPGeneralWarehousesSub.Where(e => e.SubWarehouseID == newId
                       ).Select(e => new
                       {
                e.WarehouseID,
                e.SubWarehouseID,
                e.SubWarehouseName,
                e.SubWarehouseAbb,
                e.Comments,
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
            }).FirstOrDefault();


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> AddZone(AddZoneModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var exists = _context.MasterSPGeneralWareHouses.Where(e => e.Cancelled == false && e.WarehouseID == model.WarehouseID).Any();
            if (!exists)
            {

                throw new ManagerProcessException("000148");
            }
            if(model.SubWarehouseID!=null)
            {
                exists = _context.MasterSPGeneralWarehousesSub.Where(e => e.Cancelled == false && e.WarehouseID == model.WarehouseID&&e.SubWarehouseID==model.SubWarehouseID).Any();
                if (!exists)
                {

                    throw new ManagerProcessException("000151");
                }
            }
            exists = _context.MasterSPGeneralWarehousesZones.Where(e => e.Cancelled == false && e.WarehouseID == model.WarehouseID &&
             e.SubWarehouseID == model.SubWarehouseID&&
             e.ZoneName == model.ZoneName).Any();
            if (exists)
            {

                throw new ManagerProcessException("000150");
            }
            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }


            var newId = ((int?)_context.MasterSPGeneralWarehousesZones.Max(inv => (int?)inv.ZoneID) ?? 0) + 1;
            var item = new MasterSPGeneralWarehousesZone
            {
                ZoneID = newId,
                SubWarehouseID = model.SubWarehouseID,
                WarehouseID = model.WarehouseID,
                ZoneName = model.ZoneName,
                
                Comments = model.Comments,



                Cancelled = false,

                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            await _context.MasterSPGeneralWarehousesZones.AddAsync(item);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = _context.MasterSPGeneralWarehousesZones.Where(e => e.ZoneID== newId 
            ).Select(e => new
            {
                e.ZoneID,
                e.WarehouseID,
                e.SubWarehouseID,
                e.ZoneName,
                e.Comments,
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),


            }).FirstOrDefault();

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> AddShelf(AddShelfModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var exists = _context.MasterSPGeneralWarehousesZones.Where(e => e.Cancelled == false && e.ZoneID == model.ZoneID).Any();
            if (!exists)
            {

                throw new ManagerProcessException("000152");
            }
         
            exists = _context.MasterSPGeneralWarehousesShelfs.Where(e => e.Cancelled == false && e.ZoneID == model.ZoneID &&
         
             e.ShelfName == model.ShelfName).Any();
            if (exists)
            {

                throw new ManagerProcessException("000153");
            }
            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }


            var newId = ((int?)_context.MasterSPGeneralWarehousesShelfs.Max(inv => (int?)inv.ShelfID) ?? 0) + 1;
            var item = new MasterSPGeneralWarehousesShelf
            {
                ShelfID = newId,    
                ZoneID = model.ZoneID,
                ShelfName = model.ShelfName,
                Comments = model.Comments,

                Cancelled = false,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            await _context.MasterSPGeneralWarehousesShelfs.AddAsync(item);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = _context.MasterSPGeneralWarehousesShelfs.Where(e => e.ShelfID == newId
            ).Select(e => new
            {
                e.ZoneID,
                e.ShelfID,
                e.ShelfName,
                e.Comments,
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),

            }).FirstOrDefault();

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> AddSection(AddSectionModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var exists = _context.MasterSPGeneralWarehousesShelfs.Where(e => e.Cancelled == false && e.ShelfID == model.WarehouseShelfID).Any();
            if (!exists)
            {

                throw new ManagerProcessException("000154");
            }

            exists = _context.MasterSPGeneralWarehouseSection.Where(e => e.Cancelled == false && e.WarehouseShelfID == model.WarehouseShelfID &&

             e.SectionName == model.SectionName).Any();
            if (exists)
            {

                throw new ManagerProcessException("000155");
            }
            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }


            var newId = ((int?)_context.MasterSPGeneralWarehouseSection.Max(inv => (int?)inv.SectionID) ?? 0) + 1;
            var item = new MasterSPGeneralWarehouseSection
            {
                SectionID = newId,
                WarehouseShelfID = model.WarehouseShelfID,
                SectionName = model.SectionName,
                Comments = model.Comments,

                Cancelled = false,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            await _context.MasterSPGeneralWarehouseSection.AddAsync(item);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = _context.MasterSPGeneralWarehouseSection.Where(e => e.SectionID==newId).Select(e => new
            {
                e.SectionID,
                e.WarehouseShelfID,
                e.SectionName,
                e.Comments,
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),

            }).FirstOrDefault();

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetSubwarehouse(GetSubwarehouseModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralWarehousesSub.Where(e => e.Cancelled == false &&

            e.WarehouseID == model.WareHouseId &&
             (model.Status == null || e.Status == model.Status)
            ).Select(e => new
            {
                e.WarehouseID,
                e.SubWarehouseID,
                e.SubWarehouseName,
                e.SubWarehouseAbb,
                e.Comments,
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
             

            });
          
            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);
            return result;


        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetZones(GetZonesModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralWarehousesZones.Where(e => e.Cancelled == false &&

            e.WarehouseID == model.WareHouseId &&
            (model.SubWareHouseId == null || e.SubWarehouseID == model.SubWareHouseId) &&
             (model.Status == null || e.Status == model.Status)
            ).Select(e => new
            {
                e.ZoneID,
                e.WarehouseID,
                e.SubWarehouseID,
                e.ZoneName,
                e.Comments,
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),


            });

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);
            return result;


        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetShelf(GetShelfModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralWarehousesShelfs.Where(e => e.Cancelled == false &&

            e.ZoneID == model.ZoneId &&
             (model.Status == null || e.Status == model.Status)
            ).Select(e => new
            {
                e.ZoneID,
                e.ShelfID,
                e.ShelfName,
                e.Comments,
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),

            });

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);
            return result;
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetSection(GetSectionModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralWarehouseSection.Where(e => e.Cancelled == false &&

            e.WarehouseShelfID == model.ShelfID &&
             (model.Status == null || e.Status == model.Status)
            ).Select(e => new
            {
                e.SectionID,
                e.WarehouseShelfID,
                e.SectionName,
                e.Comments,
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),

            });

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);
            return result;
        }
    }
}
