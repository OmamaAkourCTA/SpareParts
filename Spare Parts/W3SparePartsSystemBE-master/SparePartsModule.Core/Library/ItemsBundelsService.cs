using Microsoft.Extensions.Configuration;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Core.Helpers;
using SparePartsModule.Domain.Context;
using SparePartsModule.Domain.Models.Library;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items.Substitute;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.ItemsBundels;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Category;
using SparePartsModule.Infrastructure.ViewModels.Models;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.InkML;
using SparePartsModule.Infrastructure.ViewModels.Response;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.IdentityModel.Logging;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.IdentityModel.Tokens;

namespace SparePartsModule.Core.Library
{
    public class ItemsBundelsService : IItemsBundelsService
    {
        private readonly IConfiguration _config;
        private readonly SparePartsModuleContext _context;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilties;
        List<string> errors = new List<string>();

        public ItemsBundelsService(SparePartsModuleContext context, IConfiguration config, FileHelper fileHelper, UtilitiesHelper utilties)
        {

            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;
            _utilties = utilties;
        }
        public async ValueTask<ApiResponseModel> AddBundle(AddBundleModel model, int userId)
        {

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            bool isLetter = _utilties.IsLettersOnly(model.BundleNameEn);
            if (!isLetter)
            {
                throw new ManagerProcessException("000093");
            }
            var isArabic = _utilties.IsArabic(model.BundleNameEn);
            if (isArabic)
            {
                throw new ManagerProcessException("000093");
            }

            isArabic = _utilties.IsArabic(model.BundleCode);
            if (isArabic)
            {
                throw new ManagerProcessException("000092");
            }
            //isArabic = _utilties.HasEnglishCharacters(model.BundleNameAr);
            //if (isArabic)
            //{
            //    throw new ManagerProcessException("000094");
            //}
            var exist = _context.MasterSPItemBundle1.Where(e => e.BundleCode == model.BundleCode && e.Cancelled == false).FirstOrDefault();
            if (exist != null)
            {
                if (exist.Status == (int)Status.Inactive)
                {

                    throw new ManagerProcessException("000096");
                }
                throw new ManagerProcessException("000095");
            }
            exist = _context.MasterSPItemBundle1.Where(e => e.BundleNameEn == model.BundleNameEn && e.Cancelled == false).FirstOrDefault();
            if (exist != null)
            {
                if (exist.Status == (int)Status.Inactive)
                {

                    throw new ManagerProcessException("000098");
                }
                throw new ManagerProcessException("000097");
            }
            //exist = _context.MasterSPItemBundle1.Where(e => e.BundleNameAr == model.BundleNameAr && e.Cancelled == false).FirstOrDefault();
            //if (exist != null)
            //{
            //    if (exist.Status == (int)Status.Inactive)
            //    {

            //        throw new ManagerProcessException("000100");
            //    }
            //    throw new ManagerProcessException("000099");
            //}


            if (model.BundlePrice < model.BundleCost)
            {
                throw new ManagerProcessException("000088");
            }
            if (model.BundleDiscountPercentage < 0 || model.BundleDiscountPercentage > 100)
            {
                throw new ManagerProcessException("000089");
            }
            var discount = Math.Round((
                (model.BundlePrice / 100)
                * model.BundleDiscountPercentage
                ), 3);
            if (discount != model.BundleDiscountAmount)
            {
                throw new ManagerProcessException("000090");
            }
            if (model.BundlePriceAfterDiscount != model.BundlePrice - model.BundleDiscountAmount)
            {
                throw new ManagerProcessException("000091");
            }


            //var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            //if (!isValids)
            //{
            //    throw new ManagerProcessException("000032");
            //}
            var newId = ((int?)_context.MasterSPItemBundle1.Max(inv => (int?)inv.BundleID) ?? 0) + 1;
            var item = new MasterSPItemBundle1
            {
                BundleID = newId,
                BundleCode = model.BundleCode,
                //  BundleNameAr=model.BundleNameAr,
                BundleNameEn = model.BundleNameEn,
                BundlePrice = model.BundlePrice,
                BundleCost = model.BundleCost,
                BundleDiscountPercentage = model.BundleDiscountPercentage,
                BundleDiscountAmount = model.BundleDiscountAmount,
                BundleComments = model.BundleComments,
                BundleCreationDate = model.BundleCreationDate,
                BundleDesc = model.BundleDesc,
                BundleGroup = (int)Settings.Group,
                BundlePriceAfterDiscount = model.BundlePriceAfterDiscount,
                Cancelled = false,
                Status = (int)BundelStatus.Draft,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay,
                BundleValidFrom = model.BundleValidFrom,
                BundleValidTo = model.BundleValidTo,
            };


            await _context.MasterSPItemBundle1.AddAsync(item);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = item;

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetBundles(GetBundlesModel model, PaginationModel paginationPostModel)
        {

            _context.MasterSPItemBundle1.Where(e => e.Cancelled == false && e.BundleValidTo.Value.Date < DateTime.Now.Date && e.Status != (int)BundelStatus.Expired)
                    .ExecuteUpdate(e => e.SetProperty(x => x.Status, (int)BundelStatus.Expired));
           

            // Get the bundle items based on the model.BundleItemID if it is provided
            var bundleItems = model.BundleItemID != null && model.BundleItemID > 0
                ? _context.MasterSPItemBundle2
                    .Where(e => e.Cancelled == false && e.ItemID == model.BundleItemID)
                    .Select(e => e.BundleID)
                    .ToList()
                : null;
            var data = _context.MasterSPItemBundle1.Where(e => e.Cancelled == false &&
            (bundleItems == null || bundleItems.Contains(e.BundleID)) &&
            (model.BundleName == null || e.BundleNameAr.Contains(model.BundleName) || e.BundleNameEn.Contains(model.BundleName)) &&
            (model.BundleCode == null || e.BundleCode.Contains(model.BundleCode)) &&
             (model.BundleCreationDate == null || e.BundleCreationDate.Date == model.BundleCreationDate.Value.Date) &&
             (model.BundlePriceAfterDiscount == null || e.BundlePriceAfterDiscount == model.BundlePriceAfterDiscount) &&
            (model.Status == null || e.Status == model.Status)
            ).Select(e => new
            {
                e.BundleID,
                e.BundleNameEn,
                e.BundleNameAr,
                e.BundleCode,
                e.BundleDesc,
                e.BundleCreationDate,
                e.BundlePrice,
                e.BundleCost,
                e.BundlePriceAfterDiscount,
                e.BundleDiscountPercentage,
                e.BundleDiscountAmount,
                e.BundleComments,
                e.BundleTaxPercentage,
                e.BundleTaxAmount,
                e.BundleGrandAmount,
                Status = _context.MasterSPLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,
                e.EnterTime,
                e.ModDate,
                e.BundleValidFrom,
                
                e.BundleGroup,
                e.BundleValidTo,
            });

            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.BundleCode);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.BundleCode);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.BundleNameEn);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.BundleNameEn);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.BundlePrice);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.BundlePrice);
            }
            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.BundleDiscountAmount);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.BundleDiscountAmount);
            }
            if (model.Sort == 10)
            {
                data = data.OrderBy(e => e.BundlePriceAfterDiscount);
            }
            if (model.Sort == 11)
            {
                data = data.OrderByDescending(e => e.BundlePriceAfterDiscount);
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
        public async ValueTask<ApiResponseModel> DeleteBundle(string BundleID, int userId)
        {

            var afftedRows = _context.MasterSPItemBundle1.Where(e => ("," + BundleID + ",").Contains("," + e.BundleID + ","))
                  .ExecuteUpdate(e =>
                  e.SetProperty(x => x.Status, (int)Status.Deleted)
                  .SetProperty(x => x.CancelDate, DateTime.Now)
                  .SetProperty(x => x.Cancelled, true)
                  .SetProperty(x => x.ModTime, DateTime.Now.TimeOfDay)
                  .SetProperty(x => x.ModUser, userId)


                  );
            if (afftedRows < 1)
            {

                throw new ManagerProcessException("000101");
            }


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }

        public async ValueTask<ApiResponseModel> BundlesChangeStatus(string BundleIDs, int StatusId, int userId)
        {
            var isValids = _context.MasterSPLookup.Where(e => e.LookupID == StatusId && StatusId >= 23001 && StatusId <= 25999).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

            var afftedRows = _context.MasterSPItemBundle1.Where(e => ("," + BundleIDs + ",").Contains("," + e.BundleID + ","))
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
        public async ValueTask<ApiResponseModel> AddBundleItems(AddBundleItemsModel model, int userId)
        {

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var isValid = _context.MasterSPItemBundle1.Where(e => e.BundleID == model.BundleID && e.Cancelled == false).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000101");
            }
            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            var newId = ((int?)_context.MasterSPItemBundle2.Max(inv => (int?)inv.BundleLineID) ?? 0);
            var duplicate = model.Items.GroupBy(e => e.ItemID).Where(e => e.Count() > 1).Any();
            if (duplicate)
            {
                throw new ManagerProcessException("000104");
            }
            foreach (var bundelItem in model.Items)
            {
                var exist = _context.MasterSPItemBundle2.Where(e => e.BundleID == model.BundleID && e.ItemID == bundelItem.ItemID && e.Cancelled == false).Any();
                if (exist)
                {
                    throw new ManagerProcessException("000104");
                }
                isValid = _context.MasterSPItem.Where(e => e.ItemID == bundelItem.ItemID && e.Cancelled == false).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000045");
                }
                if (bundelItem.ItemQty <= 0)
                {
                    throw new ManagerProcessException("000102");
                }
                if (bundelItem.ItemPrice <= 0)
                {
                    throw new ManagerProcessException("000103");
                }
                if (bundelItem.ItemCost < 0)
                {
                    throw new ManagerProcessException("000110");

                }
                newId++;

                var item = new MasterSPItemBundle2
                {

                    BundleLineID = newId,
                    BundleID = model.BundleID,
                    ItemID = bundelItem.ItemID,
                    ItemCost = bundelItem.ItemCost,
                    ItemPrice = bundelItem.ItemPrice,
                    ItemQty = bundelItem.ItemQty,
                    TotalItemAmount = bundelItem.ItemQty * bundelItem.ItemPrice,
                    Cancelled = false,
                    Status = model.Status,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };
                await _context.MasterSPItemBundle2.AddAsync(item);
            }












            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteBundleItem(int BundleLineID, int userId)
        {


            var location = _context.MasterSPItemBundle2.Where(e => e.BundleLineID == BundleLineID).FirstOrDefault();
            if (location == null)
            {
                throw new ManagerProcessException("000105");
            }
            if (location.Cancelled)
            {
                throw new ManagerProcessException("000071");
            }
            location.Status = (int)Status.Deleted;
            location.CancelDate = DateTime.Now;
            location.Cancelled = true;
            location.ModUser = userId;

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }

        public async ValueTask<PaginationDatabaseResponseDto<object>> GetBundlesItems(GetBundlesItemsModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPItemBundle2.Where(e => e.Cancelled == false && e.BundleID == model.BundelId
            //(model.BundleName == null || e.BundleNameAr.Contains(model.BundleName) || e.BundleNameEn.Contains(model.BundleName)) &&
            //(model.BundleCode == null || e.BundleCode.Contains(model.BundleCode)) &&
            // (model.BundleCreationDate == null || e.BundleCreationDate.Date == model.BundleCreationDate.Value.Date) &&
            // (model.BundlePriceAfterDiscount == null || e.BundlePriceAfterDiscount == model.BundlePriceAfterDiscount) &&
            //(model.Status == null || e.Status == model.Status)
            ).Select(e => new
            {
                e.BundleLineID,


                e.ItemID,
                e.ItemQty,
                e.TotalItemAmount,
                e.ItemCost,
                e.ItemPrice,
                Item = (from item in _context.MasterSPItem
                        join itemName in _context.MasterSPGeneralItemNames on item.ItemNameID equals itemName.ItemNameID
                        where item.ItemID == e.ItemID
                        select new
                        {
                            itemName.ItemNameEn,
                            itemName.ItemNameAr,
                            itemName.ItemNameDesc,
                            item.ItemCode,

                            ItemTaxType = _context.MasterLookup.Where(x => x.LookupID == item.ItemTaxType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),

                            ItemCurrency = _context.MasterLookup.Where(x => x.LookupID == item.ItemCurrency).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault()
                        }).FirstOrDefault(),


                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),

            });



            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        public async ValueTask<ApiResponseModel> EditBundle(EditBundleModel model, int userId)
        {

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var item = _context.MasterSPItemBundle1.Where(e => e.BundleID == model.BundleID).FirstOrDefault();
            if (item == null)
            {
                throw new ManagerProcessException("000101");
            }
            bool isLetter = _utilties.IsLettersOnly(model.BundleNameEn);
            if (!isLetter)
            {
                throw new ManagerProcessException("000093");
            }
            var isArabic = _utilties.IsArabic(model.BundleNameEn);
            if (isArabic)
            {
                throw new ManagerProcessException("000093");
            }

            isArabic = _utilties.IsArabic(model.BundleCode);
            if (isArabic)
            {
                throw new ManagerProcessException("000092");
            }
            //isArabic = _utilties.HasEnglishCharacters(model.BundleNameAr);
            //if (isArabic)
            //{
            //    throw new ManagerProcessException("000094");
            //}
            var exist = _context.MasterSPItemBundle1.Where(e => e.BundleID != model.BundleID && e.BundleCode == model.BundleCode && e.Cancelled == false).FirstOrDefault();
            if (exist != null)
            {
                if (exist.Status == (int)Status.Inactive)
                {

                    throw new ManagerProcessException("000096");
                }
                throw new ManagerProcessException("000095");
            }
            exist = _context.MasterSPItemBundle1.Where(e => e.BundleID != model.BundleID && e.BundleNameEn == model.BundleNameEn && e.Cancelled == false).FirstOrDefault();
            if (exist != null)
            {
                if (exist.Status == (int)Status.Inactive)
                {

                    throw new ManagerProcessException("000098");
                }
                throw new ManagerProcessException("000097");
            }
            //exist = _context.MasterSPItemBundle1.Where(e => e.BundleID != model.BundleID && e.BundleNameAr == model.BundleNameAr && e.Cancelled == false).FirstOrDefault();
            //if (exist != null)
            //{
            //    if (exist.Status == (int)Status.Inactive)
            //    {

            //        throw new ManagerProcessException("000100");
            //    }
            //    throw new ManagerProcessException("000099");
            //}


            if (model.BundlePrice < model.BundleCost)
            {
                throw new ManagerProcessException("000088");
            }
            if (model.BundleDiscountPercentage < 0 || model.BundleDiscountPercentage > 100)
            {
                throw new ManagerProcessException("000089");
            }
            var discount = Math.Round((
                (model.BundlePrice / 100)
                * model.BundleDiscountPercentage
                ), 3);
            if (discount != model.BundleDiscountAmount)
            {
                throw new ManagerProcessException("000090");
            }
            if (model.BundlePriceAfterDiscount != model.BundlePrice - model.BundleDiscountAmount)
            {
                throw new ManagerProcessException("000091");
            }


            var isValids = _context.MasterSPLookup.Where(e => e.LookupID == model.Status && model.Status >= 23001 && model.Status <= 25999).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }


            item.BundleCode = model.BundleCode;
            // item.BundleNameAr = model.BundleNameAr;
            item.BundleNameEn = model.BundleNameEn;
            item.BundlePrice = model.BundlePrice;
            item.BundleCost = model.BundleCost;
            item.BundleDiscountPercentage = model.BundleDiscountPercentage;
            item.BundleDiscountAmount = model.BundleDiscountAmount;
            item.BundleComments = model.BundleComments;
            item.BundleCreationDate = model.BundleCreationDate;
            item.BundleDesc = model.BundleDesc;
            item.BundleGroup = (int)Settings.Group;
            item.BundlePriceAfterDiscount = model.BundlePriceAfterDiscount;

            item.Status = model.Status;
            item.ModUser = userId;
            item.ModDate = DateTime.Now;
            item.ModTime = DateTime.Now.TimeOfDay;
            item.BundleValidFrom = model.BundleValidFrom;
            item.BundleValidTo = model.BundleValidTo;


            var newId = ((int?)_context.MasterSPItemBundle2.Max(inv => (int?)inv.BundleLineID) ?? 0);
            var duplicate = model.Items.GroupBy(e => e.ItemID).Where(e => e.Count() > 1).Any();
            if (duplicate)
            {
                throw new ManagerProcessException("000104");
            }
            var itemList = new List<MasterSPItemBundle2>();
            foreach (var bundelItem in model.Items)
            {
                // exist = _context.MasterSPItemBundle2.Where(e => e.BundleID == model.BundleID && e.ItemID == bundelItem.ItemID && e.Cancelled == false).Any();
                //if (exist)
                //{
                //    throw new ManagerProcessException("000104");
                //}
                var isValid = _context.MasterSPItem.Where(e => e.ItemID == bundelItem.ItemID && e.Cancelled == false).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000045");
                }
                if (bundelItem.ItemQty <= 0)
                {
                    throw new ManagerProcessException("000102");
                }
                if (bundelItem.ItemPrice <= 0)
                {
                    throw new ManagerProcessException("000103");
                }
                newId++;

                var bunderItem = new MasterSPItemBundle2
                {

                    BundleLineID = newId,
                    BundleID = model.BundleID,
                    ItemID = bundelItem.ItemID,
                    ItemCost = bundelItem.ItemCost,
                    ItemPrice = bundelItem.ItemPrice,
                    ItemQty = bundelItem.ItemQty,
                    TotalItemAmount = bundelItem.ItemQty * bundelItem.ItemPrice,
                    Cancelled = false,
                    Status = model.Status,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };
                itemList.Add(bunderItem);

            }
            _context.MasterSPItemBundle2.Where(e => e.BundleID == model.BundleID).ExecuteDelete();
            await _context.MasterSPItemBundle2.AddRangeAsync(itemList);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            // response.Data = item;

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> SendBundleForApproval(int BundleID, int userId)
        {


            var bundel = _context.MasterSPItemBundle1.Where(e => e.BundleID == BundleID).FirstOrDefault();
            if (bundel == null)
            {
                throw new ManagerProcessException("000101");
            }
            if (bundel.Status != (int)BundelStatus.Draft && bundel.Status != (int)BundelStatus.NeedRevision)
            {
                throw new ManagerProcessException("000116");
            }
            bundel.Status = (int)BundelStatus.Pending;


            bundel.ModUser = userId;
            bundel.ModDate = DateTime.Now;
            bundel.ModTime = DateTime.Now.TimeOfDay;
            var newId = ((int?)_context.MasterSPItemBundleApprovalRequest.Max(inv => (int?)inv.BundleApprovalRequestID) ?? 0) + 1;
            var request = new MasterSPItemBundleApprovalRequest
            {
                BundleApprovalRequestID = newId,
                BundleID = BundleID,
                Cancelled = false,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay,
                Status = (int)BundelStatus.Pending,
                EnterUser = userId,
            };
            _context.MasterSPItemBundleApprovalRequest.Add(request);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> HandleBundleApprovalRequest(HandleBundleApprovalRequestModel model, int userId)
        {


            var bundel = _context.MasterSPItemBundle1.Where(e => e.BundleID == model.BundleID).FirstOrDefault();
            if (bundel == null)
            {
                throw new ManagerProcessException("000101");
            }
            if (bundel.Status != (int)BundelStatus.Pending)
            {
                throw new ManagerProcessException("000117");
            }
            if (
                model.Status != (int)BundelStatus.Approved &&
                model.Status != (int)BundelStatus.Rejected &&
                model.Status != (int)BundelStatus.NeedRevision)
            {
                throw new ManagerProcessException("000032");
            }
            bundel.Status = model.Status;
            //bundel.BundleComments = model.Remarks;


            bundel.ModUser = userId;
            bundel.ModDate = DateTime.Now;
            bundel.ModTime = DateTime.Now.TimeOfDay;
            if (model.RejectionResonID != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.Status == (int)Status.Active && e.LookupID == model.RejectionResonID &&
                e.LookupTypeID == (int)LookupTypes.RejectionReason).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000118");
                }
            }
            if (model.ReviseReasonID != null)
            {
                var isValid = _context.MasterSPLookup.Where(e => e.Status == (int)Status.Active && e.LookupID == model.ReviseReasonID &&
                e.LookupTypeID == (int)LookupTypes.ReviseReason).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000119");
                }
            }

            var request = _context.MasterSPItemBundleApprovalRequest.Where(e => e.BundleID == model.BundleID).OrderByDescending(e => e.BundleApprovalRequestID).FirstOrDefault();
            if (request != null)
            {
                request.Status = (int)Status.Inactive;
                request.Remarks = model.Remarks;
                request.BundleApprovalRejectionReason = model.RejectionResonID;
                request.BundleApprovalRevisionReason = model.ReviseReasonID;
                request.BundleApprovalRejectionReasonDetails = model.RrejectionReasonDetails;
                request.BundleApprovalRevisionReasonDetails = model.ReviseReasonDetails;
                request.ModDate = DateTime.Now;
                request.ModTime = DateTime.Now.TimeOfDay;
                request.ModUser = userId;


            }

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> AddBundleComplete(AddBundleModelComplete model, int userId)
        {

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            //var item = _context.MasterSPItemBundle1.Where(e => e.BundleID == model.BundleID).FirstOrDefault();
            //if (item == null)
            //{
            //    throw new ManagerProcessException("000101");
            //}
            bool isLetter = _utilties.IsLettersOnly(model.BundleNameEn);
            if (!isLetter)
            {
                throw new ManagerProcessException("000093");
            }
            var isArabic = _utilties.IsArabic(model.BundleNameEn);
            if (isArabic)
            {
                throw new ManagerProcessException("000093");
            }

            isArabic = _utilties.IsArabic(model.BundleCode);
            if (isArabic)
            {
                throw new ManagerProcessException("000092");
            }
            //isArabic = _utilties.HasEnglishCharacters(model.BundleNameAr);
            //if (isArabic)
            //{
            //    throw new ManagerProcessException("000094");
            //}
            var exist = _context.MasterSPItemBundle1.Where(e => e.BundleCode == model.BundleCode && e.Cancelled == false).FirstOrDefault();
            if (exist != null)
            {
                if (exist.Status == (int)Status.Inactive)
                {

                    throw new ManagerProcessException("000096");
                }
                throw new ManagerProcessException("000095");
            }
            exist = _context.MasterSPItemBundle1.Where(e => e.BundleNameEn == model.BundleNameEn && e.Cancelled == false).FirstOrDefault();
            if (exist != null)
            {
                if (exist.Status == (int)Status.Inactive)
                {

                    throw new ManagerProcessException("000098");
                }
                throw new ManagerProcessException("000097");
            }
            //exist = _context.MasterSPItemBundle1.Where(e => e.BundleNameAr == model.BundleNameAr && e.Cancelled == false).FirstOrDefault();
            //if (exist != null)
            //{
            //    if (exist.Status == (int)Status.Inactive)
            //    {

            //        throw new ManagerProcessException("000100");
            //    }
            //    throw new ManagerProcessException("000099");
            //}


            if (model.BundlePrice < model.BundleCost)
            {
                throw new ManagerProcessException("000088");
            }
            if (model.BundleDiscountPercentage < 0 || model.BundleDiscountPercentage > 100)
            {
                throw new ManagerProcessException("000089");
            }
            var discount = Math.Round((
                (model.BundlePrice / 100)
                * model.BundleDiscountPercentage
                ), 3);
            //if (discount != model.BundleDiscountAmount)
            //{
            //    throw new ManagerProcessException("000090");
            //}
            //if (model.BundlePriceAfterDiscount != model.BundlePrice - model.BundleDiscountAmount)
            //{
            //    throw new ManagerProcessException("000091");
            //}


            //var isValids = _context.MasterSPLookup.Where(e => e.LookupID == model.Status && model.Status >= 23001 && model.Status <= 25999).Any();
            //if (!isValids)
            //{
            //    throw new ManagerProcessException("000032");
            //}
            var newBundelId = ((int?)_context.MasterSPItemBundle1.Max(inv => (int?)inv.BundleID) ?? 0) + 1;
            var item = new MasterSPItemBundle1
            {
                BundleID = newBundelId,
                BundleCode = model.BundleCode,
                // BundleNameAr = model.BundleNameAr,
                BundleNameEn = model.BundleNameEn,
                BundlePrice = model.BundlePrice,
                BundleCost = model.BundleCost,
                BundleDiscountPercentage = model.BundleDiscountPercentage,
                BundleDiscountAmount = discount,
                BundleComments = model.BundleComments,
                BundleCreationDate = model.BundleCreationDate,
                BundleValidFrom = model.BundleValidFrom,
                BundleValidTo = model.BundleValidTo,
                BundleDesc = model.BundleDesc,
                BundleGroup = (int)Settings.Group,
                BundlePriceAfterDiscount = model.BundlePrice - discount,
                Cancelled = false,
                Status = (int)BundelStatus.Draft,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };


            await _context.MasterSPItemBundle1.AddAsync(item);







            var newId = ((int?)_context.MasterSPItemBundle2.Max(inv => (int?)inv.BundleLineID) ?? 0);
            var duplicate = model.Items.GroupBy(e => e.ItemID).Where(e => e.Count() > 1).Any();
            if (duplicate)
            {
                throw new ManagerProcessException("000104");
            }
            var itemList = new List<MasterSPItemBundle2>();
            foreach (var bundelItem in model.Items)
            {

                var isValid = _context.MasterSPItem.Where(e => e.ItemID == bundelItem.ItemID && e.Cancelled == false).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000045");
                }
                if (bundelItem.ItemQty <= 0)
                {
                    throw new ManagerProcessException("000102");
                }
                if (bundelItem.ItemPrice <= 0)
                {
                    throw new ManagerProcessException("000103");
                }
                newId++;

                var bunderItem = new MasterSPItemBundle2
                {

                    BundleLineID = newId,
                    BundleID = newBundelId,
                    ItemID = bundelItem.ItemID,
                    ItemCost = bundelItem.ItemCost,
                    ItemPrice = bundelItem.ItemPrice,
                    ItemQty = bundelItem.ItemQty,
                    TotalItemAmount = bundelItem.ItemQty * bundelItem.ItemPrice,
                    Cancelled = false,
                    Status = model.Status,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };
                itemList.Add(bunderItem);

            }
            await _context.MasterSPItemBundle2.AddRangeAsync(itemList);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            // response.Data = item;

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> ImportBundle(UpdateFileModel2 model, int userId)
        {

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "ImportBundle");
            var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            var dt = ds.Tables[0];

            var newBundelId = ((int?)_context.MasterSPItemBundle1.Max(inv => (int?)inv.BundleID) ?? 0) + 1;
            var newId = ((int?)_context.MasterSPItemBundle2.Max(inv => (int?)inv.BundleLineID) ?? 0);

            var BundleCreationDate = DateTime.FromOADate(double.Parse(dt.Rows[1][0].ToString()));

            var BundleCode = dt.Rows[1][1].ToString();
            var BundleNameEn = dt.Rows[1][2].ToString();
            var BundleDesc = dt.Rows[1][3].ToString();
            var BundleDiscountPercentage = decimal.Parse(dt.Rows[1][4].ToString());
            DateTime? validityFrom = null;
            DateTime? validityTo = null;
            if (!string.IsNullOrEmpty(dt.Rows[1][5].ToString()))
            {
                validityFrom = DateTime.FromOADate(double.Parse(dt.Rows[1][5].ToString()));
            }
            if (!string.IsNullOrEmpty(dt.Rows[1][6].ToString()))
            {
                validityTo = DateTime.FromOADate(double.Parse(dt.Rows[1][6].ToString()));
            }
            if (validityFrom != null && validityTo != null && validityFrom > validityTo)
            {
                throw new ManagerProcessException("000160");
            }


            var bundle1 = _context.MasterSPItemBundle1.Where(e => e.BundleNameEn == BundleNameEn && e.Cancelled == false).FirstOrDefault();
            var isNewBundel = false;
            if (bundle1 == null)
            {
                isNewBundel = true;
                bool isLetter = _utilties.IsLettersOnly(BundleNameEn);
                if (!isLetter)
                {
                    throw new ManagerProcessException("000093");


                }
                var isArabic = _utilties.IsArabic(BundleNameEn);
                if (isArabic)
                {
                    throw new ManagerProcessException("000093");

                }

                isArabic = _utilties.IsArabic(BundleCode);
                if (isArabic)
                {

                    throw new ManagerProcessException("000092");
                }

                var exist = _context.MasterSPItemBundle1.Where(e => e.BundleCode == BundleCode && e.Cancelled == false).FirstOrDefault();
                if (exist != null)
                {
                    if (exist.Status == (int)Status.Inactive)
                    {

                        throw new ManagerProcessException("000096");
                    }
                    throw new ManagerProcessException("000095");
                }
                //if (exist != null)
                //{
                //    if (exist.Status == (int)Status.Inactive)
                //    {

                //        throw new ManagerProcessException("000098");
                //    }
                //    throw new ManagerProcessException("000097");
                //}




                if (BundleDiscountPercentage < 0 || BundleDiscountPercentage > 100)
                {
                    throw new ManagerProcessException("000089");
                }


                bundle1 = new MasterSPItemBundle1
                {
                    BundleID = newBundelId,
                    BundleCode = BundleCode,
                    BundleNameEn = BundleNameEn,

                    BundleDiscountPercentage = BundleDiscountPercentage,


                    BundleCreationDate = BundleCreationDate,
                    BundleDesc = BundleDesc,
                    BundleGroup = (int)Settings.Group,
                    BundleValidFrom = validityFrom,
                    BundleValidTo = validityTo,
                    Cancelled = false,
                    Status = (int)BundelStatus.Draft,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };
            }




            var itemList = new List<MasterSPItemBundle2>();
            decimal totalPrice = 0;
            int InvalidItemsCount = 0;
            int InvalidItemsQtyCount = 0;
            int InvalidItemsPriceCount = 0;
            int FailedtoUploadItemsCount = 0;
            int UploaddedItemsCount = 0;
            int DuplicateItemCount = 0;
            for (int i = 4; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    continue;
                }

                //try
                //{



                string ItemCode = dt.Rows[i][0].ToString();
                int ItemQty = int.Parse(dt.Rows[i][1].ToString());


                var item = _context.MasterSPItem.Where(e => e.ItemCode == ItemCode && e.Cancelled == false).FirstOrDefault();
                if (item == null)
                {
                    InvalidItemsCount++;
                    continue;
                    // throw new ManagerProcessException("000045");
                }
                if (ItemQty <= 0)
                {
                    InvalidItemsQtyCount++;
                    continue;
                    //  throw new ManagerProcessException("000102");
                }
                if (item.ItemPrice == null || item.ItemPrice <= 0)
                {
                    InvalidItemsPriceCount++;
                    continue;
                    // throw new ManagerProcessException("000103");
                }
                bool invalid = _context.MasterSPItemBundle2.Where(e => e.BundleID == bundle1.BundleID && e.Cancelled == false && e.ItemID == item.ItemID).Any();
                if (invalid)
                {
                    DuplicateItemCount++;
                    continue;
                }

                decimal totalItemAmount = ItemQty * Convert.ToDecimal(item.ItemPrice);
                var tax = _context.MasterLookup.Where(e => e.LookupID == item.ItemTaxType).Select(e => e.LookupName).FirstOrDefault();
                var taxPercent = decimal.Parse(tax.Replace("%", ""));
                var TaxAmount = (totalItemAmount / 100) * taxPercent;
                totalItemAmount += TaxAmount;



                totalPrice += totalItemAmount;

                //  throw new ManagerProcessException("000103");

                //  item.ItemTaxType
                newId++;

                var bunderItem = new MasterSPItemBundle2
                {

                    BundleLineID = newId,
                    BundleID = bundle1.BundleID,
                    ItemID = item.ItemID,
                    ItemCost = (decimal)item.ItemPrice,
                    ItemPrice = (decimal)item.ItemPrice,
                    ItemQty = ItemQty,
                    TotalItemAmount = totalItemAmount,
                    Cancelled = false,
                    Status = (int)Status.Active,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };
                itemList.Add(bunderItem);


                await _context.MasterSPItemBundle2.AddRangeAsync(itemList);
                UploaddedItemsCount++;
                //}
                //catch 
                //{
                //    FailedtoUploadItemsCount++;
                //}
            }
            bundle1.BundlePrice = totalPrice;
            bundle1.BundleCost = totalPrice;
            var discount = Math.Round((
           (totalPrice / 100)
           * BundleDiscountPercentage
           ), 3);
            bundle1.BundleDiscountAmount = discount;
            bundle1.BundlePriceAfterDiscount = totalPrice - discount;
            if (isNewBundel)
            {
                await _context.MasterSPItemBundle1.AddAsync(bundle1);
            }
            if (model.Save != false)
            {
                var result = await _context.SaveChangesAsync();
            }
            var response = new ApiResponseModel(200, null);
            response.Data = new
            {
                InvalidItemsCount,
                InvalidItemsQtyCount,
                InvalidItemsPriceCount,
                DuplicateItemCount,
                FailedtoUploadItemsCount,

                UploaddedItemsCount
            };



            return response;

            throw new ManagerProcessException("000008");
        }
    }
}
