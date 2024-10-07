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
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items;
using MimeKit.Tnef;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels.Response;
using SparePartsModule.Interface;
using Microsoft.EntityFrameworkCore;

namespace SparePartsModule.Core.Library
{
    public class LibraryItemsNamesService: ILibraryItemsNamesService
    {
        private readonly SparePartsModuleContext _context;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilties;
        private readonly ILookupService _lookupService;

        public LibraryItemsNamesService(SparePartsModuleContext context, UtilitiesHelper utilties, ILookupService lookupService, FileHelper fileHelper)
        {

          
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;

            _utilties = utilties;
            _lookupService = lookupService;
        }
        public async ValueTask<ApiResponseModel> AddItemName(AddItemNameModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            bool isLetter = _utilties.IsArabic(model.ItemNameEn);
            if (isLetter)
            {
                throw new ManagerProcessException("000056");
            }
            var isArabic = _utilties.HasEnglishCharacters(model.ItemNameAr);
            if (isArabic)
            {
                throw new ManagerProcessException("000057");
            }

            //isArabic = _utilties.IsArabic(model.ItemNameCode);
            //if (isArabic)
            //{
            //    throw new ManagerProcessException("000058");
            //}
            //isArabic = _utilties.HasEnglishCharacters(model.ItemNameAr);
            //if (isArabic)
            //{
            //    throw new ManagerProcessException("000057");
            //}

            var CategoryExists = _context.MasterSPGeneralItemNames
                .Where(e => e.Cancelled == false && 
                e.ItemNameAr == model.ItemNameAr&&
                e.ItemNameCode==model.ItemNameCode&&
                e.ItemNameType==model.ItemNameType).FirstOrDefault();
            if (CategoryExists!=null)
            {
                if(CategoryExists.Status==(int)Status.Inactive)
                {
                    throw new ManagerProcessException("000041");
                }
                throw new ManagerProcessException("000038");
            }
            CategoryExists = _context.MasterSPGeneralItemNames.Where(e => e.Cancelled == false && 
            e.ItemNameEn == model.ItemNameEn&&
            e.ItemNameCode == model.ItemNameCode &&
            e.ItemNameType == model.ItemNameType
            ).FirstOrDefault();
            if (CategoryExists != null)
            {
                if (CategoryExists.Status == (int)Status.Inactive)
                {
                    throw new ManagerProcessException("000042");
                }
                throw new ManagerProcessException("000039");
            }
            //CategoryExists = _context.MasterSPGeneralItemNames.Where(e => e.Cancelled == false && e.ItemNameCode == model.ItemNameCode).FirstOrDefault();
            //if (CategoryExists != null)
            //{

            //    if (CategoryExists.Status == (int)Status.Inactive)
            //    {
            //        throw new ManagerProcessException("000043");
            //    }
            //    throw new ManagerProcessException("000040");
            //}
     

            if (model.ItemNameType != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemNameType&&e.LookupTypeID==(int)LookupTypes.ItemNameType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000046");
                }
            }
            if (model.ItemNameCode != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemNameCode && e.LookupTypeID == (int)LookupTypes.ItemNameCode).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000058");
                }
            }
            //if (model.ItemNameGroup != null)
            //{
            //    var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemNameGroup).Any();
            //    if (!isValid)
            //    {
            //        throw new ManagerProcessException("000044");
            //    }
            //}

            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            var newId = ((int?)_context.MasterSPGeneralItemNames.Max(inv => (int?)inv.ItemNameID) ?? 0) + 1;
            var item = new MasterSPGeneralItemName
            {
                ItemNameID=newId,
                ItemNameGroup =(int)Settings.Group,// model.ItemNameGroup,
                ItemNameEn = model.ItemNameEn,
                ItemNameCode = model.ItemNameCode,
                ItemNameComments = model.ItemNameComments,
                ItemNameDesc = model.ItemNameDesc,  
                ItemNameAr = model.ItemNameAr,  
                ItemNameType = model.ItemNameType,  
                Cancelled = false,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            await _context.MasterSPGeneralItemNames.AddAsync(item);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditItemName(EditItemNameModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            bool isLetter = _utilties.IsArabic(model.ItemNameEn);
            if (isLetter)
            {
                throw new ManagerProcessException("000056");
            }
            var isArabic = _utilties.HasEnglishCharacters(model.ItemNameAr);
            if (isArabic)
            {
                throw new ManagerProcessException("000057");
            }

            //isArabic = _utilties.IsArabic(model.ItemNameCode);
            //if (isArabic)
            //{
            //    throw new ManagerProcessException("000058");
            //}
            isArabic = _utilties.HasEnglishCharacters(model.ItemNameAr);
            if (isArabic)
            {
                throw new ManagerProcessException("000057");
            }
            var item=_context.MasterSPGeneralItemNames.Where(e=>e.ItemNameID==model.ItemNameID).FirstOrDefault();
            if (item == null)
            {
                throw new ManagerProcessException("000045");
            }

            var CategoryExists = _context.MasterSPGeneralItemNames.Where(e => e.Cancelled == false&&e.ItemNameID!=model.ItemNameID && e.ItemNameAr == model.ItemNameAr).FirstOrDefault();
            if (CategoryExists != null)
            {
                if (CategoryExists.Status == (int)Status.Inactive)
                {
                    throw new ManagerProcessException("000041");
                }
                throw new ManagerProcessException("000038");
            }
            CategoryExists = _context.MasterSPGeneralItemNames.Where(e => e.Cancelled == false && e.ItemNameID != model.ItemNameID && e.ItemNameEn == model.ItemNameEn).FirstOrDefault();
            if (CategoryExists != null)
            {
                if (CategoryExists.Status == (int)Status.Inactive)
                {
                    throw new ManagerProcessException("000042");
                }
                throw new ManagerProcessException("000039");
            }
            //CategoryExists = _context.MasterSPGeneralItemNames.Where(e => e.Cancelled == false && e.ItemNameID != model.ItemNameID && e.ItemNameCode == model.ItemNameCode).FirstOrDefault();
            //if (CategoryExists != null)
            //{

            //    if (CategoryExists.Status == (int)Status.Inactive)
            //    {
            //        throw new ManagerProcessException("000043");
            //    }
            //    throw new ManagerProcessException("000040");
            //}
       

            if (model.ItemNameType != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemNameType && e.LookupTypeID == (int)LookupTypes.ItemNameType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000046");
                }
            }
            if (model.ItemNameCode != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemNameCode && e.LookupTypeID == (int)LookupTypes.ItemNameCode).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000058");
                }
            }
            //if (model.ItemNameGroup != null)
            //{
            //    var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemNameGroup).Any();
            //    if (!isValid)
            //    {
            //        throw new ManagerProcessException("000044");
            //    }
            //}

            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

          //  item.ItemNameGroup = model.ItemNameGroup;
            item.ItemNameEn = model.ItemNameEn;
            item.ItemNameCode = model.ItemNameCode;
            item.ItemNameComments = model.ItemNameComments;
            item.ItemNameDesc = model.ItemNameDesc;
            item.ItemNameAr = model.ItemNameAr;
            item.ItemNameType = model.ItemNameType;
           
            item.Status = model.Status;
            item.ModUser = userId;
            item.ModDate = DateTime.Now;
            item.ModTime = DateTime.Now.TimeOfDay;
           
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteItemName(string ItemNameID, int userId)
        {
            var afftedRows = _context.MasterSPGeneralItemNames.Where(e => ("," + ItemNameID + ",").Contains("," + e.ItemNameID + ","))
               .ExecuteUpdate(e =>
               e.SetProperty(x => x.Status, (int)Status.Deleted)
               .SetProperty(x => x.CancelDate, DateTime.Now)
               .SetProperty(x => x.Cancelled, true)
               .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
               .SetProperty(x => x.ModUser, userId)


               );
            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000045");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");


          
        }
        public async ValueTask<ApiResponseModel> ItemNameChangeStatus(string ItemNameID, int StatusId, int userId)
        {
            var isValids = _context.MasterLookup.Where(e => e.LookupID == StatusId && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

            var afftedRows = _context.MasterSPGeneralItemNames.Where(e => ("," + ItemNameID + ",").Contains("," + e.ItemNameID + ","))
                .ExecuteUpdate(e =>
                e.SetProperty(x => x.Status, StatusId)
                .SetProperty(x => x.ModDate, DateTime.Now)
                .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                .SetProperty(x => x.ModUser, userId)


                );
            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000045");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetItemNames(GetItemNameModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralItemNames.Where(e => e.Cancelled == false &&
            (model.Search == null || e.ItemNameEn.Contains(model.Search) || e.ItemNameAr.Contains(model.Search))  &&
             (model.NameEn == null || e.ItemNameEn.Contains(model.NameEn) ) &&
            (model.Code == null  ||  e.MasterSPLookup.LookupName.Contains(model.Code))&&
            (model.Status==null||e.Status==model.Status)

            ).Select(e => new
            {
               
                e.ItemNameID,e.ItemNameAr,e.ItemNameEn,e.ItemNameDesc,e.ItemNameComments,
                ItemNameType = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemNameType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemNameGroup = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemNameGroup).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemNameCode = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemNameCode).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),

                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),

                e.EnterDate,
                e.ModDate
            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.ItemNameEn);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.ItemNameEn);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.ItemNameAr);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.ItemNameAr);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.ItemNameCode.LookupName);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.ItemNameCode.LookupName);
            }
            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.ItemNameType.LookupName);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.ItemNameType.LookupName);
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
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetENItemNames(string? search,PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralSupplierItems
                .Where(e => e.Cancelled == false&&
                !_context.MasterSPGeneralItemNames.Where(x=>x.Cancelled==false&&x.ItemNameEn==e.ItemSupDesc).Any()&&
                (search==null||e.ItemSupDesc.Contains(search)) ).Select(e => new
            {

                e.ItemSupDesc
            }).Distinct();
        

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        public async ValueTask<ApiResponseModel> UploadItemName(UpdateFileModel2 model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "UploadItemName");
            var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            var dt = ds.Tables[0];
            int UploaddedItemsCount = 0;
            int DuplicatedNameCount = 0;
            //int DuplicatedNameARCount = 0;
            int InvalidNameEnCount = 0;
            int InvalidNameARCount = 0;
            int MissingDataCount = 0;
            int FailedtoUploadItemsCount = 0;
          
            var newId = ((int?)_context.MasterSPGeneralItemNames.Max(inv => (int?)inv.ItemNameID) ?? 0) ;
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    continue;
                }

                try
                {
                    var ItemNameEn = dt.Rows[i][0].ToString();
                    var ItemNameAr = dt.Rows[i][1].ToString();
                    var ItemNameCodeName = dt.Rows[i][2].ToString();
                    var ItemNameDesc = dt.Rows[i][3].ToString();
                    var ItemNameTypeName = dt.Rows[i][4].ToString();
                    var ItemNameComments = dt.Rows[i][5].ToString();
                    if(
                        string.IsNullOrEmpty(ItemNameEn) ||
                        string.IsNullOrEmpty(ItemNameAr) || 
                        string.IsNullOrEmpty(ItemNameCodeName) ||
                        string.IsNullOrEmpty(ItemNameDesc)||
                        string.IsNullOrEmpty(ItemNameTypeName)
                        )
                    {
                        MissingDataCount++;
                        continue;
                    }

                    bool isLetter = _utilties.IsArabic(ItemNameEn);
                    if (isLetter)
                    {
                        InvalidNameEnCount++;
                        continue;
                    }
                    var isArabic = _utilties.HasEnglishCharacters(ItemNameAr);
                    if (isArabic)
                    {
                        InvalidNameEnCount++;
                        continue;
                    }

                    var ItemNameCode = _lookupService.GeSpLookup(userId, ItemNameCodeName, (int)LookupTypes.ItemNameCode);
                    var ItemNameType = _lookupService.GeSpLookup(userId, ItemNameTypeName, (int)LookupTypes.ItemNameType);
                    var CategoryExists = _context.MasterSPGeneralItemNames
                        .Where(e => e.Cancelled == false &&
                        e.ItemNameAr == ItemNameAr &&
                        e.ItemNameEn == ItemNameEn &&
                        e.ItemNameCode == ItemNameCode &&
                        e.ItemNameType == ItemNameType).FirstOrDefault();
                    if (CategoryExists != null)
                    {
                        DuplicatedNameCount++;
                        continue;
                    }
                    CategoryExists = _context.MasterSPGeneralItemNames.Where(e => e.Cancelled == false &&
                    e.ItemNameEn == ItemNameEn &&
                    e.ItemNameCode == ItemNameCode &&
                    e.ItemNameType == ItemNameType
                    ).FirstOrDefault();
                    if (CategoryExists != null)
                    {
                        DuplicatedNameCount++;
                        continue;
                    }


                    newId++;
                     var item = new MasterSPGeneralItemName
                    {
                        ItemNameID = newId,
                        ItemNameGroup = (int)Settings.Group,// model.ItemNameGroup,
                        ItemNameEn = ItemNameEn,
                        ItemNameCode = ItemNameCode,
                        ItemNameComments = ItemNameComments,
                        ItemNameDesc = ItemNameDesc,
                        ItemNameAr = ItemNameAr,
                        ItemNameType = ItemNameType,
                        Cancelled = false,
                        Status = (int)Status.Active,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };
                    if (model.Save != false)
                    {
                        await _context.MasterSPGeneralItemNames.AddAsync(item);
                        var result = await _context.SaveChangesAsync();
                    }
                    UploaddedItemsCount++;
                }
                catch
                {
                    FailedtoUploadItemsCount++;
                }
            }
            var response = new ApiResponseModel(200, null);
            response.Data = new { UploaddedItemsCount, 
                FailedtoUploadItemsCount,
                DuplicatedNameCount, 
                InvalidNameEnCount,
                InvalidNameARCount,
                MissingDataCount

            };


            return response;

            throw new ManagerProcessException("000008");
        }

    }
}
