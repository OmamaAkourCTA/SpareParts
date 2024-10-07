using Microsoft.EntityFrameworkCore;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.LibraryTariff;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Location;
using SparePartsModule.Domain.Context;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Domain.Models.Library;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers;
using Microsoft.Extensions.Configuration;
using SparePartsModule.Domain.Models;
using SparePartsModule.Infrastructure.ViewModels.Response;
using SparePartsModule.Interface;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.IdentityModel.Tokens;
using DocumentFormat.OpenXml.Bibliography;
using OfficeOpenXml;
using DocumentFormat.OpenXml.InkML;

namespace SparePartsModule.Core.Library
{
    public class LibraryLocationsService: ILibraryLocationsService
    {
        private readonly IConfiguration _config;
        private readonly SparePartsModuleContext _context;
        private readonly FileHelper _FileHelper;
        private readonly ILookupService _lookupService;
        public LibraryLocationsService(SparePartsModuleContext context, IConfiguration config, FileHelper fileHelper, ILookupService lookupService)
        {

            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;
            _lookupService = lookupService;
        }
        public async ValueTask<ApiResponseModel> AddLocations(AddLocationsModel model, int userId)
        {

          //  string errorCodes = string.Empty;
          //  if (model == null)
          //  {
          //      throw new ArgumentNullException(nameof(model));
          //  }
          //  //var locationExists = _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false && e.loca == model.SupplierName).Any();
          //  //if (locationExists)
          //  //{

          //  //    throw new ManagerProcessException("000006");
          //  //}
          //  var codeExists = _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false &&  e.LocationCode == model.LocationCode).FirstOrDefault();
          //  if (codeExists!=null)
          //  {
          //      if(codeExists.Status==(int)Status.Inactive)
          //      {
                    
          //               throw new ManagerProcessException("000048");
          //      }

          //      throw new ManagerProcessException("000047");
          //  }
          //  codeExists = _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false && e.Location == model.Location).FirstOrDefault();
          //  if (codeExists != null)
          //  {
          //      if (codeExists.Status == (int)Status.Inactive)
          //      {

          //          throw new ManagerProcessException("000107");
          //      }

          //      throw new ManagerProcessException("000106");
          //  }
          //  if (model.LocationWarehouse != null)
          //  {
          //      var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationWarehouse && e.LookupTypeID == (int)LookupTypes.Warehouse).Any();
          //      if (!isValid)
          //      {
          //          throw new ManagerProcessException("000051");
          //      }
          //  }
          //  if (model.LocationWHZone != null)
          //  {
          //      var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationWHZone && e.LookupTypeID == (int)LookupTypes.LocationZone).Any();
          //      if (!isValid)
          //      {
          //          throw new ManagerProcessException("000050");
          //      }
          //  }
          //  if (model.LocationWHShelf != null)
          //  {
          //      var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationWHShelf && e.LookupTypeID == (int)LookupTypes.LocationShelf).Any();
          //      if (!isValid)
          //      {
          //          throw new ManagerProcessException("000031");
          //      }
          //  }
          //  if (model.LocationWHSection != null)
          //  {
          //      var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationWHSection && e.LookupTypeID==(int)LookupTypes.LocationWHSection).Any();
          //      if (!isValid)
          //      {
          //          throw new ManagerProcessException("000049");
          //      }
          //  }
         

          //  if (model.LocationType != null)
          //  {
          //      var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationType && e.LookupTypeID == (int)LookupTypes.LocationType).Any();
          //      if (!isValid)
          //      {
          //          throw new ManagerProcessException("000114");
          //      }
          //  }
          //  var exists = _context.MasterSPGeneralLocations
          //.Where(e => e.Cancelled == false && 
          //e.LocationWarehouse == model.LocationWarehouse &&
          //e.LocationWHZone == model.LocationWHZone && 
          //e.LocationWarehouse == model.LocationWarehouse && 
          //e.LocationWHSection == model.LocationWHSection && 
          //e.LocationCode == model.LocationCode
          //).Any();
          //  if (exists)
          //  {
          //      throw new ManagerProcessException("000037");
          //  }

          //  var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
          //  if (!isValids)
          //  {
          //      throw new ManagerProcessException("000032");
          //  }
          //  isValids = IsValidName(model.Location, model.LocationWHZone, model.LocationWHShelf, model.LocationWHSection, model.LocationRow, model.LocationColumn);
          //  if (!isValids)
          //  {
          //      throw new ManagerProcessException("000017");
          //  }
          //  isValids = IsValidCode(model.LocationCode,model.LocationWarehouse, model.LocationWHZone, model.LocationWHShelf, model.LocationWHSection, model.LocationRow, model.LocationColumn);
          //  if (!isValids)
          //  {
          //      throw new ManagerProcessException("000058");
          //  }


          //  var newId = ((int?)_context.MasterSPGeneralLocations.Max(inv => (int?)inv.LocationID) ?? 0) + 1;

          //  var location = new MasterSPGeneralLocation
          //  {
          //      LocationID=newId,
          //      LocationCode=model.LocationCode,
          //      LocationWarehouse = model.LocationWarehouse,
          //      LocationHeight = model.LocationHeight,
          //      LocationWidth = model.LocationWidth,
          //      LocationLength = model.LocationLength,
          //      LocationType=model.LocationType,
          //      Location = model.Location,
          //      LocationWHZone = model.LocationWHZone,
          //      LocationWHSection = model.LocationWHSection,
          //      LocationWHShelf = model.LocationWHShelf,
          //      LocationColumn = model.LocationColumn,
          //      LocationRow = model.LocationRow,
              
          //      LocationGroup = (int)Settings.Group,


          //      Cancelled = false,

          //      Status = model.Status,
          //      EnterUser = userId,
          //      EnterDate = DateTime.Now,
          //      EnterTime = DateTime.Now.TimeOfDay
          //  };
          //  if (model.LocationHeight != null && model.LocationLength != null && model.LocationWidth != null)
          //  {
          //      location.LocationSizeM3 = model.LocationHeight.Value * model.LocationWidth.Value * model.LocationLength.Value;
          //  }
          //  if (model.LocationQR != null)
          //  {
          //      location.LocationQR = null;
          //      foreach (var image in model.LocationQR)
          //      {
          //          var uploadResult = await _FileHelper.WriteFile(image, "Locations");
          //          location.LocationQR += (location.LocationQR == null ? "" : ",") + uploadResult.ReturnUrl;
          //      }

          //      //    var uploadResult = await _FileHelper.WriteFile(model.LocationQR, "Locations");
          //      //    location.LocationQR = uploadResult.ReturnUrl;
          //  }

          //  await _context.MasterSPGeneralLocations.AddAsync(location);

         
          //  var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
          //  response.Data = newId;

            return response;

            throw new ManagerProcessException("000008");
        }

