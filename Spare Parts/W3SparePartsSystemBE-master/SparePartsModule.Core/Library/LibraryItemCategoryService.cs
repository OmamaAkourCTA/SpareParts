using Microsoft.Extensions.Configuration;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Domain.Context;
using SparePartsModule.Domain.Models.Library;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Category;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Core.Helpers;
using Microsoft.EntityFrameworkCore;

namespace SparePartsModule.Core.Library
{
    public class LibraryItemCategoryService: ILibraryItemCategoryService
    {
        private readonly IConfiguration _config;
        private readonly SparePartsModuleContext _context;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilties;
        List<string> errors = new List<string>();

        public LibraryItemCategoryService(SparePartsModuleContext context, IConfiguration config, FileHelper fileHelper, UtilitiesHelper utilties)
        {

            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;
            _utilties = utilties;
        }
        public async ValueTask<ApiResponseModel> AddItemCategory(AddItemCategoryModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            bool isLetter = _utilties.IsLettersOnly(model.ItemCategoryNameEn);
            if (!isLetter)
            {
                throw new ManagerProcessException("000053");
            }
            var isArabic = _utilties.IsArabic(model.ItemCategoryNameEn);
            if (isArabic)
            {
                throw new ManagerProcessException("000053");
            }
          
            isArabic = _utilties.IsArabic(model.ItemCategoryCode);
            if (isArabic)
            {
                throw new ManagerProcessException("000055");
            }
            isArabic = _utilties.HasEnglishCharacters(model.ItemCategoryNameAr);
            if (isArabic)
            {
                throw new ManagerProcessException("000054");
            }
            var CategoryExistsInactive = _context.MasterSPGeneralItemCategory
                .Where(e => e.Cancelled == false&&e.Status==(int)Status.Inactive && 
                (
                e.ItemCategoryCode == model.ItemCategoryCode|| 
                e.ItemCategoryNameAr == model.ItemCategoryNameAr||
                e.ItemCategoryNameEn == model.ItemCategoryNameEn
                )).Any();
            if (CategoryExistsInactive)
            {

                throw new ManagerProcessException("000015");
            }
           
            var CategoryExists = _context.MasterSPGeneralItemCategory.Where(e => e.Cancelled == false && e.ItemCategoryNameAr == model.ItemCategoryNameAr).Any();
            if (CategoryExists)
            {

                throw new ManagerProcessException("000012");
            }
         
            isLetter = _utilties.IsLettersOnly(model.ItemCategoryNameEn);
            if (!isLetter)
            {
                throw new ManagerProcessException("000035");
            }
            CategoryExists = _context.MasterSPGeneralItemCategory.Where(e => e.Cancelled == false && e.ItemCategoryNameEn == model.ItemCategoryNameEn).Any();
            if (CategoryExists)
            {

                throw new ManagerProcessException("000013");
            }
            CategoryExists = _context.MasterSPGeneralItemCategory.Where(e => e.Cancelled == false && e.ItemCategoryCode == model.ItemCategoryCode).Any();
            if (CategoryExists)
            {

                throw new ManagerProcessException("000014");
            }
            if (model.ItemCategoryMinMaxType != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemCategoryMinMaxType&&e.LookupTypeID==(int)LookupTypes.MinMaxType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000021");
                }
            }
            if (model.ItemCategorySerialType != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemCategorySerialType && e.LookupTypeID == (int)LookupTypes.ItemCategorySerialType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000022");
                }
            }
            if (model.ItemCategoryTaxType != null)
            {
                var isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemCategoryTaxType && e.LookupTypeID == (int)LookupTypes.TaxType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000023");
                }
            }
            if (model.ItemCategoryDiscountType != null)
            {
                var isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemCategoryDiscountType && e.LookupTypeID == (int)LookupTypes.DiscountType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000024");
                }
            }
            if (model.ItemCategoryTariffID != null)
            {
                var isValid = _context.MasterSPGeneralTariff.Where(e =>e.Cancelled==false&& e.TariffID == model.ItemCategoryTariffID).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000025");
                }
            }
            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            var newId = ((int?)_context.MasterSPGeneralItemCategory.Max(inv => (int?)inv.ItemCategoryID) ?? 0) + 1;
            var Category = new MasterSPGeneralItemCategory
            {
                ItemCategoryID=newId,
                ItemCategoryNameEn = model.ItemCategoryNameEn,
                ItemCategoryNameAr = model.ItemCategoryNameAr,
                ItemCategoryCode = model.ItemCategoryCode,
                ItemCategoryDesc = model.ItemCategoryDesc,
                ItemCategoryType = model.ItemCategoryType,
                ItemCategoryMinMaxType = model.ItemCategoryMinMaxType,
                ItemCategorySerialType = model.ItemCategorySerialType,
                ItemCategoryTariffID = model.ItemCategoryTariffID,
                ItemCategoryTaxType = model.ItemCategoryTaxType,
                ItemCategoryDiscountType = model.ItemCategoryDiscountType,
                ItemCategoryComments= model.ItemCategoryComments,
                ItemCategoryGroup = (int)Settings.Group,
                Cancelled = false,

                Status =model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };
          
        await _context.MasterSPGeneralItemCategory.AddAsync(Category);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditItemCategory(EditItemCategoryModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            bool isLetter = _utilties.IsLettersOnly(model.ItemCategoryNameEn);
            if (!isLetter)
            {
                throw new ManagerProcessException("000053");
            }
            var isArabic = _utilties.IsArabic(model.ItemCategoryNameEn);
            if (isArabic)
            {
                throw new ManagerProcessException("000053");
            }

         
             isArabic = _utilties.IsArabic(model.ItemCategoryCode);
            if (isArabic)
            {
                throw new ManagerProcessException("000055");
            }

            isArabic = _utilties.HasEnglishCharacters(model.ItemCategoryNameAr);
            if (isArabic)
            {
                throw new ManagerProcessException("000054");
            }
            var category = _context.MasterSPGeneralItemCategory.Where(e => e.Cancelled == false&&e.ItemCategoryID==model.ItemCategoryID).FirstOrDefault();
            if (category == null) {
                throw new ManagerProcessException("000016");

            }
            if (model.ItemCategoryNameAr!=null)
            {
                 isLetter = _utilties.IsLettersOnly(model.ItemCategoryNameAr);
                if (!isLetter)
                {
                    throw new ManagerProcessException("000035");
                }
                var CategoryExistsInactive = _context.MasterSPGeneralItemCategory
               .Where(e => e.Cancelled == false && e.Status == (int)Status.Inactive &&e.ItemCategoryID!=model.ItemCategoryID&&
               (
               e.ItemCategoryNameAr == model.ItemCategoryNameAr 
               )).Any();
                if (CategoryExistsInactive)
                {

                    throw new ManagerProcessException("000015");
                }
                var CategoryExists = _context.MasterSPGeneralItemCategory.Where(e => e.Cancelled == false && e.ItemCategoryID != model.ItemCategoryID && e.ItemCategoryNameAr == model.ItemCategoryNameAr).Any();
                if (CategoryExists)
                {

                    throw new ManagerProcessException("000012");
                }
                category.ItemCategoryNameAr = model.ItemCategoryNameAr;

            }
            if (model.ItemCategoryNameEn != null)
            {
                 isLetter = _utilties.IsLettersOnly(model.ItemCategoryNameEn);
                if (!isLetter)
                {
                    throw new ManagerProcessException("000035");
                }
                var CategoryExistsInactive = _context.MasterSPGeneralItemCategory
               .Where(e => e.Cancelled == false && e.Status == (int)Status.Inactive && e.ItemCategoryID != model.ItemCategoryID &&
               (
               e.ItemCategoryNameEn == model.ItemCategoryNameEn
               )).Any();
                if (CategoryExistsInactive)
                {

                    throw new ManagerProcessException("000015");
                }
                var CategoryExists = _context.MasterSPGeneralItemCategory.Where(e => e.Cancelled == false && e.ItemCategoryID != model.ItemCategoryID && e.ItemCategoryNameEn == model.ItemCategoryNameEn).Any();
                if (CategoryExists)
                {

                    throw new ManagerProcessException("000013");
                }
                category.ItemCategoryNameEn = model.ItemCategoryNameEn;

            }
            if (model.ItemCategoryCode != null)
            {
                var CategoryExistsInactive = _context.MasterSPGeneralItemCategory
               .Where(e => e.Cancelled == false && e.Status == (int)Status.Inactive && e.ItemCategoryID != model.ItemCategoryID &&
               (
               e.ItemCategoryCode == model.ItemCategoryCode 
               )).Any();
                if (CategoryExistsInactive)
                {

                    throw new ManagerProcessException("000015");
                }
               var  CategoryExists = _context.MasterSPGeneralItemCategory.Where(e => e.Cancelled == false && e.ItemCategoryID != model.ItemCategoryID && e.ItemCategoryCode == model.ItemCategoryCode).Any();
                if (CategoryExists)
                {

                    throw new ManagerProcessException("000014");
                }
                category.ItemCategoryCode = model.ItemCategoryCode;

            }
            if (model.ItemCategoryMinMaxType != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemCategoryMinMaxType&&e.LookupTypeID==(int)LookupTypes.MinMaxType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000021");
                }
            }
            if (model.ItemCategorySerialType != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemCategorySerialType && e.LookupTypeID == (int)LookupTypes.ItemCategorySerialType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000022");
                }
            }
            if (model.ItemCategoryTaxType != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemCategoryTaxType && e.LookupTypeID == (int)LookupTypes.TaxType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000023");
                }
            }
            if (model.ItemCategoryDiscountType != null)
            {
                var isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemCategoryDiscountType && e.LookupTypeID == (int)LookupTypes.DiscountType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000024");
                }
            }
            if (model.ItemCategoryTariffID != null)
            {
                var isValid = _context.MasterSPGeneralTariff.Where(e =>e.Cancelled==false&& e.TariffID == model.ItemCategoryTariffID).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000025");
                }
            }
            if (model.ItemCategoryDesc != null)
            {
                category.ItemCategoryDesc = model.ItemCategoryDesc;
            }
            if (model.ItemCategoryType != null)
            {
                category.ItemCategoryType = model.ItemCategoryType;
            }
            if (model.ItemCategoryMinMaxType != null)
            {
                category.ItemCategoryMinMaxType = model.ItemCategoryMinMaxType;
            }
            if (model.ItemCategorySerialType != null)
            {
                category.ItemCategorySerialType = model.ItemCategorySerialType;
            }
            if (model.ItemCategoryTariffID != null)
            {
                category.ItemCategoryTariffID = model.ItemCategoryTariffID;
            }
            if (model.ItemCategoryTaxType != null)
            {
                category.ItemCategoryTaxType = model.ItemCategoryTaxType;
            }
            if (model.ItemCategoryDiscountType != null)
            {
                category.ItemCategoryDiscountType = model.ItemCategoryDiscountType;
            }
            if (model.ItemCategoryComments != null)
            {
                category.ItemCategoryComments = model.ItemCategoryComments;
            }
            if (model.Status != null)
            {
                category.Status =(int) model.Status;
            }
            category.ModUser = userId;
            category.ModDate = DateTime.Now;
            category.ModTime = DateTime.Now.TimeOfDay;
            

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteItemCategory(string ItemCategoryID, int userId)
        {
            var afftedRows = _context.MasterSPGeneralItemCategory.Where(e => ("," + ItemCategoryID + ",").Contains("," + e.ItemCategoryID + ","))
                .ExecuteUpdate(e =>
                e.SetProperty(x => x.Status, (int)Status.Deleted)
                .SetProperty(x => x.CancelDate, DateTime.Now)
                .SetProperty(x => x.Cancelled, true)
                .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                .SetProperty(x => x.ModUser, userId)


                );
            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000016");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");


        }
        public async ValueTask<ApiResponseModel> ItemCategoryChangeStatus(string ItemCategoryID, int StatusId, int userId)
        {
            var isValids = _context.MasterLookup.Where(e => e.LookupID == StatusId && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

            var afftedRows = _context.MasterSPGeneralItemCategory.Where(e => ("," + ItemCategoryID + ",").Contains("," + e.ItemCategoryID + ","))
                .ExecuteUpdate(e =>
                e.SetProperty(x => x.Status, StatusId)
                .SetProperty(x => x.ModDate, DateTime.Now)
                .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                .SetProperty(x => x.ModUser, userId)


                );
            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000016");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetItemCategories(GetItemCategoriesModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralItemCategory.Where(e => e.Cancelled == false &&
            (model.Search == null || e.ItemCategoryNameAr.Contains(model.Search) || e.ItemCategoryNameEn.Contains(model.Search) ) &&
            (model.Code == null || e.ItemCategoryCode.Contains(model.Code)) &&
             (model.ItemCategoryType == null || model.ItemCategoryType.Contains( e.ItemCategoryType.ToString())) &&
            (model.Status == null || e.Status == model.Status)
            ).Select(e => new
            {
                e.ItemCategoryID,
                e.ItemCategoryNameAr ,e.ItemCategoryNameEn , e.ItemCategoryCode ,e.ItemCategoryDesc,e.ItemCategoryComments,
                ItemCategoryType = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemCategoryType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemCategoryMinMaxType = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemCategoryMinMaxType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemCategorySerialType = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemCategorySerialType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemCategoryTariffID = _context.MasterSPGeneralTariff.Where(x => x.TariffID == e.ItemCategoryTariffID).Select(x => new { x.TariffID, x.TariffPer,x.TariffCode }).FirstOrDefault(),
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                ItemCategoryTaxType = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemCategoryTaxType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemCategoryDiscountType = _context.MasterLookup.Where(x => x.LookupID == e.ItemCategoryDiscountType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                e.EnterDate,
                e.ModDate,
                e.EnterTime
            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e=>e.EnterTime);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.ItemCategoryNameEn);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.ItemCategoryNameEn);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.ItemCategoryNameAr);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.ItemCategoryNameAr);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.ItemCategoryCode);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.ItemCategoryCode);
            }
            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.ItemCategoryType.LookupName);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.ItemCategoryType.LookupName);
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
    }
}
