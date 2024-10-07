
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MigraDoc.DocumentObjectModel;
using OfficeOpenXml;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Core.Helpers;
using SparePartsModule.Domain.Context;
using SparePartsModule.Domain.Models.Library;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.SupplierDelivery;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers;
using SparePartsModule.Infrastructure.ViewModels.Response;
using SparePartsModule.Interface;
using SparePartsModule.Interface.Library;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Settings = SparePartsModule.Infrastructure.ViewModels.Dtos.Enums.Settings;

namespace SparePartsModule.Core.Library
{
    public class LibrarySuppliersService : ILibrarySuppliersService
    {
        private readonly IConfiguration _config;
        private readonly SparePartsModuleContext _context;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilties;
        private readonly ILookupService _lookupService;
        //  List<string> errors = new List<string>();
        //  List<string> locations = new List<string> { "Local", "Gulf", "Internaional" };
        public LibrarySuppliersService(SparePartsModuleContext context, IConfiguration config, FileHelper fileHelper,
            UtilitiesHelper utilitiesHelper, ILookupService lookupService)
        {

            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;
            _utilties = utilitiesHelper;
            _lookupService = lookupService;
        }

        public async ValueTask<ApiResponseModel> AddSupplier(AddSupplierModel model, int userId)
        {

            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            //bool isLetter = _utilties.IsLettersOnly(model.SupplierName);
            //if (!isLetter)
            //{
            //    throw new ManagerProcessException("000035");
            //}
            var supplierExists = _context.GettingSPSupplier.Where(e => e.Cancelled == false && e.SupplierName == model.SupplierName).FirstOrDefault();
            if (supplierExists != null)
            {
                if (supplierExists.Status == (int)Status.Inactive)
                {
                    throw new ManagerProcessException("000034");
                }
                throw new ManagerProcessException("000006");
            }


            if (model.OriginalCountry != null)
            {
                var isValid = _context.MasterLookup.Where(e => e.LookupID == model.OriginalCountry && e.LookupTypeID == (int)LookupTypes.Country).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000008");
                }
            }
            if (model.SupplierCurrency != null)
            {
                var isValid = _context.MasterLookup.Where(e => e.LookupID == model.SupplierCurrency && e.LookupTypeID == (int)LookupTypes.Currency).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000009");
                }
            }

