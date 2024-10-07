using SparePartsModule.Core.Exceptions;
using SparePartsModule.Domain.Models.Library;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items;
using SparePartsModule.Infrastructure.ViewModels.Response;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Interface.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SparePartsModule.Core.Helpers;
using SparePartsModule.Domain.Context;
using SparePartsModule.Domain.Models;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.POSS;
using DocumentFormat.OpenXml.Bibliography;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.SPOrder;
using SparePartsModule.Infrastructure.ViewModels.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using MigraDoc.DocumentObjectModel;

namespace SparePartsModule.Core.Library
{
    public class POSSService : IPOSSService
    {
        private readonly IConfiguration _config;
        private readonly SparePartsModuleContext _context;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilties;
        List<string> errors = new List<string>();
        List<MasterSPLookup> MasterSPLookups = new List<MasterSPLookup>();

        public POSSService(SparePartsModuleContext context, IConfiguration config, FileHelper fileHelper, UtilitiesHelper utilties)
        {

            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;
            _utilties = utilties;
        }

        public async ValueTask<ApiResponseModel> UploadPOSS(UploadPOSSModle model, int userId)
        {
            int UploaddedItemsCount = 0;
            int DuplicatedItemsCount = 0;
            int FailedtoUploadItemsCount = 0;
            int InvalidOrderCount = 0;
            int InvalidNotApprovedCount = 0;
            int InvalidOrderLineCount = 0;
            int InvalidOrderSupplierCount = 0;
            // int InvalidOrderIdCount = 0;
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var isValid = _context.GettingSPSupplier.Where(e => e.SupplierID == model.POSSSupplierID).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000007");
            }
            isValid = _utilties.EnglishCharNumber(model.POSSNo);
            if (!isValid)
            {
                throw new ManagerProcessException("000135");
            }

            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "UploadPOSS");
            var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            var dt = ds.Tables[0];
            int newId = ((int?)_context.GettingSPOrderPOSS1.Max(inv => (int?)inv.POSSID) ?? 0) + 1;
            var POSSSeq = int.Parse(DateTime.Now.ToString("yyyy") + newId.ToString("0000"));
            var poss1 = new GettingSPOrderPOSS1
            {
                POSSID = newId,
                POSSSeq = POSSSeq,
                POSSNo = model.POSSNo,
                POSSFile = tempFile.ReturnUrl,
                POSSSupplierID = model.POSSSupplierID,
                Cancelled = false,
                EnterDate = DateTime.Now,
                POSSGroup = (int)Settings.Group,
                EnterTime = DateTime.Now.TimeOfDay,
                EnterUser = userId,
                POSSStatus = (int)POSSStatus.Uploaded,
                Status = (int)Status.Active,
                POSSComments = model.POSSComments,
                POSSDate = DateTime.Now,

            };
            _context.GettingSPOrderPOSS1.Add(poss1);