        public async ValueTask<ApiResponseModel> EditLocations(EditLocationsModel model, int userId)
        {

          //  string errorCodes = string.Empty;
          //  if (model == null)
          //  {
          //      throw new ArgumentNullException(nameof(model));
          //  }
          //  //var locationExists = _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false && e.loca == model.SupplierName).Any();
          //  //if (locationExists)
          //  //{

          //  //    throw new ManagerProcessException("000006");
          //  //}
          //  var location = _context.MasterSPGeneralLocations.Where(e => e.LocationID == model.LocationId).FirstOrDefault();
          //  if (location == null)
          //  {
          //      throw new ManagerProcessException("000036");
          //  }
          //  var exists=_context.MasterSPGeneralLocations
          //      .Where(e=>e.Cancelled==false&& e.LocationID!=model.LocationId&&e.LocationWHZone==model.LocationWHZone&&e.LocationWHZone==model.LocationWHZone
          //      && e.LocationWarehouse==model.LocationWarehouse&&e.LocationWHSection==model.LocationWHSection && e.LocationCode==model.LocationCode
          //      ).Any();
          //  if (exists)
          //  {
          //      throw new ManagerProcessException("000037");
          //  }
          //  var codeExists = _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false&&e.LocationID!=model.LocationId && e.LocationCode == model.LocationCode).FirstOrDefault();
          //  if (codeExists != null)
          //  {
          //      if (codeExists.Status == (int)Status.Inactive)
          //      {

          //          throw new ManagerProcessException("000048");
          //      }

          //      throw new ManagerProcessException("000047");
          //  }
          //  codeExists = _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false && e.LocationID != model.LocationId && e.Location == model.Location).FirstOrDefault();
          //  if (codeExists != null)
          //  {
          //      if (codeExists.Status == (int)Status.Inactive)
          //      {

          //          throw new ManagerProcessException("000107");
          //      }

          //      throw new ManagerProcessException("000106");
          //  }
          //  if (model.LocationWarehouse != null)
          //  {
          //      var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationWarehouse && e.LookupTypeID == (int)LookupTypes.Warehouse).Any();
          //      if (!isValid)
          //      {
          //          throw new ManagerProcessException("000051");
          //      }
          //  }
          //  if (model.LocationWHZone != null)
          //  {
          //      var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationWHZone && e.LookupTypeID == (int)LookupTypes.LocationZone).Any();
          //      if (!isValid)
          //      {
          //          throw new ManagerProcessException("000050");
          //      }
          //  }
          //  if (model.LocationWHShelf != null)
          //  {
          //      var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationWHShelf && e.LookupTypeID == (int)LookupTypes.LocationShelf).Any();
          //      if (!isValid)
          //      {
          //          throw new ManagerProcessException("000031");
          //      }
          //  }
          //  if (model.LocationWHSection != null)
          //  {
          //      var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationWHSection && e.LookupTypeID == (int)LookupTypes.LocationWHSection).Any();
          //      if (!isValid)
          //      {
          //          throw new ManagerProcessException("000049");
          //      }
          //  }
          //  if (model.LocationType != null)
          //  {
          //      var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationType && e.LookupTypeID == (int)LookupTypes.LocationType).Any();
          //      if (!isValid)
          //      {
          //          throw new ManagerProcessException("000114");
          //      }
          //  }

          //  //if (model.LocationGroup != null)
          //  //{
          //  //    var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationGroup).Any();
          //  //    if (!isValid)
          //  //    {
          //  //        throw new ManagerProcessException("000030");
          //  //    }
          //  //}

          //  var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
          //  if (!isValids)
          //  {
          //      throw new ManagerProcessException("000032");
          //  }

          //  isValids = IsValidName(model.Location, model.LocationWHZone, model.LocationWHShelf, model.LocationWHSection, model.LocationRow, model.LocationColumn);
          //  if (!isValids)
          //  {
          //      throw new ManagerProcessException("000017");
          //  }
          //  isValids = IsValidCode(model.LocationCode, model.LocationWarehouse, model.LocationWHZone, model.LocationWHShelf, model.LocationWHSection, model.LocationRow, model.LocationColumn);
          //  if (!isValids)
          //  {
          //      throw new ManagerProcessException("000058");
          //  }



          //  location.LocationType= model.LocationType;  

          //  location.LocationWarehouse = model.LocationWarehouse;
          //  location.LocationHeight = model.LocationHeight;
          //  location.LocationCode= model.LocationCode;  
          //  location.LocationWidth = model.LocationWidth;
          //  location.LocationLength = model.LocationLength;
          //  location.Location = model.Location;
          //  location.LocationType = model.LocationType;
          //  location.LocationWHZone = model.LocationWHZone;
          //  location.LocationWHSection = model.LocationWHSection;
          //  location.LocationWarehouse= model.LocationWarehouse;
          //  location.LocationWHZone = model.LocationWHZone;
          ////  location.LocationGroup = model.LocationGroup;


          //  location.Cancelled = false;

          //  location.Status = model.Status;
          //  location.ModUser = userId;
          //  location.ModDate = DateTime.Now;
          //  location.ModTime = DateTime.Now.TimeOfDay;
           
          //  if (model.LocationHeight != null && model.LocationLength != null && model.LocationWidth != null)
          //  {
          //      location.LocationSizeM3 = model.LocationHeight.Value * model.LocationWidth.Value * model.LocationLength.Value;
          //  }
          //  if (model.LocationQR != null)
          //  {
          //      location.LocationQR = null;
          //      foreach (var image in model.LocationQR)
          //      {
          //          var uploadResult = await _FileHelper.WriteFile(image, "Locations");
          //          location.LocationQR += (location.LocationQR == null ? "" : ",") + uploadResult.ReturnUrl;
          //      }

          //      //var uploadResult = await _FileHelper.WriteFile(model.LocationQR, "Locations");
          //      //location.LocationQR = uploadResult.ReturnUrl;
          //  }

          //  var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteLocations(string LocationId, int userId)
        {
            var afftedRows = _context.MasterSPGeneralLocations.Where(e => ("," + LocationId + ",").Contains("," + e.LocationID + ","))
                      .ExecuteUpdate(e =>
                      e.SetProperty(x => x.Status, (int)Status.Deleted)
                      .SetProperty(x => x.CancelDate, DateTime.Now)
                      .SetProperty(x => x.Cancelled, true)
                      .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                      .SetProperty(x => x.ModUser, userId)


                      );
            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000036");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");


           
        }
        public async ValueTask<ApiResponseModel> LocationsChangeStatus(string LocationId, int StatusId, int userId)
        {
            var isValids = _context.MasterLookup.Where(e => e.LookupID == StatusId && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

            var afftedRows = _context.MasterSPGeneralLocations.Where(e => ("," + LocationId + ",").Contains("," + e.LocationID + ","))
                .ExecuteUpdate(e =>
                e.SetProperty(x => x.Status, StatusId)
                .SetProperty(x => x.ModDate, DateTime.Now)
                .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                .SetProperty(x => x.ModUser, userId)


                );
            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000036");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetLocations(GetLocationsModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false &&
            (model.Search == null || e.Location.Contains(model.Search))&&
            (model.Code == null || e.LocationCode.Contains(model.Code)) &&
              // (model.LocationWHZone == null || e.LocationWHZone == model.LocationWHZone) &&
              //    (model.LocationWHShelf == null || e.LocationWHShelf == model.LocationWHShelf) &&
              //       (model.LocationWHSection == null || e.LocationWHSection == model.LocationWHSection) &&
           // (model.LocationWarehouse==null|| model.LocationWarehouse.Contains(e.LocationWarehouse.ToString()))&&
              (model.LocationType == null || model.LocationType.Contains(e.LocationType.ToString()))
            ).Select(e => new
            {
                e.LocationID,e.Location,e.LocationWidth,e.LocationHeight,e.LocationLength,e.LocationSizeM3,e.LocationCode,e.LocationColumn,e.LocationRow,
                // LocationQR=e.LocationQR==null?null: _config["Settings:BaseUrl"] +e.LocationQR.Trim(),
                LocationQR = e.LocationQR == null ? null : (_config["Settings:BaseUrl"] + e.LocationQR.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None),
               // LocationWHSection = _context.MasterSPLookup.Where(x => x.LookupID == e.LocationWHSection).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
              //  LocationWarehouse = _context.MasterSPLookup.Where(x => x.LookupID == e.LocationWarehouse).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
              //  LocationWHZone = _context.MasterSPLookup.Where(x => x.LookupID == e.LocationWHZone).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
              //  LocationWHShelf = _context.MasterSPLookup.Where(x => x.LookupID == e.LocationWHShelf).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                LocationType = _context.MasterSPLookup.Where(x => x.LookupID == e.LocationType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),

                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,e.EnterTime,e.ModDate,e.ModTime,
                EnterUser = _context.Users.Where(x=>x.UserId==e.EnterUser).FirstOrDefault(),
                ModUser = _context.Users.Where(x => x.UserId == e.ModUser).FirstOrDefault(),

            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate);
            }
            //if (model.Sort == 2)
            //{
            //    data = data.OrderBy(e => e.LocationWarehouse.LookupName);
            //}
            //if (model.Sort == 3)
            //{
            //    data = data.OrderByDescending(e => e.LocationWarehouse.LookupName);
            //}
            //if (model.Sort == 4)
            //{
            //    data = data.OrderBy(e => e.LocationWHZone.LookupName);
            //}
            //if (model.Sort == 5)
            //{
            //    data = data.OrderByDescending(e => e.LocationWHZone.LookupName);
            //}
            //if (model.Sort == 6)
            //{
            //    data = data.OrderBy(e => e.LocationWHZone.LookupName);
            //}
            //if (model.Sort == 7)
            //{
            //    data = data.OrderByDescending(e => e.LocationWHShelf.LookupName);
            //}
            //if (model.Sort == 8)
            //{
            //    data = data.OrderBy(e => e.LocationWHSection.LookupName);
            //}
            //if (model.Sort == 9)
            //{
            //    data = data.OrderByDescending(e => e.LocationWHSection.LookupName);
            //}
            if (model.Sort == 10)
            {
                data = data.OrderBy(e => e.Status.LookupName);
            }
            if (model.Sort == 11)
            {
                data = data.OrderByDescending(e => e.Status.LookupName);
            }

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetLocationsV2(GetLocationsModelV2 model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false &&
            (model.Search == null || e.Location.Contains(model.Search)) &&
            (model.Code == null || e.LocationCode.Contains(model.Code)) &&
               (model.LocationWHZone == null || e.LocationWHZoneID == model.LocationWHZone) &&
                  (model.LocationWHShelf == null || e.LocationWHShelfID == model.LocationWHShelf) &&
                     (model.LocationWHSection == null || e.LocationWHSectionID == model.LocationWHSection) &&
            (model.LocationWarehouse == null || model.LocationWarehouse.Contains(e.LocationWarehouseID.ToString())) &&
            (model.LocationSubWarehouse == null || model.LocationSubWarehouse.Contains(e.LocationSubWarehouseID.ToString())) &&
              (model.LocationType == null || model.LocationType.Contains(e.LocationType.ToString()))
            ).Select(e => new
            {
                e.LocationID,
                e.Location,
                e.LocationWidth,
                e.LocationHeight,
                e.LocationLength,
                e.LocationSizeM3,
                e.LocationCode,
                e.LocationColumn,
                e.LocationRow,
                e.LocationWarehouseID,
                e.LocationSubWarehouseID,
                // LocationQR=e.LocationQR==null?null: _config["Settings:BaseUrl"] +e.LocationQR.Trim(),
                LocationQR = e.LocationQR == null ? null : (_config["Settings:BaseUrl"] + e.LocationQR.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None),
                LocationWHSection = _context.MasterSPGeneralWarehouseSection.Where(x => x.SectionID == e.LocationWHSectionID).Select(x => new { x.SectionID, x.SectionName }).FirstOrDefault(),
                LocationWarehouse = _context.MasterSPGeneralWareHouses.Where(x => x.WarehouseID == e.LocationWarehouseID).Select(x => new { x.WarehouseID, x.WarehouseName }).FirstOrDefault(),
                LocationWHZone = _context.MasterSPGeneralWarehousesZones.Where(x => x.ZoneID == e.LocationWHZoneID).Select(x => new { x.ZoneID, x.ZoneName }).FirstOrDefault(),
                LocationWHShelf = _context.MasterSPGeneralWarehousesShelfs.Where(x => x.ShelfID == e.LocationWHShelfID).Select(x => new { x.ShelfID, x.ShelfName }).FirstOrDefault(),
                LocationType = _context.MasterSPLookup.Where(x => x.LookupID == e.LocationType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                SubWarehouse = _context.MasterSPGeneralWarehousesSub.Where(x => x.SubWarehouseID == e.LocationSubWarehouseID).Select(x => new { x.SubWarehouseID, x.SubWarehouseName }).FirstOrDefault(),

                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,
                e.EnterTime,
                e.ModDate,
                e.ModTime,
                EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).FirstOrDefault(),
                ModUser = _context.Users.Where(x => x.UserId == e.ModUser).FirstOrDefault(),

            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.LocationWarehouse.WarehouseName);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.LocationWarehouse.WarehouseName);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.LocationWHZone.ZoneName);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.LocationWHZone.ZoneName);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.LocationWHShelf.ShelfName);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.LocationWHShelf.ShelfName);
            }
            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.LocationWHSection.SectionName);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.LocationWHSection.SectionName);
            }
            if (model.Sort == 10)
            {
                data = data.OrderBy(e => e.Status.LookupName);
            }
            if (model.Sort == 11)
            {
                data = data.OrderByDescending(e => e.Status.LookupName);
            }

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        private bool IsValidName(string name,int zone,int shelf,int section, string row, string column)
        {
            bool isValid = false;
          //  var warehouseName = _context.MasterSPLookup.Where(e=>e.LookupID== warehouse).Select(e=>e.LookupName).FirstOrDefault();
            var zoneName = _context.MasterSPLookup.Where(e => e.LookupID == zone).Select(e => e.LookupName).FirstOrDefault();
            var shelfName = _context.MasterSPLookup.Where(e => e.LookupID == shelf).Select(e => e.LookupName).FirstOrDefault();
            var sectionName = _context.MasterSPLookup.Where(e => e.LookupID == section).Select(e => e.LookupName).FirstOrDefault();
            var tempName= (zoneName.Length<3? zoneName: zoneName.Substring(0,2)) + shelfName.Substring(0, 2) + "-"+ sectionName.Substring(0, 2) + "-"+row.ToString()+column.ToString();

            return tempName==name;

        }
        private bool IsValidNameV2(string name, int zone, int shelf, int section, string row, string column)
        {
            return true;
            bool isValid = false;
            //  var warehouseName = _context.MasterSPLookup.Where(e=>e.LookupID== warehouse).Select(e=>e.LookupName).FirstOrDefault();
            var zoneName = _context.MasterSPGeneralWarehousesZones.Where(e => e.ZoneID == zone).Select(e => e.ZoneName).FirstOrDefault();
            var shelfName = _context.MasterSPGeneralWarehousesShelfs.Where(e => e.ShelfID == shelf).Select(e => e.ShelfName).FirstOrDefault();
            var sectionName = _context.MasterSPGeneralWarehouseSection .Where(e => e.SectionID  == section).Select(e => e.SectionName ).FirstOrDefault();
            var tempName = (zoneName.Length < 3 ? zoneName : zoneName.Substring(0, 2)) +
                (shelfName.Length < 3 ? shelfName : shelfName.Substring(0, 2)) + "-" +
              
                (sectionName.Length < 3 ?sectionName: sectionName.Substring(0, 2)) + "-" + row.ToString() + column.ToString();

            return true;// tempName == name;

        }
        private bool IsValidCode(string Code, int warehouse, int zone, int shelf, int section, string row, string column)
        {
            bool isValid = false;
            var warehouseName = _context.MasterSPLookup.Where(e => e.LookupID == warehouse).Select(e => e.LookupName).FirstOrDefault();
            var zoneName = _context.MasterSPLookup.Where(e => e.LookupID == zone).Select(e => e.LookupName).FirstOrDefault();
            var shelfName = _context.MasterSPLookup.Where(e => e.LookupID == shelf).Select(e => e.LookupName).FirstOrDefault();
            var sectionName = _context.MasterSPLookup.Where(e => e.LookupID == section).Select(e => e.LookupName).FirstOrDefault();
            var tempCode = 
                (warehouseName.Length < 3 ? warehouseName : warehouseName.Substring(0, 2).ToUpper()) + "-" +
                (zoneName.Length < 3 ? zoneName : zoneName.Substring(0, 2)) + shelfName.Substring(0, 2) + "-" + 
                sectionName.Substring(0, 2) + "-" + row.ToString() + column.ToString();

            return tempCode == Code;

        }
        private bool IsValidCodeV2(string Code, int warehouse, int zone, int shelf, int section, string row, string column,int? subwarehouse)
        {
          // return true;    
            bool isValid = false;
            var WarehouseAbb = _context.MasterSPGeneralWareHouses.Where(e => e.WarehouseID == warehouse).Select(e => e.WarehouseAbb).FirstOrDefault();
            var zoneName = _context.MasterSPGeneralWarehousesZones.Where(e => e.ZoneID == zone).Select(e => e.ZoneName).FirstOrDefault();
            var shelfName = _context.MasterSPGeneralWarehousesShelfs.Where(e => e.ShelfID == shelf).Select(e => e.ShelfName).FirstOrDefault();
            var sectionName = _context.MasterSPGeneralWarehouseSection.Where(e => e.SectionID == section).Select(e => e.SectionName).FirstOrDefault();
            var SubWarehouseAbb = _context.MasterSPGeneralWarehousesSub.Where(e => e.SubWarehouseID == subwarehouse).Select(e => e.SubWarehouseAbb).FirstOrDefault();

            var tempCode = WarehouseAbb + (SubWarehouseAbb == null ? "" : ("-" + SubWarehouseAbb))+"-"+

        
                 zoneName.Substring(0, 1) +
                 (shelfName.Length < 3 ? shelfName : shelfName.Substring(0, 2)) + "-" +
                (sectionName.Length < 3 ? sectionName : sectionName.Substring(0, 2)) + "-" +
                row.Substring(0, 1) + column.Substring(0, 1);

            return tempCode == Code;

        }
        public async ValueTask<ApiResponseModel> ImportLocations(UpdateFileModel2 model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "UploadLocations");
            var ExcelSheetData = ReadExcelFile(tempFile.ReturnUrl);

            int UploaddedItemsCount = 0;
            int DuplicatedLocationCount = 0;
            int FailedtoUploadItemsCount = 0;
            int FailedtoUploadItemsWidthCount = 0;
            int FailedtoUploadItemsHeightCount = 0;
            int FailedtoUploadItemsLengthCount = 0;
            int FailedtoUploadItemsRowCount = 0;
            int FailedtoUploadItemsColumnCount = 0;
            int InvalidWarehouseCount = 0;

            int LocationWHZone = 0;
            int LocationWHShelf = 0;
            int LocationWHSection = 0;
            int LocationType = 0;
            int subWareHouseId = 0;

            decimal? LocationHeight = null;
            decimal? LocationWidth = null;
            decimal? LocationLength = null;
            decimal? volume = null;

            var LocationData = await _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false).ToListAsync();
            var GeneralWareHouses = await _context.MasterSPGeneralWareHouses.Where(e => e.Cancelled == false).ToListAsync();
            var GeneralWarehousesSub = await _context.MasterSPGeneralWarehousesSub.Where(e=>e.Cancelled==false).ToListAsync();
            var GeneralWarehousesZones = await _context.MasterSPGeneralWarehousesZones.Where(e => e.Cancelled == false).ToListAsync();
            var GeneralWarehousesShelfs = await _context.MasterSPGeneralWarehousesShelfs.Where(e => e.Cancelled == false).ToListAsync();
            var GeneralWarehouseSection = await _context.MasterSPGeneralWarehouseSection.Where(e => e.Cancelled == false).ToListAsync();

            var newId = ((int?)_context.MasterSPGeneralLocations.Max(inv => (int?)inv.LocationID) ?? 0);
            var subWareHouseIdnew = ((int?)GeneralWarehousesSub.Max(inv => (int?)inv.SubWarehouseID) ?? 0);
            var zoneNew = ((int?)GeneralWarehousesZones.Max(inv => (int?)inv.ZoneID) ?? 0);
            var shelfNew = ((int?)GeneralWarehousesShelfs.Max(inv => (int?)inv.ShelfID) ?? 0);
            var sectionNew = ((int?)GeneralWarehouseSection.Max(inv => (int?)inv.SectionID) ?? 0);

            foreach (var item in ExcelSheetData)
            {
                try
                {
                    LocationWHZone = LocationWHShelf = LocationWHSection = LocationType = subWareHouseId = 0;
                    LocationHeight=decimal.Parse(item.Height);
                    LocationWidth = decimal.Parse(item.Width);
                    LocationLength = decimal.Parse(item.Length);
                    //LocationHeight = LocationWidth = LocationLength = volume = null;


                    if (string.IsNullOrEmpty(item.WarehouseName))
                    {
                        continue;
                    }

                    var LocationWarehouse = GeneralWareHouses.FirstOrDefault(e =>e.Cancelled==false&& e.WarehouseName == item.WarehouseName);
                    if (LocationWarehouse == null)
                    {
                        InvalidWarehouseCount++;
                        continue;
                    }
                   // LocationWarehouse.WarehouseAbb = item.WarehouseAbb;

                    subWareHouseId = GeneralWarehousesSub.FirstOrDefault(e => e.WarehouseID == LocationWarehouse.WarehouseID &&
                    e.SubWarehouseName == item.Subwarehouse)?.SubWarehouseID ?? 0;
                    if (subWareHouseId == 0)
                    {
                        subWareHouseIdnew++;
                        subWareHouseId = subWareHouseIdnew;
                        MasterSPGeneralWarehousesSub SubWarehouse = new MasterSPGeneralWarehousesSub
                        {
                            SubWarehouseID = subWareHouseId,
                            WarehouseID = LocationWarehouse.WarehouseID,
                            SubWarehouseName = item.Subwarehouse,
                            SubWarehouseAbb=item.SubWarehouseAbb,
                            Cancelled = false,
                            Status = (int)Status.Active,
                            EnterUser = userId,
                            EnterDate = DateTime.Now,
                            EnterTime = DateTime.Now.TimeOfDay
                        };
                        await _context.MasterSPGeneralWarehousesSub.AddAsync(SubWarehouse);

                        GeneralWarehousesSub.Add(SubWarehouse);
                    }

                    LocationWHZone = GeneralWarehousesZones.
                        FirstOrDefault(e => e.ZoneName == item.Zone && e.WarehouseID == LocationWarehouse.WarehouseID && e.SubWarehouseID == subWareHouseId)?.ZoneID ?? 0;
                    if (LocationWHZone == 0)
                    {
                        zoneNew++;
                        LocationWHZone = zoneNew;
                        var ZoneData = await AddWarehousesZones(LocationWHZone, LocationWarehouse.WarehouseID, subWareHouseId, item.Zone, userId);
                        await _context.MasterSPGeneralWarehousesZones.AddAsync(ZoneData);
                        GeneralWarehousesZones.Add(ZoneData);

                        shelfNew++;
                        LocationWHShelf = shelfNew;
                        var Shelfs = await AddWarehousesShelfs(LocationWHShelf, LocationWHZone, item.Shelf, userId);
                        await _context.MasterSPGeneralWarehousesShelfs.AddAsync(Shelfs);
                        GeneralWarehousesShelfs.Add(Shelfs);

                        sectionNew++;
                        LocationWHSection = sectionNew;
                        var Section = await AddWarehouseSection(LocationWHSection, LocationWHShelf, item.Section, userId);
                        await _context.MasterSPGeneralWarehouseSection.AddAsync(Section);
                        GeneralWarehouseSection.Add(Section);
                    }
                    else
                    {
                        LocationWHShelf = GeneralWarehousesShelfs
                                    .FirstOrDefault(e => e.ShelfName == item.Shelf && e.ZoneID == LocationWHZone)?.ShelfID ?? 0;
                        if (LocationWHShelf == 0)
                        {
                            shelfNew++;
                            LocationWHShelf = shelfNew;
                            var Shelfs = await AddWarehousesShelfs(LocationWHShelf, LocationWHZone, item.Shelf, userId);
                            await _context.MasterSPGeneralWarehousesShelfs.AddAsync(Shelfs);
                            GeneralWarehousesShelfs.Add(Shelfs);

                            sectionNew++;
                            LocationWHSection = sectionNew;
                            var Section = await AddWarehouseSection(LocationWHSection, LocationWHShelf, item.Section, userId);
                            await _context.MasterSPGeneralWarehouseSection.AddAsync(Section);
                            GeneralWarehouseSection.Add(Section);
                        }
                        else
                        {
                            LocationWHSection = GeneralWarehouseSection
                                .FirstOrDefault(e => e.SectionName == item.Section && e.WarehouseShelfID == LocationWHShelf)?.SectionID ?? 0;
                            if (LocationWHSection == 0)
                            {
                                sectionNew++;
                                LocationWHSection = sectionNew;
                                var Section = await AddWarehouseSection(LocationWHSection, LocationWHShelf, item.Section, userId);
                                await _context.MasterSPGeneralWarehouseSection.AddAsync(Section);
                                GeneralWarehouseSection.Add(Section);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(item.LocationType))
                    {
                        LocationType = _lookupService.GeSpLookup(userId, item.LocationType, (int)LookupTypes.LocationType);
                    }

                    if (LocationLength != null && LocationHeight != null && LocationWidth != null)
                    {

                        volume = LocationWidth * LocationHeight * LocationLength;
                    }
         

                    var exists = _context.MasterSPGeneralLocations                      // If record exist in table then insert or not
                          .Where(e => e.Cancelled == false &&
                          e.LocationWarehouseID == LocationWarehouse.WarehouseID &&
                          e.LocationSubWarehouseID == subWareHouseId &&
                          e.LocationWHZoneID == LocationWHZone &&
                          e.LocationWHShelfID == LocationWHShelf && 
                          e.LocationWHSectionID == LocationWHSection &&
                          e.LocationType == LocationType &&
                          e.LocationRow == item.Row &&
                          e.LocationColumn == item.Column
                          ).Any();
                    if (exists)
                    {
                        DuplicatedLocationCount++;
                        continue;
                    }

                    var Location = GetLocation(item.Zone,item.Shelf , item.Section, item.Row, item.Column);
                    var LocationCode = GetLocationCode(item.WarehouseAbb, item.Zone, item.Shelf, item.Section, item.Row, item.Column, item.SubWarehouseAbb);

                    newId++;
                    var location = new MasterSPGeneralLocationsNew
                    {
                        LocationID = newId,
                        LocationCode = LocationCode,
                        LocationWarehouseID = LocationWarehouse.WarehouseID,
                        LocationSubWarehouseID = subWareHouseId,
                        LocationHeight = decimal.Parse( item.Height),
                        LocationWidth = decimal.Parse(item.Width),
                        LocationLength = LocationLength,
                        LocationType = LocationType,
                        Location = Location,
                        LocationWHZoneID = LocationWHZone,
                        LocationWHSectionID = LocationWHSection,
                        LocationWHShelfID = LocationWHShelf,
                        LocationColumn = item.Column,
                        LocationRow = item.Row,
                        LocationSizeM3 = volume,
                        LocationGroup = (int)Settings.Group,
                        Cancelled = false,
                        Status = (int)Status.Active,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };

                    UploaddedItemsCount++;
                    if (model.Save != false)
                    {
                        _context.MasterSPGeneralWareHouses.Update(LocationWarehouse);
                        await _context.MasterSPGeneralLocations.AddAsync(location);
                        var result = await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    FailedtoUploadItemsCount++;
                }
            }

            var response = new ApiResponseModel(200, null);
            response.Data = new
            {

                UploaddedItemsCount,
                FailedtoUploadItemsCount,
                DuplicatedLocationCount,
                FailedtoUploadItemsHeightCount,
                FailedtoUploadItemsWidthCount,
                FailedtoUploadItemsLengthCount,
                FailedtoUploadItemsRowCount,
                FailedtoUploadItemsColumnCount,
                InvalidWarehouseCount,
            };

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> ImportLocationsOld(UpdateFileModel2 model, int userId)
        {

            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "UploadSuppliers");
            var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            var dt = ds.Tables[0];
            int UploaddedItemsCount = 0;
            int DuplicatedLocationCount = 0;
            int FailedtoUploadItemsCount = 0;
            int FailedtoUploadItemsWidthCount = 0;
            int FailedtoUploadItemsHeightCount = 0;
            int FailedtoUploadItemsLengthCount = 0;
            int FailedtoUploadItemsRowCount = 0;
            int FailedtoUploadItemsColumnCount = 0;
            int InvalidWarehouseCount = 0;
            var newId = ((int?)_context.MasterSPGeneralLocations.Max(inv => (int?)inv.LocationID) ?? 0);
            var subWareHouseIdnew = ((int?)_context.MasterSPGeneralWarehousesSub.Max(inv => (int?)inv.SubWarehouseID) ?? 0);
            var zoneNew = ((int?)_context.MasterSPGeneralWarehousesZones.Max(inv => (int?)inv.ZoneID) ?? 0);
            var shelfNew = ((int?)_context.MasterSPGeneralWarehousesShelfs.Max(inv => (int?)inv.ShelfID) ?? 0);
            var sectionNew = ((int?)_context.MasterSPGeneralWarehouseSection.Max(inv => (int?)inv.SectionID) ?? 0);

            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    continue;
                }

                try
                {


                    var wareHouse = dt.Rows[i][0].ToString();

                    int LocationWarehouse = _context.MasterSPGeneralWareHouses.Where(e=>e.WarehouseName==wareHouse).Select(e=>e.WarehouseID).FirstOrDefault();
                    if(LocationWarehouse==0)
                    {
                        InvalidWarehouseCount++;
                        continue;
                    }
                    var subWarehouse = dt.Rows[i][1].ToString();   
                    int?subWareHouseId = null;
                    if(!string.IsNullOrEmpty(subWarehouse))
                    {
                        subWareHouseId=_context.MasterSPGeneralWarehousesSub.Where(e =>e.WarehouseID==LocationWarehouse&&
                        e.SubWarehouseName == subWarehouse).Select(e => e.SubWarehouseID).FirstOrDefault();
                        if(subWareHouseId==0)
                        {
                            subWareHouseIdnew++;
                            subWareHouseId = subWareHouseIdnew;
                            _context.MasterSPGeneralWarehousesSub.Add(new MasterSPGeneralWarehousesSub
                            {
                                SubWarehouseID=(int) subWareHouseId,
                                WarehouseID= LocationWarehouse,
                                SubWarehouseName=subWarehouse,
                                Cancelled=false,
                                Status=(int)Status.Active,
                                EnterUser=userId,
                                EnterDate=DateTime.Now,
                                EnterTime=DateTime.Now.TimeOfDay


                            });
                        }   
                    }
                    var zone = dt.Rows[i][2].ToString();

                    int LocationWHZone =_context.MasterSPGeneralWarehousesZones.Where(e=>e.ZoneName==zone&&e.WarehouseID== LocationWarehouse)
                        .Select(e=>e.ZoneID).FirstOrDefault() ;
                    if(LocationWHZone==0)
                    {
                        zoneNew++;
                        LocationWHZone = zoneNew;
                        _context.MasterSPGeneralWarehousesZones.Add(new MasterSPGeneralWarehousesZone
                        {
                            ZoneID = (int)LocationWHZone,
                            WarehouseID = LocationWarehouse,
                            SubWarehouseID=subWareHouseId,
                            ZoneName = zone,
                            Cancelled = false,
                            Status = (int)Status.Active,
                            EnterUser = userId,
                            EnterDate = DateTime.Now,
                            EnterTime = DateTime.Now.TimeOfDay
                        });
                    }

                    var shelf = dt.Rows[i][3].ToString();
                    int LocationWHShelf = _context.MasterSPGeneralWarehousesShelfs.Where(e => e.ShelfName == shelf && e.ZoneID == LocationWHZone)
                        .Select(e => e.ShelfID).FirstOrDefault();
                    if (LocationWHShelf == 0)
                    {
                        shelfNew++;
                        LocationWHShelf = shelfNew;
                        _context.MasterSPGeneralWarehousesShelfs.Add(new MasterSPGeneralWarehousesShelf
                        {
                            ShelfID = (int)LocationWHShelf,
                            ZoneID = LocationWHZone,
                            ShelfName = shelf,
                            Cancelled = false,
                            Status = (int)Status.Active,
                            EnterUser = userId,
                            EnterDate = DateTime.Now,
                            EnterTime = DateTime.Now.TimeOfDay
                        });
                    }   

          

                    var section = dt.Rows[i][4].ToString();
                    int LocationWHSection = _context.MasterSPGeneralWarehouseSection.Where(e => e.SectionName == section && e.WarehouseShelfID == LocationWHShelf)
                        .Select(e => e.SectionID).FirstOrDefault();
                    if (LocationWHSection == 0)
                    {
                        sectionNew++;
                        LocationWHSection = sectionNew;
                        _context.MasterSPGeneralWarehouseSection.Add(new MasterSPGeneralWarehouseSection
                        {
                            SectionID = (int)LocationWHSection,
                            WarehouseShelfID = LocationWHShelf,
                            SectionName = section,
                            Cancelled = false,
                            Status = (int)Status.Active,
                            EnterUser = userId,
                            EnterDate = DateTime.Now,
                            EnterTime = DateTime.Now.TimeOfDay
                        });
                    }


                  

                
                    var LocationRow = dt.Rows[i][6].ToString();
                    var LocationColumn = dt.Rows[i][7].ToString();

                    int? LocationType = null;
                    if (!string.IsNullOrEmpty(dt.Rows[i][5].ToString()))
                    {
                        var locationTypeName = dt.Rows[i][5].ToString();
                        LocationType = _lookupService.GeSpLookup(userId, locationTypeName, (int)LookupTypes.LocationType);
                    }


                    decimal? LocationHeight = null;
                    if (!string.IsNullOrEmpty(dt.Rows[i][8].ToString()))
                    {
                         LocationHeight = decimal.Parse(dt.Rows[i][8].ToString());
                        if (LocationHeight <= 0)
                        {
                            FailedtoUploadItemsHeightCount++;
                            continue;
                        }
                    }
                    decimal? LocationWidth = null;
                    if (!string.IsNullOrEmpty(dt.Rows[i][9].ToString()))
                    {
                        LocationWidth = decimal.Parse(dt.Rows[i][9].ToString());
                        if (LocationWidth <= 0)
                        {
                            FailedtoUploadItemsWidthCount++;
                            continue;
                        }
                    }
                    decimal? LocationLength = null;
                    if (!string.IsNullOrEmpty(dt.Rows[i][10].ToString()))
                    {
                        LocationLength = decimal.Parse(dt.Rows[i][10].ToString());
                        if (LocationLength <= 0)
                        {
                            FailedtoUploadItemsLengthCount++;
                            continue;
                        }
                    }
                    decimal? volume = null;
                    if(LocationLength != null&& LocationHeight != null && LocationWidth != null )
                    {
                        volume = LocationWidth * LocationHeight * LocationLength;
                    }

                    //if (LocationRow <1|| LocationRow>9)
                    //{
                    //    FailedtoUploadItemsRowCount++;
                    //    continue;
                    //}
                    //if (LocationColumn < 1 || LocationRow > 9)
                    //{
                    //    FailedtoUploadItemsColumnCount++;
                    //    continue;
                    //}



                    var exists = _context.MasterSPGeneralLocations
                          .Where(e => e.Cancelled == false &&
                          e.LocationWarehouseID == LocationWarehouse &&
                          e.LocationWHZoneID == LocationWHZone &&
                          e.LocationWHSectionID == LocationWHSection
                          ).Any();
                    if (exists)
                    {
                        DuplicatedLocationCount++;
                        continue;
                    }


                    var Location = GetLocation(zone, shelf, section, LocationRow, LocationColumn);

                    var LocationCode = GetLocationCode(wareHouse, zone, shelf, section, LocationRow, LocationColumn,subWarehouse);



                    newId++;
                    var location = new MasterSPGeneralLocationsNew
                    {
                        LocationID = newId,
                        LocationCode = LocationCode,
                        LocationWarehouseID = LocationWarehouse,
                        LocationSubWarehouseID = subWareHouseId,
                        LocationHeight = LocationHeight,
                        LocationWidth = LocationWidth,
                        LocationLength = LocationLength,
                        LocationType = LocationType,
                        Location = Location,
                        LocationWHZoneID = LocationWHZone,
                        LocationWHSectionID = LocationWHSection,
                        LocationWHShelfID = LocationWHShelf,
                        LocationColumn = LocationColumn,
                        LocationRow = LocationRow,
                        LocationSizeM3=volume,

                        LocationGroup = (int)Settings.Group,


                        Cancelled = false,

                        Status = (int)Status.Active,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };

                  //  location.LocationSizeM3 = LocationHeight??0 * LocationWidth??0 * LocationLength??0;

                    UploaddedItemsCount++;

                    await _context.MasterSPGeneralLocations.AddAsync(location);


                    var result = await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    FailedtoUploadItemsCount++;
                }
            }

            var response = new ApiResponseModel(200, null);
            response.Data = new {

                UploaddedItemsCount,
                FailedtoUploadItemsCount, 
                DuplicatedLocationCount,
                FailedtoUploadItemsHeightCount,
                FailedtoUploadItemsWidthCount,
                FailedtoUploadItemsLengthCount,
                FailedtoUploadItemsRowCount,
                FailedtoUploadItemsColumnCount,
                InvalidWarehouseCount,

            };

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> AddLocationsV2(AddLocationsModelV2 model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            //var locationExists = _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false && e.loca == model.SupplierName).Any();
            //if (locationExists)
            //{

            //    throw new ManagerProcessException("000006");
            //}
            var codeExists = _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false && e.LocationCode == model.LocationCode).FirstOrDefault();
            if (codeExists != null)
            {
                if (codeExists.Status == (int)Status.Inactive)
                {

                    throw new ManagerProcessException("000048");
                }

                throw new ManagerProcessException("000047");
            }
            codeExists = _context.MasterSPGeneralLocations.Where(e => e.Cancelled == false && e.Location == model.Location).FirstOrDefault();
            if (codeExists != null)
            {
                if (codeExists.Status == (int)Status.Inactive)
                {
                    throw new ManagerProcessException("000107");
                }

                throw new ManagerProcessException("000106");
            }
            if (model.LocationWarehouse != null)
            {
                var isValid = _context.MasterSPGeneralWareHouses.Where(e => e.WarehouseID == model.LocationWarehouse ).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000051");
                }
            }
            if (model.LocationSubWarehouse != null)
            {
                var isValid = _context.MasterSPGeneralWarehousesSub.Where(e =>e.WarehouseID==model.LocationWarehouse&& e.SubWarehouseID == model.LocationSubWarehouse).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000151");
                }
            }
            if (model.LocationWHZone != null)
            {
                var isValid = _context.MasterSPGeneralWarehousesZones.Where(e => e.ZoneID == model.LocationWHZone ).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000050");
                }
            }
            if (model.LocationWHShelf != null)
            {
                var isValid = _context.MasterSPGeneralWarehousesShelfs.Where(e => e.ShelfID == model.LocationWHShelf) .Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000154");
                }
            }
            if (model.LocationWHSection != null)
            {
                var isValid = _context.MasterSPGeneralWarehouseSection.Where(e => e.SectionID == model.LocationWHSection ).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000156");
                }
            }

            if (model.LocationType != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000114");
                }
            }
            var exists = _context.MasterSPGeneralLocations
          .Where(e => e.Cancelled == false &&
          e.LocationWarehouseID == model.LocationWarehouse &&
          e.LocationWHZoneID == model.LocationWHZone &&
          e.LocationWarehouseID == model.LocationWarehouse &&
           e.LocationSubWarehouseID == model.LocationSubWarehouse &&
          e.LocationWHSectionID == model.LocationWHSection &&
          e.LocationCode == model.LocationCode
          ).Any();
            if (exists)
            {
                throw new ManagerProcessException("000037");
            }

            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            isValids = IsValidNameV2(model.Location, model.LocationWHZone, model.LocationWHShelf, model.LocationWHSection, model.LocationRow, model.LocationColumn);
            if (!isValids)
            {
                throw new ManagerProcessException("000017");
            }
            isValids = IsValidCodeV2(model.LocationCode, model.LocationWarehouse, model.LocationWHZone, model.LocationWHShelf, model.LocationWHSection, model.LocationRow, model.LocationColumn,model.LocationSubWarehouse);
            if (!isValids)
            {
                throw new ManagerProcessException("000058");
            }

            var newId = ((int?)_context.MasterSPGeneralLocations.Max(inv => (int?)inv.LocationID) ?? 0) + 1;

            var location = new MasterSPGeneralLocationsNew
            {
                LocationID = newId,
                LocationCode = model.LocationCode,
                LocationSubWarehouseID = model.LocationSubWarehouse,
                LocationWarehouseID = model.LocationWarehouse,
                LocationHeight = model.LocationHeight ,
                LocationWidth = model.LocationWidth,
                LocationLength = model.LocationLength,
                LocationType = model.LocationType,
                Location = model.Location,
                LocationWHZoneID = model.LocationWHZone,
                LocationWHSectionID = model.LocationWHSection,
                LocationWHShelfID = model.LocationWHShelf,
                LocationColumn = model.LocationColumn,
                LocationRow = model.LocationRow,

                LocationGroup = (int)Settings.Group,

                Cancelled = false,

                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };
            if (model.LocationHeight != null && model.LocationLength != null && model.LocationWidth != null)
            {
                location.LocationSizeM3 = model.LocationHeight * model.LocationWidth * model.LocationLength;
            }
            if (model.LocationQR != null)
            {
                location.LocationQR = null;
                foreach (var image in model.LocationQR)
                {
                    var uploadResult = await _FileHelper.WriteFile(image, "Locations");
                    location.LocationQR += (location.LocationQR == null ? "" : ",") + uploadResult.ReturnUrl;
                }

                //    var uploadResult = await _FileHelper.WriteFile(model.LocationQR, "Locations");
                //    location.LocationQR = uploadResult.ReturnUrl;
            }

            await _context.MasterSPGeneralLocations.AddAsync(location);


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = newId;

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditLocationsV2(EditLocationsModelV2 model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var location = _context.MasterSPGeneralLocations.Where(e => e.LocationID == model.LocationId).FirstOrDefault();
            if (location == null)
            {
                throw new ManagerProcessException("000036");
            }
            var codeExists = _context.MasterSPGeneralLocations.Where(e =>e.LocationID!=model.LocationId&& e.Cancelled == false && e.LocationCode == model.LocationCode).FirstOrDefault();
            if (codeExists != null)
            {
                if (codeExists.Status == (int)Status.Inactive)
                {

                    throw new ManagerProcessException("000048");
                }

                throw new ManagerProcessException("000047");
            }
            codeExists = _context.MasterSPGeneralLocations.Where(e => e.LocationID != model.LocationId && e.Cancelled == false && e.Location == model.Location).FirstOrDefault();
            if (codeExists != null)
            {
                if (codeExists.Status == (int)Status.Inactive)
                {

                    throw new ManagerProcessException("000107");
                }

                throw new ManagerProcessException("000106");
            }
            if (model.LocationWarehouse != null)
            {
                var isValid = _context.MasterSPGeneralWareHouses.Where(e => e.WarehouseID == model.LocationWarehouse).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000051");
                }
            }
            if (model.LocationSubWarehouse != null)
            {
                var isValid = _context.MasterSPGeneralWarehousesSub.Where(e => e.WarehouseID == model.LocationWarehouse && e.SubWarehouseID == model.LocationSubWarehouse).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000151");
                }
            }
            if (model.LocationWHZone != null)
            {
                var isValid = _context.MasterSPGeneralWarehousesZones.Where(e => e.ZoneID == model.LocationWHZone).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000050");
                }
            }
            if (model.LocationWHShelf != null)
            {
                var isValid = _context.MasterSPGeneralWarehousesShelfs.Where(e => e.ShelfID == model.LocationWHShelf).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000154");
                }
            }
            if (model.LocationWHSection != null)
            {
                var isValid = _context.MasterSPGeneralWarehouseSection.Where(e => e.SectionID == model.LocationWHSection).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000156");
                }
            }


            if (model.LocationType != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.LocationType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000114");
                }
            }
            var exists = _context.MasterSPGeneralLocations
          .Where(e => e.LocationID != model.LocationId && e.Cancelled == false &&
          e.LocationWarehouseID == model.LocationWarehouse &&
          e.LocationWHZoneID == model.LocationWHZone &&
          e.LocationWarehouseID == model.LocationWarehouse &&
           e.LocationSubWarehouseID == model.LocationSubWarehouse &&
          e.LocationWHSectionID == model.LocationWHSection &&
          e.LocationCode == model.LocationCode
          ).Any();
            if (exists)
            {
                throw new ManagerProcessException("000037");
            }

            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            isValids = IsValidNameV2(model.Location, model.LocationWHZone, model.LocationWHShelf, model.LocationWHSection, model.LocationRow, model.LocationColumn);
            if (!isValids)
            {
                throw new ManagerProcessException("000017");
            }
            isValids = IsValidCodeV2(model.LocationCode, model.LocationWarehouse, model.LocationWHZone, model.LocationWHShelf, model.LocationWHSection, model.LocationRow, model.LocationColumn,model.LocationSubWarehouse);
            if (!isValids)
            {
                throw new ManagerProcessException("000058");
            }


         

               
             location.LocationCode = model.LocationCode;
             location.LocationSubWarehouseID = model.LocationSubWarehouse;
             location.LocationWarehouseID = model.LocationWarehouse;
             location.LocationHeight = model.LocationHeight;
             location.LocationWidth = model.LocationWidth;
             location.LocationLength = model.LocationLength;
             location.LocationType = model.LocationType;
             location.Location = model.Location;
             location.LocationWHZoneID = model.LocationWHZone;
             location.LocationWHSectionID = model.LocationWHSection;
             location.LocationWHShelfID = model.LocationWHShelf;
             location.LocationColumn = model.LocationColumn;
             location.LocationRow = model.LocationRow;


            location.Status = model.Status;
            location.ModUser = userId;
            location.ModDate = DateTime.Now;
            location.ModTime = DateTime.Now.TimeOfDay;
           
            if (model.LocationHeight != null && model.LocationLength != null && model.LocationWidth != null)
            {
                location.LocationSizeM3 = model.LocationHeight * model.LocationWidth * model.LocationLength;
            }
            if (model.LocationQR != null)
            {
                location.LocationQR = null;
                foreach (var image in model.LocationQR)
                {
                    var uploadResult = await _FileHelper.WriteFile(image, "Locations");
                    location.LocationQR += (location.LocationQR == null ? "" : ",") + uploadResult.ReturnUrl;
                }

                //    var uploadResult = await _FileHelper.WriteFile(model.LocationQR, "Locations");
                //    location.LocationQR = uploadResult.ReturnUrl;
            }

         


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
           

            return response;

            throw new ManagerProcessException("000008");
        }

        #region Helper method 

        private string GetLocation(string zoneName, string shelfName, string sectionName, string row, string column)
        {
            var tempName = zoneName + shelfName + "-" + sectionName + "-" + row.ToString() + column.ToString();
            return tempName;
        }
        private string GetLocationCode(string WarehouseAbb, string zoneName, string shelfName, string sectionName, string row, string column, string SubWarehouseAbb)
        {
            var tempCode = WarehouseAbb + (SubWarehouseAbb == null ? "" : ("-" + SubWarehouseAbb)) + "-" +


              (zoneName.Length < 3 ? zoneName : zoneName.Substring(0, 1)) +
               (shelfName.Length < 3 ? shelfName : shelfName.Substring(0, 2)) + "-" +
              (sectionName.Length < 3 ? sectionName : sectionName.Substring(0, 2)) + "-" +
              row.Substring(0, 1) + column.Substring(0, 1);

            //var tempCode = warehouseName.Substring(0, 4).ToUpper() + "-" + subwarehouseName.Substring(0, 4).ToUpper() + "-" + zoneName.Substring(0, 1) + shelfName + "-" + sectionName + "-" + row.ToString() + column.ToString();
            return tempCode;
        }
        private List<WarehouseLocation> ReadExcelFile(string filePath)
        {
            var dataList = new List<WarehouseLocation>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++) // Assuming the first row is the header
                {
                    var data = new WarehouseLocation
                    {
                        WarehouseName = worksheet.Cells[row, 1].Text,
                        WarehouseAbb= worksheet.Cells[row, 2].Text,

                        Subwarehouse = worksheet.Cells[row, 3].Text,
                        SubWarehouseAbb = worksheet.Cells[row, 4].Text,
                        Zone = worksheet.Cells[row, 5].Text,
                        Shelf = worksheet.Cells[row,6].Text,
                        Section = worksheet.Cells[row, 7].Text,
                        LocationType = worksheet.Cells[row, 8].Text,
                        Row = string.IsNullOrEmpty(worksheet.Cells[row, 9].Text) ? "0" : worksheet.Cells[row, 9].Text,
                        Column =string.IsNullOrEmpty(worksheet.Cells[row, 10].Text)?"0": worksheet.Cells[row, 10].Text,
                        Height = string.IsNullOrEmpty(worksheet.Cells[row, 11].Text) ? "0" : worksheet.Cells[row, 11].Text,
                        Width = string.IsNullOrEmpty(worksheet.Cells[row, 12].Text) ? "0" : worksheet.Cells[row, 12].Text,
                        Length = string.IsNullOrEmpty(worksheet.Cells[row, 13].Text) ? "0" : worksheet.Cells[row, 13].Text,
                    };

                    dataList.Add(data);

                    //if(row > 200)
                    //{
                    //    break;
                    //}
                }
            }

            return dataList;
        }
        private async Task<MasterSPGeneralWarehousesZone> AddWarehousesZones(int LocationWHZone, int LocationWarehouse, int? subWareHouseId, string ZoneName, int userId)
        {
            MasterSPGeneralWarehousesZone Zone = new MasterSPGeneralWarehousesZone
            {
                ZoneID = LocationWHZone,
                WarehouseID = LocationWarehouse,
                SubWarehouseID = subWareHouseId,
                ZoneName = ZoneName,
                Cancelled = false,
                Status = (int)Status.Active,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            return Zone;
        }
        private async Task<MasterSPGeneralWarehousesShelf> AddWarehousesShelfs(int LocationWHShelf, int LocationWHZone, string ShelfName, int userId)
        {
            MasterSPGeneralWarehousesShelf Shelfs = new MasterSPGeneralWarehousesShelf
            {
                ShelfID = LocationWHShelf,
                ZoneID = LocationWHZone,
                ShelfName = ShelfName,
                Cancelled = false,
                Status = (int)Status.Active,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            return Shelfs;
        }
        private async Task<MasterSPGeneralWarehouseSection> AddWarehouseSection(int LocationWHSection, int LocationWHShelf, string SectionName, int userId)
        {
            MasterSPGeneralWarehouseSection Section = new MasterSPGeneralWarehouseSection
            {
                SectionID = LocationWHSection,
                WarehouseShelfID = LocationWHShelf,
                SectionName = SectionName,
                Cancelled = false,
                Status = (int)Status.Active,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            return Section;
        }
        #endregion
    }
}