            //if (model.DeliveryMethod != null)
            //{
            //    var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.DeliveryMethod && e.LookupTypeID == (int)LookupTypes.DeliveryMethod).Any();
            //    if (!isValid)
            //    {
            //        throw new ManagerProcessException("000011");
            //    }
            //}
            if (model.PaymentMethod != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.PaymentMethod && e.LookupTypeID == (int)LookupTypes.PaymentMethod).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000010");
                }
            }
            if (model.SupplierLocalInternational != null)
            {

                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.SupplierLocalInternational && e.LookupTypeID == (int)LookupTypes.SupplierRegion).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000017");
                }
            }
            if (model.SupplierParent != null)
            {
                var isValidSupplier = _context.GettingSPSupplier.Where(e => e.SupplierID == model.SupplierParent).Any();
                if (!isValidSupplier)
                {
                    throw new ManagerProcessException("000018");
                }
            }

            if (model.Discount != null)
            {
                var isValid = _context.MasterLookup.Where(e => e.LookupID == model.Discount && e.LookupTypeID == (int)LookupTypes.DiscountType).Any();

                if (!isValid)
                {
                    throw new ManagerProcessException("000020");
                }
            }
            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            if (model.SupplierTaxType != null)
            {
                var isValid = _context.MasterLookup.Where(e => e.LookupID == model.SupplierTaxType && e.LookupTypeID == (int)LookupTypes.TaxType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000052");
                }

            }
            var newId = ((int?)_context.GettingSPSupplier.Max(inv => (int?)inv.SupplierID) ?? 0) + 1;
            var newSupplierNo = ((int?)_context.GettingSPSupplier.Max(inv => (int?)inv.SupplierNo) ?? 0) + 1;
            var supplier = new GettingSPSupplier
            {
                SupplierID = newId,
                SupplierNo = newSupplierNo,
                SupplierName = model.SupplierName,
                SupplierAbbCode = model.SupplierAbbCode,
                SupplierOriginCountry = model.OriginalCountry,
                SupplierAccountNo = model.SupplierAccountNo,
                SupplierCurrency = model.SupplierCurrency,
                SupplierAgreementStartDate = model.AgreementStart,
                SupplierAgreementEndDate = model.AgreementEnd,
                SupplierPaymentMethod = model.PaymentMethod,
                //SupplierDeliveryMethod = model.DeliveryMethod,
                SupplierLocalInternational = model.SupplierLocalInternational,
                SupplierParent = model.SupplierParent,
                SupplierTaxType = model.SupplierTaxType,
                SupplierDiscount = model.Discount,
                SupplierComments = model.Comments,
                SupplierGroup = (int)Infrastructure.ViewModels.Dtos.Enums.Settings.Group,
                SupplierAverageLeadTime = (int)Infrastructure.ViewModels.Dtos.Enums.Settings.verageLeadTime,
                Cancelled = false,
                TMCSupplier = model.TMCSupplier,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay,
                CostFactor = model.CostFactor,
                PriceFactor = model.PriceFactor,
            };
            if (model.SupplierAttachment != null)
            {
                supplier.SupplierAttachments = null;
                foreach (var image in model.SupplierAttachment)
                {
                    var uploadResult = await _FileHelper.WriteFile(image, "Suppliers");
                    supplier.SupplierAttachments += (supplier.SupplierAttachments == null ? "" : ",") + uploadResult.ReturnUrl;
                }

                //var uploadResult = await _FileHelper.WriteFile(model.SupplierAttachment, "Suppliers");
                //supplier.SupplierAttachments = uploadResult.ReturnUrl;
            }
            await _context.GettingSPSupplier.AddAsync(supplier);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }


        public async ValueTask<ApiResponseModel> AddSupplierDeliveryMethod(AddSupplierDeliveryModel model, int userId)
        {

            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var supplierExists = _context.GettingSPSupplier.Where(e => e.Cancelled == false && e.SupplierID == model.SupplierID).FirstOrDefault();
            if (supplierExists == null)
            {
                throw new ManagerProcessException("000109");
            }
            var SupplierDeliveryMethodExists = _context.GettingSPSupplierDeliveryMethod.Where(e => e.Cancelled == false && e.SupplierDeliveryMethod == model.DeliveryMethodID && e.SupplierID == model.SupplierID).FirstOrDefault();
            if (SupplierDeliveryMethodExists != null)
            {
                throw new ManagerProcessException("000108");
            }

            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

            isValids = _context.MasterLookup.Where(e => e.LookupID == model.DeliveryMethodID && e.LookupTypeID == (int)LookupTypes.DeliveryMethod).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000110");
            }

            var newId = ((int?)_context.GettingSPSupplierDeliveryMethod.Max(inv => (int?)inv.SupplierDeliveryMethodID) ?? 0) + 1;

            var SupplierDeliveryMethod = new GettingSPSupplierDeliveryMethod
            {
                SupplierDeliveryMethodID = newId,
                SupplierDeliveryMethod = model.DeliveryMethodID,
                SupplierID = model.SupplierID,
                SupplierAverageLeadTime = (int)Infrastructure.ViewModels.Dtos.Enums.Settings.verageLeadTime,
                Cancelled = false,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            await _context.GettingSPSupplierDeliveryMethod.AddAsync(SupplierDeliveryMethod);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditSupplierDeliveryMethod(EditSupplierDeliveryModel model, int userId)
        {

            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var deliveryMethod = _context.GettingSPSupplierDeliveryMethod.Where(e => e.SupplierDeliveryMethodID == model.SupplierDeliveryMethodID && e.Cancelled == false).FirstOrDefault();
            if (deliveryMethod == null)
            {
                throw new ManagerProcessException("000110");
            }
            var supplierExists = _context.GettingSPSupplier.Where(e => e.Cancelled == false && e.SupplierID == model.SupplierID).Any();
            if (!supplierExists)
            {
                throw new ManagerProcessException("000109");
            }
            var SupplierDeliveryMethodExists = _context.GettingSPSupplierDeliveryMethod
                .Where(e => e.Cancelled == false &&
                e.SupplierDeliveryMethodID != model.SupplierDeliveryMethodID &&
                e.SupplierDeliveryMethod == model.DeliveryMethodID &&
                e.SupplierID == model.SupplierID).FirstOrDefault();
            if (SupplierDeliveryMethodExists != null)
            {
                throw new ManagerProcessException("000108");
            }

            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

            isValids = _context.MasterLookup.Where(e => e.LookupID == model.DeliveryMethodID && e.LookupTypeID == (int)LookupTypes.DeliveryMethod).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000110");
            }

            var newId = ((int?)_context.GettingSPSupplierDeliveryMethod.Max(inv => (int?)inv.SupplierDeliveryMethodID) ?? 0) + 1;


            deliveryMethod.SupplierDeliveryMethod = model.DeliveryMethodID;
            deliveryMethod.SupplierID = model.SupplierID;
            // deliveryMethod.SupplierAverageLeadTime = (int)Settings.verageLeadTime;

            deliveryMethod.Status = model.Status;
            deliveryMethod.ModUser = userId;
            deliveryMethod.ModDate = DateTime.Now;
            deliveryMethod.ModTime = DateTime.Now.TimeOfDay;


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteSupplierDeliveryMethod(int SupplierDeliveryMethodID, int userId)
        {


            var deliveryMethod = _context.GettingSPSupplierDeliveryMethod.Where(e => e.SupplierDeliveryMethodID == SupplierDeliveryMethodID).FirstOrDefault();
            if (deliveryMethod == null)
            {
                throw new ManagerProcessException("000110");
            }
            if (deliveryMethod.Cancelled == true)
            {
                throw new ManagerProcessException("000071");
            }

            deliveryMethod.Cancelled = true;
            deliveryMethod.Status = (int)Status.Deleted;
            deliveryMethod.ModUser = userId;
            deliveryMethod.CancelDate = DateTime.Now;
            deliveryMethod.ModDate = DateTime.Now;
            deliveryMethod.ModTime = DateTime.Now.TimeOfDay;


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditSupplier(EditSupplierModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!string.IsNullOrEmpty(errorCodes))
            {
                throw new ManagerProcessException(errorCodes);
            }

            var supplier = _context.GettingSPSupplier.Where(e => e.SupplierID == model.SupplierID).FirstOrDefault();
            if (supplier == null)
            {

                throw new ManagerProcessException("000007");
            }
            if (model.SupplierName != null)
            {
                //bool isLetter = _utilties.IsLettersOnly(model.SupplierName);
                //if (!isLetter)
                //{
                //    throw new ManagerProcessException("000035");
                //}
                var supplierExists = _context.GettingSPSupplier.Where(e => e.Cancelled == false && e.SupplierID != model.SupplierID && e.SupplierName == model.SupplierName).Any();
                if (supplierExists)
                {

                    throw new ManagerProcessException("000006");
                }
                supplier.SupplierName = model.SupplierName;
            }
            // if (model.SupplierAbbCode != null)
            //   {
            supplier.SupplierAbbCode = model.SupplierAbbCode;
            // }
            if (model.OriginalCountry != null)
            {
                var isValid = _context.MasterLookup.Where(e => e.LookupID == model.OriginalCountry && e.LookupTypeID == (int)LookupTypes.Country).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000008");
                }
                supplier.SupplierOriginCountry = model.OriginalCountry;
            }
            if (model.SupplierAccountNo != null)
            {
                supplier.SupplierAccountNo = model.SupplierAccountNo;
            }
            if (model.SupplierCurrency != null)
            {
                var isValid = _context.MasterLookup.Where(e => e.LookupID == model.SupplierCurrency && e.LookupTypeID == (int)LookupTypes.Currency).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000009");
                }
                supplier.SupplierCurrency = model.SupplierCurrency;
            }
            if (model.AgreementStart != null)
            {
                supplier.SupplierAgreementStartDate = model.AgreementStart;
            }
            if (model.AgreementEnd != null)
            {
                supplier.SupplierAgreementEndDate = model.AgreementEnd;
            }
            if (model.PaymentMethod != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.PaymentMethod && e.LookupTypeID == (int)LookupTypes.PaymentMethod).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000010");
                }
                supplier.SupplierPaymentMethod = model.PaymentMethod;
            }
            //if (model.DeliveryMethod != null)
            //{
            //    var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.DeliveryMethod && e.LookupTypeID == (int)LookupTypes.DeliveryMethod).Any();
            //    if (!isValid)
            //    {
            //        throw new ManagerProcessException("000011");
            //    }
            //    supplier.SupplierDeliveryMethod = model.DeliveryMethod;
            //}

            if (model.SupplierLocalInternational != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.SupplierLocalInternational && e.LookupTypeID == (int)LookupTypes.SupplierRegion).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000017");
                }
                supplier.SupplierLocalInternational = model.SupplierLocalInternational;
            }
            //if (model.SupplierParent != null)
            //{
            supplier.SupplierParent = model.SupplierParent;
            //}
            if (model.SupplierTaxType != null)
            {
                var isValid = _context.MasterLookup.Where(e => e.LookupID == model.SupplierTaxType && e.LookupTypeID == (int)LookupTypes.TaxType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000052");
                }
                supplier.SupplierTaxType = model.SupplierTaxType;
            }
            if (model.Discount != null)
            {
                supplier.SupplierDiscount = model.Discount;
            }
            if (model.SupplierParent != null)
            {
                supplier.SupplierParent = model.SupplierParent;
            }
            if (model.Comments != null)
            {
                supplier.SupplierComments = model.Comments;
            }
            if (model.Status != null)
            {
                supplier.Status = model.Status;
            }
            if (model.SupplierParent != null)
            {
                var isValidSupplier = _context.GettingSPSupplier.Where(e => e.SupplierID == model.SupplierParent).Any();
                if (!isValidSupplier)
                {
                    throw new ManagerProcessException("000018");
                }
            }

            if (model.Discount != null)
            {
                var isValid = _context.MasterLookup.Where(e => e.LookupID == model.Discount && e.LookupTypeID == (int)LookupTypes.DiscountType).Any();

                if (!isValid)
                {
                    throw new ManagerProcessException("000020");
                }
            }
            supplier.TMCSupplier = model.TMCSupplier;
            supplier.ModUser = userId;
            supplier.ModDate = DateTime.Now;
            supplier.ModTime = DateTime.Now.TimeOfDay;
            supplier.CostFactor = model.CostFactor;
            supplier.PriceFactor = model.PriceFactor;

            if (model.SupplierAttachment != null)
            {
                supplier.SupplierAttachments = null;
                foreach (var image in model.SupplierAttachment)
                {
                    var uploadResult = await _FileHelper.WriteFile(image, "Suppliers");
                    supplier.SupplierAttachments += (supplier.SupplierAttachments == null ? "" : ",") + uploadResult.ReturnUrl;
                }
                //var uploadResult = await _FileHelper.WriteFile(model.SupplierAttachment, "Suppliers");
                //supplier.SupplierAttachments = uploadResult.ReturnUrl;
            }
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteSupplier(int SupplierID, int userId)
        {


            var supplier = _context.GettingSPSupplier.Where(e => e.SupplierID == SupplierID).FirstOrDefault();
            if (supplier == null)
            {

                throw new ManagerProcessException("000007");
            }

            supplier.CancelDate = DateTime.Now;
            supplier.Cancelled = true;
            supplier.ModUser = userId;
            supplier.ModDate = DateTime.Now;
            supplier.ModTime = DateTime.Now.TimeOfDay;


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteSuppliers(string SupplierIDs, int userId)
        {


            var afftedRows = _context.GettingSPSupplier.Where(e => ("," + SupplierIDs + ",").Contains("," + e.SupplierID + ","))
                .ExecuteUpdate(e =>
                e.SetProperty(x => x.Status, (int)Status.Deleted)
                .SetProperty(x => x.CancelDate, DateTime.Now)
                .SetProperty(x => x.Cancelled, true)
                .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                .SetProperty(x => x.ModUser, userId)


                );
            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000007");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> SuppliersChangeStatus(string SupplierIDs, int StatusId, int userId)
        {
            var isValids = _context.MasterLookup.Where(e => e.LookupID == StatusId && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

            var afftedRows = _context.GettingSPSupplier.Where(e => ("," + SupplierIDs + ",").Contains("," + e.SupplierID + ","))
                .ExecuteUpdate(e =>
                e.SetProperty(x => x.Status, StatusId)
                .SetProperty(x => x.ModDate, DateTime.Now)
                .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                .SetProperty(x => x.ModUser, userId)


                );
            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000007");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetSuppliers(GetSupplierModel model, PaginationModel paginationPostModel)
        {

            var data = _context.GettingSPSupplier.Where(e => e.Cancelled == false &&
            (model.Search == null || e.SupplierName.Contains(model.Search)) &&
             (model.Code == null || e.SupplierAbbCode.Contains(model.Code)) &&
             (model.SupplierLocalInternational == null || ("," + model.SupplierLocalInternational + ",").Contains("," + e.SupplierLocalInternational + ",")) &&
            (model.SupplierDeliveryMethod == null || e.GettingSPSupplierDeliveryMethod.Where(x => x.Cancelled == false && model.SupplierDeliveryMethod.Contains(x.SupplierDeliveryMethod.ToString())).Any()) &&
             (model.Status == null || e.Status == model.Status) &&
              (model.IsMainSupplier == null || (e.SupplierParent == null && model.IsMainSupplier == true) || (e.SupplierParent != null && model.IsMainSupplier == false))
            ).Select(e => new
            {
                e.SupplierID,
                e.SupplierName,
                e.SupplierAbbCode,
                e.SupplierAccountNo,
                e.SupplierAgreementEndDate,
                e.SupplierNo,
                e.SupplierAgreementStartDate,
                e.SupplierAverageLeadTime,
                e.TMCSupplier,
                e.CostFactor,
                e.PriceFactor,
               
                // e.SupplierDiscount,
                e.SupplierComments,
                GettingSPSupplierDeliveryMethod = _context.GettingSPSupplierDeliveryMethod.Where(x => x.Status == (int)Status.Active && x.Cancelled == false && x.SupplierID == e.SupplierID)
                .Select(x => new
                {
                    x.SupplierDeliveryMethodID,
                    x.SupplierAverageLeadTime,
                    SupplierDeliveryMethod = _context.MasterSPLookup.Where(y => y.LookupID == x.SupplierDeliveryMethod)
                    .Select(y => new { y.LookupID, y.LookupName }).FirstOrDefault()
                }).ToList(),
                SupplierDiscount = _context.MasterLookup.Where(x => x.LookupID == e.SupplierDiscount).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),

                SupplierTaxType = _context.MasterLookup.Where(x => x.LookupID == e.SupplierTaxType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                SupplierLocalInternational = _context.MasterSPLookup.Where(x => x.LookupID == e.SupplierLocalInternational).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                SupplierOriginCountry = _context.MasterLookup.Where(x => x.LookupID == e.SupplierOriginCountry).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                SupplierCurrency = _context.MasterLookup.Where(x => x.LookupID == e.SupplierCurrency).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                CurrencyRate =  _context.MasterLookup.
                Where (currency=>currency.LookupID == e.SupplierCurrency)
                .Select(currency=> new { CurrencyRate= currency.LookupValue, CurrencyDate=DateTime.Now } ).FirstOrDefault(),
                SupplierPaymentMethodVehMaster = _context.MasterSPLookup.Where(x => x.LookupID == e.SupplierPaymentMethod).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                SupplierGroup = _context.MasterLookup.Where(x => x.LookupID == e.SupplierGroup).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                // SupplierAttachments =e.SupplierAttachments==null?null: _config["Settings:BaseUrl"] + e.SupplierAttachments ?? "",
                SupplierAttachments = e.SupplierAttachments == null ? null : (_config["Settings:BaseUrl"] + e.SupplierAttachments.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None),

                e.EnterDate,
                e.ModDate,
                SupplierParent = _context.GettingSPSupplier.Where(x => x.SupplierID == e.SupplierParent).Select(x => new { x.SupplierID, x.SupplierName }).FirstOrDefault(),
            });
            if (model.Sort == 1)
            {
                data = data.OrderBy(e => e.SupplierID);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.SupplierName);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.SupplierName);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.SupplierLocalInternational.LookupName);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.SupplierLocalInternational.LookupName);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.SupplierOriginCountry.LookupName);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.SupplierOriginCountry.LookupName);
            }
            //if (model.Sort == 8)
            //{
            //    data = data.OrderBy(e => e.SupplierDeliveryMethod.LookupName);
            //}
            //if (model.Sort == 9)
            //{
            //    data = data.OrderByDescending(e => e.SupplierDeliveryMethod.LookupName);
            //}
            if (model.Sort == 10)
            {
                data = data.OrderBy(e => e.EnterDate);
            }
            if (model.Sort == 11)
            {
                data = data.OrderByDescending(e => e.EnterDate);
            }
            if (model.Sort == 12)
            {
                data = data.OrderBy(e => e.Status.LookupName);
            }
            if (model.Sort == 13)
            {
                data = data.OrderByDescending(e => e.Status.LookupName);
            }
            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);
            return result;


        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetSupplierMasterItems(GetSupplierMasterItemsModel model, PaginationModel paginationPostModel)
        {
            return null;

            //var data = _context.MasterSPGeneralSupplierItems.Where(e => e.Cancelled == false && e.ItemSupID == model.SupplierId &&
            //(model.ItemSupplierNumber == null || e.ItemSupNo.Contains(model.ItemSupplierNumber))

            //).Select(e => new
            //{
            //    Id = e.MasterFileId,
            //    part_no = e.ItemSupNo,
            //    desc = e.ItemSupDesc,
            //    Sub = e.ItemSubSubstitute,
            //    SubPart = e.ItemSupSubstituteItem,
            //    Price = e.ItemSupPrice,
            //    wieght = e.ItemSupWeight,
            //    Length = e.ItemSupLength,
            //    Width = e.ItemSupWidth,
            //    Height = e.ItemSupHeight,
            //    Size = e.ItemSupSizeM3,
            //    Lexus = e.ItemSupLexus,
            //    start = e.ItemSupStart,
            //    end = e.ItemSupEnd,
            //    fl_id = e.ItemSupFI_Id,
            //    pro = e.ItemSupPro,
            //    na1 = e.ItemSupFlag,
            //    na2 = e.ItemSupna2,
            //    upq = e.ItemSupUPQ,
            //    MaxOrd = e.ItemSupMaxOrd,
            //    MinOrd = e.ItemSupMinOrd,
            //    Orgin = e.ItemSupOrigin,




            //});

            //var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            //return result;


        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetSupplierMasterItemsV2(GetSupplierMasterItemsModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPGeneralSupplierItems.Where(e => e.Cancelled == false && e.ItemSupID == model.SupplierId &&
            (model.ItemSupplierNumber == null || e.ItemSupNo.Contains(model.ItemSupplierNumber))

            ).Select(item => new
            {
                id = item.ItemSupID,
                PartNo = item.ItemSupNo,
                partNowithdash = item.partNowithdash,
                fl_id = item.ItemSupFI_Id,
                Non_Usable = item.Non_Usable,
                Non_Usage = item.Non_Usage,
                UsagePostponed = item.UsagePostponed,
                Allotment = item.Allotment,
                TMCPartsCatalogueError = item.TMCPartsCatalogueError,
                ExportDestinationCode = item.ExportDestinationCode,
                SpecialStorageFollowParts = item.SpecialStorageFollowParts,
                ManufactureDiscontinuation = item.ManufactureDiscontinuation,
                NoService = item.NoService,
                Discontinuation = item.Discontinuation,
                All_time_buypartcode = item.All_time_buypartcode,
                OrderUnitMax = item.ItemSupMaxOrd,
                SubstitutionCode = item.ItemSupSubstituteType,
                ItemSupSubstituteItem = item.ItemSupSubstituteItem,
                QuantityinUse = item.QuantityinUse,
                UnitFOBPrice = item.UnitFOBPrice,
                Corrunitprice = item.Corrunitprice,
                PriceClass = item.PriceClass,
                ProductCode = item.ItemSupPro,
                PartName = item.ItemSupDesc,
                ItemSupStart = item.ItemSupStart,
                ItemSupEnd = item.ItemSupEnd,
                BinCode = item.ItemSupPrice,
                QUP = item.ItemSupUPQ,
                Qty_per_TMC = item.Qty_per_TMC,
                item.DistributionPackage1,
                item.DistributionPackage2,
                item.DistributionPackage3,
                Length = item.ItemSupLength,
                Width = item.ItemSupWidth,
                Height = item.ItemSupHeight,
                Size = item.ItemSupSizeM3,
                Weight = item.ItemSupWeight,
                item.Parts,
                item.TMCStockCode,
                item.ItemSupLexus,
                item.MultiplePNCCode,
                item.PNC1,
                item.PNC2,
                item.TMCPartsCenterCode,
                MinimumOrderUnit = item.ItemSupMinOrd,
                item.Filler,
                Origin = item.ItemSupOrigin,




            });

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }

        public async ValueTask<object> GetSupplierDeliveryMethod(int SupplierId)
        {
            SupplierResopnseModel supplierResopnse = new SupplierResopnseModel();

            var data = _context.GettingSPSupplierDeliveryMethod.Where(e => e.Cancelled == false && e.SupplierID == SupplierId)
                .Select(e => new
                {

                    e.SupplierDeliveryMethodID,
                    e.SupplierDeliveryMethod,
                    DeliveryMethodName = _context.MasterSPLookup.Where(x => x.LookupID == e.SupplierDeliveryMethod).Select(x => new { x.LookupName }).FirstOrDefault(),
                    Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                    e.SupplierAverageLeadTime,

                });
            supplierResopnse.Supplier = _context.GettingSPSupplier
                   .Where(x => x.SupplierID == SupplierId && x.Cancelled == false)
                   .Select(x => new { x.SupplierID, x.SupplierName })
                   .FirstOrDefault();
            supplierResopnse.DeliveryMethod = data;


            return supplierResopnse;
        }


        public async ValueTask<PaginationDatabaseResponseDto<object>> GetSupplierDeliveryMethod1(int SupplierId, PaginationModel paginationPostModel)
        {
            SupplierResopnseModel supplierResopnse = new SupplierResopnseModel();
            var data = _context.GettingSPSupplierDeliveryMethod.Where(e => e.Cancelled == false && e.SupplierID == SupplierId)
                .Select(e => new
                {
                    e.SupplierDeliveryMethodID,
                    e.SupplierDeliveryMethod,
                    DeliveryMethodName = _context.MasterSPLookup.Where(x => x.LookupID == e.SupplierDeliveryMethod).Select(x => new { x.LookupName }).FirstOrDefault(),
                    Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                    e.SupplierAverageLeadTime,

                });
            supplierResopnse.Supplier = _context.GettingSPSupplier
                   .Where(x => x.SupplierID == SupplierId && x.Cancelled == false)
                   .Select(x => new { x.SupplierID, x.SupplierName })
                   .FirstOrDefault();
            supplierResopnse.DeliveryMethod = data;

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);
            return result;
        }
        public async ValueTask<ApiResponseModel> ImportSupplier(UpdateFileModel2 model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "UploadSuppliers");
            var ExcelFileData = ReadExcelFile(tempFile.ReturnUrl);

            int UploaddedItemsCount = 0;
            int DuplicatedItemsCount = 0;
            int FailedtoUploadItemsCount = 0;
            int InvalidParentCounts = 0;
            int invalidDiscountCounts = 0;
            var newId = ((int?)_context.GettingSPSupplier.Max(inv => (int?)inv.SupplierID) ?? 0);
            foreach (var item in ExcelFileData)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.SupplierName))
                    {
                        continue;
                    }

                    var supplierExists = _context.GettingSPSupplier
                                    .Where(e => e.Cancelled == false && (e.SupplierName == item.SupplierName || e.SupplierAbbCode == item.SupplierAbbreviation)).FirstOrDefault();
                    if (supplierExists != null)
                    {
                        DuplicatedItemsCount++;
                        continue;
                    }

                    int SupplierLocalInternational = _lookupService.GeSpLookup(userId, item.Location, (int)LookupTypes.SupplierRegion);
                    int OriginalCountry = _lookupService.GetMasterLookup(userId, item.OriginalCountry, (int)LookupTypes.Country);
                    int? SupplierParent = null;
                    if (!string.IsNullOrEmpty(item.SupplierParent))
                    {
                        SupplierParent = _context.GettingSPSupplier.Where(e => e.SupplierName == item.SupplierParent).Select(e => e.SupplierID).FirstOrDefault();
                        if (SupplierParent == null || SupplierParent == 0)
                        {
                            InvalidParentCounts++;
                            continue;
                        }
                    }

                    int SupplierTaxType = _lookupService.GetMasterLookup(userId, item.TaxType, (int)LookupTypes.TaxType);
                    int SupplierDiscType = _lookupService.GetMasterLookup(userId, item.Discount, (int)LookupTypes.DiscountType);
                    newId++;

                    var supplier = new GettingSPSupplier
                    {
                        SupplierID = newId,
                        SupplierNo = newId,
                        SupplierName = item.SupplierName,
                        SupplierAbbCode = item.SupplierAbbreviation,
                        SupplierOriginCountry = OriginalCountry,
                        SupplierLocalInternational = SupplierLocalInternational,
                        SupplierParent = SupplierParent,
                        SupplierTaxType = SupplierTaxType,
                        SupplierDiscount = SupplierDiscType,
                        SupplierComments = item.Remarks,
                        SupplierGroup = (int)Settings.Group,
                        SupplierAverageLeadTime = (int)Settings.verageLeadTime,
                        TMCSupplier = item.IsTMC == "1" ? true : false,
                        CostFactor = decimal.Parse(item.CostFactor),
                        PriceFactor = decimal.Parse(item.PriceFactor),
                        Cancelled = false,
                        Status = (int)Status.Active,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };
                    if (model.Save)
                    {
                        await _context.GettingSPSupplier.AddAsync(supplier);
                        var result = await _context.SaveChangesAsync();
                    }
                    UploaddedItemsCount++;
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
                InvalidParentCounts,
                DuplicatedItemsCount,
                invalidDiscountCounts
            };

            return response;

            throw new ManagerProcessException("000008");
        }
        private List<SupplierData> ReadExcelFile(string filePath)
        {
            var dataList = new List<SupplierData>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++) // Assuming the first row is the header
                {
                    var data = new SupplierData
                    {
                        SupplierName = worksheet.Cells[row, 1].Text,
                        SupplierAbbreviation = worksheet.Cells[row, 2].Text,
                        Location = worksheet.Cells[row, 3].Text,
                        OriginalCountry = worksheet.Cells[row, 4].Text,
                        SupplierParent = worksheet.Cells[row, 5].Text,
                        Discount = worksheet.Cells[row, 6].Text,
                        TaxType = worksheet.Cells[row, 7].Text,
                        Remarks = worksheet.Cells[row, 8].Text,
                        IsTMC = worksheet.Cells[row, 9].Text,
                        CostFactor = worksheet.Cells[row, 10].Text,
                        PriceFactor = worksheet.Cells[row, 11].Text,
                    };

                    dataList.Add(data);
                }
            }

            return dataList;
        }
    }
}

