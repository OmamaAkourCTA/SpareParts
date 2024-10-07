using Microsoft.Extensions.Configuration;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Core.Helpers;
using SparePartsModule.Domain.Context;
using SparePartsModule.Domain.Models.Library;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.ItemsBundels;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.SPOrder;
using SparePartsModule.Infrastructure.ViewModels.Models;
using Microsoft.SqlServer.Server;
using SparePartsModule.Infrastructure.ViewModels.Response;
using SparePartsModule.Interface;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Http.Metadata;
using OfficeOpenXml.Drawing.Slicer.Style;
using SparePartsModule.Domain.Models.MarkaziaMaster;
using Microsoft.Extensions.Options;
using SparePartsModule.Infrastructure.ViewModels.Dtos;

namespace SparePartsModule.Core.Library
{
    public class SparePartsOrdersService : ISparePartsOrdersService
    {
        private readonly IConfiguration _config;
        private readonly SparePartsModuleContext _context;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilties;
        private readonly MarkaziaMasterContext _contextMaster;
        private readonly ILookupService _lookupService;
        private readonly EMailService _eMailService;
        private readonly ExcelExportOrder _excelExportOrder;
        private readonly IOptionsMonitor<EmailConfiguration> _emailConfig;


        public SparePartsOrdersService(SparePartsModuleContext context, IConfiguration config, FileHelper fileHelper,
            UtilitiesHelper utilties, MarkaziaMasterContext contextMaster, ILookupService lookupService,
             EMailService eMailService, ExcelExportOrder excelExportOrder, IOptionsMonitor<EmailConfiguration> emailConfig)
        {

            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;
            _utilties = utilties;
            _contextMaster = contextMaster;
            _lookupService = lookupService;
            _eMailService = eMailService;
            _excelExportOrder = excelExportOrder;
            _emailConfig = emailConfig;
        }
        public async ValueTask<ApiResponseModel> CreateSPOrder(CreateSPOrderModel model, int userId)
        {

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var supplier = _context.GettingSPSupplier.Where(e => e.SupplierID == model.OrderSupplierId).FirstOrDefault();
            if (supplier == null)
            {
                throw new ManagerProcessException("000007");
            }
            //var currencyRate =model. (from currency in _contextMaster.MasterLookup
            //                   // join rate in _contextMaster.MasterCurrencyRate on currency.LookupName equals rate.Currency
            //                    where currency.LookupID == supplier.SupplierCurrency
            //                    select Convert.ToDecimal( currency.LookupValue)).FirstOrDefault();
            //if (currencyRate > 0)
            //{
            //    currencyRate = decimal.Round(currencyRate, 3, MidpointRounding.AwayFromZero);
            //}
            //else
            //{
            //    currencyRate = 1;
            //}
            if (model.TaxPercentage > 100 || model.TaxPercentage < 0)
            {

                throw new ManagerProcessException("000126");
            }
            if (model.DiscountPercentage > 100 || model.DiscountPercentage < 0)
            {

                throw new ManagerProcessException("000089");
            }
            if (model.DiscountAmount < 0)
            {
                throw new ManagerProcessException("000020");
            }
            if (model.TaxAmount < 0)
            {
                throw new ManagerProcessException("000019");
            }
            if (model.NetAmount < 0)
            {
                throw new ManagerProcessException("000127");
            }
            if (model.OrderBusinessArea != null)
            {
                var valid = _context.MasterBusinessArea
                     .Where(e => e.Status == (int)Status.Active && e.BusinessAreaID == model.OrderBusinessArea).Any();
                if (!valid)
                {
                    throw new ManagerProcessException("000145");
                }
            }




            var orderType = _context.MasterSPLookup.Where(e => e.LookupID == model.OrderType && e.LookupTypeID == (int)LookupTypes.OrderType).FirstOrDefault();
            if (orderType == null)
            {
                throw new ManagerProcessException("000121");
            }
            var isValids = _context.MasterSPLookup.Where(e => e.LookupID == model.OrderMethod && e.LookupTypeID == (int)LookupTypes.OrderMethod).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000122");
            }
            var freight = _context.MasterSPLookup.Where(e => e.LookupID == model.OrderFrieght && e.LookupTypeID == (int)LookupTypes.OrderFrieght).FirstOrDefault();
            if (freight == null)
            {
                throw new ManagerProcessException("000123");
            }
            if (model.OrderSource != null)
            {
                var isValid = _context.MasterSPLookup
                    .Where(e => e.LookupID == model.OrderSource && e.LookupTypeID == (int)LookupTypes.OrderSource).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000157");
                }
            }
            var newId = ((int?)_context.GettingSPOrder1.Max(inv => (int?)inv.OrderID) ?? 0) + 1;
            var newItemId = ((int?)_context.GettingSPOrder2.Max(inv => (int?)inv.OrderLineID) ?? 0);
            string orderNo = freight.LookupName.Substring(0, 1) +
                             orderType.LookupName.Substring(0, 1) +
                             supplier.SupplierAbbCode.Substring(0, 2).ToUpper() + newId.ToString("0000");

            decimal lineTotal = 0;
            decimal subTotal = decimal.Round(model.ItemsModel.Sum(e => e.ItemPrice * e.Qty), 3, MidpointRounding.AwayFromZero);
            decimal discountAmount = 0;
            decimal totalDiscountmount = 0;
            decimal netAmount = 0;
            decimal totalNetAmount = 0;
            decimal taxAmount = 0;
            decimal totalTaxAmount = 0;
            decimal lineTotalFinal = 0;
            decimal OrderTotalAmount = 0;
            int OrderLineSeq = 0;

            foreach (var item in model.ItemsModel)
            {
                if (item.ItemDiscountPercentage > 100 || item.ItemDiscountPercentage < 0)
                {

                    throw new ManagerProcessException("000089");
                }
                if (item.ItemTaxPercentage > 100 || item.ItemTaxPercentage < 0)
                {

                    throw new ManagerProcessException("000126");
                }
                var spItem = _context.MasterSPItem.Where(e => e.ItemID == item.ItemId).FirstOrDefault();
                if (spItem == null)
                {
                    throw new ManagerProcessException("000045");
                }
                var SPItemSupplier = _context.MasterSPItemSupplier.Where(e => e.ItemID == item.ItemId && e.SupplierID == model.OrderSupplierId && e.Cancelled == false).FirstOrDefault();
                if (SPItemSupplier == null)
                {
                    throw new ManagerProcessException("000131");
                }

                if (item.Qty <= 0)
                {
                    throw new ManagerProcessException("000124");
                }
                if (item.ItemPrice <= 0)
                {
                    throw new ManagerProcessException("000125");
                }
                if (item.ItemTotal < 0)
                {
                    throw new ManagerProcessException("000125");
                }
                if (item.ItemNetAmount < 0)
                {
                    throw new ManagerProcessException("000127");
                }
                if (item.ItemDiscountAmount < 0)
                {
                    throw new ManagerProcessException("000020");
                }
                //if (item.Qty < SPItemSupplier.ItemMinQty || item.Qty > SPItemSupplier.ItemMaxQty)
                //{
                //    throw new ManagerProcessException("000129");
                //}
                if (item.SupplierFlag != null)
                {
                    isValids = _context.MasterSPLookup
                        .Where(e => e.Status == (int)Status.Active && e.LookupID == item.SupplierFlag && e.LookupTypeID == (int)LookupTypes.SupplierFlage).Any();
                    if (!isValids)
                    {
                        throw new ManagerProcessException("000068");
                    }
                }
                if (item.SupplierFlag != null)
                {
                    isValids = _context.MasterSPLookup
                   .Where(e => e.Status == (int)Status.Active && e.LookupID == item.OrderItemOrderFlag && e.LookupTypeID == (int)LookupTypes.OrderFlage).Any();
                    if (!isValids)
                    {
                        throw new ManagerProcessException("000130");
                    }
                }


                lineTotal = decimal.Round(item.Qty * item.ItemPrice, 3, MidpointRounding.AwayFromZero);


                discountAmount = decimal.Round(lineTotal * item.ItemDiscountPercentage / 100, 3, MidpointRounding.AwayFromZero);

                totalDiscountmount += discountAmount;


                netAmount = decimal.Round(lineTotal - discountAmount, 3, MidpointRounding.AwayFromZero);
                totalNetAmount += netAmount;

                taxAmount = decimal.Round(item.ItemTaxPercentage * netAmount / 100, 3, MidpointRounding.AwayFromZero);
                totalTaxAmount += taxAmount;

                lineTotalFinal = netAmount + taxAmount;
                OrderTotalAmount += lineTotalFinal;

                newItemId++;
                OrderLineSeq++;
                var orderItem = new GettingSPOrder2
                {
                    OrderLineID = newItemId,
                    OrderItemID = item.ItemId,
                    OrderID = newId,
                    OrderItemQty = item.Qty,
                    OrderItemPrice = item.ItemPrice,


                    OrderItemTaxAmount = taxAmount,
                    OrderItemTaxPer = item.ItemTaxPercentage,
                    OrderItemTax = item.OrderItemTax,
                    OrderItemDis = item.OrderItemDis,

                    OrderItemDisAmount = discountAmount,
                    OrderItemDisPer = item.ItemDiscountPercentage,

                    OrderItemNetAmount = netAmount,
                    OrderItemOrderFlag = spItem.ItemFlagOrder,
                    OrderItemSupplierFlag = spItem.ItemFlagSup,
                    OrderItemQtyOrderMin = SPItemSupplier.ItemMinQty,
                    OrderItemQtyOrderMax = SPItemSupplier.ItemMaxQty,
                    OrderItemAmount = lineTotal,
                    OrderLineSeq = OrderLineSeq,
                    OrderItemTotalAmount = lineTotalFinal,


                    Status = (int)Status.Active,
                    Cancelled = false,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay,
                    EnterUser = userId,


                };
                await _context.GettingSPOrder2.AddAsync(orderItem);
            }