            int POSSLineID = ((int?)_context.GettingSPOrderPOSS2.Max(inv => (int?)inv.POSSLineID) ?? 0);
            List<GettingSPOrderPOSS2> GettingSPOrderPOSS2List = new List<GettingSPOrderPOSS2>();
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    continue;
                }

                try
                {

                    var dataid = dt.Rows[i][0].ToString();
                    //DateTime? procDate = dt.Rows[i][1].ToString() == "" ? null : DateTime.FromOADate(double.Parse(dt.Rows[i][1].ToString()));
                    DateTime? procDate = null;
                    if (!string.IsNullOrEmpty(dt.Rows[i][1].ToString()))
                    {
                        if (DateTime.TryParse(dt.Rows[i][1].ToString(), out DateTime parsedDate))
                        {
                            procDate = parsedDate;
                        }
                        else
                        {
                            // If it's an OADate, try converting it
                            if (double.TryParse(dt.Rows[i][1].ToString(), out double oaDateValue))
                            {
                                procDate = DateTime.FromOADate(oaDateValue);
                            }
                        }
                    }
                    var yusou = int.Parse(dt.Rows[i][2].ToString());
                    var dIS_FD = int.Parse(dt.Rows[i][3].ToString());
                    //DateTime? BO_ReleaseDate = dt.Rows[i][4].ToString()==""?null: DateTime.FromOADate(double.Parse(dt.Rows[i][4].ToString()));
                    DateTime? BO_ReleaseDate = null;
                    if (!string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
                    {
                        if (DateTime.TryParse(dt.Rows[i][4].ToString(), out DateTime parsedDa))
                        {
                            BO_ReleaseDate = parsedDa;
                        }
                        else
                        {
                            // If it's an OADate, try converting it
                            if (double.TryParse(dt.Rows[i][4].ToString(), out double oaDateVal))
                            {
                                BO_ReleaseDate = DateTime.FromOADate(oaDateVal);
                            }
                        }
                    }
                    var FL_ID = dt.Rows[i][5].ToString();
                    var Order_ID = int.Parse(dt.Rows[i][6].ToString());
                    var Portion = int.Parse(dt.Rows[i][7].ToString());
                    var BO_Instruction = int.Parse(dt.Rows[i][8].ToString());
                    int? BO_CD = dt.Rows[i][9].ToString() == "" ? null : int.Parse(dt.Rows[i][9].ToString());
                    var RA_CD = dt.Rows[i][10].ToString();
                    var Exchange_rate = dt.Rows[i][11].ToString();
                    var Ordered_Part = dt.Rows[i][12].ToString();//oldpartno
                    var Ordered_Qty = int.Parse(dt.Rows[i][13].ToString());
                    string Order_No = dt.Rows[i][14].ToString();
                    int? Item_No = dt.Rows[i][15].ToString() == "" ? null : int.Parse(dt.Rows[i][15].ToString());
                    var Part_No = dt.Rows[i][16].ToString();//new partno
                    var Part_Name = dt.Rows[i][17].ToString();
                    var TRF_CD = dt.Rows[i][18].ToString();
                    var Accepted_Qty = int.Parse(dt.Rows[i][19].ToString());
                    //var FOB_UnitPrice = double.Parse(dt.Rows[i][20].ToString());
                    var FOB_UnitPrice = double.Parse(dt.Rows[i][20].ToString());
                    var FOB_UnitPriceresult = FOB_UnitPrice / 1000;
                    var isvalidOrder = _context.GettingSPOrder1.Where(e => e.Cancelled == false && e.OrderNo == Order_No).FirstOrDefault();
                    if (isvalidOrder == null)
                    {
                        InvalidOrderCount++;
                        continue;
                    }
                    if (isvalidOrder.OrderSupplierID != model.POSSSupplierID)
                    {
                        InvalidOrderSupplierCount++;
                        continue;
                    }
                    var orderLine = _context.GettingSPOrder2.Where(e => e.GettingSPOrder1.OrderNo == Order_No && e.OrderLineSeq == Item_No).FirstOrDefault();
                    if (orderLine != null)
                    {
                        //if(orderLine.OrderID!=Order_ID)
                        //{
                        //    InvalidOrderCount++;  
                        //    continue;
                        //}
                        var orderStatus = _context.GettingSPOrder1.Where(e => e.OrderID == orderLine.OrderID).Select(e => e.OrderApproval).FirstOrDefault();
                        if (orderStatus != (int)OrderStatus.Approved && orderStatus != (int)OrderStatus.Processing)
                        {
                            InvalidNotApprovedCount++;
                            continue;
                        }
                        var qty = _context.GettingSPOrderPOSS2.Where(e => e.Cancelled == false && e.Order_No == Order_No && e.Item_No == Item_No).Sum(e => e.Accepted_Qty);
                        if (qty + Accepted_Qty >= orderLine.OrderItemQty)
                        {
                            orderLine.Status = (int)OrderStatus.Processed;
                        }
                        else
                        {
                            orderLine.Status = (int)OrderStatus.Processing;
                        }

                    }
                    else
                    {
                        InvalidOrderLineCount++; continue;
                    }
                    //var exists= GettingSPOrderPOSS2List.Where(e=>e.Ordered_Qty== Ordered_Qty&& e.Order_No==Order_No&&e.Item_No==Item_No&&e.Part_No==Part_No).FirstOrDefault();
                    //if(exists!=null)
                    //{
                    //    exists.Accepted_Qty += Accepted_Qty;
                    //    continue;
                    //}



                    POSSLineID++;
                    var item = new GettingSPOrderPOSS2
                    {
                        POSSLineID = POSSLineID,
                        POSSID = newId,
                        DataID = dataid,
                        ProcDate = procDate,
                        Yusou = yusou,
                        DIS_FD = dIS_FD,
                        BO_ReleaseDate = BO_ReleaseDate,
                        FL_ID = FL_ID,
                        Order_ID = Order_ID,
                        Portion = Portion,
                        BO_Instruction = BO_Instruction,
                        BO_CD = BO_CD,
                        RA_CD = RA_CD,
                        Exchange_rate = Exchange_rate,
                        Ordered_Part = Ordered_Part,
                        Ordered_Qty = FOB_UnitPrice == 0 && Accepted_Qty == 0 ? 0 : Ordered_Qty,
                        Order_No = Order_No,
                        Item_No = Item_No,
                        Part_No = Part_No,
                        Part_Name = Part_Name,
                        TRF_CD = TRF_CD,
                        Accepted_Qty = Accepted_Qty,
                        CancelledQty = FOB_UnitPrice == 0 && Accepted_Qty == 0 ? Ordered_Qty : 0,
                        FOB_UnitPrice = FOB_UnitPriceresult,

                        Cancelled = false,
                        Status = (int)POSSStatus.Uploaded,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };

                    GettingSPOrderPOSS2List.Add(item);

                    UploaddedItemsCount++;
                }
                catch (Exception ex)
                {

                    FailedtoUploadItemsCount++;
                }

            }


            await _context.GettingSPOrderPOSS2.AddRangeAsync(GettingSPOrderPOSS2List);
            var result = await _context.SaveChangesAsync();
            var ordersIds = GettingSPOrderPOSS2List.Select(e => e.Order_No).Distinct().ToList();
            foreach (var orderId in ordersIds)
            {
                var order = _context.GettingSPOrder1.Where(e => e.OrderNo == orderId).FirstOrDefault();
                if (order != null)
                {
                    var UnPorcessed = _context.GettingSPOrder2.Where(e => e.Cancelled == false && e.OrderID == order.OrderID && e.Status != (int)OrderStatus.Processed).Any();
                    if (UnPorcessed)
                    {
                        order.OrderApproval = (int)OrderStatus.Processing;
                    }
                    else
                    {
                        order.OrderApproval = (int)OrderStatus.Processed;
                    }
                }


            }
            await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = new
            {
                InvalidOrderLineCount,
                UploaddedItemsCount,
                FailedtoUploadItemsCount,
                InvalidOrderCount,
                InvalidNotApprovedCount,
                InvalidOrderSupplierCount

            };

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetPOSS(GetPOSSModel model, PaginationModel paginationPostModel)
        {

            var data = _context.GettingSPOrderPOSS1.Where(e => e.Cancelled == false &&
              (model.SupplierId == null || ("," + model.SupplierId + ",").Contains("," + e.POSSSupplierID.ToString() + ",")) &&
               (model.Search == null
               || e.POSSNo.Contains(model.Search)
               || e.POSSID.ToString().Contains(model.Search)
               || e.POSSSeq.ToString().Contains(model.Search)
               || e.GettingSPOrderPOSS2.Where(x => x.Cancelled == false && x.Order_No == model.Search).Any()
               ) &&
               (model.FromDate == null || e.EnterDate.Date >= model.FromDate) &&
               (model.ToDate == null || e.EnterDate.Date <= model.ToDate) &&
                (model.SupplierItemNo == null || e.GettingSPOrderPOSS2.Where(x =>
                (!string.IsNullOrEmpty(x.Ordered_Part) && x.Ordered_Part.Contains(model.SupplierItemNo)) ||
                (string.IsNullOrEmpty(x.Ordered_Part) && x.Part_No.Contains(model.SupplierItemNo))
                ).Any())

            ).Select(e => new
            {
                e.POSSID,
                e.POSSNo,
                e.POSSSeq,
                e.POSSDate,
                Supplier = _context.GettingSPSupplier.Where(x => x.SupplierID == e.POSSSupplierID).Select(x => new { x.SupplierID, x.SupplierName, x.SupplierNo, x.TMCSupplier }).FirstOrDefault(),
                POSSFile = _config["Settings:BaseUrl"] + e.POSSFile,
                e.POSSComments,
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,
                e.EnterTime,
                EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),
                Orsers = e.GettingSPOrderPOSS2.Select(x => new { x.Order_No, x.Order_ID }).ToList(),
            });

            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.POSSSeq);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.POSSSeq);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.POSSNo);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.POSSNo);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.EnterDate);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.EnterDate);
            }
            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.Supplier.SupplierName);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.Supplier.SupplierName);
            }


            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetPOSSRawViewDetails(int POSSID, PaginationModel paginationPostModel)
        {

            var data = _context.GettingSPOrderPOSS2.Where(e => e.Cancelled == false &&
           e.POSSID == POSSID


            ).Select(e => new
            {


                e.POSSLineID,

                e.DataID,
                e.ProcDate,
                e.Yusou,
                e.DIS_FD,
                e.BO_ReleaseDate,
                e.FL_ID,
                e.Order_ID,
                e.Portion,
                e.BO_Instruction,
                e.BO_CD,
                e.RA_CD,
                e.Exchange_rate,
                e.Ordered_Part,
                e.Ordered_Qty,
                e.Order_No,
                e.Item_No,
                e.Part_No,
                e.Part_Name,
                e.TRF_CD,
                e.Accepted_Qty,
                e.FOB_UnitPrice,

                e.POSS_Dummy,
                // e.OrderID,
                //e.OrderLineID,
                e.ActionOrderType,
                e.ActionOrderStatus,
                e.ActionItemType,
                e.ActionItemStatus,
                e.ActionManualType,
                e.ActionManualStatus,
                e.POSSLineComments,

                Status = _context.MasterSPLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                e.EnterDate,
                e.EnterTime,



            });




            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        public async ValueTask<ApiResponseModel> AddPOSSManually(AddPOSSManuallyModle model, int userId)
        {
            int UploaddedItemsCount = 0;
            int DuplicatedItemsCount = 0;
            int FailedtoUploadItemsCount = 0;
            int InvalidOrderCount = 0;
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var isValid = _context.GettingSPSupplier.Where(e => e.SupplierID == model.POSSSupplierID).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000007");
            }
            isValid = _utilties.EnglishCharNumber(model.POSSNo);
            if (!isValid)
            {
                throw new ManagerProcessException("000135");
            }


            int newId = ((int?)_context.GettingSPOrderPOSS1.Max(inv => (int?)inv.POSSID) ?? 0) + 1;
            var POSSSeq = int.Parse(DateTime.Now.ToString("yyyy") + newId.ToString("0000"));
            var poss1 = new GettingSPOrderPOSS1
            {
                POSSID = newId,
                POSSSeq = POSSSeq,
                POSSNo = model.POSSNo,

                POSSSupplierID = model.POSSSupplierID,
                Cancelled = false,
                EnterDate = DateTime.Now,
                POSSGroup = (int)Settings.Group,
                EnterTime = DateTime.Now.TimeOfDay,
                EnterUser = userId,
                POSSStatus = (int)POSSStatus.Uploaded,
                Status = (int)Status.Active,
                POSSComments = model.POSSComments,
                POSSDate = DateTime.Now,

            };
            _context.GettingSPOrderPOSS1.Add(poss1);

            int POSSLineID = ((int?)_context.GettingSPOrderPOSS2.Max(inv => (int?)inv.POSSLineID) ?? 0);
            var GettingSPOrderPOSS2List = new List<GettingSPOrderPOSS2>();
            foreach (var item in model.POOSItems)

            {





                var isvalidOrder = _context.GettingSPOrder1.Where(e => e.Cancelled == false && e.OrderID == item.Order_ID).Any();
                if (!isvalidOrder)
                {
                    throw new ManagerProcessException("000132");
                }
                POSSLineID++;
                var itemObj = new GettingSPOrderPOSS2
                {
                    POSSLineID = POSSLineID,
                    POSSID = newId,
                    DataID = item.DataID,
                    ProcDate = item.ProcDate,
                    Yusou = item.Yusou,
                    DIS_FD = item.DIS_FD,
                    BO_ReleaseDate = item.BO_ReleaseDate,
                    FL_ID = item.FL_ID,
                    Order_ID = item.Order_ID,
                    Portion = item.Portion,
                    BO_Instruction = item.BO_Instruction,
                    BO_CD = item.BO_CD,
                    RA_CD = item.RA_CD,
                    Exchange_rate = item.Exchange_rate,
                    Ordered_Part = item.Ordered_Part,
                    Ordered_Qty = item.Ordered_Qty,
                    Order_No = item.Order_No,
                    Item_No = item.Item_No,
                    Part_No = item.Part_No,
                    Part_Name = item.Part_Name,
                    TRF_CD = item.TRF_CD,
                    Accepted_Qty = item.Accepted_Qty,
                    CancelledQty = 0,
                    FOB_UnitPrice = item.FOB_UnitPrice,
                    POSS_Dummy = item.POSS_Dummy,
                    // OrderID=item. OrderID,
                    //  OrderLineID=item.OrderLineID,
                    ActionOrderType = item.ActionOrderType,
                    ActionOrderStatus = item.ActionOrderStatus,
                    ActionItemType = item.ActionItemType,
                    ActionItemStatus = item.ActionItemStatus,
                    ActionManualType = item.ActionManualType,
                    ActionManualStatus = item.ActionManualStatus,
                    POSSLineComments = item.POSSLineComments,
                    Cancelled = false,
                    Status = (int)POSSStatus.Uploaded,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };


                GettingSPOrderPOSS2List.Add(itemObj);
                UploaddedItemsCount++;
                var orderLine = _context.GettingSPOrder2.Where(e => e.GettingSPOrder1.OrderNo == item.Order_No && e.OrderLineSeq == item.Item_No).FirstOrDefault();
                if (orderLine != null)
                {

                    var orderStatus = _context.GettingSPOrder1.Where(e => e.OrderID == orderLine.OrderID).Select(e => e.OrderApproval).FirstOrDefault();

                    var qty = _context.GettingSPOrderPOSS2.Where(e => e.Cancelled == false && e.Order_No == item.Order_No && e.Item_No == item.Item_No).Sum(e => e.Accepted_Qty);
                    if (qty + item.Accepted_Qty >= orderLine.OrderItemQty)
                    {
                        orderLine.Status = (int)OrderStatus.Processed;
                    }
                    else
                    {
                        orderLine.Status = (int)OrderStatus.Processing;
                    }

                }


            }

            _context.GettingSPOrderPOSS2.AddRange(GettingSPOrderPOSS2List);

            var result = await _context.SaveChangesAsync();
            var ordersIds = GettingSPOrderPOSS2List.Select(e => e.Order_No).Distinct().ToList();
            foreach (var orderId in ordersIds)
            {
                var order = _context.GettingSPOrder1.Where(e => e.OrderNo == orderId).FirstOrDefault();
                if (order != null)
                {
                    var UnPorcessed = _context.GettingSPOrder2.Where(e => e.Cancelled == false && e.OrderID == order.OrderID && e.Status != (int)OrderStatus.Processed).Any();
                    if (UnPorcessed)
                    {
                        order.OrderApproval = (int)OrderStatus.Processing;
                    }
                    else
                    {
                        order.OrderApproval = (int)OrderStatus.Processed;
                    }
                }


            }
            await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<object> GetPOSSTMCSummeryView(int POSSID)
        {
            //var isTmc=  _context.GettingSPOrderPOSS1.Where(e =>e.POSSID==POSSID&& e.GettingSPSupplier.SupplierName.Contains("TMC")).Any();
            //  if (!isTmc)
            //  {
            //      throw new ManagerProcessException("000141");
            //  }

           // var data = _context.GettingSPOrderPOSS2.Where(e => e.Cancelled == false &&
           //e.POSSID == POSSID


           // )
           // .GroupBy(e => e.Order_No)
           // .Select(x => new
           // {
           //     SupplierOrderNo = x.Key,

           //     Items = x.GroupBy(c => new { c.Part_No, c.Item_No }).Select(e => new
           //     {
           //         POSSID,
           //         e.FirstOrDefault().Order_ID,
           //         ItemSeq = e.Key.Item_No,
           //         SupplierOrderNo = x.Key,
           //         SupplierItemNo = string.IsNullOrEmpty(e.FirstOrDefault().Ordered_Part) ? e.Key.Part_No : e.FirstOrDefault().Ordered_Part,
           //         ItemDes = e.FirstOrDefault().Part_Name,
           //         SubstituteNumber = string.IsNullOrEmpty(e.FirstOrDefault().Ordered_Part) ? "" : e.Key.Part_No,
           //         Flag = string.Join(",", e.Select(w => w.FL_ID).Distinct().ToList()),

           //         FlagDesc = string.Join(",", _context.MasterSPLookup.Where(z => z.LookupTypeID == (int)LookupTypes.SupplierFlage && e.Where(w => w.FL_ID == z.LookupName).Any())
           //     .Select(z => z.LookupDesc).Distinct().ToList()),
           //         FOB_UnitPrice = e.Max(W => W.FOB_UnitPrice),
           //         Ordered_Qty = _context.GettingSPOrder2.Where(w => e.Select(y => y.Order_ID).Contains(w.OrderID) && w.OrderLineSeq == e.Key.Item_No)
           //     .Sum(w => w.OrderItemQty),
           //         ProcessedQty = _context.GettingSPOrderPOSS2.Where(w => e.Select(y => y.Order_ID).Contains(w.Order_ID) && w.Item_No == e.Key.Item_No)
           //     .Sum(w => w.Accepted_Qty),
           //         CancelledQty = _context.GettingSPOrderPOSS2.Where(w => e.Select(y => y.Order_ID).Contains(w.Order_ID) && w.Item_No == e.Key.Item_No)
           //     .Sum(w => w.CancelledQty),  
           //         BaBackOrderedQty =
           //         _context.GettingSPOrder2.Where(w => e.Select(y => y.Order_ID).Contains(w.OrderID) && w.OrderLineSeq == e.Key.Item_No)
           //     .Sum(w => w.OrderItemQty)
           //        - _context.GettingSPOrderPOSS2.Where(w => e.Select(y => y.Order_ID).Contains(w.Order_ID) && w.Item_No == e.Key.Item_No)
           //     .Sum(w => w.Accepted_Qty)
           //         - _context.GettingSPOrderPOSS2.Where(w => e.Select(y => y.Order_ID).Contains(w.Order_ID) && w.Item_No == e.Key.Item_No)
           //     .Sum(w => w.CancelledQty),
           //         // e.Sum(w => w.Ordered_Qty - w.Accepted_Qty - w.CancelledQty) ,//(ordered - accepted - cancelled)

           //     })


           // });


             var data = _context.GettingSPOrderPOSS2.Where(e => e.Cancelled == false && e.POSSID == POSSID)
            .GroupBy(e => e.Order_No)
            .Select(x => new
            {
                SupplierOrderNo = x.Key,

                Items = x.GroupBy(c => new { c.Part_No, c.Item_No }).Select(e => new
                {
                    POSSID,
                    e.FirstOrDefault().Order_ID,
                    ItemSeq = e.Key.Item_No,
                    SupplierOrderNo = x.Key,
                    SupplierItemNo = string.IsNullOrEmpty(e.FirstOrDefault().Ordered_Part) ? e.Key.Part_No : e.FirstOrDefault().Ordered_Part,
                    ItemDes = e.FirstOrDefault().Part_Name,
                    SubstituteNumber = string.IsNullOrEmpty(e.FirstOrDefault().Ordered_Part) ? "" : e.Key.Part_No,
                    Flag = string.Join(",", e.Select(w => w.FL_ID).Distinct().ToList()),

                    FlagDesc = string.Join(",", _context.MasterSPLookup.Where(z => z.LookupTypeID == (int)LookupTypes.SupplierFlage && e.Where(w => w.FL_ID == z.LookupName).Any())
                .Select(z => z.LookupDesc).Distinct().ToList()),
                    FOB_UnitPrice = e.Max(W => W.FOB_UnitPrice),
                    Ordered_Qty = _context.GettingSPOrderPOSS2.Where(w => e.Select(y => y.Order_ID).Contains(w.Order_ID) && w.Item_No == e.Key.Item_No)
                .Sum(w => w.Ordered_Qty),
                    ProcessedQty = _context.GettingSPOrderPOSS2.Where(w => e.Select(y => y.Order_ID).Contains(w.Order_ID) && w.Item_No == e.Key.Item_No)
                .Sum(w => w.Accepted_Qty),
                    CancelledQty = _context.GettingSPOrderPOSS2.Where(w => e.Select(y => y.Order_ID).Contains(w.Order_ID) && w.Item_No == e.Key.Item_No)
                .Sum(w => w.CancelledQty),  
                    BaBackOrderedQty =
                    _context.GettingSPOrderPOSS2.Where(w => e.Select(y => y.Order_ID).Contains(w.Order_ID) && w.Item_No == e.Key.Item_No)
                .Sum(w => w.Ordered_Qty) 
                   - _context.GettingSPOrderPOSS2.Where(w => e.Select(y => y.Order_ID).Contains(w.Order_ID) && w.Item_No == e.Key.Item_No)
                .Sum(w => w.Accepted_Qty)
                    -_context.GettingSPOrderPOSS2.Where(w => e.Select(y => y.Order_ID).Contains(w.Order_ID) && w.Item_No == e.Key.Item_No)
                .Sum(w => w.CancelledQty)//,
                    // e.Sum(w => w.Ordered_Qty - w.Accepted_Qty - w.CancelledQty) ,//(ordered - accepted - cancelled)
                })
            });




            return data;


        }
        public async ValueTask<ApiResponseModel> AddNonTMCPOSS(AddNonTMCPOSSModel model, int userId)
        {
            int UploaddedItemsCount = 0;
            int DuplicatedItemsCount = 0;
            int FailedtoUploadItemsCount = 0;
            int InvalidOrderCount = 0;
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var supplier = _context.GettingSPSupplier.Where(e => e.SupplierID == model.POSSSupplierID).FirstOrDefault();
            if (supplier == null)
            {
                throw new ManagerProcessException("000007");
            }
            if (supplier.SupplierName.Contains("TMC"))
            {
                throw new ManagerProcessException("000142");
            }

            int newId = ((int?)_context.GettingSPOrderPOSS1.Max(inv => (int?)inv.POSSID) ?? 0) + 1;
            var POSSSeq = int.Parse(DateTime.Now.ToString("yyyy") + newId.ToString("0000"));
            var poss1 = new GettingSPOrderPOSS1
            {
                POSSID = newId,
                POSSSeq = POSSSeq,
                POSSNo = newId.ToString(),

                POSSSupplierID = model.POSSSupplierID,
                Cancelled = false,
                EnterDate = DateTime.Now,
                POSSGroup = (int)Settings.Group,
                EnterTime = DateTime.Now.TimeOfDay,
                EnterUser = userId,
                POSSStatus = (int)POSSStatus.Uploaded,
                Status = (int)Status.Active,
                POSSComments = model.POSSComments,
                POSSDate = DateTime.Now,

            };
            _context.GettingSPOrderPOSS1.Add(poss1);

            int POSSLineID = ((int?)_context.GettingSPOrderPOSS2.Max(inv => (int?)inv.POSSLineID) ?? 0);
            foreach (var item in model.NonTMCPOSSItems)

            {





                var isvalidOrder = _context.GettingSPOrder1.Where(e => e.Cancelled == false && e.OrderID == item.Order_ID).Any();
                if (!isvalidOrder)
                {
                    throw new ManagerProcessException("000132");
                }
                if (item.Ordered_Qty <= 0)
                {
                    throw new ManagerProcessException("000143");
                }
                POSSLineID++;
                var itemObj = new GettingSPOrderPOSS2
                {
                    POSSLineID = POSSLineID,
                    POSSID = newId,

                    ProcDate = item.ProcDate,

                    Order_ID = item.Order_ID,

                    Ordered_Part = item.Ordered_Part,
                    Ordered_Qty = item.Ordered_Qty,
                    Order_No = item.Order_No,
                    Item_No = item.Item_No,
                    Part_No = item.Part_No,
                    Part_Name = item.Part_Name,
                    Accepted_Qty = item.Accepted_Qty,
                    CancelledQty = item.CancelledQty,
                    POSSLineComments = item.POSSLineComments,
                    Cancelled = false,
                    Status = (int)Status.Active,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };


                await _context.GettingSPOrderPOSS2.AddAsync(itemObj);
                UploaddedItemsCount++;


            }



            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> UpdateNonTMCPOSS(UpdateNonTMCPOSSModel model, int userId)
        {
            int UploaddedItemsCount = 0;
            int DuplicatedItemsCount = 0;
            int FailedtoUploadItemsCount = 0;
            int InvalidOrderCount = 0;
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var poss1 = _context.GettingSPOrderPOSS1.Where(e => e.POSSID == model.POSSID).FirstOrDefault();
            if (poss1 == null)
            {
                throw new ManagerProcessException("000144");
            }
            var supplier = _context.GettingSPSupplier.Where(e => e.SupplierID == model.POSSSupplierID).FirstOrDefault();
            if (supplier == null)
            {
                throw new ManagerProcessException("000007");
            }
            if (supplier.SupplierName.Contains("TMC"))
            {
                throw new ManagerProcessException("000142");
            }




            poss1.POSSSupplierID = model.POSSSupplierID;

            poss1.ModDate = DateTime.Now;
            poss1.ModTime = DateTime.Now.TimeOfDay;
            poss1.ModUser = userId;
            poss1.POSSStatus = (int)POSSStatus.Uploaded;

            poss1.POSSComments = model.POSSComments;





            int POSSLineID = ((int?)_context.GettingSPOrderPOSS2.Max(inv => (int?)inv.POSSLineID) ?? 0);

            var GettingSPOrderPOSS2List = new List<GettingSPOrderPOSS2>();
            foreach (var item in model.NonTMCPOSSItems)

            {

                var isvalidOrder = _context.GettingSPOrder1.Where(e => e.Cancelled == false && e.OrderID == item.Order_ID).Any();
                if (!isvalidOrder)
                {
                    throw new ManagerProcessException("000132");
                }
                if (item.Ordered_Qty <= 0)
                {
                    throw new ManagerProcessException("000143");
                }
                POSSLineID++;
                var itemObj = new GettingSPOrderPOSS2
                {
                    POSSLineID = POSSLineID,
                    POSSID = model.POSSID,

                    ProcDate = item.ProcDate,

                    Order_ID = item.Order_ID,

                    Ordered_Part = item.Ordered_Part,
                    Ordered_Qty = item.Ordered_Qty,
                    Order_No = item.Order_No,
                    Item_No = item.Item_No,
                    Part_No = item.Part_No,
                    Part_Name = item.Part_Name,
                    Accepted_Qty = item.Accepted_Qty,
                    CancelledQty = item.CancelledQty,
                    POSSLineComments = item.POSSLineComments,
                    Cancelled = false,
                    Status = (int)Status.Active,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };

                GettingSPOrderPOSS2List.Add(itemObj);

                UploaddedItemsCount++;


            }

            _context.GettingSPOrderPOSS2.Where(e => e.POSSID == model.POSSID).ExecuteDelete();
            await _context.GettingSPOrderPOSS2.AddRangeAsync(GettingSPOrderPOSS2List);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<object> GetOrderPOSSSummeryView(int OrderId)
        {
            var orderNo = _context.GettingSPOrder1.Where(e => e.OrderID == OrderId).Select(e => e.OrderNo).FirstOrDefault();

            var data = _context.GettingSPOrderPOSS2.Where(e => e.Cancelled == false &&
           e.Order_No == orderNo


            )
            .GroupBy(e => e.POSSID)
            .Select(x => new
            {
                SupplierOrderNo = x.Key,

                GettingSPOrderPOSS1 = x.Select(y => new
                {

                    y.GettingSPOrderPOSS1.POSSID,
                    y.GettingSPOrderPOSS1.POSSNo,
                    y.GettingSPOrderPOSS1.POSSSeq,
                    y.GettingSPOrderPOSS1.POSSDate,
                    Supplier = _context.GettingSPSupplier.Where(x => x.SupplierID == y.GettingSPOrderPOSS1.POSSSupplierID).Select(z => new { z.SupplierID, z.SupplierName, z.SupplierNo }).FirstOrDefault(),
                    POSSFile = _config["Settings:BaseUrl"] + y.GettingSPOrderPOSS1.POSSFile,
                    y.GettingSPOrderPOSS1.POSSComments,
                    Status = _context.MasterLookup.Where(z => z.LookupID == y.GettingSPOrderPOSS1.Status).Select(z => new { z.LookupID, z.LookupName, z.LookupBGColor, z.LookupTextColor }).FirstOrDefault(),


                }).FirstOrDefault(),

                Items = x.GroupBy(c => new { c.Part_No, c.Item_No }).Select(e => new
                {

                    ItemSeq = e.Key.Item_No,
                    SupplierOrderNo = x.Key,
                    SupplierItemNo = string.IsNullOrEmpty(e.FirstOrDefault().Ordered_Part) ? e.Key.Part_No : e.FirstOrDefault().Ordered_Part,
                    ItemDes = e.FirstOrDefault().Part_Name,
                    SubstituteNumber = string.IsNullOrEmpty(e.FirstOrDefault().Ordered_Part) ? "" : e.Key.Part_No,
                    Flag = string.Join(",", e.Select(w => w.FL_ID).Distinct().ToList()),

                    FlagDesc = string.Join(",", _context.MasterSPLookup.Where(z => z.LookupTypeID == (int)LookupTypes.SupplierFlage && e.Where(w => w.FL_ID == z.LookupName).Any())
                .Select(z => z.LookupDesc).Distinct().ToList()),
                    FOB_UnitPrice = e.Max(W => W.FOB_UnitPrice),
                    Ordered_Qty = e.Sum(w => w.Ordered_Qty),
                    ProcessedQty = e.Sum(w => w.Accepted_Qty),
                    CancelledQty = e.Sum(w => w.CancelledQty),
                    BaBackOrderedQty = e.Sum(w => w.Ordered_Qty - w.Accepted_Qty - w.CancelledQty),//(ordered - accepted - cancelled)

                })


            });







            return data;


        }
    }
}
