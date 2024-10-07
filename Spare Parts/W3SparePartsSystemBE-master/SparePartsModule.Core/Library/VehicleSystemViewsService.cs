
using Microsoft.Extensions.Configuration;
using SparePartsModule.Core.Helpers;
using SparePartsModule.Domain.Context;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Infrastructure.ViewModels.Models.ListNames;

namespace SparePartsModule.Core.Library
{
    public class VehicleSystemViewsService : IVehicleSystemViewsService
    {
        private readonly IConfiguration _config;
        private readonly SparePartsModuleContext _context;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilties;

        private readonly VehicleSystemContext _contextVehicle;

        private readonly PdfExportSalesSpecs _PdfExportSalesSpecs;
        private readonly ExcelExportSalesSpecs _ExcelExportSalesSpecs;

        List<string> errors = new List<string>();

        public VehicleSystemViewsService(SparePartsModuleContext context, IConfiguration config, FileHelper fileHelper, UtilitiesHelper utilties, 
            VehicleSystemContext contextVehicle, ExcelExportSalesSpecs ExcelExportSalesSpecs, PdfExportSalesSpecs PdfExportSalesSpecs)
        {

            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;
            _utilties = utilties;
            _contextVehicle = contextVehicle;
            _PdfExportSalesSpecs= PdfExportSalesSpecs;
            _ExcelExportSalesSpecs= ExcelExportSalesSpecs;
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetBrands(GetBrandModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterVehicleBrand.Where(e => e.Cancelled == false &&
            (model.Search == null || e.BrandName.Contains(model.Search) || e.BrandNameAR.Contains(model.Search)) &&
            (model.BrandOrigionCountry == null || e.BrandOrigionCountry == model.BrandOrigionCountry) &&
             (model.MarkaziaBrand == null || e.MarkaziaBrand == model.MarkaziaBrand) &&
               (model.Status == null || e.Status == model.Status)

            ).Select(e => new
            {
                e.BrandID,
                e.BrandName,
                e.BrandNameAR,
                //e.BrandComments,
                //e.MarkaziaBrand,
                //// BrandLogo=e.BrandLogo==null?null: _config["Settings:BaseUrl"] + e.BrandLogo,
                //// BrandBusinessPlan = e.BrandBusinessPlan == null ? null : _config["Settings:BaseUrl"] + e.BrandBusinessPlan,
                //BrandLogo = string.IsNullOrEmpty(e.BrandLogo) ? null : (_config["Settings:BaseUrl"] + e.BrandLogo.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None),
                //BrandBusinessPlan = string.IsNullOrEmpty(e.BrandBusinessPlan) ? null : (_config["Settings:BaseUrl"] + e.BrandBusinessPlan.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None),
                //BrandOrigionCountry = _context.MasterLookup.Where(x => x.LookupID == e.BrandOrigionCountry).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                //BrandGroup = _context.MasterLookup.Where(x => x.LookupID == e.BrandGroup).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                //Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                //e.EnterDate,
                //e.EnterTime,
                //e.ModDate,
                //e.ModTime,
                //EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).FirstOrDefault(),
                //ModUser = _context.Users.Where(x => x.UserId == e.ModUser).FirstOrDefault(),
            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.BrandID);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.BrandName);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.BrandName);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.BrandNameAR);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.BrandNameAR);
            }
            //if (model.Sort == 6)
            //{
            //    data = data.OrderBy(e => e.BrandOrigionCountry.LookupName);
            //}
            //if (model.Sort == 7)
            //{
            //    data = data.OrderByDescending(e => e.BrandOrigionCountry.LookupName);
            //}

            //if (model.Sort == 10)
            //{
            //    data = data.OrderBy(e => e.Status.LookupName);
            //}
            //if (model.Sort == 11)
            //{
            //    data = data.OrderByDescending(e => e.Status.LookupName);
            //}

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetModelCode(GetModelCodeModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterVehicleModelCode.Where(e => e.Cancelled == false &&
            (model.Search == null || e.ModelCodeVehicles.Contains(model.Search) || e.ModelCodeSP.Contains(model.Search) || e.ModelCodeService.Contains(model.Search)) &&

            (model.ModelId == null || e.ModelID == model.ModelId) &&

            (model.Status == null || e.Status == model.Status)
            ).Select(e => new
            {
                e.ModelCodeID,
                e.ModelCodeVehicles,
                e.ModelCodeSP,
                e.ModelCodeService,
                e.ModelCodeComments,
                e.ModelCodeValidFrom, e.ModelCodeValidTo,e.ModelCodeYear,
                Model = _context.MasterVehicleModel.Where(x => x.ModelID == e.ModelID).Select(x => new { x.ModelID, x.ModelName, x.ModelNameAR, }).FirstOrDefault(),
                Brand = (from brand in _context.MasterVehicleBrand
                         join mod in _context.MasterVehicleModel on brand.BrandID equals mod.ModelBrandID
                         where mod.ModelID == e.ModelID
                         select new { brand.BrandID, brand.BrandName, brand.BrandNameAR }).FirstOrDefault(),
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),

                e.EnterDate,
                e.ModDate,
                e.EnterTime,
                e.ModTime,
                EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).FirstOrDefault(),
                ModUser = _context.Users.Where(x => x.UserId == e.ModUser).FirstOrDefault(),
                //ExtColors = (from ext in _context.MasterVehicleModelCodeColorExt
                //             join color in _context.MasterVehicleGeneralColors on ext.ColorExtID equals color.ColorID
                //             where ext.ModelCodeID == e.ModelCodeID && ext.Cancelled == false
                //             select new
                //             {
                //                 ext.ModelCodeColorExtID,
                //                 color.ColorID,
                //                 color.ColorHEXCode,
                //                 color.ColorSupCode,
                //                 color.ColorNameAr,
                //                 color.ColorNameEn,
                //                 color.ColorCustoms,


                //                 Status = _context.MasterLookup.Where(x => x.LookupID == ext.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                //                 ext.EnterDate,
                //                 ext.ModDate,
                //                 ext.EnterTime,
                //                 ext.ModTime,
                //                 EnterUser = _context.Users.Where(x => x.UserId == ext.EnterUser).FirstOrDefault(),
                //                 ModUser = _context.Users.Where(x => x.UserId == ext.ModUser).FirstOrDefault(),
                //             }).ToList()
            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.ModelCodeID);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.ModelCodeVehicles);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.ModelCodeVehicles);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.ModelCodeService);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.ModelCodeService);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.ModelCodeSP);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.ModelCodeSP);
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
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetModels(GetModelModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterVehicleModel.Where(e => e.Cancelled == false &&
            (model.Search == null || e.ModelName.Contains(model.Search) || e.ModelNameAR.Contains(model.Search)) &&
            (model.BrandId == null || e.ModelBrandID == model.BrandId) &&
            (model.ModelId == null || e.ModelID == model.ModelId) &&
            (model.ModelSegment == null || e.ModelSegment == model.ModelSegment) &&
            (model.ModelCustomType == null || e.ModelCustomType == model.ModelCustomType) &&
            (model.Status == null || e.Status == model.Status) &&
            (model.supplierID == null || e.ModelSupplierID == model.supplierID) &&
             (model.ModelType == null || e.ModelType == model.ModelType) &&
              (model.EquivalentMarkaziaModelID == null || e.EquivalentMarkaziaModelID == model.EquivalentMarkaziaModelID)
            ).Select(e => new
            {
                e.ModelID,
                e.ModelName,
                e.ModelNameAR,
                //e.ModelRemarks,
                //e.ModelBrandID,
                //e.MarkaziaModel,
                //e.ModelYear,
                //ModelAttachment = string.IsNullOrEmpty(e.ModelAttachments) ? null : (_config["Settings:BaseUrl"] + e.ModelAttachments.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None).ToList(),
                //ModelImages = string.IsNullOrEmpty(e.ModelImages) ? null : (_config["Settings:BaseUrl"] + e.ModelImages.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None),//.Select(x => _config["Settings:BaseUrl"] + x).ToList(),
                //ModelMarketingImages = string.IsNullOrEmpty(e.ModelMarketingImages) ? null : (_config["Settings:BaseUrl"] + e.ModelMarketingImages.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None),//.Select(x => _config["Settings:BaseUrl"] + x).ToList(),
                ////ModelSupplier = _context.GettingVehicleSupplier.Where(x => x.SupplierID == e.ModelSupplierID).Select(x => new { x.SupplierID, x.SupplierName, x.SupplierCode }).FirstOrDefault(),
                //Brand = _context.MasterVehicleBrand.Where(x => x.BrandID == e.ModelBrandID).Select(x => new { x.BrandID, x.BrandName, x.BrandNameAR }).FirstOrDefault(),
                ////ModelType = _context.MasterVehicleLookup.Where(x => x.LookupID == e.ModelType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ////ModelGroup = _context.MasterVehicleLookup.Where(x => x.LookupID == e.ModelGroup).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ////ModelSegment = _context.MasterVehicleLookup.Where(x => x.LookupID == e.ModelSegment).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ////ModelCustomType = _context.MasterVehicleLookup.Where(x => x.LookupID == e.ModelCustomType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                //// Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                //Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,
                //e.ModDate,
                e.EnterTime
                //e.ModTime,
                //EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).FirstOrDefault(),
                //ModUser = _context.Users.Where(x => x.UserId == e.ModUser).FirstOrDefault(),
                //e.EquivalentMarkaziaModelID,
                //EquivalentMarkaziaModel = _context.MasterVehicleModel.Where(x => x.ModelID == e.EquivalentMarkaziaModelID).Select(x => new { x.ModelID, x.ModelName, x.ModelNameAR }).FirstOrDefault(),
                //EquivalentMarkaziaModels = _context.MasterVehicleModel.Where(x => x.EquivalentMarkaziaModelID == e.ModelID).Select(x => new { x.ModelID, x.ModelName, x.ModelNameAR }).ToList()
            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.ModelName);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.ModelName);
            }
            //if (model.Sort == 4)
            //{
            //    data = data.OrderBy(e => e.ModelSegment.LookupName);
            //}
            //if (model.Sort == 5)
            //{
            //    data = data.OrderByDescending(e => e.ModelSegment.LookupName);
            //}
            //if (model.Sort == 6)
            //{
            //    data = data.OrderBy(e => e.Brand.BrandName);
            //}
            //if (model.Sort == 7)
            //{
            //    data = data.OrderByDescending(e => e.Brand.BrandName);
            //   }
            //if (model.Sort == 8)
            //{
            //    data = data.OrderBy(e => e.ModelSupplier.SupplierName);
            //}
            //if (model.Sort == 9)
            //{
            //    data = data.OrderByDescending(e => e.ModelSupplier.SupplierName);
            //}
            //if (model.Sort == 10)
            //{
            //    data = data.OrderBy(e => e.MarkaziaModel);
            //}
            //if (model.Sort == 11)
            //{
            //    data = data.OrderByDescending(e => e.MarkaziaModel);
            //}
            //if (model.Sort == 12)
            //{
            //    data = data.OrderBy(e => e.EquivalentMarkaziaModelID);
            //}
            //if (model.Sort == 13)
            //{
            //    data = data.OrderByDescending(e => e.EquivalentMarkaziaModelID);
            //}
            //if (model.Sort == 14)
            //{
            //    data = data.OrderBy(e => e.Status.LookupName);
            //}
            //if (model.Sort == 15)
            //{
            //    data = data.OrderByDescending(e => e.Status.LookupName);
            //}

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetListName(GetListNameModel model, PaginationModel paginationPostModel)
        {

            var data = _contextVehicle.MasterVehilceList.Where(e => e.Cancelled == false &&
            (model.Search == null || e.ListName.Contains(model.Search)) &&
             //  (model.BrandOrigionCountry == null || e.BrandOrigionCountry == model.BrandOrigionCountry) &&
             (model.BrandId == null || ("," + model.BrandId + ",").Contains("," + e.ListBrandID.ToString() + ",")) &&
              (model.ModelId == null || ("," + model.ModelId + ",").Contains("," + e.ListModelID.ToString() + ",")) &&
               (model.ModelYear == null || model.ModelYear.Contains(e.ProductionYear.ToString())) &&
                (model.ModelCode == null || ("," + model.ModelCode + ",").Contains("," + e.ListModelCodeID.ToString() + ",")) &&
                 (model.MarkaziaList == null || e.ListMarkazia == model.MarkaziaList) &&
                 (model.Status == null || e.Status == model.Status) &&
                  (model.SegmentId == null || model.SegmentId.Contains(e.ListSegment.ToString())) &&
                   (model.SupplierId == null || ("," + model.SupplierId + ",").Contains("," + e.ListSupplierID.ToString() + ",")) &&
                   (model.ProductionCode == null || e.ListSFX.Contains(model.ProductionCode)) &&
                    (model.GeneralSpecItemID == null || e.MasterVehicleGeneralSpecsValues.Where(x => x.GeneralSpecsItemID == model.GeneralSpecItemID).Any())


            ).Select(e => new
            {
                e.ListID,
                e.ListName,



                e.ListYear_TBD,
                e.ListSFX,
                e.ListIndex,
                e.ListKCode,

                e.ListMarkazia,
                ListMrkaziaEquivalentID = _contextVehicle.MasterVehicleModel.Where(x => x.ModelID == e.ListMrkaziaEquivalentID).Select(x => new { x.ModelID, x.ModelName, x.ModelNameAR }).FirstOrDefault(),
                ListCustomType = _contextVehicle.MasterVehicleLookup.Where(x => x.LookupID == e.ListCustomType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                e.ProductionMonth,
                e.ProductionYear,
                e.CandFPrice,

                FuelType = _contextVehicle.MasterVehicleGeneralSpecs.Where(x => x.GeneralSpecID == e.FuelType).Select(x => new { x.GeneralSpecID, x.GeneralSpecsNameAR, x.GeneralSpecsNameEN }).FirstOrDefault(),

                e.ListGroup,

                //  ListSpecsFile = e.ListSpecsFile == null ? null : _config["Settings:BaseUrl"] + e.ListSpecsFile,
                ListSpecsFile = string.IsNullOrEmpty(e.ListSpecsFile) ? null : (_config["Settings:BaseUrl"] + e.ListSpecsFile.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None),

                CandFCurrency = _contextVehicle.MasterLookup.Where(x => x.LookupID == e.CandFCurrency).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ListSegment = _contextVehicle.MasterVehicleLookup.Where(x => x.LookupID == e.ListSegment).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ListBrand = _contextVehicle.MasterVehicleBrand.Where(x => x.BrandID == e.ListBrandID).Select(x => new { x.BrandID, x.BrandName, x.BrandNameAR }).FirstOrDefault(),
                ListModel = _contextVehicle.MasterVehicleModel.Where(x => x.ModelID == e.ListModelID).Select(x => new { x.ModelID, x.ModelName, x.ModelNameAR }).FirstOrDefault(),
                ListModelCode = _contextVehicle.MasterVehicleModelCode.Where(x => x.ModelCodeID == e.ListModelCodeID).Select(x => new { x.ModelCodeVehicles, x.ModelCodeID }).FirstOrDefault(),
                ListSupplier = _contextVehicle.GettingVehicleSupplier.Where(x => x.SupplierID == e.ListSupplierID).Select(x => new { x.SupplierID, x.SupplierName }).FirstOrDefault(),

                Status = _contextVehicle.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,
                e.EnterTime,
                e.ModDate,
                e.ModTime,
                EnterUser = _contextVehicle.Users.Where(x => x.UserId == e.EnterUser).FirstOrDefault(),
                ModUser = _contextVehicle.Users.Where(x => x.UserId == e.ModUser).FirstOrDefault(),
            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.ListID);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.ListName);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.ListName);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.ListBrand.BrandName);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.ListBrand.BrandName);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.ListModel.ModelName);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.ListModel.ModelName);
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


        public async ValueTask<ApiResponseModel> ExportSalesSpecsSheet(int ListId)
        {


            var excelEn = _ExcelExportSalesSpecs.CreateExcelFile(ListId, "ExportSalesSpecsSheetExcel", "En");
            var excelAr = _ExcelExportSalesSpecs.CreateExcelFile(ListId, "ExportSalesSpecsSheetExcel", "Ar");
            var pdfEn = _PdfExportSalesSpecs.CreateDocument(ListId, "En");
            var pdfEnFile = await _FileHelper.WriteFile(pdfEn, "ExportSalesSpecsSheet");

            var pdfAr = _PdfExportSalesSpecs.CreateDocument(ListId, "AR");
            var pdfArFile = await _FileHelper.WriteFile(pdfAr, "ExportSalesSpecsSheet");
            var data = new
            {
                PdfEngFileEn = _config["Settings:BaseUrl"] + pdfEnFile.ReturnUrl,
                PdfEngFileAr = _config["Settings:BaseUrl"] + pdfArFile.ReturnUrl,
                ExcelEngFileEn = _config["Settings:BaseUrl"] + excelEn,
                ExcelEngFileAr = _config["Settings:BaseUrl"] + excelAr
            };
            var response = new ApiResponseModel(200, data);


            return response;

            throw new ManagerProcessException("000008");
        }

        public async ValueTask<PaginationDatabaseResponseDto<object>> GetListNameSpecsValues(GetListNameSpecsValuesModel model, PaginationModel paginationPostModel)
        {

            var data = _contextVehicle.MasterVehilceListSpecsValues.Where(e => e.Cancelled == false && e.ListID == model.ListNameID &&
            (model.Status == null || e.Status == model.Status) &&
            (model.GeneralSpecsCategory == null || e.MasterVehicleGeneralSpec.GeneralSpecsCategory == model.GeneralSpecsCategory)



            ).Select(e => new
            {
                e.MasterVehicleGeneralSpec.GeneralSpecsNameEN,
                e.MasterVehicleGeneralSpec.GeneralSpecsNameAR,
                e.MasterVehicleGeneralSpec.GeneralSpecID,
                Category = _contextVehicle.MasterVehicleLookup.Where(y => y.LookupID == e.MasterVehicleGeneralSpec.GeneralSpecsCategory).Select(y => y.LookupName).FirstOrDefault(),
                e.MasterVehicleGeneralSpecsValue.GeneralSpecsItemValueEN,
                e.MasterVehicleGeneralSpecsValue.GeneralSpecsItemValueAR,
                e.MasterVehicleGeneralSpecsValue.GeneralSpecsItemID,
                e.MasterVehicleGeneralSpec.GeneralSpecsCategory,
                e.ListSpecID,
                Status = _contextVehicle.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),






            });
            //.GroupBy(e => e.Category).Select(e => new
            //{
            //    Category= e.Key,
            //    model.ListNameID,

            //    GeneralSpecsName =  e.Select(x=>new { x.GeneralSpecsNameEN, x.GeneralSpecsNameAR,x.GeneralSpecID,x.GeneralSpecsCategory }).FirstOrDefault(),
            //    GeneralSpecsValuees = e.Select(x => new { x.GeneralSpecsItemValueEN, x.GeneralSpecsItemValueAR,x.GeneralSpecsItemID,x.Status,x.ListSpecID }).ToList(),

            //});


            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }


    }
}