            var order = new GettingSPOrder1
            {
                OrderID = newId,
                OrderNo = orderNo,
                OrderSeq = int.Parse(DateTime.Now.ToString("yy") + newId.ToString("0000")),
                OrderTaxAmount = totalTaxAmount,
                OrderTaxPer = model.TaxPercentage,
                OrderDisAmount = totalDiscountmount,
                OrderDisPer = model.DiscountPercentage,
                OrderCurrencyExchangeDate = DateTime.Now,
                OrderCurrencyExchangeRate = model.OrderCurrencyRate,
                OrderSupplierID = model.OrderSupplierId,
                OrderCurrency = model.CurrencyLookup,
                OrderNetAmount = totalNetAmount,
                OrderAmount = subTotal,
                OrderTotalAmountCur = decimal.Round(OrderTotalAmount, 3, MidpointRounding.AwayFromZero),
                OrderTotalAmountJOD = decimal.Round(OrderTotalAmount * model.OrderCurrencyRate, 3, MidpointRounding.AwayFromZero),
                OrderApproval = (int)OrderStatus.Draft,
                OrderDate = DateTime.Now,
                Cancelled = false,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay,
                EnterUser = userId,
                Status = (int)Status.Active,
                OrderGroup = (int)Settings.Group,
                OrderMethod = model.OrderMethod,
                OrderFreight = model.OrderFrieght,
                OrderType = model.OrderType,
                OrderComments = model.Comments,
                SourceSequence = model.SourceSequence,
                OrderSource = model.OrderSource,
                OrderBusinessArea = model.OrderBusinessArea


            };
            await _context.GettingSPOrder1.AddAsync(order);

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = newId;

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetSPOrders(GetSPOrdersModel model, PaginationModel paginationPostModel)
        {

            var data = _context.GettingSPOrder1.Where(e => e.Cancelled == false &&
              (model.SupplierId == null || ("," + model.SupplierId + ",").Contains("," + e.OrderSupplierID.ToString() + ",")) &&
                (model.Currency == null || ("," + model.Currency + ",").Contains("," + e.OrderCurrency.ToString() + ",")) &&
               (model.OrderType == null || model.OrderType.Contains(e.OrderType.ToString())) &&
               (model.OrderSeqNo == null || e.OrderSeq.ToString().Contains(model.OrderSeqNo)) &&
                (model.OrderNo == null || e.OrderNo.Contains(model.OrderNo)) &&
                (model.OrderMethod == null || model.OrderMethod.Contains(e.OrderMethod.ToString())) &&
              (model.OrderFreight == null || model.OrderFreight.Contains(e.OrderFreight.ToString())) &&
               (model.User == null || e.User.FullName.Contains(model.User) || e.User.UserId.ToString().Contains(model.User)) &&
                (model.CreationDateFrom == null || e.OrderDate.Date >= model.CreationDateFrom.Value.Date) &&
                   (model.CreationDateTo == null || e.OrderDate.Date <= model.CreationDateTo.Value.Date) &&
            (model.Status == null || model.Status.Contains(e.OrderApproval.ToString())) &&
            (model.ItemNo == null || e.GettingSPOrder2.Where(x => x.MasterSPItem.ItemCode.Contains(model.ItemNo)).Any())
            ).Select(e => new
            {
                e.OrderID,
                e.OrderNo,
                e.OrderSeq,
                e.OrderDate,
                e.OrderTotalAmountCur,
                e.OrderCurrencyExchangeRate,
                e.OrderTotalAmountJOD,
                e.SourceSequence,
                NoOfLines = e.GettingSPOrder2.Where(e => e.Cancelled == false).Count(),
                OrderSource = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderSource).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),

                OrderType = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderType).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                OrderMethod = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderMethod).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                OrderFreight = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderFreight).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                Supplier = _context.GettingSPSupplier.Where(x => x.SupplierID == e.OrderSupplierID).Select(x => new { x.SupplierID, x.SupplierName, x.SupplierNo }).FirstOrDefault(),
                OrderCurrency = _context.MasterLookup.Where(x => x.LookupID == e.OrderCurrency)
                .Select(x => new { x.LookupID, x.LookupName, }).FirstOrDefault(),


                OrderApproval = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderApproval).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                OrderBusinessArea = _context.MasterBusinessArea
                .Where(x => x.BusinessAreaID == e.OrderBusinessArea).Select(x => new { x.BusinessAreaID, x.BAName }).FirstOrDefault(),
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,
                e.EnterTime,
                EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),


            });
            // [SwaggerSchema("1 default creation date desc \r\n- 2/3 order seq\r\n- 4/5 order supplier no \r\n- 6/7 type \r\n- 8/9 supplier \r\n- 10/ 11 method \r\n- 12/ 13 currency \r\n- 14/15 frieght \r\n- 16/17 creation date \r\n- 18/19 status  ")]

            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.OrderSeq);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.OrderSeq);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.OrderNo);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.OrderNo);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.OrderType.LookupName);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.OrderType.LookupName);
            }
            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.Supplier.SupplierName);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.Supplier.SupplierName);
            }
            if (model.Sort == 10)
            {
                data = data.OrderBy(e => e.OrderMethod.LookupName);
            }
            if (model.Sort == 11)
            {
                data = data.OrderByDescending(e => e.OrderMethod.LookupName);
            }
            if (model.Sort == 12)
            {
                data = data.OrderBy(e => e.OrderCurrency.LookupName);
            }
            if (model.Sort == 13)
            {
                data = data.OrderByDescending(e => e.OrderCurrency.LookupName);
            }
            if (model.Sort == 14)
            {
                data = data.OrderBy(e => e.OrderFreight.LookupName);
            }
            if (model.Sort == 15)
            {
                data = data.OrderByDescending(e => e.OrderFreight.LookupName);
            }
            if (model.Sort == 16)
            {
                data = data.OrderBy(e => e.EnterDate).ThenBy(e => e.EnterTime);
            }
            if (model.Sort == 17)
            {
                data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
            }
            if (model.Sort == 18)
            {
                data = data.OrderBy(e => e.Status.LookupName);
            }
            if (model.Sort == 19)
            {
                data = data.OrderByDescending(e => e.Status.LookupName);
            }

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }


        public async ValueTask<PaginationDatabaseResponseDto<object>> GetInquiryOrders(GetSPInquiryOrdersModel model, PaginationModel paginationPostModel)
        {

            var data = _context.GettingSPOrder1.Where(e => e.Cancelled == false &&
              (model.SupplierId == null || ("," + model.SupplierId + ",").Contains("," + e.OrderSupplierID.ToString() + ",")) &&
                (model.Currency == null || ("," + model.Currency + ",").Contains("," + e.OrderCurrency.ToString() + ",")) &&
               (model.OrderType == null || model.OrderType.Contains(e.OrderType.ToString())) &&
               (model.OrderSeqNo == null || e.OrderSeq.ToString().Contains(model.OrderSeqNo)) &&
                (model.OrderNo == null || e.OrderNo.Contains(model.OrderNo)) &&
                (model.OrderMethod == null || model.OrderMethod.Contains(e.OrderMethod.ToString())) &&
              (model.OrderFreight == null || model.OrderFreight.Contains(e.OrderFreight.ToString())) &&
               (model.User == null || e.User.FullName.Contains(model.User) || e.User.UserId.ToString().Contains(model.User)) &&
                (model.CreationDateFrom == null || e.OrderDate.Date >= model.CreationDateFrom.Value.Date) &&
                   (model.CreationDateTo == null || e.OrderDate.Date <= model.CreationDateTo.Value.Date) &&
            (model.Status == null || model.Status.Contains(e.OrderApproval.ToString())) &&
             (model.OrderItemId == null || e.GettingSPOrder2.Any(x => x.OrderItemID == model.OrderItemId)) &&
            (model.ItemNo == null || e.GettingSPOrder2.Where(x => x.MasterSPItem.ItemCode.Contains(model.ItemNo)).Any())
            ).Select(e => new
            {
                e.OrderID,
                e.OrderNo,
                e.OrderSeq,
                e.OrderDate,
                e.OrderTotalAmountCur,
                e.OrderCurrencyExchangeRate,
                e.OrderTotalAmountJOD,
                e.SourceSequence,
                NoOfLines = e.GettingSPOrder2.Where(e => e.Cancelled == false).Count(),
                OrderSource = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderSource).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                OrderType = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderType).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                OrderMethod = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderMethod).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                OrderFreight = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderFreight).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                Supplier = _context.GettingSPSupplier.Where(x => x.SupplierID == e.OrderSupplierID).Select(x => new { x.SupplierID, x.SupplierName, x.SupplierNo }).FirstOrDefault(),
                OrderCurrency = _context.MasterLookup.Where(x => x.LookupID == e.OrderCurrency)
                .Select(x => new { x.LookupID, x.LookupName, }).FirstOrDefault(),


                OrderApproval = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderApproval).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                OrderBusinessArea = _context.MasterBusinessArea
                .Where(x => x.BusinessAreaID == e.OrderBusinessArea).Select(x => new { x.BusinessAreaID, x.BAName }).FirstOrDefault(),


                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),


                OrderInquiryItem = _context.GettingSPOrder2
    .Where(item2 => (item2.OrderItemID == model.OrderItemId) &&
                    !item2.Cancelled &&
                    item2.GettingSPOrder1.OrderApproval == (int)OrderStatus.Confirmed &&
                    !item2.GettingSPOrder1.Cancelled)
    .Select(item2 => new
    {
        item2.OrderLineID,
        item2.OrderID,
        item2.OrderLineSeq,
        item2.OrderItemID,
        item2.OrderItemQty,
        item2.OrderItemQtyCancelled,
        item2.OrderItemQtyConfirmed,
        item2.OrderItemAmount,
        item2.OrderItemDisPer,
        item2.OrderItemDisAmount,
        item2.OrderItemNetAmount,
        item2.OrderItemTaxPer,
        item2.OrderItemTaxAmount,
        item2.OrderItemTotalAmount,
        item2.OrderItemOrderFlag,
        item2.OrderItemQtyOnHand,
        item2.OrderItemQtyOnOrder,
        item2.OrderItemQtyOrderMin,
        item2.OrderItemQtyOrderMax,
        item2.OrderItemTax,
        item2.OrderItemDis
    })
    .ToList(),
                e.EnterDate,
                e.EnterTime,

                EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),


            });
            // [SwaggerSchema("1 default creation date desc \r\n- 2/3 order seq\r\n- 4/5 order supplier no \r\n- 6/7 type \r\n- 8/9 supplier \r\n- 10/ 11 method \r\n- 12/ 13 currency \r\n- 14/15 frieght \r\n- 16/17 creation date \r\n- 18/19 status  ")]

            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.OrderSeq);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.OrderSeq);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.OrderNo);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.OrderNo);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.OrderType.LookupName);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.OrderType.LookupName);
            }
            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.Supplier.SupplierName);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.Supplier.SupplierName);
            }
            if (model.Sort == 10)
            {
                data = data.OrderBy(e => e.OrderMethod.LookupName);
            }
            if (model.Sort == 11)
            {
                data = data.OrderByDescending(e => e.OrderMethod.LookupName);
            }
            if (model.Sort == 12)
            {
                data = data.OrderBy(e => e.OrderCurrency.LookupName);
            }
            if (model.Sort == 13)
            {
                data = data.OrderByDescending(e => e.OrderCurrency.LookupName);
            }
            if (model.Sort == 14)
            {
                data = data.OrderBy(e => e.OrderFreight.LookupName);
            }
            if (model.Sort == 15)
            {
                data = data.OrderByDescending(e => e.OrderFreight.LookupName);
            }
            if (model.Sort == 16)
            {
                data = data.OrderBy(e => e.EnterDate).ThenBy(e => e.EnterTime);
            }
            if (model.Sort == 17)
            {
                data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
            }
            if (model.Sort == 18)
            {
                data = data.OrderBy(e => e.Status.LookupName);
            }
            if (model.Sort == 19)
            {
                data = data.OrderByDescending(e => e.Status.LookupName);
            }

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }





        public async ValueTask<object> GetSPOrdersDetails(int OrderId)
        {

            var data = _context.GettingSPOrder1.Where(e => e.Cancelled == false && e.OrderID == OrderId
            ).Select(e => new
            {
                e.OrderID,
                e.OrderNo,
                e.OrderSeq,
                e.OrderDate,
                e.OrderTotalAmountCur,
                e.OrderCurrencyExchangeRate,
                e.OrderTotalAmountJOD,
                e.OrderAmount,
                e.OrderDisAmount,
                e.OrderDisPer,
                e.OrderTaxAmount,
                e.OrderTaxPer,
                e.OrderNetAmount,
                e.OrderComments,
                e.SourceSequence,
                OrderDis = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderDisAmount).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),

                OrderSource = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderSource).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),


                OrderType = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderType).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                OrderMethod = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderMethod).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                OrderFreight = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderFreight).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                Supplier = _context.GettingSPSupplier.Where(x => x.SupplierID == e.OrderSupplierID).Select(x => new { x.SupplierID, x.SupplierName, x.SupplierNo }).FirstOrDefault(),
                OrderCurrency = _context.MasterLookup.Where(x => x.LookupID == e.OrderCurrency).Select(x => new { x.LookupID, x.LookupName, }).FirstOrDefault(),


                OrderApproval = _context.MasterSPLookup.Where(x => x.LookupID == e.OrderApproval).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                OrderBusinessArea = _context.MasterBusinessArea
                .Where(x => x.BusinessAreaID == e.OrderBusinessArea).Select(x => new { x.BusinessAreaID, x.BAName }).FirstOrDefault(),

                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,
                e.EnterTime,
                e.ModDate,
                e.ModTime,
                EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),
                ModUser = _context.Users.Where(x => x.UserId == e.ModUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),
                ApprovalStatus = _context.MasterApproval.Where(x => x.RequestRecord == e.OrderID)
                .OrderByDescending(x => x.RequestID).FirstOrDefault(),
                GettingSPOrder2 = _context.GettingSPOrder2.Where(x => x.OrderID == e.OrderID && x.Cancelled == false)
                .Select(x => new
                {
                    x.OrderLineID,
                    x.OrderID,
                    x.OrderLineSeq,
                    x.OrderItemID,
                    x.OrderItemQty,
                    x.OrderItemQtyCancelled,
                    x.OrderItemQtyConfirmed,
                    x.OrderItemPrice,
                    x.OrderItemPriceConfirmed,
                    x.OrderItemAmount,
                    // x.OrderItemDisPer,
                    OrderItemDisPer = _context.MasterLookup.Where(y => y.LookupID == x.OrderItemDis).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor, x.LookupValue }).FirstOrDefault(),
                    OrderItemTaxPer = _context.MasterLookup.Where(y => y.LookupID == x.OrderItemTax).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor, x.LookupValue }).FirstOrDefault(),

                    x.OrderItemDisAmount,
                    x.OrderItemNetAmount,
                    //x.OrderItemTaxPer,
                    x.OrderItemTaxAmount,
                    x.OrderItemTotalAmount,
                    OrderItemSupplierFlag = _context.MasterSPLookup.Where(y => y.LookupID == x.OrderItemSupplierFlag).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                    OrderItemOrderFlag = _context.MasterSPLookup.Where(y => y.LookupID == x.OrderItemOrderFlag).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),

                    Item = _context.MasterSPItem.Where(z => z.ItemID == x.OrderItemID && z.Cancelled == false).Select(w => new
                    {
                        w.ItemID,
                        w.ItemPartCode,
                        w.ItemCode,
                        w.ItemNameID,
                        ItemDiscountType = _context.MasterLookup.Where(y => y.LookupID == w.ItemDiscountType).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                        ItemTaxType = _context.MasterLookup.Where(y => y.LookupID == w.ItemTaxType).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),


                        ItemName = _context.MasterSPGeneralItemNames.Where(z => z.ItemNameID == w.ItemNameID)
                   .Select(z => new { z.ItemNameID, z.ItemNameDesc, z.ItemNameAr, z.ItemNameEn, w.ItemCode }).FirstOrDefault()
                    }).FirstOrDefault(),


                    x.OrderItemQtyOnHand,
                    x.OrderItemQtyOnOrder,
                    x.OrderItemQtyOrderMin,
                    x.OrderItemQtyOrderMax,
                    x.Status,
                    x.Cancelled,
                    x.CancelDate,
                    EnterUser = _context.Users.Where(y => y.UserId == x.EnterUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),


                    x.EnterDate,
                    x.EnterTime,
                    x.ModUser,
                    x.ModDate,
                    x.ModTime
                }).ToList()



            });





            return data.FirstOrDefault();


        }
        public async ValueTask<ApiResponseModel> UploadOrderItems(UploadOrderItemsModel model, int userId)
        {
            int InvalidItemNumber = 0;
            int UploadedItemsCount = 0;
            int InvalidFOB = 0;
            int InvalidOrderedQty = 0;
            int InvalidDiscountPercent = 0;
            int InvalidDiscountAmount = 0;
            int InvalidTaxPercent = 0;
            int InvalidTaxamount = 0;
            int InvalidItemSupplier = 0;
            int FailedToUpload = 0;
            int DuplicateItemCount = 0;
            int InvalidQTYCount = 0;
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "UploadOrderItems");
            var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            var dt = ds.Tables[0];
            var order = _context.GettingSPOrder1.Where(e => e.Cancelled == false && e.OrderID == model.OrderId).FirstOrDefault();
            if (order == null)
            {
                throw new ManagerProcessException("000132");
            }

            var currencyRate = (from currency in _context.MasterLookup

                                join supplier in _context.GettingSPSupplier on currency.LookupID equals supplier.SupplierCurrency


                                where supplier.SupplierID == order.OrderSupplierID
                                select Convert.ToDecimal(currency.LookupValue)).FirstOrDefault();
            if (currencyRate > 0)
            {
                currencyRate = decimal.Round(currencyRate, 3, MidpointRounding.AwayFromZero);
            }
            else
            {
                currencyRate = 1;
            }
            decimal lineTotal = 0;
            decimal subTotal = 0;
            decimal discountAmount = 0;
            decimal totalDiscountmount = 0;
            decimal netAmount = 0;
            decimal totalNetAmount = 0;
            decimal taxAmount = 0;
            decimal totalTaxAmount = 0;
            decimal lineTotalFinal = 0;
            decimal OrderTotalAmount = 0;
            var newItemId = ((int?)_context.GettingSPOrder2.Max(inv => (int?)inv.OrderLineID) ?? 0);
            // var OrderLineSeq = ((int?)_context.GettingSPOrder2.Where(e => e.OrderID == model.OrderId).Max(inv => (int?)inv.OrderLineSeq) ?? 0);
            var GettingSPOrder2List = new List<GettingSPOrder2>();
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    continue;
                }

                try
                {
                    var seq = int.Parse(dt.Rows[i][0].ToString());
                    var itemNo = dt.Rows[i][1].ToString();
                    // var fob = decimal.Parse(dt.Rows[i][1].ToString());
                    var Qty = decimal.Parse(dt.Rows[i][2].ToString());
                    decimal fob = 0;
                    if (!string.IsNullOrEmpty(dt.Rows[i][3].ToString()))
                    {
                        fob = Convert.ToDecimal(dt.Rows[i][3].ToString());
                    }

                    //var discountPercent = string.IsNullOrEmpty(dt.Rows[i][3].ToString()) ? 0 : Convert.ToDecimal(dt.Rows[i][3].ToString());
                    //decimal discountAmountItem = string.IsNullOrEmpty(dt.Rows[i][4].ToString()) ? 0 : Convert.ToDecimal(dt.Rows[i][4].ToString());
                    //var taxPercent = string.IsNullOrEmpty(dt.Rows[i][5].ToString()) ? 0 : Convert.ToDecimal(dt.Rows[i][5].ToString());
                    //var taxAmountItem = string.IsNullOrEmpty(dt.Rows[i][6].ToString()) ? 0 : Convert.ToDecimal(dt.Rows[i][6].ToString());
                    //var localFlag = dt.Rows[i][7].ToString();
                    //var SupplierFlag = dt.Rows[i][8].ToString();

                    var item = _context.MasterSPItem.Where(e => e.ItemCode == itemNo && e.Status == (int)Status.Active).FirstOrDefault();
                    if (item == null)
                    {
                        InvalidItemNumber++;
                        continue;
                    }

                    var supplier = _context.GettingSPSupplier.Where(e => e.SupplierID == order.OrderSupplierID).FirstOrDefault();

                    var SPItemSupplier = _context.MasterSPItemSupplier.Where(e => e.ItemID == item.ItemID && e.SupplierID == order.OrderSupplierID && e.Cancelled == false).FirstOrDefault();
                    if (SPItemSupplier == null)
                    {
                        InvalidItemSupplier++;
                        continue;
                    }
                    if (string.IsNullOrEmpty(dt.Rows[i][3].ToString()))
                    {
                        fob = (decimal)SPItemSupplier.SupplierFOB;

                    }
                    var lotQTY = SPItemSupplier.ItemMinQty;
                    var mode = Qty % lotQTY;
                    if (mode > 0)
                    {
                        InvalidQTYCount++;
                        continue;
                    }
                    //var fob =(decimal) SPItemSupplier.SupplierFOB;
                    var discountPercent = _context.MasterLookup.Where(e => e.LookupID == supplier.SupplierDiscount)
                        .Select(e => Convert.ToDecimal(e.LookupName.Replace("%", ""))).FirstOrDefault();

                    var taxPercent = _context.MasterLookup.Where(e => e.LookupID == supplier.SupplierTaxType)
                      .Select(e => Convert.ToDecimal(e.LookupName.Replace("%", ""))).FirstOrDefault();


                    //if (fob <= 0)
                    //{
                    //    InvalidFOB++;
                    //    continue;
                    //}

                    if (Qty <= 0)
                    {
                        InvalidOrderedQty++;
                        continue;
                    }
                    if (Qty < SPItemSupplier.ItemMinQty || Qty > SPItemSupplier.ItemMaxQty)
                    {
                        InvalidOrderedQty++;
                        continue;
                    }

                    //if (discountPercent > 100 || discountPercent < 0)
                    //{

                    //    InvalidDiscountPercent++;
                    //    continue;
                    //}
                    //if (InvalidTaxPercent > 100 || InvalidTaxPercent < 0)
                    //{

                    //    InvalidTaxPercent++;
                    //    continue;
                    //}


                    //if (discountAmountItem > 0 && discountPercent > 0 && discountAmount != discountAmountItem)
                    //{
                    //    InvalidDiscountAmount++;
                    //    continue;
                    //}
                    //if (taxAmountItem > 0 && taxPercent > 0 && taxAmount != taxAmountItem)
                    //{
                    //    InvalidDiscountAmount++;
                    //    continue;
                    //}



                    //   var orderFlagId = _lookupService.GeSpLookup(userId, localFlag, (int)LookupTypes.OrderFlage);
                    //  var supplierFlagId = _lookupService.GeSpLookup(userId, SupplierFlag, (int)LookupTypes.SupplierFlage);


                    lineTotal = decimal.Round(Qty * fob, 3, MidpointRounding.AwayFromZero);

                    //if (discountAmountItem > 0 && taxPercent <= 0)
                    //{
                    //    discountPercent = discountAmountItem / (lineTotal / 100);
                    //    if (discountPercent > 100)
                    //    {
                    //        InvalidDiscountAmount++;
                    //        continue;
                    //    }
                    //}
                    var existItem = _context.GettingSPOrder2.Where(e => e.Cancelled == false && e.OrderID == model.OrderId && e.OrderItemID == item.ItemID).FirstOrDefault();
                    if (existItem != null)
                    {
                        existItem.OrderItemQty = Qty;
                        existItem.OrderItemPrice = fob;
                        existItem.OrderLineSeq = seq;

                        discountAmount = decimal.Round(lineTotal * existItem.OrderItemDisPer / 100, 3, MidpointRounding.AwayFromZero);
                        totalDiscountmount += discountAmount;
                        totalDiscountmount -= existItem.OrderItemDisAmount;

                        netAmount = decimal.Round(lineTotal - discountAmount, 3, MidpointRounding.AwayFromZero);
                        totalNetAmount += netAmount;
                        totalNetAmount -= existItem.OrderItemNetAmount;

                        taxAmount = decimal.Round(taxPercent * netAmount / 100, 3, MidpointRounding.AwayFromZero);
                        totalTaxAmount += taxAmount;

                        lineTotalFinal = netAmount + taxAmount;

                        totalTaxAmount -= existItem.OrderItemTaxAmount;

                        OrderTotalAmount += lineTotalFinal;
                        OrderTotalAmount -= existItem.OrderItemTotalAmount;
                        order.OrderTotalAmountJOD -= decimal.Round(existItem.OrderItemTotalAmount * currencyRate, 3, MidpointRounding.AwayFromZero);

                        existItem.OrderItemAmount = lineTotal;
                        existItem.OrderItemNetAmount = netAmount;
                        existItem.OrderItemTaxAmount = taxAmount;
                        existItem.OrderItemDisAmount = discountAmount;
                        existItem.OrderItemTotalAmount = lineTotalFinal;


                        DuplicateItemCount++;
                        continue;

                    }
                    discountAmount = decimal.Round(lineTotal * discountPercent / 100, 3, MidpointRounding.AwayFromZero);

                    totalDiscountmount += discountAmount;


                    netAmount = decimal.Round(lineTotal - discountAmount, 3, MidpointRounding.AwayFromZero);
                    totalNetAmount += netAmount;
                    //if (taxAmountItem > 0 && taxPercent <= 0)
                    //{
                    //    taxPercent = taxAmountItem / (netAmount / 100);
                    //    if (taxPercent > 100)
                    //    {
                    //        InvalidTaxamount++;
                    //        continue;
                    //    }
                    //}
                    taxAmount = decimal.Round(taxPercent * netAmount / 100, 3, MidpointRounding.AwayFromZero);
                    totalTaxAmount += taxAmount;

                    lineTotalFinal = netAmount + taxAmount;
                    OrderTotalAmount += lineTotalFinal;


                    newItemId++;
                    // OrderLineSeq++;
                    var orderItem = new GettingSPOrder2
                    {
                        OrderLineID = newItemId,
                        OrderItemID = item.ItemID,
                        OrderID = model.OrderId,
                        OrderItemQty = Qty,
                        OrderItemPrice = (decimal)fob,

                        OrderItemTaxAmount = taxAmount,
                        OrderItemTaxPer = taxPercent,

                        OrderItemDisAmount = discountAmount,
                        OrderItemDisPer = discountPercent,
                        OrderItemTax = supplier.SupplierTaxType,
                        OrderItemDis = (int)supplier.SupplierDiscount,

                        OrderItemNetAmount = netAmount,
                        OrderItemOrderFlag = item.ItemFlagOrder,
                        OrderItemSupplierFlag = item.ItemFlagSup,

                        OrderItemAmount = lineTotal,
                        OrderLineSeq = seq,
                        OrderItemTotalAmount = lineTotalFinal,
                        Status = (int)Status.Active,
                        Cancelled = false,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay,
                        EnterUser = userId,
                        OrderItemQtyOrderMax = item.ItemMaxOrder,
                        OrderItemQtyOrderMin = item.ItemMinOrder,


                    };
                    GettingSPOrder2List.Add(orderItem);
                    UploadedItemsCount++;

                }
                catch
                {
                    FailedToUpload++;
                }



            }

            await _context.GettingSPOrder2.AddRangeAsync(GettingSPOrder2List);
            subTotal = decimal.Round(GettingSPOrder2List.Sum(e => e.OrderItemPrice * e.OrderItemQty), 3, MidpointRounding.AwayFromZero);
            order.OrderTaxAmount += totalTaxAmount;
            order.OrderDisAmount += totalDiscountmount;
            order.OrderNetAmount += totalNetAmount;
            order.OrderAmount += subTotal;
            order.OrderTotalAmountCur += decimal.Round(OrderTotalAmount, 3, MidpointRounding.AwayFromZero);
            order.OrderTotalAmountJOD += decimal.Round(OrderTotalAmount * currencyRate, 3, MidpointRounding.AwayFromZero);

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = new
            {
                UploadedItemsCount,
                InvalidItemNumber,
                DuplicateItemCount,
                InvalidFOB,
                InvalidOrderedQty,
                InvalidDiscountPercent,
                InvalidDiscountAmount,
                InvalidTaxPercent,
                InvalidTaxamount,
                InvalidItemSupplier,
                FailedToUpload,
                InvalidQTYCount
            };

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditSPOrder(EditSPOrderModel model, int userId)
        {

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var order = _context.GettingSPOrder1.Where(e => e.OrderID == model.OrderId).FirstOrDefault();
            if (order == null)
            {
                throw new ManagerProcessException("000132");
            }
            if (order.Cancelled == true)
            {
                throw new ManagerProcessException("000071");
            }
            if (order.OrderApproval != (int)OrderStatus.Draft && order.OrderApproval != (int)OrderStatus.NeedRevision)
            {
                throw new ManagerProcessException("000133");
            }
            var supplier = _context.GettingSPSupplier.Where(e => e.SupplierID == model.OrderSupplierId).FirstOrDefault();
            if (supplier == null)
            {
                throw new ManagerProcessException("000007");
            }
            //var currencyRate = (from currency in _contextMaster.MasterLookup
            //                 //   join rate in _contextMaster.MasterCurrencyRate on currency.LookupName equals rate.Currency
            //                    where currency.LookupID == supplier.SupplierCurrency
            //                    select Convert.ToDecimal(currency.LookupValue)).FirstOrDefault();
            //if (currencyRate > 0)
            //{
            //    currencyRate = decimal.Round(currencyRate, 3, MidpointRounding.AwayFromZero);
            //}
            //else
            //{
            //    currencyRate = 1;
            //}
            if (model.TaxPercentage > 100 || model.TaxPercentage < 0)
            {

                throw new ManagerProcessException("000126");
            }
            if (model.DiscountPercentage > 100 || model.DiscountPercentage < 0)
            {

                throw new ManagerProcessException("000089");
            }
            if (model.DiscountAmount < 0)
            {
                throw new ManagerProcessException("000020");
            }
            if (model.TaxAmount < 0)
            {
                throw new ManagerProcessException("000019");
            }
            if (model.NetAmount < 0)
            {
                throw new ManagerProcessException("000127");
            }
            if (model.OrderSource != null)
            {
                var isValid = _context.MasterSPLookup
                    .Where(e => e.LookupID == model.OrderSource && e.LookupTypeID == (int)LookupTypes.OrderSource).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000157");
                }
            }
            if (model.OrderSource != null)
            {
                var isValid = _context.MasterSPLookup
                    .Where(e => e.LookupID == model.OrderSource && e.LookupTypeID == (int)LookupTypes.OrderSource).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000157");
                }
            }



            var isValids = _context.MasterSPLookup.Where(e => e.LookupID == model.OrderType && e.LookupTypeID == (int)LookupTypes.OrderType).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000121");
            }
            isValids = _context.MasterSPLookup.Where(e => e.LookupID == model.OrderMethod && e.LookupTypeID == (int)LookupTypes.OrderMethod).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000122");
            }
            isValids = _context.MasterSPLookup.Where(e => e.LookupID == model.OrderFrieght && e.LookupTypeID == (int)LookupTypes.OrderFrieght).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000123");
            }
            if (model.OrderBusinessArea != null)
            {
                var valid = _context.MasterBusinessArea
                     .Where(e => e.Status == (int)Status.Active && e.BusinessAreaID == model.OrderBusinessArea).Any();
                if (!valid)
                {
                    throw new ManagerProcessException("000145");
                }
                order.OrderBusinessArea = model.OrderBusinessArea;
            }
            if (order.OrderApproval == (int)OrderStatus.NeedRevision)
            {
                var newId = ((int?)_context.GettingSPOrderHistory1.Max(inv => (int?)inv.OrderID) ?? 0) + 1;


                var version = _context.GettingSPOrderHistory1.Where(e => e.OrignalOrderID == order.OrderID).Count() + 1;
                var orderHistory = new GettingSPOrderHistory1
                {
                    OrderID = newId,
                    Versions = version,
                    OrignalOrderID = model.OrderId,
                    OrderNo = order.OrderNo,
                    OrderSeq = order.OrderSeq,
                    OrderTaxAmount = order.OrderTaxAmount,
                    OrderTaxPer = order.OrderTaxPer,
                    OrderDisAmount = order.OrderDisAmount,
                    OrderDisPer = order.OrderDisPer,

                    OrderCurrencyExchangeDate = order.OrderCurrencyExchangeDate,
                    OrderCurrencyExchangeRate = order.OrderCurrencyExchangeRate,
                    OrderSupplierID = order.OrderSupplierID,
                    OrderCurrency = order.OrderCurrency,
                    OrderNetAmount = order.OrderNetAmount,
                    OrderAmount = order.OrderAmount,
                    OrderTotalAmountCur = order.OrderTotalAmountCur,
                    OrderTotalAmountJOD = order.OrderTotalAmountJOD,
                    SourceSequence = order.SourceSequence,
                    OrderSource = order.OrderSource,
                    OrderApproval = order.OrderApproval,
                    OrderDate = order.OrderDate,
                    Cancelled = false,
                    EnterDate = order.EnterDate,
                    EnterTime = order.EnterTime,
                    EnterUser = order.EnterUser,
                    Status = order.Status,
                    OrderGroup = order.OrderGroup,
                    OrderMethod = order.OrderMethod,
                    OrderFreight = order.OrderFreight,
                    OrderType = order.OrderType,
                    OrderComments = order.OrderComments,
                    OrderBusinessArea = order.OrderBusinessArea,

                };
                await _context.GettingSPOrderHistory1.AddAsync(orderHistory);
                var newItemIdHistory = ((int?)_context.GettingSPOrderHistory2.Max(inv => (int?)inv.OrderLineID) ?? 0);
                var oldItems = _context.GettingSPOrder2.Where(e => e.OrderID == model.OrderId)
                    .Select(e => new GettingSPOrderHistory2
                    {
                        OrderLineID = e.OrderLineID,
                        OrderID = e.OrderID,
                        OrderLineSeq = e.OrderLineSeq,
                        OrderItemID = e.OrderItemID,
                        OrderItemQty = e.OrderItemQty,
                        OrderItemQtyCancelled = e.OrderItemQtyCancelled,
                        OrderItemQtyConfirmed = e.OrderItemQtyConfirmed,
                        OrderItemPrice = e.OrderItemPrice,
                        OrderItemPriceConfirmed = e.OrderItemPriceConfirmed,
                        OrderItemAmount = e.OrderItemAmount,
                        OrderItemDisPer = e.OrderItemDisPer,
                        OrderItemDisAmount = e.OrderItemDisAmount,
                        OrderItemNetAmount = e.OrderItemNetAmount,
                        OrderItemTaxPer = e.OrderItemTaxPer,
                        OrderItemTaxAmount = e.OrderItemTaxAmount,
                        OrderItemTotalAmount = e.OrderItemTotalAmount,
                        OrderItemSupplierFlag = e.OrderItemSupplierFlag,
                        OrderItemOrderFlag = e.OrderItemOrderFlag,
                        OrderItemQtyOnHand = e.OrderItemQtyOnHand,
                        OrderItemQtyOnOrder = e.OrderItemQtyOnOrder,
                        OrderItemQtyOrderMin = e.OrderItemQtyOrderMin,
                        OrderItemQtyOrderMax = e.OrderItemQtyOrderMax,

                        Status = e.Status,
                        Cancelled = e.Cancelled,
                        CancelDate = e.CancelDate,
                        EnterUser = e.EnterUser,
                        EnterDate = e.EnterDate,
                        EnterTime = e.EnterTime,
                        ModUser = e.ModUser,
                        ModDate = e.ModDate,
                        ModTime = e.ModTime
                    }).ToList();

                foreach (var item in oldItems)
                {
                    newItemIdHistory++;
                    item.OrderID = newId;
                    item.OrderLineID = newItemIdHistory;
                }
                _context.GettingSPOrderHistory2.AddRange(oldItems);
            }

            var newItemId = ((int?)_context.GettingSPOrder2.Max(inv => (int?)inv.OrderLineID) ?? 0);


            decimal lineTotal = 0;
            decimal subTotal = decimal.Round(model.ItemsModel.Sum(e => e.ItemPrice * e.Qty), 3, MidpointRounding.AwayFromZero);
            decimal discountAmount = 0;
            decimal totalDiscountmount = 0;
            decimal netAmount = 0;
            decimal totalNetAmount = 0;
            decimal taxAmount = 0;
            decimal totalTaxAmount = 0;
            decimal lineTotalFinal = 0;
            decimal OrderTotalAmount = 0;
            int OrderLineSeq = 0;
            List<GettingSPOrder2> GettingSPOrder2 = new List<GettingSPOrder2>();
            foreach (var item in model.ItemsModel)
            {
                if (item.ItemDiscountPercentage > 100 || item.ItemDiscountPercentage < 0)
                {

                    throw new ManagerProcessException("000089");
                }
                if (item.ItemTaxPercentage > 100 || item.ItemTaxPercentage < 0)
                {

                    throw new ManagerProcessException("000126");
                }
                var spItem = _context.MasterSPItem.Where(e => e.ItemID == item.ItemId).FirstOrDefault();
                if (spItem == null)
                {
                    throw new ManagerProcessException("000045");
                }
                isValids = _context.MasterSPItemSupplier.Where(e => e.ItemID == item.ItemId && e.SupplierID == model.OrderSupplierId && e.Cancelled == false).Any();
                if (!isValids)
                {
                    throw new ManagerProcessException("000131");
                }

                if (item.Qty <= 0)
                {
                    throw new ManagerProcessException("000124");
                }
                if (item.ItemPrice <= 0)
                {
                    throw new ManagerProcessException("000125");
                }
                if (item.ItemTotal < 0)
                {
                    throw new ManagerProcessException("000125");
                }
                if (item.ItemNetAmount < 0)
                {
                    throw new ManagerProcessException("000127");
                }
                if (item.ItemDiscountAmount < 0)
                {
                    throw new ManagerProcessException("000020");
                }
                //if (item.Qty < spItem.ItemMinOrder || item.Qty > spItem.ItemMaxOrder)
                //{
                //    throw new ManagerProcessException("000129");
                //}
                if (item.SupplierFlag != null)
                {
                    isValids = _context.MasterSPLookup
                        .Where(e => e.Status == (int)Status.Active && e.LookupID == item.SupplierFlag && e.LookupTypeID == (int)LookupTypes.SupplierFlage).Any();
                    if (!isValids)
                    {
                        throw new ManagerProcessException("000068");
                    }
                }
                if (item.SupplierFlag != null)
                {
                    isValids = _context.MasterSPLookup
                   .Where(e => e.Status == (int)Status.Active && e.LookupID == item.OrderItemOrderFlag && e.LookupTypeID == (int)LookupTypes.OrderFlage).Any();
                    if (!isValids)
                    {
                        throw new ManagerProcessException("000130");
                    }
                }
                lineTotal = decimal.Round(item.Qty * item.ItemPrice, 3, MidpointRounding.AwayFromZero);


                discountAmount = decimal.Round(lineTotal * item.ItemDiscountPercentage / 100, 3, MidpointRounding.AwayFromZero);

                totalDiscountmount += discountAmount;


                netAmount = decimal.Round(lineTotal - discountAmount, 3, MidpointRounding.AwayFromZero);
                totalNetAmount += netAmount;

                taxAmount = decimal.Round(item.ItemTaxPercentage * netAmount / 100, 3, MidpointRounding.AwayFromZero);
                totalTaxAmount += taxAmount;

                lineTotalFinal = netAmount + taxAmount;
                OrderTotalAmount += lineTotalFinal;

                newItemId++;
                OrderLineSeq++;
                var orderItem = new GettingSPOrder2
                {
                    OrderLineID = newItemId,
                    OrderItemID = item.ItemId,
                    OrderID = order.OrderID,
                    OrderItemQty = item.Qty,
                    OrderItemPrice = item.ItemPrice,

                    OrderItemTaxAmount = taxAmount,
                    OrderItemTaxPer = item.ItemTaxPercentage,

                    OrderItemDisAmount = discountAmount,
                    OrderItemDisPer = item.ItemDiscountPercentage,
                    OrderItemTax = item.OrderItemTax,
                    OrderItemDis = item.OrderItemDis,

                    OrderItemNetAmount = netAmount,
                    OrderItemOrderFlag = item.OrderItemOrderFlag,
                    OrderItemSupplierFlag = item.SupplierFlag,
                    OrderItemQtyOrderMin = spItem.ItemMinOrder,
                    OrderItemQtyOrderMax = spItem.ItemMaxOrder,
                    OrderItemAmount = lineTotal,
                    OrderLineSeq = OrderLineSeq,
                    OrderItemTotalAmount = lineTotalFinal,
                    Status = (int)Status.Active,
                    Cancelled = false,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay,
                    EnterUser = userId,


                };
                GettingSPOrder2.Add(orderItem);
            }

            _context.GettingSPOrder2.Where(e => e.OrderID == model.OrderId).ExecuteDelete();
            await _context.GettingSPOrder2.AddRangeAsync(GettingSPOrder2);
            order.OrderTaxAmount = totalTaxAmount;
            order.OrderTaxPer = model.TaxPercentage;
            order.OrderDisAmount = totalDiscountmount;
            order.OrderDisPer = model.DiscountPercentage;
            //   order.OrderCurrencyExchangeDate = DateTime.Now,
            order.OrderCurrencyExchangeRate = model.OrderCurrencyRate;
            order.OrderSupplierID = model.OrderSupplierId;
            order.OrderCurrency = model.CurrencyLookup;
            order.OrderNetAmount = totalNetAmount;
            order.OrderAmount = subTotal;
            order.SourceSequence = model.SourceSequence;
            order.OrderSource = model.OrderSource;
            order.OrderTotalAmountCur = decimal.Round(OrderTotalAmount, 3, MidpointRounding.AwayFromZero);
            order.OrderTotalAmountJOD = decimal.Round(OrderTotalAmount * model.OrderCurrencyRate, 3, MidpointRounding.AwayFromZero);


            order.ModDate = DateTime.Now;
            order.ModTime = DateTime.Now.TimeOfDay;
            order.ModUser = userId;
            order.OrderMethod = model.OrderMethod;
            order.OrderFreight = model.OrderFrieght;
            order.OrderType = model.OrderType;
            order.OrderComments = model.Comments;





            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = model.OrderId;

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteSPOrder(string OrderId, int userId)
        {


            var orders = _context.GettingSPOrder1.Where(e => ("," + OrderId + ",").Contains("," + e.OrderID.ToString() + ",")).ToList();
            foreach (var order in orders)
            {
                if (order == null)
                {
                    throw new ManagerProcessException("000132");
                }
                if (order.OrderApproval != (int)OrderStatus.Draft)
                {
                    throw new ManagerProcessException("000134");
                }
                if (order.Cancelled == true)
                {
                    throw new ManagerProcessException("000071");
                }
                order.CancelDate = DateTime.Now;
                order.Cancelled = true;
                order.Status = (int)Status.Deleted;
                order.ModUser = userId;
            }


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<object> GetFlagsSummery(int OrderId)
        {
            var SuppliersFlag = _context.GettingSPOrder2.Where(e => e.Cancelled == false && e.OrderID == OrderId)
                  .Select(e => new
                  {
                      e.OrderLineID,
                      e.OrderLineSeq,
                      e.OrderItemID,
                      SupplierItemNumber = e.MasterSPItem.ItemCode,
                      e.MasterSPItem.MasterSPGeneralItemName.ItemNameEn,
                      SupplierFlag = e.SupplierFlag == null ? null : e.SupplierFlag.LookupName,
                      SupplierFlagId = e.SupplierFlag == null ? 0 : e.SupplierFlag.LookupID,
                      FlagDescription = e.SupplierFlag == null ? null : e.SupplierFlag.LookupDesc,
                      LookupAction = e.SupplierFlag == null ? null : e.SupplierFlag.LookupAction,
                      LookupActionName = e.SupplierFlag == null ? null : e.SupplierFlag.LookupActionObj.LookupName,
                      e.OrderItemQty,
                      Substitute = e.MasterSPItem.MasterSPItemSubstitute == null ? null : e.MasterSPItem.MasterSPItemSubstitute.OrderByDescending(x => x.EnterDate)
                      .Select(x => new { x.MasterSPItem.ItemCode, x.MasterSPItem.MasterSPGeneralItemName.ItemNameEn }).FirstOrDefault(),
                  }).ToList();

            var OrderFlags = _context.GettingSPOrder2.Where(e => e.Cancelled == false && e.OrderID == OrderId)
              .Select(e => new
              {
                  e.OrderLineID,
                  e.OrderLineSeq,
                  e.OrderItemID,
                  SupplierItemNumber = e.MasterSPItem.ItemCode,
                  e.MasterSPItem.MasterSPGeneralItemName.ItemNameEn,
                  SupplierFlag = e.OrderFlag == null ? null : e.OrderFlag.LookupName,
                  SupplierFlagId = e.OrderFlag == null ? 0 : e.OrderFlag.LookupID,
                  FlagDescription = e.OrderFlag == null ? null : e.OrderFlag.LookupDesc,
                  LookupAction = e.OrderFlag == null ? null : e.OrderFlag.LookupAction,
                  LookupActionName = e.OrderFlag == null ? null : e.OrderFlag.LookupActionObj.LookupName,
                  e.OrderItemQty,
                  Substitute = e.MasterSPItem.MasterSPItemSubstitute == null ? null : e.MasterSPItem.MasterSPItemSubstitute.OrderByDescending(x => x.EnterDate)
                  .Select(x => new { x.MasterSPItem.ItemCode, x.MasterSPItem.MasterSPGeneralItemName.ItemNameEn }).FirstOrDefault(),
              }).ToList();

            var DuplicatesIds = _context.GettingSPOrder2
              .Where(e => e.Cancelled == false && e.OrderID == OrderId)
              .GroupBy(e => e.OrderItemID)
              .Where(g => g.Count() > 1).Select(e => e.Key).ToList();




            var Duplicates = _context.GettingSPOrder2
                .Where(e => e.Cancelled == false && e.OrderID == OrderId && DuplicatesIds.Contains(e.OrderItemID))

                .Select(e => new
                {
                    e.OrderLineID,
                    e.OrderLineSeq,
                    e.OrderItemID,
                    SupplierItemNumber = e.MasterSPItem.ItemCode,
                    e.MasterSPItem.MasterSPGeneralItemName.ItemNameEn,
                    Flag = _context.MasterSPLookup.Where(x => x.LookupID == (int)Lookups.DUB)
                    .Select(x => new { x.LookupID, x.LookupName, x.LookupDesc, x.LookupAction, LookupActionName = x.LookupActionObj.LookupName }).FirstOrDefault(),

                    e.OrderItemQty,
                    Substitute = e.MasterSPItem.MasterSPItemSubstitute == null ? null : e.MasterSPItem.MasterSPItemSubstitute.OrderByDescending(x => x.EnterDate)
                    .Select(x => new { x.MasterSPItem.ItemCode, x.MasterSPItem.MasterSPGeneralItemName.ItemNameEn }).FirstOrDefault(),
                }).OrderBy(e => e.OrderItemID).ToList();

            var HighFOBValue = _context.MasterSPLookup.Where(e => e.LookupID == (int)Lookups.HighFOB).Select(e => (decimal)e.LookupValue).FirstOrDefault();

            var HighWeightValue = _context.MasterSPLookup.Where(e => e.LookupID == (int)Lookups.HighWeight).Select(e => e.LookupValue).FirstOrDefault();

            var HighVolumeValue = _context.MasterSPLookup.Where(e => e.LookupID == (int)Lookups.HighVolume).Select(e => e.LookupValue).FirstOrDefault();


            var Irregularities = _context.GettingSPOrder2
                .Where(e => e.Cancelled == false && e.OrderID == OrderId &&
                (
                (e.OrderItemQtyOrderMin > e.OrderItemQty || e.OrderItemQtyOrderMax < e.OrderItemQty) ||
                e.OrderItemTotalAmount > HighFOBValue ||
                 e.MasterSPItem.ItemWeight > HighWeightValue ||
                 e.MasterSPItem.ItemSizeM3 > HighVolumeValue
                )

                )
       .Select(e => new IrregularitiesModel
       {
           OrderLineID = e.OrderLineID,
           OrderLineSeq = e.OrderLineSeq,
           OrderItemID = e.OrderItemID,
           SupplierItemNumber = e.MasterSPItem.ItemCode,
           TotalLine = e.OrderItemTotalAmount,
           ItemNameEn = e.MasterSPItem.MasterSPGeneralItemName.ItemNameEn,
           FlagMinMax = e.OrderItemQtyOrderMin > e.OrderItemQty || e.OrderItemQtyOrderMax < e.OrderItemQty ?

           _context.MasterSPLookup.Where(x => x.LookupID == (int)Lookups.ExceedMinMax)
           .Select(x => new { x.LookupID, IrregularitiesDescription = x.LookupName, x.LookupDesc, MaxValue = x.LookupValue, x.LookupAction, LookupActionName = x.LookupActionObj.LookupName }).FirstOrDefault() : null,

           FlagHighFOB = e.OrderItemPrice > HighFOBValue ?
                     _context.MasterSPLookup.Where(x => x.LookupID == (int)Lookups.HighFOB)
           .Select(x => new { x.LookupID, IrregularitiesDescription = x.LookupName, x.LookupDesc, MaxValue = x.LookupValue, x.LookupAction, LookupActionName = x.LookupActionObj.LookupName }).FirstOrDefault() : null,

           FlagHighWeight = e.MasterSPItem.ItemWeight > HighWeightValue ?
                     _context.MasterSPLookup.Where(x => x.LookupID == (int)Lookups.HighWeight)
           .Select(x => new { x.LookupID, IrregularitiesDescription = x.LookupName, x.LookupDesc, MaxValue = x.LookupValue, x.LookupAction, LookupActionName = x.LookupActionObj.LookupName }).FirstOrDefault() : null,

           FlagHighVolume = e.MasterSPItem.ItemSizeM3 > HighVolumeValue ?
                     _context.MasterSPLookup.Where(x => x.LookupID == (int)Lookups.HighVolume)
           .Select(x => new { x.LookupID, IrregularitiesDescription = x.LookupName, x.LookupDesc, MaxValue = x.LookupValue, x.LookupAction, LookupActionName = x.LookupActionObj.LookupName }).FirstOrDefault() :
           null,
           FlagMixed = e.MasterSPItem.ItemSizeM3 > HighVolumeValue ?
                     _context.MasterSPLookup.Where(x => x.LookupID == (int)Lookups.Mixed)
           .Select(x => new { x.LookupID, IrregularitiesDescription = x.LookupName, x.LookupDesc, x.LookupAction, LookupActionName = x.LookupActionObj.LookupName }).FirstOrDefault() :
           null,
           OrderItemQty = e.OrderItemQty,
           OrderItemQtyOrderMin = e.OrderItemQtyOrderMin,
           OrderItemQtyOrderMax = e.OrderItemQtyOrderMax,
           ItemWeight = e.MasterSPItem.ItemWeight,
           ItemSizeM3 = e.MasterSPItem.ItemSizeM3,
           ItemHeight = e.MasterSPItem.ItemHeight,
           OrderItemPrice = e.OrderItemPrice,
           Substitute = e.MasterSPItem.MasterSPItemSubstitute == null ? null : e.MasterSPItem.MasterSPItemSubstitute.OrderByDescending(x => x.EnterDate)
           .Select(x => new { x.MasterSPItem.ItemCode, x.MasterSPItem.MasterSPGeneralItemName.ItemNameEn }).FirstOrDefault(),
       }).ToList();
            foreach (var item in Irregularities)
            {
                int count = 0;
                if (item.FlagMinMax != null)
                {
                    count++;
                }
                if (item.FlagHighFOB != null)
                {
                    count++;
                }
                if (item.FlagHighWeight != null)
                {
                    count++;
                }
                if (item.FlagHighVolume != null)
                {
                    count++;
                }
                if (count < 2)
                {
                    item.FlagMixed = null;
                }
            }


            var StockMonthMinMAX = _context.GettingSPOrder2
              .Where(e => e.Cancelled == false && e.OrderID == OrderId &&
              (
              e.MasterSPItem.ItemMinOrder > e.OrderItemQty || e.MasterSPItem.ItemMaxOrder < e.OrderItemQty

              )

              )
     .Select(e => new
     {
         OrderLineID = e.OrderLineID,
         OrderLineSeq = e.OrderLineSeq,
         OrderItemID = e.OrderItemID,
         SupplierItemNumber = e.MasterSPItem.ItemCode,
         ItemNameEn = e.MasterSPItem.MasterSPGeneralItemName.ItemNameEn,
         e.MasterSPItem.ItemMinOrder,
         e.MasterSPItem.ItemMaxOrder,
         e.OrderItemQty,
         Flag =

         _context.MasterSPLookup.Where(x => x.LookupID == (int)Lookups.ExceedMinMaxLibrary)
         .Select(x => new { x.LookupID, x.LookupName, x.LookupDesc, x.LookupAction, LookupActionName = x.LookupActionObj.LookupName }).FirstOrDefault(),



         Substitute = e.MasterSPItem.MasterSPItemSubstitute == null ? null : e.MasterSPItem.MasterSPItemSubstitute.OrderByDescending(x => x.EnterDate)
         .Select(x => new { x.MasterSPItem.ItemCode, x.MasterSPItem.MasterSPGeneralItemName.ItemNameEn }).FirstOrDefault(),
     }).ToList();

            return new { SuppliersFlag, OrderFlags, Duplicates, Irregularities, StockMonthMinMAX };


        }
        public async ValueTask<ApiResponseModel> DeleteOrderItem(string OrderLineID, int userId)
        {



            decimal totalDiscountmount = 0;

            decimal totalNetAmount = 0;

            decimal totalTaxAmount = 0;

            decimal OrderTotalAmount = 0;
            decimal subTotal = 0;

            var orderMain = _context.GettingSPOrder2
                .Where(e => ("," + OrderLineID + ",").Contains("," + e.OrderLineID.ToString() + ",")).Select(e => e.GettingSPOrder1).FirstOrDefault();


            var orders = _context.GettingSPOrder2.Where(e => ("," + OrderLineID + ",").Contains("," + e.OrderLineID.ToString() + ",")).ToList();
            foreach (var order in orders)
            {




                totalDiscountmount += order.OrderItemDisAmount;


                totalTaxAmount += order.OrderItemTaxAmount;
                subTotal = order.OrderItemAmount;
                totalNetAmount = order.OrderItemNetAmount;

                OrderTotalAmount += order.OrderItemTotalAmount;
                if (order == null)
                {
                    throw new ManagerProcessException("000132");
                }

                if (order.Cancelled == true)
                {
                    throw new ManagerProcessException("000071");
                }
                order.CancelDate = DateTime.Now;
                order.Cancelled = true;
                order.Status = (int)Status.Deleted;
                order.ModUser = userId;
            }
            // _context.GettingSPOrder1.Update(orderMain);


            orderMain.OrderTaxAmount -= totalTaxAmount;
            orderMain.OrderDisAmount -= totalDiscountmount;
            orderMain.OrderNetAmount -= totalNetAmount;
            orderMain.OrderAmount -= subTotal;
            orderMain.OrderTotalAmountCur -= OrderTotalAmount;
            orderMain.OrderTotalAmountJOD -= decimal.Round(OrderTotalAmount * orderMain.OrderCurrencyExchangeRate, 3, MidpointRounding.AwayFromZero);
            _context.GettingSPOrder1.Update(orderMain);


            var result = await _context.SaveChangesAsync();



            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditOrderItem(int OrderLineID, int Qty, int userId)
        {


            var order = _context.GettingSPOrder2.Where(e => e.OrderLineID == OrderLineID).FirstOrDefault();
            if (order == null)
            {
                throw new ManagerProcessException("000132");
            }
            if (order.Cancelled == true)
            {
                throw new ManagerProcessException("000071");
            }
            var orderMain = _context.GettingSPOrder1
              .Where(e => e.OrderID == order.OrderID).FirstOrDefault();
            var orginalQty = order.OrderItemQty;
            orderMain.OrderTaxAmount -= order.OrderItemTaxAmount;
            orderMain.OrderDisAmount -= order.OrderItemDisAmount;
            orderMain.OrderNetAmount -= order.OrderItemNetAmount;
            orderMain.OrderAmount -= order.OrderItemAmount;
            orderMain.OrderTotalAmountCur -= order.OrderItemTotalAmount;
            orderMain.OrderTotalAmountJOD -= decimal.Round(order.OrderItemTotalAmount * orderMain.OrderCurrencyExchangeRate, 3, MidpointRounding.AwayFromZero);


            order.OrderItemQty = Qty;
            order.OrderItemAmount = (order.OrderItemAmount / orginalQty) * Qty;
            order.OrderItemDisAmount = (order.OrderItemDisAmount / orginalQty) * Qty;
            order.OrderItemNetAmount = (order.OrderItemNetAmount / orginalQty) * Qty;
            order.OrderItemTaxAmount = (order.OrderItemTaxAmount / orginalQty) * Qty;
            order.OrderItemTotalAmount = (order.OrderItemTotalAmount / orginalQty) * Qty;


            orderMain.OrderTaxAmount += order.OrderItemTaxAmount;
            orderMain.OrderDisAmount += order.OrderItemDisAmount;
            orderMain.OrderNetAmount += order.OrderItemNetAmount;
            orderMain.OrderAmount += order.OrderItemAmount;
            orderMain.OrderTotalAmountCur += order.OrderItemTotalAmount;
            orderMain.OrderTotalAmountJOD += decimal.Round(order.OrderItemTotalAmount * orderMain.OrderCurrencyExchangeRate, 3, MidpointRounding.AwayFromZero);



            order.ModDate = DateTime.Now;
            order.ModTime = DateTime.Now.TimeOfDay;
            var qtyProcessed = _context.GettingSPOrderPOSS2.Where(e => e.Cancelled == false && e.POSSLineID == OrderLineID).Sum(e => e.Accepted_Qty);
            if (qtyProcessed >= Qty)
            {
                order.Status = (int)OrderStatus.Processed;
            }


            order.ModUser = userId;
            await _context.SaveChangesAsync();
            //var order1 = _context.GettingSPOrder1.Where(e => e.OrderID == order.OrderID).FirstOrDefault();
            //if (order != null)
            //{
            //    var UnPorcessed = _context.GettingSPOrder2.Where(e => e.Cancelled == false && e.OrderID == order.OrderID && e.Status != (int)OrderStatus.Processed).Any();
            //    if (!UnPorcessed)
            //    {
            //        order.Status = (int)OrderStatus.Processed;
            //    }
            //}

            var result = await _context.SaveChangesAsync();



            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> SkipPosss(int OrderId, int userId)
        {


            var order = _context.GettingSPOrder1.Where(e => e.OrderID == OrderId).FirstOrDefault();

            if (order == null)
            {
                throw new ManagerProcessException("000132");
            }
            if (order.OrderApproval != (int)OrderStatus.Approved)
            {
                throw new ManagerProcessException("000158");
            }

            order.ModDate = DateTime.Now;
            order.ModTime = DateTime.Now.TimeOfDay;
            order.OrderApproval = (int)OrderStatus.LookedForPermissions;
            order.ModUser = userId;


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> WithdrawOrder(int OrderId, int userId)
        {


            var order = _context.GettingSPOrder1.Where(e => e.OrderID == OrderId).FirstOrDefault();

            if (order == null)
            {
                throw new ManagerProcessException("000132");
            }
            if (order.OrderApproval != (int)OrderStatus.Pending)
            {
                throw new ManagerProcessException("000161");
            }

            order.ModDate = DateTime.Now;
            order.ModTime = DateTime.Now.TimeOfDay;
            order.OrderApproval = (int)OrderStatus.Draft;
            order.ModUser = userId;


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            _context.GettingSPOrder2.Where(e => e.OrderID == OrderId)
              .ExecuteUpdate(e => e.SetProperty(x => x.Status, (int)OrderStatus.Draft));

            _contextMaster.MasterApproval.Where(e => e.RequestRecord == OrderId && e.Status == (int)Status.Active)
                .ExecuteUpdate(e => e.SetProperty(x => x.Status, (int)Status.Deleted));


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> ConfirmOrder(ConfirmOrderModel model, int userId)
        {

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var order = _context.GettingSPOrder1.Where(e => e.OrderID == model.OrderId).FirstOrDefault();
            if (order == null)
            {
                throw new ManagerProcessException("000132");
            }
            if (order.Cancelled == true)
            {
                throw new ManagerProcessException("000071");
            }

            foreach (var item in model.OrderLines)
            {
                if (item.ConfirmedQty <= 0)
                {
                    throw new ManagerProcessException("000124");
                }
                if (item.ConfirmedFob <= 0)
                {
                    throw new ManagerProcessException("000125");
                }
                var orderline = _context.GettingSPOrder2
                    .Where(e => e.OrderID == model.OrderId && e.OrderLineID == item.OrderLineId).FirstOrDefault();
                if (orderline == null)
                {
                    throw new ManagerProcessException("000163");
                }
                order.OrderTaxAmount -= orderline.OrderItemTaxAmount;
                order.OrderDisAmount -= orderline.OrderItemDisAmount;
                order.OrderNetAmount -= orderline.OrderItemNetAmount;
                order.OrderAmount -= orderline.OrderItemAmount;
                order.OrderTotalAmountCur -= orderline.OrderItemTotalAmount;
                order.OrderTotalAmountJOD -= decimal.Round(orderline.OrderItemTotalAmount * order.OrderCurrencyExchangeRate, 3, MidpointRounding.AwayFromZero);


                orderline.OrderItemPriceConfirmed = item.ConfirmedFob;
                orderline.OrderItemQtyConfirmed = item.ConfirmedQty;
                orderline.OrderItemQtyCancelled = orderline.OrderItemQty - orderline.OrderItemQtyConfirmed;
                orderline.ModDate = DateTime.Now;
                orderline.ModUser = userId;
                orderline.ModTime = DateTime.Now.TimeOfDay;


                var lineTotal = decimal.Round(item.ConfirmedQty * item.ConfirmedFob, 3, MidpointRounding.AwayFromZero);


                var discountAmount = decimal.Round(lineTotal * orderline.OrderItemDisPer / 100, 3, MidpointRounding.AwayFromZero);




                var netAmount = decimal.Round(lineTotal - discountAmount, 3, MidpointRounding.AwayFromZero);


                var taxAmount = decimal.Round(orderline.OrderItemTaxPer * netAmount / 100, 3, MidpointRounding.AwayFromZero);


                var lineTotalFinal = netAmount + taxAmount;



                orderline.OrderItemAmount = lineTotal;
                orderline.OrderItemDisAmount = discountAmount;
                orderline.OrderItemNetAmount = netAmount;
                orderline.OrderItemTaxAmount = taxAmount;
                orderline.OrderItemTotalAmount = lineTotalFinal;


                order.OrderTaxAmount += orderline.OrderItemTaxAmount;
                order.OrderDisAmount += orderline.OrderItemDisAmount;
                order.OrderNetAmount += orderline.OrderItemNetAmount;
                order.OrderAmount += orderline.OrderItemAmount;
                order.OrderTotalAmountCur += orderline.OrderItemTotalAmount;
                order.OrderTotalAmountJOD += decimal.Round(orderline.OrderItemTotalAmount * order.OrderCurrencyExchangeRate, 3, MidpointRounding.AwayFromZero);
                order.ModDate = DateTime.Now;
                order.ModUser = userId;
                order.ModTime = DateTime.Now.TimeOfDay;



            }


            var result = await _context.SaveChangesAsync();

            var isConfirmed = _context.GettingSPOrder2.Where(e =>
            e.OrderID == model.OrderId &&
            e.Cancelled == false &&
            (e.OrderItemQty != e.OrderItemQtyConfirmed ||
            e.OrderItemPrice != e.OrderItemPriceConfirmed)


            ).Any();
            var subject = string.Empty;
            var email = _contextMaster.MasterApprovalsEmailsManagement
                .Where(e => e.Cancelled == false &&
                e.PortalID == 9008 &&
                e.PortalModuleID == 10002 &&
                e.RequestTypeID == 11004).FirstOrDefault();

            string file = /*_config["Settings:BaseUrl"] + */_excelExportOrder.CreateExcelFile(model.OrderId, "ApprovalOrders");
            string linkDetails = string.Empty;

            if (email != null)
            {
                var managerName = _contextMaster.Users.Where(e => ("," + email.ToUserID + ",").Contains("," + e.UserId.ToString() + ",")).Select(e => e.FullName).FirstOrDefault();

                var body = $"Dear {managerName},</br></br>";
                var ToEmailAddress = string.Join(",",
               _contextMaster.Users.Where(e => ("," + email.ToUserID + ",").Contains("," + e.UserId.ToString() + ","))
               .Select(e => e.UserEmail).ToList());

                var CCEmailAddress = string.Join(",",
       _contextMaster.Users.Where(e => ("," + email.CCUserID + ",").Contains("," + e.UserId.ToString() + ","))
       .Select(e => e.UserEmail).ToList());

                var BccEmailAddress = string.Join(",",
       _contextMaster.Users.Where(e => ("," + email.BccUserID + ",").Contains("," + e.UserId.ToString() + ","))
       .Select(e => e.UserEmail).ToList());

                if (!isConfirmed)
                {
                    order.OrderApproval = (int)OrderStatus.Confirmed;
                    subject = "Order confirmation";
                    //body += $"Your {order.OrderNo} order has been Confirmed. You can access you order via ";
                    //body += @$" <a href='{_config["Settings:BaseUrl"] + file}'>Order Attachment</a> </br>";
                    body += $"Order no {order.OrderNo} has been confirmed and can be accessed through this ";
                    body += @$" <a href='{_config["OrderLinks:SparPartOrder"] + order.OrderID}'>Link</a> </br>";

                }
                else
                {
                    var newId = ((int?)_contextMaster.MasterApproval.Max(inv => (int?)inv.RequestID) ?? 0) + 1;
                    order.OrderApproval = (int)OrderStatus.PendingConfirmation;
                    subject = $"Confirmation order request ##{newId}##";
                    //body += $" Kindly find the attached  spare part order  for your review & action. You can access you order via ";
                    //body += @$" <a href='{_config["Settings:BaseUrl"] + file}'>link</a> </br>
                    //<a style = ""background-color: #BBF3E0;"" href='mailto:{_emailConfig.CurrentValue.From}?subject={subject}$$Approve$$'>Approve</a>&nbsp; &nbsp; 
                    //<a style = ""background-color: #FFE5E8;"" href='mailto:{_emailConfig.CurrentValue.From}?subject={subject}$$Reject$$'>Reject</a>&nbsp; &nbsp; 
                    //<a style = ""background-color: #FDD97A;"" href='mailto:{_emailConfig.CurrentValue.From}?subject={subject}$$NeedRevision$$'>Need Revision </a></br>
                    //</br></br> ";
                    body += $" Order Number {order.OrderNo} Needs Your Confirmation, You can View the order through this ";

                    body += @$" <a href='{_config["OrderLinks:SparPartOrder"] + order.OrderID}'>link</a> or Find it attached</br></br>
                    <a style = ""background-color: #BBF3E0;"" href='mailto:{_emailConfig.CurrentValue.From}?subject={subject}$$Confirm$$'>Confirm</a>&nbsp; &nbsp; 
                    <a style = ""background-color: #FFE5E8;"" href='mailto:{_emailConfig.CurrentValue.From}?subject={subject}$$Reject$$'>Reject</a>&nbsp; &nbsp; 
                    <a style = ""background-color: #FDD97A;"" href='mailto:{_emailConfig.CurrentValue.From}?subject={subject}$$NeedRevision$$'>Need Revision </a></br>
                    ";

                    var item = new MasterApproval
                    {
                        RequestID = newId,
                        RequestType = 11004,
                        RequestDate = DateTime.Now,
                        RequestTime = DateTime.Now.TimeOfDay,
                        RequestModule = 10002,
                        RequestPortal = 9008,
                        EmailContent = body,
                        Attachments = file,
                        ApprovalStatus = (int)OrderStatus.PendingConfirmation,
                        Status = (int)Infrastructure.ViewModels.Dtos.Enums.Status.Active,
                        RequestRecord = model.OrderId,
                        Cancelled = false,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay,
                        EnterUser = userId,
                    };
                    item.EmailAddress = ToEmailAddress;
                    item.EmailAddressCC = CCEmailAddress;
                    item.EmailAddressBcc = BccEmailAddress;
                    await _contextMaster.MasterApproval.AddAsync(item);
                    _contextMaster.SaveChanges();


                }
                body += @$"</br> <b>Spareparts Order System</b> ";
                await _eMailService.SendEmail(subject, body, ToEmailAddress, CCEmailAddress, BccEmailAddress, file);



                // item.SentTo = email.UserID;

            }
            result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = model.OrderId;

            return response;

            throw new ManagerProcessException("000008");
        }

    }


}
