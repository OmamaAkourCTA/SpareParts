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
using Microsoft.AspNetCore.Http;
using SparePartsModule.Infrastructure.ViewModels.Response;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http.Metadata;
using SparePartsModule.Infrastructure.ViewModels.Models;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items.Substitute;
using MimeKit.Cryptography;
using DocumentFormat.OpenXml.Presentation;
using SparePartsModule.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Hosting;
using DocumentFormat.OpenXml.Drawing.Charts;
using SparePartsModule.Interface;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SparePartsModule.Core.Library
{
    public class LibraryItemsService : ILibraryItemsService
    {
        private readonly IConfiguration _config;
        private readonly SparePartsModuleContext _context;
        private readonly VehicleSystemContext _contextVechile;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilties;
        private readonly ILookupService _lookupService;
        List<string> errors = new List<string>();
        List<MasterSPLookup> MasterSPLookups = new List<MasterSPLookup>();

        public LibraryItemsService(SparePartsModuleContext context, IConfiguration config, FileHelper fileHelper, UtilitiesHelper utilties,
            VehicleSystemContext contextVechile, ILookupService lookupService)
        {

            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;
            _utilties = utilties;
            _contextVechile = contextVechile;
            _lookupService = lookupService;
        }
        public async ValueTask<ApiResponseModel> UploadSupplierMasterItems(UploadSupplierMasterItemsModle model, int userId)
        {
            //int UploaddedItemsCount = 0;
            //int DuplicatedItemsCount = 0;
            //int FailedtoUploadItemsCount = 0;
            //string errorCodes = string.Empty;
            //if (model == null)
            //{
            //    throw new ArgumentNullException(nameof(model));
            //}
            //FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "ItemsExcels");
            //var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            //var dt = ds.Tables[0];
            //var isValidSupplier = _context.GettingSPSupplier.Where(e => e.SupplierID == model.SupplierId).Any();
            //if (!isValidSupplier)
            //{
            //    throw new ManagerProcessException("000007");
            //}
            //var MasterSPGeneralItemNames = new List<MasterSPGeneralItemName>();
            //var MasterSPGeneralItemCategory = new List<MasterSPGeneralItemCategory>();
            //// int? ItemSupplierID = ((int?)_context.MasterSPItemSupplier.Max(inv => (int?)inv.ItemSupplierID) ?? 0) ;
            //// int ItemCategoryID = ((int?)_context.MasterSPGeneralItemCategory.Max(inv => (int?)inv.ItemCategoryID) ?? 0) ;
            ////int? ItemNameID = ((int?)_context.MasterSPGeneralItemNames.Max(inv => (int?)inv.ItemNameID) ?? 0) ;
            //int ItemSupID = ((int?)_context.MasterSPGeneralSupplierItems.Max(inv => (int?)inv.ItemSupID) ?? 0);
            //for (int i = 1; i < dt.Rows.Count; i++)
            //{
            //    if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
            //    {
            //        continue;
            //    }

            //    try
            //    {

            //  var id=int.Parse(dt.Rows[i][0].ToString());
            //        var ItemSupNo = dt.Rows[i][1].ToString();
            //    var ItemSupDesc = dt.Rows[i][2].ToString();
            //    var sub = dt.Rows[i][3].ToString();
            //    var subPart = dt.Rows[i][4].ToString();
            //    var price = double.Parse(dt.Rows[i][5].ToString());
            //    var wieght = double.Parse(dt.Rows[i][6].ToString());
            //    var Length = double.Parse(dt.Rows[i][7].ToString());
            //    var Width = double.Parse(dt.Rows[i][8].ToString());
            //    var Height = double.Parse(dt.Rows[i][9].ToString());
            //    var Size = double.Parse(dt.Rows[i][10].ToString());
            //    var Lexus = int.Parse(dt.Rows[i][11].ToString());
            //    var Start = int.Parse(dt.Rows[i][12].ToString());
            //    var End = int.Parse(dt.Rows[i][13].ToString());
            //    var fl_id = dt.Rows[i][14].ToString();
            //    var pro = dt.Rows[i][15].ToString();
            //    var nat1 = dt.Rows[i][16].ToString();
            //    var supplerFlageId = _context.MasterSPLookup.Where(e => e.LookupName == nat1 && e.LookupTypeID == (int)LookupTypes.SupplierFlage).Select(e => e.LookupID).FirstOrDefault();
            //    var nat2 = int.Parse(dt.Rows[i][17].ToString());
            //    var upq = int.Parse(dt.Rows[i][18].ToString());
            //    var MaxOrd = int.Parse(dt.Rows[i][19].ToString());
            //    var MinOrd = int.Parse(dt.Rows[i][20].ToString());
            //    var Orgin = dt.Rows[i][21].ToString();
            //    var orginId = _context.MasterLookup.Where(e => e.LookupDesc == Orgin && e.LookupTypeID == (int)LookupTypes.Country).Select(e => e.LookupID).FirstOrDefault();

            //    var item = _context.MasterSPGeneralSupplierItems.Where(e => e.MasterFileId == id&&e.ItemSupplierID==model.SupplierId).FirstOrDefault();

            //    if (item != null)
            //    {
            //            item.MasterFileId = id;
            //        item.ItemSupplierID = model.SupplierId;
            //        item.ItemSupDesc = ItemSupDesc;
            //        item.ItemSupNo = ItemSupNo;
            //        item.ItemSubSubstitute = sub;
            //        item.ItemSupSubstituteItem = subPart;
            //        item.ItemSupPrice = price;
            //        item.ItemSupWeight = wieght;
            //        item.ItemSupHeight = Height;
            //        item.ItemSupWidth = Width;
            //        item.ItemSupSizeM3 = Size;
            //        item.ItemSupLexus = Lexus;
            //        item.ItemSupStart = Start;
            //        item.ItemSupEnd = End;
            //        item.ItemSupFI_Id = fl_id;
            //        item.ItemSupPro = pro;
            //        item.ItemSupFlag = nat1;
            //        item.ItemSupna2 = nat2;
            //        item.ItemSupUPQ = upq;
            //        item.ItemSupMaxOrd = MaxOrd;
            //        item.ItemSupMinOrd = MinOrd;
            //        item.ItemSupOrigin = Orgin;
            //        item.ItemSupOrigionMaster = orginId;

            //        item.ItemSupFlagSP = supplerFlageId;

            //        DuplicatedItemsCount++;



            //        item.ModUser = userId;
            //        item.ModDate = DateTime.Now;
            //        item.ModTime = DateTime.Now.TimeOfDay;
            //        var supplierItemtemp = _context.MasterSPItemSupplier.Where(e => e.SupplierID == model.SupplierId && e.ItemID == int.Parse(dt.Rows[i][0].ToString())).FirstOrDefault();
            //        if (supplierItemtemp != null)
            //        {
            //            supplierItemtemp.SupplierFOB = double.Parse(dt.Rows[i][5].ToString());
            //            supplierItemtemp.ModDate = DateTime.Now;
            //            supplierItemtemp.ModUser = userId;
            //            supplierItemtemp.ModTime = DateTime.Now.TimeOfDay;
            //        }
            //        continue;
            //    }
            //    //object sub = dt.Rows[i][3];
            //    //if(sub != null)
            //    //{
            //    //    var itemSubstituteCode = _context.MasterSPItemSubstitute.Where(e=>e.SubstituteCode==sub.ToString()).FirstOrDefault();

            //    //}
            //    UploaddedItemsCount++;
            //    ItemSupID++;
            //    item = new MasterSPGeneralSupplierItem
            //    {
            //        MasterFileId=id,
            //        ItemSupID = ItemSupID,
            //        ItemSupplierID = model.SupplierId,
            //        ItemSupDesc = ItemSupDesc,
            //        ItemSupNo = ItemSupNo,
            //        ItemSubSubstitute = sub,
            //        ItemSupSubstituteItem = subPart,
            //        ItemSupPrice = price,
            //        ItemSupWeight = wieght,
            //        ItemSupHeight = Height,
            //        ItemSupLength= Length,  
            //        ItemSupWidth = Width,
            //        ItemSupSizeM3 = Size,
            //        ItemSupLexus = Lexus,
            //        ItemSupStart = Start,
            //        ItemSupEnd = End,
            //        ItemSupFI_Id = fl_id,
            //        ItemSupPro = pro,
            //        ItemSupFlag = nat1,
            //        ItemSupFlagSP = supplerFlageId,
            //        ItemSupna2 = nat2,
            //        ItemSupUPQ = upq,
            //        ItemSupMaxOrd = MaxOrd,
            //        ItemSupMinOrd = MinOrd,
            //        ItemSupOrigin = Orgin,
            //        ItemSupOrigionMaster = orginId,
            //        Cancelled = false,
            //        Status = (int)Status.Active,
            //        EnterUser = userId,
            //        EnterDate = DateTime.Now,
            //        EnterTime = DateTime.Now.TimeOfDay
            //    };


            //    await _context.MasterSPGeneralSupplierItems.AddAsync(item);
            //    }
            //    catch 
            //    {

            //        FailedtoUploadItemsCount++;
            //    }

            //}
            ////  var newId = ((int?)_context.MasterSPGeneralItemCategory.Max(inv => (int?)inv.ItemCategoryID) ?? 0) + 1;
            //if (MasterSPGeneralItemNames.Count > 0)
            //{
            //    _context.MasterSPGeneralItemNames.AddRange(MasterSPGeneralItemNames);
            //}
            //if (MasterSPGeneralItemCategory.Count > 0)
            //{
            //    _context.MasterSPGeneralItemCategory.AddRange(MasterSPGeneralItemCategory);
            //}


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            // response.Data = new { UploaddedItemsCount, DuplicatedItemsCount, FailedtoUploadItemsCount };

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> UploadSupplierMasterItemsV2(UpdateFileModel2 model, int userId)
        {
            int UploaddedItemsCount = 0;
            int DuplicatedItemsCount = 0;
            int FailedtoUploadItemsCount = 0;
            int InvalidSupplierCount = 0;
            int NONTMCSupplierCount = 0;
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "UploadSupplierMasterItems");
            var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            var dt = ds.Tables[0];
            //var isValidSupplier = _context.GettingSPSupplier.Where(e => e.SupplierID == model.SupplierId).Any();
            //if (!isValidSupplier)
            //{
            //    throw new ManagerProcessException("000007");
            //}
            var MasterSPGeneralItemNames = new List<MasterSPGeneralItemName>();
            var MasterSPGeneralItemCategory = new List<MasterSPGeneralItemCategory>();

            int newId = ((int?)_context.MasterSPGeneralSupplierItems.Max(inv => (int?)inv.RecordID) ?? 0);
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    continue;
                }

                try
                {

                    var id = int.Parse(dt.Rows[i][0].ToString());
                    var PartNo = dt.Rows[i][1].ToString();
                    var partNowithdash = dt.Rows[i][2].ToString();
                    var fl_id = dt.Rows[i][3].ToString();
                    var Non_Usable = int.Parse(dt.Rows[i][4].ToString());
                    var Non_Usage = int.Parse(dt.Rows[i][5].ToString());
                    var UsagePostponed = int.Parse(dt.Rows[i][6].ToString());
                    var Allotment = int.Parse(dt.Rows[i][7].ToString());
                    var TMCPartsCatalogueError = int.Parse(dt.Rows[i][8].ToString());
                    var ExportDestinationCode = int.Parse(dt.Rows[i][9].ToString());
                    var SpecialStorageFollowParts = int.Parse(dt.Rows[i][10].ToString());
                    var ManufactureDiscontinuation = int.Parse(dt.Rows[i][11].ToString());
                    var NoService = int.Parse(dt.Rows[i][12].ToString());
                    var Discontinuation = int.Parse(dt.Rows[i][13].ToString());
                    var All_time_buypartcode = int.Parse(dt.Rows[i][14].ToString());
                    var OrderUnitMax = int.Parse(dt.Rows[i][15].ToString());
                    var SubstitutionCode = dt.Rows[i][16].ToString();
                    var ItemSupSubstituteItem = dt.Rows[i][17].ToString();
                    var QuantityinUse = int.Parse(dt.Rows[i][18].ToString());
                    var UnitFOBPrice = int.Parse(dt.Rows[i][19].ToString());
                    var Corrunitprice = decimal.Parse(dt.Rows[i][20].ToString());
                    var PriceClass = int.Parse(dt.Rows[i][21].ToString());
                    var ProductCode = dt.Rows[i][22].ToString();
                    var PartName = dt.Rows[i][23].ToString();
                    var ItemSupStart = int.Parse(dt.Rows[i][24].ToString());
                    var ItemSupEnd = int.Parse(dt.Rows[i][25].ToString());
                    var BinCode = dt.Rows[i][26].ToString();
                    var QUP = int.Parse(dt.Rows[i][27].ToString());
                    var xx = dt.Rows[i][28].ToString();
                    var Qty_per_TMC = int.Parse(dt.Rows[i][29].ToString());
                    var DistributionPackage1 = int.Parse(dt.Rows[i][30].ToString());
                    var DistributionPackage2 = int.Parse(dt.Rows[i][31].ToString());
                    var DistributionPackage3 = int.Parse(dt.Rows[i][32].ToString());
                    var ItemSupLength = int.Parse(dt.Rows[i][33].ToString());
                    var ItemSupWidth = int.Parse(dt.Rows[i][34].ToString());
                    var ItemSupHeight = int.Parse(dt.Rows[i][35].ToString());
                    var ItemSupSizeM3 = int.Parse(dt.Rows[i][36].ToString());
                    var ItemSupWeight = int.Parse(dt.Rows[i][37].ToString());
                    var Parts = dt.Rows[i][38].ToString();
                    var TMCStockCode = int.Parse(dt.Rows[i][39].ToString());
                    var ItemSupLexus = int.Parse(dt.Rows[i][40].ToString());
                    var MultiplePNCCode = int.Parse(dt.Rows[i][41].ToString());
                    var PNC1 = dt.Rows[i][42].ToString();
                    var PNC2 = dt.Rows[i][43].ToString();
                    var TMCPartsCenterCode = dt.Rows[i][44].ToString();
                    var ItemSupMinOrd = int.Parse(dt.Rows[i][45].ToString());
                    var Filler = dt.Rows[i][46].ToString();
                    var ItemSupOrigin = dt.Rows[i][47].ToString();

                    var supplier = _context.GettingSPSupplier.Where(e => e.SupplierID == id).Select(e => new { e.SupplierID, e.TMCSupplier }).FirstOrDefault();

                    if (supplier == null)
                    {
                        InvalidSupplierCount++;
                        continue;
                    }
                    var supplierId = supplier.SupplierID;
                    if (supplier.TMCSupplier != true)
                    {
                        NONTMCSupplierCount++;

                        continue;
                    }
                    var item = _context.MasterSPGeneralSupplierItems.Where(e => e.ItemSupID == id && e.ItemSupNo == PartNo).FirstOrDefault();

                    if (item != null)
                    {
                        item.ItemSupID = id;
                        item.ItemSupNo = PartNo;
                        item.partNowithdash = partNowithdash;
                        item.ItemSupFI_Id = fl_id;
                        item.Non_Usable = Non_Usable;
                        item.Non_Usage = Non_Usage;
                        item.UsagePostponed = UsagePostponed;
                        item.Allotment = Allotment;
                        item.TMCPartsCatalogueError = TMCPartsCatalogueError;
                        item.ExportDestinationCode = ExportDestinationCode;
                        item.SpecialStorageFollowParts = SpecialStorageFollowParts;
                        item.ManufactureDiscontinuation = ManufactureDiscontinuation;
                        item.NoService = NoService;
                        item.Discontinuation = Discontinuation;
                        item.All_time_buypartcode = All_time_buypartcode;
                        item.ItemSupMaxOrd = OrderUnitMax;
                        item.ItemSupSubstituteType = SubstitutionCode;
                        item.ItemSupSubstituteItem = ItemSupSubstituteItem;
                        item.QuantityinUse = QuantityinUse;
                        item.UnitFOBPrice = UnitFOBPrice;
                        item.Corrunitprice = Corrunitprice;
                        item.PriceClass = PriceClass;
                        item.ItemSupPro = ProductCode;
                        item.ItemSupDesc = PartName;
                        item.ItemSupStart = ItemSupStart;
                        item.ItemSupEnd = ItemSupEnd;
                        item.ItemSupPrice = BinCode;
                        item.ItemSupUPQ = QUP;
                        item.Qty_per_TMC = Qty_per_TMC;
                        item.DistributionPackage1 = DistributionPackage1;
                        item.DistributionPackage2 = DistributionPackage2;
                        item.DistributionPackage3 = DistributionPackage3;
                        item.ItemSupLength = ItemSupLength;
                        item.ItemSupWidth = ItemSupWidth;
                        item.ItemSupHeight = ItemSupHeight;
                        item.ItemSupSizeM3 = ItemSupSizeM3;
                        item.ItemSupWeight = ItemSupWeight;
                        item.Parts = Parts;
                        item.TMCStockCode = TMCStockCode;
                        item.ItemSupLexus = ItemSupLexus;
                        item.MultiplePNCCode = MultiplePNCCode;
                        item.PNC1 = PNC1;
                        item.PNC2 = PNC2;
                        item.TMCPartsCenterCode = TMCPartsCenterCode;
                        item.ItemSupMinOrd = ItemSupMinOrd;
                        item.Filler = Filler;
                        item.ItemSupOrigin = ItemSupOrigin;
                        item.Cancelled = false;
                        item.Status = (int)Status.Active;
                        item.ModUser = userId;
                        item.ModDate = DateTime.Now;
                        item.ModTime = DateTime.Now.TimeOfDay;
                        DuplicatedItemsCount++;
                        continue;
                    }
                    //object sub = dt.Rows[i][3];
                    //if(sub != null)
                    //{
                    //    var itemSubstituteCode = _context.MasterSPItemSubstitute.Where(e=>e.SubstituteCode==sub.ToString()).FirstOrDefault();

                    //}

                    UploaddedItemsCount++;
                    newId++;
                    item = new MasterSPGeneralSupplierItem
                    {
                        RecordID = newId,

                        ItemSupID = id,
                        ItemSupNo = PartNo,
                        partNowithdash = partNowithdash,
                        ItemSupFI_Id = fl_id,
                        Non_Usable = Non_Usable,
                        Non_Usage = Non_Usage,
                        UsagePostponed = UsagePostponed,
                        Allotment = Allotment,
                        TMCPartsCatalogueError = TMCPartsCatalogueError,
                        ExportDestinationCode = ExportDestinationCode,
                        SpecialStorageFollowParts = SpecialStorageFollowParts,
                        ManufactureDiscontinuation = ManufactureDiscontinuation,
                        NoService = NoService,
                        Discontinuation = Discontinuation,
                        All_time_buypartcode = All_time_buypartcode,
                        ItemSupMaxOrd = OrderUnitMax,
                        ItemSupSubstituteType = SubstitutionCode,
                        ItemSupSubstituteItem = ItemSupSubstituteItem,
                        QuantityinUse = QuantityinUse,
                        UnitFOBPrice = UnitFOBPrice,
                        Corrunitprice = Corrunitprice,
                        PriceClass = PriceClass,
                        ItemSupPro = ProductCode,
                        ItemSupDesc = PartName,
                        ItemSupStart = ItemSupStart,
                        ItemSupEnd = ItemSupEnd,
                        ItemSupPrice = BinCode,
                        ItemSupUPQ = QUP,
                        Qty_per_TMC = Qty_per_TMC,
                        DistributionPackage1 = DistributionPackage1,
                        DistributionPackage2 = DistributionPackage2,
                        DistributionPackage3 = DistributionPackage3,
                        ItemSupLength = ItemSupLength,
                        ItemSupWidth = ItemSupWidth,
                        ItemSupHeight = ItemSupHeight,
                        ItemSupSizeM3 = ItemSupSizeM3,
                        ItemSupWeight = ItemSupWeight,
                        Parts = Parts,
                        TMCStockCode = TMCStockCode,
                        ItemSupLexus = ItemSupLexus,
                        MultiplePNCCode = MultiplePNCCode,
                        PNC1 = PNC1,
                        PNC2 = PNC2,
                        TMCPartsCenterCode = TMCPartsCenterCode,
                        ItemSupMinOrd = ItemSupMinOrd,
                        Filler = Filler,
                        ItemSupOrigin = ItemSupOrigin,
                        Cancelled = false,
                        Status = (int)Status.Active,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };

                    if (model.Save != false)
                    {
                        await _context.MasterSPGeneralSupplierItems.AddAsync(item);
                    }
                }
                catch (Exception ex)
                {

                    FailedtoUploadItemsCount++;
                }

            }


            if (model.Save != false)
            {
                var result = await _context.SaveChangesAsync();
            }

            var response = new ApiResponseModel(200, null);
            response.Data = new
            {
                UploaddedItemsCount,
                DuplicatedItemsCount,
                FailedtoUploadItemsCount,
                InvalidSupplierCount,
                NONTMCSupplierCount
            };

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> AddItem(AddItemModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var isArabic = _utilties.IsArabic(model.ItemCode);
            if (isArabic)
            {
                throw new ManagerProcessException("000059");
            }


            var isValid = _context.MasterSPGeneralItemNames.Where(e => e.ItemNameID == model.ItemNameID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000062");
            }

            isValid = _context.MasterSPGeneralItemCategory.Where(e => e.ItemCategoryID == model.ItemCategoryID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000053");
            }
            isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemUnitofMeasurement && e.Status == (int)Status.Active && e.LookupTypeID == (int)LookupTypes.UnitofMeasurement).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000063");
            }
            if (model.ItemPrice < 0)
            {
                throw new ManagerProcessException("000064");
            }
            isValid = _context.GettingSPSupplier.Where(e => e.SupplierID == model.ItemMainSup && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000007");
            }

            isValid = _context.MasterSPItem.Where(e => e.ItemCode == model.ItemCode && e.Cancelled == false).Any();
            if (isValid)
            {
                throw new ManagerProcessException("000065");
            }
            if (model.ItemCodeSup != null)
            {
                isArabic = _utilties.IsArabic(model.ItemCodeSup);
                if (isArabic)
                {
                    throw new ManagerProcessException("000060");
                }
                isValid = _context.MasterSPItem.Where(e => e.ItemCodeSup == model.ItemCodeSup && e.Cancelled == false).Any();
                if (isValid)
                {
                    throw new ManagerProcessException("000066");
                }
            }
            if (model.ItemCodeCust != null)
            {
                isArabic = _utilties.IsArabic(model.ItemCodeCust);
                if (isArabic)
                {
                    throw new ManagerProcessException("000061");
                }
                isValid = _context.MasterSPItem.Where(e => e.ItemCodeCust == model.ItemCodeCust && e.Cancelled == false).Any();
                if (isValid)
                {
                    throw new ManagerProcessException("000067");
                }
            }




            isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemFlagSup && e.LookupTypeID == (int)LookupTypes.SupplierFlage).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000068");
            }
            isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemFlagOrder && e.LookupTypeID == (int)LookupTypes.OrderFlage).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000069");
            }
            isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemFlagSales && e.LookupTypeID == (int)LookupTypes.SalesFlage).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000070");
            }
            isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemMinMaxType && e.LookupTypeID == (int)LookupTypes.MinMaxType).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000021");
            }
            //isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemGroup && e.LookupTypeID == (int)LookupTypes.Group).Any();
            //if (!isValid)
            //{
            //    throw new ManagerProcessException("000044");
            //}
            isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemTaxType && e.LookupTypeID == (int)LookupTypes.TaxType).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000052");
            }
            //if (model.ItemCurrency != null)
            //{
            //    isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemCurrency && e.LookupTypeID == (int)LookupTypes.Currency).Any();
            //    if (!isValid)
            //    {
            //        throw new ManagerProcessException("000009");
            //    }
            //}
            if (model.ItemDiscountType != null)
            {
                isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemDiscountType && e.LookupTypeID == (int)LookupTypes.DiscountType).Any();
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
            var newId = ((int?)_context.MasterSPItem.Max(inv => (int?)inv.ItemID) ?? 0) + 1;
            var item = new MasterSPItem
            {
                ItemID = newId,
                ItemCode = model.ItemCode,
                ItemCodeSup = model.ItemCodeSup
            ,
                ItemCodeCust = model.ItemCodeCust
            ,
                ItemNameID = model.ItemNameID
            ,
                ItemCategoryID = model.ItemCategoryID
            ,
                ItemUnitofMeasurement = model.ItemUnitofMeasurement
            ,
                ItemQuantityPerUnit = model.ItemQuantityPerUnit

            ,
                ItemPrice = model.ItemPrice
            ,
                ItemLength = model.ItemLength
            ,
                ItemWidth = model.ItemWidth
            ,
                ItemHeight = model.ItemHeight
            ,
                ItemSizeM3 = model.ItemSizeM3
            ,
                ItemWeight = model.ItemWeight
            ,
                ItemMainSup = model.ItemMainSup
            ,
                ItemBarcode = model.ItemBarcode
            ,
                ItemPartCode = model.ItemPartCode
            ,
                ItemProdCode = model.ItemProdCode
            ,
                ItemFlagSup = model.ItemFlagSup
            ,
                ItemFlagOrder = model.ItemFlagOrder
            ,
                ItemFlagSales = model.ItemFlagSales
            ,
                ItemMinMaxType = model.ItemMinMaxType
            ,
                ItemMinOrder = model.ItemMinOrder
            ,
                ItemMaxOrder = model.ItemMaxOrder
            ,
                ItemSerialType = model.ItemSerialType
            ,
                ItemTariffID = model.ItemTariffID
            ,
                ItemTaxType = model.ItemTaxType
            ,
                ItemCreationDate = model.ItemCreationDate
            ,
                ItemGroup = (int)Infrastructure.ViewModels.Dtos.Enums.Settings.Group,
                ItemDiscountType = model.ItemDiscountType,
                ItemComments = model.ItemComments,
                //  ItemCurrency= model.ItemCurrency,   


                Cancelled = false,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };

            if (model.ItemQR != null)
            {
                var file = await _FileHelper.WriteFile(model.ItemQR, "ItemQR");
                item.ItemQR = file.ReturnUrl;

            }
            if (model.ItemImage != null)
            {
                var file = await _FileHelper.WriteFile(model.ItemImage, "ItemImage");
                item.ItemImage = file.ReturnUrl;

            }
            await _context.MasterSPItem.AddAsync(item);

            var ItemSupSubstituteItem = _context.MasterSPGeneralSupplierItems.Where(e => e.ItemSupNo == model.ItemCode).Select(e => e.ItemSupSubstituteItem).FirstOrDefault();
            if (!string.IsNullOrEmpty(ItemSupSubstituteItem))
            {
                var substituteItem = _context.MasterSPItem.Where(e => e.ItemCode == ItemSupSubstituteItem).Select(e => e.ItemID).FirstOrDefault();
                if (substituteItem == 0)
                {
                    substituteItem = newId + 1;
                    var itemSub = new MasterSPItem
                    {
                        ItemID = substituteItem,
                        ItemCode = ItemSupSubstituteItem,
                        ItemCodeSup = model.ItemCodeSup
            ,
                        ItemCodeCust = model.ItemCodeCust
            ,
                        ItemNameID = model.ItemNameID
            ,
                        ItemCategoryID = model.ItemCategoryID
            ,
                        ItemUnitofMeasurement = model.ItemUnitofMeasurement
            ,
                        ItemQuantityPerUnit = model.ItemQuantityPerUnit

            ,
                        ItemPrice = model.ItemPrice
            ,
                        ItemLength = model.ItemLength
            ,
                        ItemWidth = model.ItemWidth
            ,
                        ItemHeight = model.ItemHeight
            ,
                        ItemSizeM3 = model.ItemSizeM3
            ,
                        ItemWeight = model.ItemWeight
            ,
                        ItemMainSup = model.ItemMainSup
            ,
                        ItemBarcode = model.ItemBarcode
            ,
                        ItemPartCode = model.ItemPartCode
            ,
                        ItemProdCode = model.ItemProdCode
            ,
                        ItemFlagSup = model.ItemFlagSup
            ,
                        ItemFlagOrder = model.ItemFlagOrder
            ,
                        ItemFlagSales = model.ItemFlagSales
            ,
                        ItemMinMaxType = model.ItemMinMaxType
            ,
                        ItemMinOrder = model.ItemMinOrder
            ,
                        ItemMaxOrder = model.ItemMaxOrder
            ,
                        ItemSerialType = model.ItemSerialType
            ,
                        ItemTariffID = model.ItemTariffID
            ,
                        ItemTaxType = model.ItemTaxType
            ,
                        ItemCreationDate = model.ItemCreationDate
            ,
                        ItemGroup = (int)Infrastructure.ViewModels.Dtos.Enums.Settings.Group,
                        ItemDiscountType = model.ItemDiscountType,
                        ItemComments = model.ItemComments,
                        //  ItemCurrency= model.ItemCurrency,   


                        Cancelled = false,
                        Status = model.Status,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };
                    await _context.MasterSPItem.AddAsync(itemSub);

                }
                var newIdN = ((int?)_context.MasterSPItemSubstitute.Max(inv => (int?)inv.ItemSubstituteID) ?? 0) + 1;
                var itemSubN = new MasterSPItemSubstitute
                {
                    ItemSubstituteID = newIdN,
                    SubstituteID = substituteItem,
                    SubstituteType = (int)SubstituteTypes.Substitute,
                    //SubstituteCode = model.ItemCode,

                    SubstituteBarcode = model.ItemBarcode,
                    ItemID = newId,
                    Cancelled = false,
                    Status = model.Status,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };
                await _context.MasterSPItemSubstitute.AddAsync(itemSubN);


            }

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);
            response.Data = newId;

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditItem(EditItemModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var item = _context.MasterSPItem.Where(e => e.ItemID == model.ItemId).FirstOrDefault();
            if (item == null)
            {
                throw new ManagerProcessException("000045");
            }


            var isArabic = _utilties.IsArabic(model.ItemCode);
            if (isArabic)
            {
                throw new ManagerProcessException("000059");
            }

            var isValid = _context.MasterSPGeneralItemNames.Where(e => e.ItemNameID == model.ItemNameID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000062");
            }

            isValid = _context.MasterSPGeneralItemCategory.Where(e => e.ItemCategoryID == model.ItemCategoryID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000053");
            }
            isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemUnitofMeasurement && e.Status == (int)Status.Active && e.LookupTypeID == (int)LookupTypes.UnitofMeasurement).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000063");
            }
            if (model.ItemPrice < 0)
            {
                throw new ManagerProcessException("000064");
            }
            isValid = _context.GettingSPSupplier.Where(e => e.SupplierID == model.ItemMainSup && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000007");
            }

            isValid = _context.MasterSPItem.Where(e => e.ItemID != model.ItemId && e.ItemCode == model.ItemCode && e.Cancelled == false).Any();
            if (isValid)
            {
                throw new ManagerProcessException("000065");
            }
            if (model.ItemCodeSup != null)
            {
                isArabic = _utilties.IsArabic(model.ItemCodeSup);
                if (isArabic)
                {
                    throw new ManagerProcessException("000060");
                }


                isValid = _context.MasterSPItem.Where(e => e.ItemID != model.ItemId && e.ItemCodeSup == model.ItemCodeSup && e.Cancelled == false).Any();
                if (isValid)
                {
                    throw new ManagerProcessException("000066");
                }
            }
            if (model.ItemCodeCust != null)
            {
                isArabic = _utilties.IsArabic(model.ItemCodeCust);
                if (isArabic)
                {
                    throw new ManagerProcessException("000061");
                }
                isValid = _context.MasterSPItem.Where(e => e.ItemID != model.ItemId && e.ItemCodeCust == model.ItemCodeCust && e.Cancelled == false).Any();
                if (isValid)
                {
                    throw new ManagerProcessException("000067");
                }
            }




            isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemFlagSup && e.LookupTypeID == (int)LookupTypes.SupplierFlage).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000068");
            }
            isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemFlagOrder && e.LookupTypeID == (int)LookupTypes.OrderFlage).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000069");
            }
            isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemFlagSales && e.LookupTypeID == (int)LookupTypes.SalesFlage).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000070");
            }
            isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemMinMaxType && e.LookupTypeID == (int)LookupTypes.MinMaxType).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000021");
            }
            //if (model.ItemCurrency != null)
            //{
            //    isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemCurrency && e.LookupTypeID == (int)LookupTypes.Currency).Any();
            //    if (!isValid)
            //    {
            //        throw new ManagerProcessException("000009");
            //    }
            //  }
            if (model.ItemDiscountType != null)
            {
                isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemDiscountType && e.LookupTypeID == (int)LookupTypes.DiscountType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000020");
                }
            }
            //isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemGroup && e.LookupTypeID == (int)LookupTypes.Group).Any();
            //if (!isValid)
            //{
            //    throw new ManagerProcessException("000044");
            //}
            isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemTaxType && e.LookupTypeID == (int)LookupTypes.TaxType).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000052");
            }

            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            var newId = ((int?)_context.MasterSPItem.Max(inv => (int?)inv.ItemID) ?? 0) + 1;


            item.ItemCode = model.ItemCode;
            item.ItemCodeSup = model.ItemCodeSup;
            item.ItemCodeCust = model.ItemCodeCust;
            item.ItemNameID = model.ItemNameID;
            item.ItemCategoryID = model.ItemCategoryID;
            item.ItemUnitofMeasurement = model.ItemUnitofMeasurement;
            item.ItemQuantityPerUnit = model.ItemQuantityPerUnit;
            item.ItemPrice = model.ItemPrice;
            item.ItemLength = model.ItemLength;
            item.ItemWidth = model.ItemWidth;
            item.ItemHeight = model.ItemHeight;
            item.ItemSizeM3 = model.ItemSizeM3;
            item.ItemWeight = model.ItemWeight;
            item.ItemMainSup = model.ItemMainSup;
            item.ItemBarcode = model.ItemBarcode;
            item.ItemPartCode = model.ItemPartCode;
            item.ItemProdCode = model.ItemProdCode;
            item.ItemFlagSup = model.ItemFlagSup;
            item.ItemFlagOrder = model.ItemFlagOrder;
            item.ItemFlagSales = model.ItemFlagSales;
            item.ItemMinMaxType = model.ItemMinMaxType;
            item.ItemMinOrder = model.ItemMinOrder;
            item.ItemMaxOrder = model.ItemMaxOrder;
            item.ItemSerialType = model.ItemSerialType;
            item.ItemTariffID = model.ItemTariffID;
            item.ItemTaxType = model.ItemTaxType;
            item.ItemCreationDate = model.ItemCreationDate;
            item.ItemDiscountType = model.ItemDiscountType;
            // item.ItemCurrency= model.ItemCurrency;
            // item.ItemGroup = model.ItemGroup;
            item.Status = model.Status;
            item.ModUser = userId;
            item.ModDate = DateTime.Now;
            item.ModTime = DateTime.Now.TimeOfDay;
            item.ItemComments = model.ItemComments;


            if (model.ItemQR != null)
            {
                var file = await _FileHelper.WriteFile(model.ItemQR, "ItemQR");
                item.ItemQR = file.ReturnUrl;

            }
            if (model.ItemImage != null)
            {
                var file = await _FileHelper.WriteFile(model.ItemImage, "ItemImage");
                item.ItemImage = file.ReturnUrl;

            }
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteItem(string ItemId, int userId)
        {
            var afftedRows = _context.MasterSPItem.Where(e => ("," + ItemId + ",").Contains("," + e.ItemID + ","))
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
        public async ValueTask<ApiResponseModel> ItemChangeStatus(string ItemId, int StatusId, int userId)
        {
            var isValids = _context.MasterLookup.Where(e => e.LookupID == StatusId && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }

            var afftedRows = _context.MasterSPItem.Where(e => ("," + ItemId + ",").Contains("," + e.ItemID + ","))
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

        public async ValueTask<PaginationDatabaseResponseDto<object>> GetItems(GetItemsModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPItem.Where(e => e.Cancelled == false &&

             (model.ItemCode == null || e.ItemCode.Contains(model.ItemCode) ||
             e.MasterSPItemSubstitute.Where(x => x.Cancelled == false && x.SubstituteType == (int)SubstituteTypes.Substitute && x.SubstituteNo.Contains(model.ItemCode)).Any()) &&
              (model.ItemCodeSup == null || e.ItemCodeSup.Contains(model.ItemCodeSup)) &&
               (model.ItemCodeCust == null || e.ItemCodeCust.Contains(model.ItemCodeCust)) &&
              (model.ItemNameID == null
              || e.MasterSPGeneralItemName.ItemNameEn.Contains(model.ItemNameID)
              || e.MasterSPGeneralItemName.ItemNameAr.Contains(model.ItemNameID)
              ) &&
               (model.ItemCategoryID == null || ("," + model.ItemCategoryID + ",").Contains("," + e.ItemCategoryID + ",")) &&
                (model.ItemMainSup == null || ("," + model.ItemMainSup + ",").Contains("," + e.ItemMainSup + ",")) &&
                 (model.ItemPartCode == null || e.ItemPartCode.Contains(model.ItemPartCode)) &&
                  (model.ItemProdCode == null || e.ItemProdCode.Contains(model.ItemProdCode)) &&
                   (model.Status == null || e.Status == model.Status) &&
                    (model.ItemFlagSales == null || model.ItemFlagSales.Contains(e.ItemFlagSales.ToString())) &&
                     (model.ItemFlagOrder == null || model.ItemFlagOrder.Contains(e.ItemFlagOrder.ToString())) &&
                     (model.ItemID == null || e.ItemID == model.ItemID) &&
                     (model.ItemNameNo == null || e.ItemNameID == model.ItemNameNo) &&
                       (
                        (model.CreationDateFrom == null || e.EnterDate >= model.CreationDateFrom) &&
                        (model.CreationDateTo == null || e.EnterDate <= model.CreationDateTo)
                    ) &&
                     (model.SupplierFlag == null || model.SupplierFlag.Contains(e.ItemFlagSup.ToString()))
            ).Select(e => new
            {
                e.ItemID,
                e.ItemCode,
                e.ItemCodeSup,
                e.ItemCodeCust,
                e.ItemQuantityPerUnit,
                e.MasterSPItemSubstitute,
                e.ItemPrice,
                e.ItemLength,
                e.ItemWidth,
                e.ItemHeight,
                e.ItemSizeM3,
                e.ItemWeight,
                e.ItemComments,

                e.ItemBarcode,
                ItemQR = e.ItemQR == null ? null : (_config["Settings:BaseUrl"] + e.ItemQR),
                ItemImage = e.ItemImage == null ? null : (_config["Settings:BaseUrl"] + e.ItemImage),
                e.ItemPartCode,
                e.ItemProdCode,
                e.ItemMinOrder,
                e.ItemMaxOrder,
                e.ItemCreationDate,
                ItemMainSup = _context.GettingSPSupplier.Where(x => x.SupplierID == e.ItemMainSup).Select(x => new { e.ItemMainSup, x.SupplierName }).FirstOrDefault(),
                ItemNameID = _context.MasterSPGeneralItemNames.Where(x => x.ItemNameID == e.ItemNameID).Select(x => new { x.ItemNameID, x.ItemNameEn, x.ItemNameAr, x.ItemNameDesc }).FirstOrDefault(),
                ItemCategoryID = _context.MasterSPGeneralItemCategory.Where(x => x.ItemCategoryID == e.ItemCategoryID).Select(x => new { x.ItemCategoryID, x.ItemCategoryNameAr, x.ItemCategoryNameEn }).FirstOrDefault(),
                ItemUnitofMeasurement = _context.MasterLookup.Where(x => x.LookupID == e.ItemUnitofMeasurement).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemFlagSup = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemFlagSup).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemFlagOrder = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemFlagOrder).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemFlagSales = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemFlagSales).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemMinMaxType = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemMinMaxType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemSerialType = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemSerialType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemTariffID = _context.MasterSPGeneralTariff.Where(x => x.TariffID == e.ItemTariffID).Select(x => new { x.TariffID, x.TariffPer, x.TariffCode }).FirstOrDefault(),
                ItemCurrency = _context.MasterLookup.Where(x => x.LookupID == e.ItemCurrency).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemDiscountType = _context.MasterLookup.Where(x => x.LookupID == e.ItemDiscountType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemDiscountTypeId = e.ItemDiscountType,
                ItemTaxType = _context.MasterLookup.Where(x => x.LookupID == e.ItemTaxType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemGroup = _context.MasterLookup.Where(x => x.LookupID == e.ItemGroup).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                EnterUser = _context.Users.Where(x => x.UserId == e.EnterUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),

                e.EnterDate,
                e.EnterTime,
                e.ModUser,
                e.ModDate,
                e.ModTime
            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.ItemCode);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.ItemCode);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.ItemNameID.ItemNameDesc);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.ItemNameID.ItemNameDesc);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.ItemCategoryID.ItemCategoryNameEn);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.ItemCategoryID.ItemCategoryNameEn);
            }
            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.ItemUnitofMeasurement.LookupName);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.ItemUnitofMeasurement.LookupName);
            }
            if (model.Sort == 10)
            {
                data = data.OrderBy(e => e.ItemPrice);
            }
            if (model.Sort == 11)
            {
                data = data.OrderByDescending(e => e.ItemPrice);
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
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetItemInquiryCards(GetItemsModel model, PaginationModel paginationPostModel)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString("Default")))
            {
                var dbParams = new DynamicParameters();
                dbParams.Add("@Action", "GetItemInquiryCards");
                dbParams.Add("@ItemID", model.ItemID, DbType.Int32);
                dbParams.Add("@ItemCode", model.ItemCode, DbType.String);
                dbParams.Add("@ItemCodeSup", model.ItemCodeSup, DbType.String);
                dbParams.Add("@ItemCodeCust", model.ItemCodeCust, DbType.String);
                dbParams.Add("@ItemNameID", model.ItemNameID, DbType.String);
                dbParams.Add("@ItemNameNo", model.ItemNameNo, DbType.Int32);
                dbParams.Add("@CreationDateFrom", model.CreationDateFrom, DbType.DateTime);
                dbParams.Add("@CreationDateTo", model.CreationDateTo, DbType.DateTime);
                dbParams.Add("@ItemCategoryID", model.ItemCategoryID, DbType.String);
                dbParams.Add("@ItemMainSup", model.ItemMainSup, DbType.String);
                dbParams.Add("@ItemPartCode", model.ItemPartCode, DbType.String);
                dbParams.Add("@ItemProdCode", model.ItemProdCode, DbType.String);
                dbParams.Add("@Status", model.Status, DbType.Int32);
                dbParams.Add("@SupplierFlag", model.SupplierFlag, DbType.String);
                dbParams.Add("@ItemFlagOrder", model.ItemFlagOrder, DbType.String);
                dbParams.Add("@ItemFlagSales", model.ItemFlagSales, DbType.String);

                var data = connection.Query<GetItemInquiryCards>("SP_Load_GetItemInquiryCards", commandType: System.Data.CommandType.StoredProcedure, param: dbParams);

                if (model.Sort == 1)
                {
                    data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
                }
                if (model.Sort == 2)
                {
                    data = data.OrderBy(e => e.ItemCode);
                }
                if (model.Sort == 3)
                {
                    data = data.OrderByDescending(e => e.ItemCode);
                }
                if (model.Sort == 4)
                {
                    data = data.OrderBy(e => e.ItemNameDesc);
                }
                if (model.Sort == 5)
                {
                    data = data.OrderByDescending(e => e.ItemNameDesc);
                }
                if (model.Sort == 6)
                {
                    data = data.OrderBy(e => e.ItemCategoryName);
                }
                if (model.Sort == 7)
                {
                    data = data.OrderByDescending(e => e.ItemCategoryName);
                }
                if (model.Sort == 8)
                {
                    data = data.OrderBy(e => e.ItemUnitofMeasurementName);
                }
                if (model.Sort == 9)
                {
                    data = data.OrderByDescending(e => e.ItemUnitofMeasurementName);
                }
                if (model.Sort == 10)
                {
                    data = data.OrderBy(e => e.ItemPrice);
                }
                if (model.Sort == 11)
                {
                    data = data.OrderByDescending(e => e.ItemPrice);
                }
                if (model.Sort == 12)
                {
                    data = data.OrderBy(e => e.StatusName);
                }
                if (model.Sort == 13)
                {
                    data = data.OrderByDescending(e => e.StatusName);
                }

                //var queryable = data.AsQueryable();
                var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data.AsQueryable());
                return result;
            }
        }

        public async ValueTask<ApiResponseModel> AddItemSupplier(AddItemSupplierModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var exist = _context.MasterSPItemSupplier.Where(e => e.SupplierID == model.SupplierID && e.ItemID == model.ItemID && e.Cancelled == false).Any();
            if (exist)
            {
                throw new ManagerProcessException("000073");
            }

            var sPItem = _context.MasterSPItem.Where(e => e.ItemID == model.ItemID && e.Status == (int)Status.Active).FirstOrDefault();
            if (sPItem == null)
            {
                throw new ManagerProcessException("000045");
            }

            var isValid = _context.GettingSPSupplier.Where(e => e.SupplierID == model.SupplierID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000007");
            }
            if (model.ItemSupFlag != null)
            {
                isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemSupFlag && e.LookupTypeID == (int)LookupTypes.SupplierFlage).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000068");
                }
            }
            if (model.ItemSupDiscountType != null)
            {
                isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemSupDiscountType && e.LookupTypeID == (int)LookupTypes.DiscountType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000024");
                }
            }
            if (model.ItemSupTaxType != null)
            {
                isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemSupTaxType && e.LookupTypeID == (int)LookupTypes.TaxType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000023");
                }
            }
            if (model.ItemMaxQty != null && model.ItemMinQty != null)
            {
                if (model.ItemMinQty >= model.ItemMaxQty)
                {
                    throw new ManagerProcessException("000120");
                }
                if (model.ItemMinQty <= 0)
                {
                    throw new ManagerProcessException("000136");
                }
                if (model.ItemMaxQty <= 0)
                {
                    throw new ManagerProcessException("000137");
                }
            }
            if (model.SupplierFOB == 0 || model.SupplierFOB == null)
            {
                model.SupplierFOB = _context.MasterSPGeneralSupplierItems
           .Where(e => e.Cancelled == false && e.ItemSupID == model.SupplierID &&
           e.ItemSupNo == sPItem.ItemCode).Select(e => e.UnitFOBPrice).FirstOrDefault() ?? 0;
                if (model.SupplierFOB == 0)
                {
                    throw new ManagerProcessException("000162");
                }
            }

            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            var newId = ((int?)_context.MasterSPItemSupplier.Max(inv => (int?)inv.ItemSupplierID) ?? 0) + 1;
            var item = new MasterSPItemSupplier
            {

                ItemSupplierID = newId,
                ItemID = model.ItemID,
                SupplierFOB = model.SupplierFOB,
                SupplierID = model.SupplierID,
                ItemSupplierBarcode = model.ItemBarcode,
                ItemSupNumber = model.ItemSupNumber,
                ItemSupFlag = model.ItemSupFlag,
                Cancelled = false,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay,
                ItemSupAvgCostFactor = model.ItemSupAvgCostFactor,
                ItemSupAvgCost = model.ItemSupCost,
                // ItemSupFinalPrice=model.ItemSupCost*model.ItemSupAvgCostFactor,
                ItemSupDiscountType = model.ItemSupDiscountType,
                ItemSupTaxType = model.ItemSupTaxType,
                ItemMinQty = model.ItemMinQty,
                ItemMaxQty = model.ItemMaxQty,
            };


            await _context.MasterSPItemSupplier.AddAsync(item);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditItemSupplier(EditItemSupplierModel model, int userId)
        {
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var item = _context.MasterSPItemSupplier.Where(e => e.ItemSupplierID == model.ItemSupplierID).FirstOrDefault();
            if (item == null)
            {
                throw new ManagerProcessException("000072");
            }
            var exist = _context.MasterSPItemSupplier.Where(e => e.ItemSupplierID != model.ItemSupplierID && e.SupplierID == model.SupplierID && e.ItemID == model.ItemID && e.Cancelled == false).Any();
            if (exist)
            {
                throw new ManagerProcessException("000073");
            }

            var isValid = _context.MasterSPItem.Where(e => e.ItemID == model.ItemID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000045");
            }

            isValid = _context.GettingSPSupplier.Where(e => e.SupplierID == model.SupplierID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000007");
            }
            if (model.ItemSupFlag != null)
            {
                isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ItemSupFlag && e.LookupTypeID == (int)LookupTypes.SupplierFlage).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000068");
                }
            }


            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            if (model.ItemSupDiscountType != null)
            {
                isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemSupDiscountType && e.LookupTypeID == (int)LookupTypes.DiscountType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000024");
                }
            }
            if (model.ItemSupTaxType != null)
            {
                isValid = _context.MasterLookup.Where(e => e.LookupID == model.ItemSupTaxType && e.LookupTypeID == (int)LookupTypes.TaxType).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000023");
                }
            }

            if (model.ItemMaxQty != null && model.ItemMinQty != null)
            {
                if (model.ItemMinQty >= model.ItemMaxQty)
                {
                    throw new ManagerProcessException("000120");
                }
                if (model.ItemMinQty <= 0)
                {
                    throw new ManagerProcessException("000136");
                }
                if (model.ItemMaxQty <= 0)
                {
                    throw new ManagerProcessException("000137");
                }
            }


            item.ItemID = model.ItemID;
            item.SupplierFOB = model.SupplierFOB;
            item.SupplierID = model.SupplierID;
            item.ItemSupplierBarcode = model.ItemBarcode;
            item.ItemSupNumber = model.ItemSupNumber;
            item.ItemSupFlag = model.ItemSupFlag;

            item.Status = model.Status;
            item.ModUser = userId;
            item.ModDate = DateTime.Now;
            item.ModTime = DateTime.Now.TimeOfDay;
            item.ItemSupDiscountType = model.ItemSupDiscountType;
            item.ItemSupAvgCostFactor = model.ItemSupAvgCostFactor;
            item.ItemSupTaxType = model.ItemSupTaxType;
            item.ItemSupAvgCost = model.ItemSupCost;
            //item.ItemSupFinalPrice = model.ItemSupAvgCostFactor * item.ItemSupCost;
            item.ItemMaxQty = model.ItemMaxQty;
            item.ItemMinQty = model.ItemMinQty;

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteItemSupplier(DeleteItemSupplierModel model, int userId)
        {

            var item = _context.MasterSPItemSupplier.Where(e => e.ItemSupplierID == model.ItemSupplierID).FirstOrDefault();
            if (item == null)
            {
                throw new ManagerProcessException("000072");
            }
            if (item.Cancelled)
            {
                throw new ManagerProcessException("000071");
            }


            item.Status = (int)Status.Deleted;
            item.Cancelled = true;
            item.ModUser = userId;
            item.CancelDate = DateTime.Now;
            item.ModTime = DateTime.Now.TimeOfDay;

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }

        public async ValueTask<PaginationDatabaseResponseDto<object>> GetItemSupplier(GetItemSupplierModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPItemSupplier.Where(e => e.Cancelled == false &&

             (model.SupplierId == null || e.SupplierID == model.SupplierId) &&
               // (model.ItemId == null || e.ItemID.ToString().Contains(model.ItemId))&&
               (model.ItemCode == null || e.MasterSPItem.ItemCode.Contains(model.ItemCode)) &&
                (model.ItemId == null || e.MasterSPItem.ItemID == model.ItemId)
            // (model.ItemNameID == null || e.ItemNameID == model.ItemNameID) &&
            //  (model.ItemCategoryID == null || e.ItemCategoryID == model.ItemCategoryID) &&
            //   (model.ItemMainSup == null || e.ItemMainSup == model.ItemMainSup) &&
            //    (model.ItemPartCode == null || e.ItemPartCode.Contains(model.ItemPartCode)) &&
            //     (model.ItemProdCode == null || e.ItemProdCode.Contains(model.ItemProdCode)) &&
            //(model.Status == null || e.Status == model.Status)
            ).Select(y => new
            {
                y.ItemSupplierID,
                y.SupplierFOB,
                y.ItemSupNumber,


                y.ItemSupAvgCostFactor,
                y.ItemSupAvgCost,
                // y.ItemSupFinalPrice,
                y.ItemMinQty,
                y.ItemMaxQty,
                y.ItemSupplierBarcode,
                ItemItemSupFlagFlagSup = _context.MasterSPLookup.Where(x => x.LookupID == y.ItemSupFlag).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemSupTaxType = _context.MasterLookup.Where(x => x.LookupID == y.ItemSupTaxType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemSupDiscountType = _context.MasterLookup.Where(x => x.LookupID == y.ItemSupDiscountType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),

                Supplier = _context.GettingSPSupplier.Where(e => e.SupplierID == y.SupplierID).Select(e =>
                new
                {

                    e.SupplierID,
                    e.SupplierName,
                    e.SupplierAbbCode,
                    e.SupplierAverageLeadTime,

                    SupplierOriginCountry = _context.MasterLookup.Where(x => x.LookupID == e.SupplierOriginCountry).Select(x => x.LookupName).FirstOrDefault(),
                    SupplierLocalInternational = _context.MasterSPLookup.Where(x => x.LookupID == e.SupplierLocalInternational).Select(x => x.LookupName).FirstOrDefault(),
                    SupplierDeliveryMethod = string.Join(",", _context.GettingSPSupplierDeliveryMethod.Where(x => x.Cancelled == false && x.SupplierID == e.SupplierID).Select(x => x.MasterSPLookup.LookupName).ToList()),
                    SupplierCurrency = _context.MasterLookup.Where(x => x.LookupID == e.SupplierCurrency).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                    Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).ToList(),

                }).FirstOrDefault(),
                Item = _context.MasterSPItem.Where(x => x.ItemID == y.ItemID && x.Cancelled == false).Select(x => new
                {
                    x.ItemID,
                    x.ItemPartCode,
                    x.ItemCode,
                    x.ItemMaxOrder,
                    x.ItemMinOrder,
                    x.ItemBarcode,
                    x.ItemCodeSup,
                    ItemFlagSup = _context.MasterSPLookup.Where(y => y.LookupID == x.ItemFlagSup).Select(y => new { y.LookupName, y.LookupID }).FirstOrDefault(),
                    ItemFlagOrder = _context.MasterSPLookup.Where(y => y.LookupID == x.ItemFlagOrder).Select(y => new { y.LookupName, y.LookupID }).FirstOrDefault(),

                    ItemName = _context.MasterSPGeneralItemNames.Where(z => z.ItemNameID == x.ItemNameID)
                    .Select(z => new { z.ItemNameID, z.ItemNameDesc, z.ItemNameAr, z.ItemNameEn, x.ItemCode }).FirstOrDefault()

                }).FirstOrDefault(),
                Status = _context.MasterLookup.Where(x => x.LookupID == y.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                y.EnterUser,
                y.EnterDate,
                y.EnterTime,
                y.ModUser,
                y.ModDate,
                y.ModTime
            }).Where(x => x.Item != null);

            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
            }
            if (model.Sort == 2)
            {
                data = data.OrderBy(e => e.Supplier.SupplierID);
            }
            if (model.Sort == 3)
            {
                data = data.OrderByDescending(e => e.Supplier.SupplierID);
            }
            if (model.Sort == 4)
            {
                data = data.OrderBy(e => e.Supplier.SupplierName);
            }
            if (model.Sort == 5)
            {
                data = data.OrderByDescending(e => e.Supplier.SupplierName);
            }
            if (model.Sort == 6)
            {
                data = data.OrderBy(e => e.Supplier.SupplierLocalInternational);
            }
            if (model.Sort == 7)
            {
                data = data.OrderByDescending(e => e.Supplier.SupplierLocalInternational);
            }
            if (model.Sort == 8)
            {
                data = data.OrderBy(e => e.Supplier.SupplierDeliveryMethod);
            }
            if (model.Sort == 9)
            {
                data = data.OrderByDescending(e => e.Supplier.SupplierDeliveryMethod);
            }
            if (model.Sort == 10)
            {
                data = data.OrderBy(e => e.Supplier.SupplierCurrency.LookupName);
            }
            if (model.Sort == 11)
            {
                data = data.OrderByDescending(e => e.Supplier.SupplierCurrency.LookupName);
            }
            if (model.Sort == 12)
            {
                data = data.OrderBy(e => e.SupplierFOB);
            }
            if (model.Sort == 13)
            {
                data = data.OrderByDescending(e => e.SupplierFOB);
            }
            if (model.Sort == 14)
            {
                data = data.OrderBy(e => e.Status.LookupName);
            }
            if (model.Sort == 15)
            {
                data = data.OrderByDescending(e => e.Status.LookupName);
            }

            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);


            return result;


        }
        public async ValueTask<ApiResponseModel> AddItemLocation(AddItemLocationModel model, int userId)
        {
            var exist = _context.MasterSPItemLocation.Where(e => e.LocationID == model.LocationId && e.ItemID == model.ItemId && e.Cancelled == false).Any();
            if (exist)
            {
                throw new ManagerProcessException("000074");
            }

            var isValid = _context.MasterSPItem.Where(e => e.ItemID == model.ItemId && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000045");
            }

            isValid = _context.MasterSPGeneralLocations.Where(e => e.LocationID == model.LocationId && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000017");
            }


            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            var newId = ((int?)_context.MasterSPItemLocation.Max(inv => (int?)inv.ItemLocationID) ?? 0) + 1;
            var item = new MasterSPItemLocation
            {
                ItemLocationID = newId,
                LocationID = model.LocationId,
                ItemID = model.ItemId,

                Cancelled = false,
                Status = model.Status,
                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };


            await _context.MasterSPItemLocation.AddAsync(item);
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditItemLocation(EditItemLocationModel model, int userId)
        {
            var item = _context.MasterSPItemLocation.Where(e => e.ItemLocationID == model.ItemLocationID).FirstOrDefault();
            if (item == null)
            {
                throw new ManagerProcessException("000075");
            }
            var exist = _context.MasterSPItemLocation.Where(e => e.ItemLocationID != model.ItemLocationID && e.LocationID == model.LocationId && e.ItemID == item.ItemID && e.Cancelled == false).Any();
            if (exist)
            {
                throw new ManagerProcessException("000074");
            }



            var isValid = _context.MasterSPGeneralLocations.Where(e => e.LocationID == model.LocationId && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000017");
            }


            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }


            item.LocationID = model.LocationId;


            item.Status = model.Status;
            item.ModUser = userId;
            item.ModDate = DateTime.Now;
            item.ModTime = DateTime.Now.TimeOfDay;

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteItemLocation(int ItemLocationID, int userId)
        {
            var item = _context.MasterSPItemLocation.Where(e => e.ItemLocationID == ItemLocationID).FirstOrDefault();
            if (item == null)
            {
                throw new ManagerProcessException("000075");
            }
            if (item.Cancelled)
            {
                throw new ManagerProcessException("000071");
            }

            item.Cancelled = true;

            item.Status = (int)Status.Deleted;
            item.ModUser = userId;
            item.CancelDate = DateTime.Now;
            item.ModTime = DateTime.Now.TimeOfDay;

            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetItemLocations(GetItemLocationsModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPItemLocation.Where(e => e.Cancelled == false &&
            e.ItemID == model.ItemId


            ).Select(y => new
            {
                y.ItemLocationID,
                y.ItemID,



                Location = _context.MasterSPGeneralLocations.Where(e => e.LocationID == y.LocationID
            ).Select(e => new
            {
                e.LocationID,
                e.Location,

                e.LocationWidth,
                e.LocationHeight,
                e.LocationLength,
                e.LocationSizeM3,
                e.LocationCode,
                LocationQR = e.LocationQR == null ? null : (_config["Settings:BaseUrl"] + e.LocationQR.Replace(",", "," + _config["Settings:BaseUrl"])).Split(",", StringSplitOptions.None),
                LocationType = _context.MasterSPLookup.Where(x => x.LookupID == e.LocationType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),

                LocationWHSection = _context.MasterSPGeneralWarehouseSection.Where(x => x.SectionID == e.LocationWHSectionID).Select(x => new { x.SectionID, x.SectionName }).FirstOrDefault(),
                LocationWarehouse = _context.MasterSPGeneralWareHouses.Where(x => x.WarehouseID == e.LocationWarehouseID).Select(x => new { x.WarehouseID, x.WarehouseName }).FirstOrDefault(),
                LocationWHZone = _context.MasterSPGeneralWarehousesZones.Where(x => x.ZoneID == e.LocationWHZoneID).Select(x => new { x.ZoneID, x.ZoneName }).FirstOrDefault(),
                LocationWHShelf = _context.MasterSPGeneralWarehousesShelfs.Where(x => x.ShelfID == e.LocationWHShelfID).Select(x => new { x.ShelfID, x.ShelfName }).FirstOrDefault(),
                LocationSubWarehouseID = _context.MasterSPGeneralWarehousesSub.Where(x => x.SubWarehouseID == e.LocationSubWarehouseID).Select(x => new { x.SubWarehouseID, x.SubWarehouseName }).FirstOrDefault(),

                Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),


            }).FirstOrDefault(),
                Item = _context.MasterSPItem.Where(x => x.ItemID == y.ItemID).Select(x => new { x.ItemID, x.ItemPartCode, x.ItemCode }).FirstOrDefault(),
                Status = _context.MasterLookup.Where(x => x.LookupID == y.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                y.EnterUser,
                y.EnterDate,
                y.EnterTime,
                y.ModUser,
                y.ModDate,
                y.ModTime
            });
            if (model.Sort == 1)
            {
                data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
            }
            //if (model.Sort == 2)
            //{
            //    data = data.OrderBy(e => e.Supplier.SupplierID);
            //}
            //if (model.Sort == 3)
            //{
            //    data = data.OrderByDescending(e => e.Supplier.SupplierID);
            //}
            //if (model.Sort == 4)
            //{
            //    data = data.OrderBy(e => e.Supplier.SupplierName);
            //}
            //if (model.Sort == 5)
            //{
            //    data = data.OrderByDescending(e => e.Supplier.SupplierName);
            //}
            //if (model.Sort == 6)
            //{
            //    data = data.OrderBy(e => e.Supplier.SupplierLocalInternational);
            //}
            //if (model.Sort == 7)
            //{
            //    data = data.OrderByDescending(e => e.Supplier.SupplierLocalInternational);
            //}
            //if (model.Sort == 8)
            //{
            //    data = data.OrderBy(e => e.Supplier.SupplierDeliveryMethod);
            //}
            //if (model.Sort == 9)
            //{
            //    data = data.OrderByDescending(e => e.Supplier.SupplierDeliveryMethod);
            //}
            //if (model.Sort == 10)
            //{
            //    data = data.OrderBy(e => e.Supplier.SupplierCurrency.LookupName);
            //}
            //if (model.Sort == 11)
            //{
            //    data = data.OrderByDescending(e => e.Supplier.SupplierCurrency.LookupName);
            //}
            //if (model.Sort == 12)
            //{
            //    data = data.OrderBy(e => e.SupplierFOB);
            //}
            //if (model.Sort == 13)
            //{
            //    data = data.OrderByDescending(e => e.SupplierFOB);
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
        public async ValueTask<ApiResponseModel> AddItemVehicleModelCode(AddItemVehicleModelCodeModel model, int userId)
        {
            var exist = _context.MasterSPItemModelCode.Where(e => e.ModelCodeID == model.ModelCodeID && e.ItemID == model.ItemID && e.Cancelled == false).Any();
            if (exist)
            {
                throw new ManagerProcessException("000082");
            }

            var isValid = _context.MasterVehicleModelCode.Where(e => e.ModelCodeID == model.ModelCodeID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000076");
            }
            isValid = _context.MasterSPItem.Where(e => e.ItemID == model.ItemID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000045");
            }
            isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ModelCode && e.LookupTypeID == (int)LookupTypes.ModelCodeType && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000081");
            }
            isValid = _context.MasterVehilceList.Where(e => e.ListID == model.ListNameID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000115");
            }

            //if (model.FromYear < 1900 || model.FromYear > 2100)
            //{
            //    throw new ManagerProcessException("000079");
            //}
            //if (model.ToYear < 1900 || model.ToYear > 2100)
            //{
            //    throw new ManagerProcessException("000079");
            //}
            //if (model.FromYear>model.ToYear)
            //{
            //    throw new ManagerProcessException("000080");
            //}


            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            var newId = ((int?)_context.MasterSPItemModelCode.Max(inv => (int?)inv.ItemModelCodeID) ?? 0) + 1;
            var item = new MasterSPItemModelCode
            {
                ItemModelCodeID = newId,
                ModelCodeID = model.ModelCodeID,
                ListNameID = model.ListNameID,
                //ModelCodeFrom=model.FromYear,
                //ModelCodeTo=model.ToYear,   
                ItemID = model.ItemID,
                Cancelled = false,
                Status = model.Status,
                ModelCodeLinkType = model.ModelCode,
                ModelCodeSpareParts = model.ModelCodeSP,
                ModelCodeService = model.ModelCodeService,


                EnterUser = userId,
                EnterDate = DateTime.Now,
                EnterTime = DateTime.Now.TimeOfDay
            };


            await _context.MasterSPItemModelCode.AddAsync(item);
            var result = await _context.SaveChangesAsync();
            _contextVechile.MasterVehicleModelCode.Where(e => e.ModelCodeID == model.ModelCodeID).ExecuteUpdate(e => e
  .SetProperty(x => x.ModelCodeSP, model.ModelCodeSP)
  .SetProperty(x => x.ModelCodeService, model.ModelCodeService));
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> EditItemVehicleModelCode(EditItemVehicleModelCodeModel model, int userId)
        {

            var item = _context.MasterSPItemModelCode.Where(e => e.ItemModelCodeID == model.ItemModelCodeID).FirstOrDefault();
            if (item == null)
            {
                throw new ManagerProcessException("000076");
            }
            var exist = _context.MasterSPItemModelCode.Where(e => e.ItemModelCodeID != model.ItemModelCodeID && e.ModelCodeID == model.ModelCodeID && e.ItemID == model.ItemID && e.Cancelled == false).Any();
            if (exist)
            {
                throw new ManagerProcessException("000082");
            }

            var isValid = _context.MasterVehicleModelCode.Where(e => e.ModelCodeID == model.ModelCodeID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000076");
            }
            isValid = _context.MasterSPItem.Where(e => e.ItemID == model.ItemID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000045");
            }
            isValid = _context.MasterSPLookup.Where(e => e.LookupID == model.ModelCode && e.LookupTypeID == (int)LookupTypes.ModelCodeType && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000081");
            }
            isValid = _context.MasterVehilceList.Where(e => e.ListID == model.ListNameID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000115");
            }


            //if (model.FromYear < 1900 || model.FromYear > 2100)
            //{
            //    throw new ManagerProcessException("000079");
            //}
            //if (model.ToYear < 1900 || model.ToYear > 2100)
            //{
            //    throw new ManagerProcessException("000079");
            //}
            //if (model.FromYear > model.ToYear)
            //{
            //    throw new ManagerProcessException("000080");
            //}


            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }


            item.ModelCodeID = model.ModelCodeID;
            //item.ModelCodeFrom = model.FromYear;
            //item.ModelCodeTo = model.ToYear;
            item.ItemID = model.ItemID;
            item.ListNameID = model.ListNameID;
            item.Status = model.Status;
            item.ModUser = userId;
            item.ModDate = DateTime.Now;
            item.ModTime = DateTime.Now.TimeOfDay;
            item.ModelCodeSpareParts = model.ModelCodeSP;
            item.ModelCodeService = model.ModelCodeService;

            var result = await _context.SaveChangesAsync();
            _contextVechile.MasterVehicleModelCode.Where(e => e.ModelCodeID == model.ModelCodeID).ExecuteUpdate(e => e
            .SetProperty(x => x.ModelCodeSP, model.ModelCodeSP)
            .SetProperty(x => x.ModelCodeService, model.ModelCodeService));


            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeleteItemVehicleModelCode(int ItemModelCodeID, int userId)
        {

            var item = _context.MasterSPItemModelCode.Where(e => e.ItemModelCodeID == ItemModelCodeID).FirstOrDefault();
            if (item == null)
            {
                throw new ManagerProcessException("000076");
            }
            if (item.Cancelled)
            {
                throw new ManagerProcessException("000071");
            }




            item.Status = (int)Status.Deleted;
            item.Cancelled = true;
            item.ModUser = userId;
            item.CancelDate = DateTime.Now;
            item.ModTime = DateTime.Now.TimeOfDay;


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetItemVehicleModelCode(GetItemVehicleModelCodeModel model, PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPItemModelCode.Where(e => e.Cancelled == false &&
                // e.ItemID == model.ItemId
                (model.BrandID == null || e.MasterVehicleModelCode.BrandID == model.BrandID) &&
                (model.ModelID == null || e.MasterVehicleModelCode.ModelID == model.ModelID) &&
                 (model.ModelCodeVehicle == null || e.MasterVehicleModelCode.ModelCodeVehicles.Contains(model.ModelCodeVehicle)) &&
                    //  (model.FromYear == null || e.MasterVehicleModelCode.ModelCodeFrom >= model.FromYear) &&
                    //  (model.ToYear == null || e.ModelCodeTo <= model.ToYear) &&
                    (model.LinkTypeID == null || e.ModelCodeLinkType == model.LinkTypeID) &&
                       (model.ItemId == null || e.ItemID == model.ItemId) &&
               (model.Status == null || e.Status == model.Status)

                ).Select(e => new
                {
                    e.ItemModelCodeID,
                    e.ItemID,

                    e.MasterVehicleModelCode.BrandID,
                    e.MasterVehicleModelCode.BrandName,
                    e.MasterVehicleModelCode.BrandNameAR,


                    e.MasterVehicleModelCode.ModelID,
                    e.MasterVehicleModelCode.ModelName,
                    e.MasterVehicleModelCode.ModelNameAR,
                    e.MasterVehicleModelCode.ModelCodeVehicles,
                    e.MasterVehicleModelCode.ModelCodeSP,
                    e.MasterVehicleModelCode.ModelCodeService,
                    e.ModelCodeID,
                    ListName = _context.MasterVehilceList.Where(x => x.ListID == e.ListNameID).Select(x => new { x.ListID, x.ListName }).FirstOrDefault(),
                    // e.ModelCodeFrom,
                    //e.ModelCodeTo,
                    ModelCodeLinkType = _context.MasterSPLookup.Where(x => x.LookupID == e.ModelCodeLinkType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                    Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                    e.EnterUser,
                    e.EnterDate,
                    e.EnterTime,
                    e.ModUser,
                    e.ModDate,
                    e.ModTime
                });
            //if (model.Sort == 1)
            //{
            //    data = data.OrderByDescending(e => e.EnterDate).ThenByDescending(e => e.EnterTime);
            //}
            //if (model.Sort == 2)
            //{
            //    data = data.OrderBy(e => e.Supplier.SupplierID);
            //}
            //if (model.Sort == 3)
            //{
            //    data = data.OrderByDescending(e => e.Supplier.SupplierID);
            //}
            //if (model.Sort == 4)
            //{
            //    data = data.OrderBy(e => e.Supplier.SupplierName);
            //}
            //if (model.Sort == 5)
            //{
            //    data = data.OrderByDescending(e => e.Supplier.SupplierName);
            //}
            //if (model.Sort == 6)
            //{
            //    data = data.OrderBy(e => e.Supplier.SupplierLocalInternational);
            //}
            //if (model.Sort == 7)
            //{
            //    data = data.OrderByDescending(e => e.Supplier.SupplierLocalInternational);
            //}
            //if (model.Sort == 8)
            //{
            //    data = data.OrderBy(e => e.Supplier.SupplierDeliveryMethod);
            //}
            //if (model.Sort == 9)
            //{
            //    data = data.OrderByDescending(e => e.Supplier.SupplierDeliveryMethod);
            //}
            //if (model.Sort == 10)
            //{
            //    data = data.OrderBy(e => e.Supplier.SupplierCurrency.LookupName);
            //}
            //if (model.Sort == 11)
            //{
            //    data = data.OrderByDescending(e => e.Supplier.SupplierCurrency.LookupName);
            //}
            //if (model.Sort == 12)
            //{
            //    data = data.OrderBy(e => e.SupplierFOB);
            //}
            //if (model.Sort == 13)
            //{
            //    data = data.OrderByDescending(e => e.SupplierFOB);
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
        public async ValueTask<ApiResponseModel> AddItemSubsitute(AddItemSubsituteModel model, int userId)
        {


            var isValid = _context.MasterSPItem.Where(e => e.ItemID == model.ItemID && e.Status == (int)Status.Active).Any();
            if (!isValid)
            {
                throw new ManagerProcessException("000045");
            }
            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }
            var newId = ((int?)_context.MasterSPItemSubstitute.Max(inv => (int?)inv.ItemSubstituteID) ?? 0);
            foreach (var Subsitute in model.Subsitutes)
            {
                isValid = _context.MasterSPItem.Where(e => e.ItemID == Subsitute.SubstituteID && e.Status == (int)Status.Active).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000085");
                }


                isValid = _context.MasterSPLookup.Where(e => e.LookupID == Subsitute.SubstituteType && e.LookupTypeID == (int)LookupTypes.SubstituteType && e.Status == (int)Status.Active).Any();
                if (!isValid)
                {
                    throw new ManagerProcessException("000086");
                }
                if (Subsitute.SubstituteType == (int)SubstituteTypes.Substitute)
                {
                    if (model.ItemID != Subsitute.SubstituteID)
                    {
                        throw new ManagerProcessException("000138");
                    }
                    if (string.IsNullOrEmpty(Subsitute.SubstituteNo))
                    {
                        throw new ManagerProcessException("000139");
                    }
                    var exist = _context.MasterSPItemSubstitute
               .Where(e => e.ItemID != model.ItemID &&
               e.SubstituteID == Subsitute.SubstituteID &&
               e.SubstituteNo == Subsitute.SubstituteNo &&
               e.Cancelled == false).FirstOrDefault();
                    if (exist != null)
                    {
                        if (exist.Status == (int)Status.Inactive)
                        {

                            throw new ManagerProcessException("000084");
                        }
                        throw new ManagerProcessException("000083");
                    }
                    string SubstituteNo = Subsitute.SubstituteNo.ToString();
                    isValid = _context.MasterSPItem.Where(e => e.ItemID != Subsitute.SubstituteID &&
                    (
                    e.ItemCode == SubstituteNo ||
                    e.ItemCodeSup == SubstituteNo ||
                    e.ItemCodeCust == SubstituteNo
                    )
                    &&
                         e.Status == (int)Status.Active
                         ).Any();
                    if (isValid)
                    {
                        throw new ManagerProcessException("000140");

                    }
                }
                if (Subsitute.SubstituteType == (int)SubstituteTypes.Alternative)
                {
                    if (model.ItemID == Subsitute.SubstituteID)
                    {
                        throw new ManagerProcessException("000087");
                    }
                    var exist = _context.MasterSPItemSubstitute
                            .Where(e => e.ItemID == model.ItemID &&
                            e.SubstituteID == Subsitute.SubstituteID &&

                            e.Cancelled == false).FirstOrDefault();
                    if (exist != null)
                    {
                        if (exist.Status == (int)Status.Inactive)
                        {

                            throw new ManagerProcessException("000084");
                        }
                        throw new ManagerProcessException("000083");
                    }
                }
                if (Subsitute.SubstituteType == (int)SubstituteTypes.OMultiSubstitute)
                {
                    if (model.ItemID == Subsitute.SubstituteID)
                    {
                        throw new ManagerProcessException("000087");
                    }
                    var exist = _context.MasterSPItemSubstitute
                            .Where(e => e.ItemID == model.ItemID &&
                            e.SubstituteID == Subsitute.SubstituteID &&

                            e.Cancelled == false).FirstOrDefault();
                    if (exist != null)
                    {
                        if (exist.Status == (int)Status.Inactive)
                        {

                            throw new ManagerProcessException("000084");
                        }
                        throw new ManagerProcessException("000083");
                    }
                }

                newId++;
                var item = new MasterSPItemSubstitute
                {
                    ItemSubstituteID = newId,
                    SubstituteID = Subsitute.SubstituteID,
                    SubstituteType = Subsitute.SubstituteType,
                    SubstituteCode = Subsitute.SubstituteCode,
                    SubstituteNo = Subsitute.SubstituteNo,
                    SubstituteBarcode = Subsitute.SubstituteBarcode,
                    ItemID = model.ItemID,
                    Cancelled = false,
                    Status = model.Status,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };
                await _context.MasterSPItemSubstitute.AddAsync(item);
                if (Subsitute.SubstituteType == (int)SubstituteTypes.Alternative)
                {
                    newId++;
                    var item1 = new MasterSPItemSubstitute
                    {
                        ItemSubstituteID = newId,
                        SubstituteID = model.ItemID,
                        SubstituteType = Subsitute.SubstituteType,
                        SubstituteCode = Subsitute.SubstituteCode,
                        SubstituteNo = Subsitute.SubstituteNo,
                        SubstituteBarcode = Subsitute.SubstituteBarcode,
                        ItemID = (int)Subsitute.SubstituteID,
                        Cancelled = false,
                        Status = model.Status,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };
                    await _context.MasterSPItemSubstitute.AddAsync(item1);
                }

            }


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }

        public async ValueTask<object> GetItemSubstitutes(GetItemSubstitutesModel model)
        {

            var Substitute = _context.MasterSPItemSubstitute.Where(e => e.Cancelled == false && e.SubstituteType == (int)SubstituteTypes.Substitute &&
                // e.ItemID == model.ItemId
                (model.ItemId == null || e.ItemID == model.ItemId) &&
                 (model.SubstituteID == null || e.SubstituteID == model.SubstituteID) &&
                  (model.SubstituteCode == null || e.SubstituteCode == model.SubstituteCode) &&
                    (model.SubstituteType == null || e.SubstituteType == model.SubstituteType) &&
               (model.Status == null || e.Status == model.Status)

                )
               .GroupBy(e => e.SubstituteType)
                .Select(e => new
                {
                    SubstituteType = _context.MasterSPLookup.Where(x => x.LookupID == e.Key).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                    // x= e.Select(x => x.SubstituteCode).FirstOrDefault(),
                    // SubstituteCode = _context.MasterSPLookup.Where(x => x.LookupID == e.Select(x=>x.SubstituteCode).FirstOrDefault()).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                    Substitutes = e.Select(x => new
                    {
                        x.ItemSubstituteID,
                        x.SubstituteID,
                        // x.SubstituteCode,
                        SubstituteCode = _context.MasterSPLookup.Where(y => y.LookupID == x.SubstituteCode).Select(y => new { y.LookupID, y.LookupName }).FirstOrDefault(),

                        x.SubstituteNo,
                        // SubstituteNo = x.MasterSPItem.ItemCode,

                        x.SubstituteBarcode,
                        Status = _context.MasterLookup.Where(y => y.LookupID == x.Status)
                        .Select(y => new { y.LookupID, y.LookupName, y.LookupBGColor, y.LookupTextColor }).FirstOrDefault(),


                    }).ToList()



                }).FirstOrDefault();

            var Alternative = _context.MasterSPItemSubstitute.Where(e => e.Cancelled == false && e.SubstituteType == (int)SubstituteTypes.Alternative &&
               // e.ItemID == model.ItemId
               (model.ItemId == null || e.ItemID == model.ItemId) &&
                (model.SubstituteID == null || e.SubstituteID == model.SubstituteID) &&
                 (model.SubstituteCode == null || e.SubstituteCode == model.SubstituteCode) &&
                   (model.SubstituteType == null || e.SubstituteType == model.SubstituteType) &&
              (model.Status == null || e.Status == model.Status)

               )
              .GroupBy(e => e.SubstituteType)
               .Select(e => new
               {
                   SubstituteType = _context.MasterSPLookup.Where(x => x.LookupID == e.Key).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                   // SubstituteCode = _context.MasterSPLookup.Where(x => x.LookupID == e.Select(x => x.SubstituteCode).FirstOrDefault()).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                   Substitutes = e.Select(x => new
                   {
                       x.ItemSubstituteID,

                       x.SubstituteID,
                       SubstituteCode = _context.MasterSPLookup.Where(y => y.LookupID == x.SubstituteCode).Select(y => new { y.LookupID, y.LookupName }).FirstOrDefault(),

                       // x.SubstituteCode,
                       Substitute = _context.MasterSPItem.Where(y => y.ItemID == x.SubstituteID)
                       .Select(y =>
                       _context.MasterSPGeneralItemNames.Where(z => z.ItemNameID == y.ItemNameID)
                       .Select(z => new
                       {
                           z.ItemNameID,
                           z.ItemNameAr,
                           z.ItemNameDesc,
                           z.ItemNameEn,
                           itemNameCode = y.ItemCode,
                           //  z.ItemNameCode,

                       }).FirstOrDefault()
                       ).FirstOrDefault(),

                       Status = _context.MasterLookup.Where(y => y.LookupID == x.Status)
                       .Select(y => new { y.LookupID, y.LookupName, y.LookupBGColor, y.LookupTextColor }).FirstOrDefault(),


                   }).ToList()

               }).FirstOrDefault();

            var OMultiSubstitute = _context.MasterSPItemSubstitute.Where(e => e.Cancelled == false && e.SubstituteType == (int)SubstituteTypes.OMultiSubstitute &&
              // e.ItemID == model.ItemId
              (model.ItemId == null || e.ItemID == model.ItemId) &&
               (model.SubstituteID == null || e.SubstituteID == model.SubstituteID) &&
                (model.SubstituteCode == null || e.SubstituteCode == model.SubstituteCode) &&
                  (model.SubstituteType == null || e.SubstituteType == model.SubstituteType) &&
             (model.Status == null || e.Status == model.Status)

              )
             .GroupBy(e => e.SubstituteType)
              .Select(e => new
              {
                  SubstituteType = _context.MasterSPLookup.Where(x => x.LookupID == e.Key).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                  Substitutes = e.Select(x => new
                  {
                      x.ItemSubstituteID,
                      x.SubstituteID,

                      SubstituteCode = _context.MasterSPLookup.Where(y => y.LookupID == x.SubstituteCode).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),

                      Substitute = _context.MasterSPItem.Where(y => y.ItemID == x.SubstituteID)
                    .Select(y =>
                    _context.MasterSPGeneralItemNames.Where(z => z.ItemNameID == y.ItemNameID)
                    .Select(z => new
                    {
                        itemNameCode = y.ItemCode,

                        z.ItemNameID,
                        z.ItemNameAr,
                        z.ItemNameDesc,
                        z.ItemNameEn, // z.ItemNameCode,

                    }).FirstOrDefault()
                    ).FirstOrDefault(),

                      Status = _context.MasterLookup.Where(y => y.LookupID == x.Status)
                    .Select(y => new { y.LookupID, y.LookupName, y.LookupBGColor, y.LookupTextColor }).FirstOrDefault(),


                  }).ToList()

              }).FirstOrDefault();

            return new { Substitute, Alternative, OMultiSubstitute };


        }
        public async ValueTask<ApiResponseModel> DeleteItemSubstitute(int ItemSubstituteID, int userId)
        {

            var item = _context.MasterSPItemSubstitute.Where(e => e.ItemSubstituteID == ItemSubstituteID).FirstOrDefault();
            if (item == null)
            {
                throw new ManagerProcessException("000085");
            }
            if (item.Cancelled)
            {
                throw new ManagerProcessException("000071");
            }




            item.Status = (int)Status.Deleted;
            item.Cancelled = true;
            item.ModUser = userId;
            item.CancelDate = DateTime.Now;
            item.ModTime = DateTime.Now.TimeOfDay;

            if (item.SubstituteType == (int)SubstituteTypes.Alternative)
            {
                var item1 = _context.MasterSPItemSubstitute.Where(e => e.Cancelled == false && e.ItemID == item.SubstituteID && e.SubstituteID == item.ItemID).FirstOrDefault();
                if (item1 != null)
                {
                    item1.Status = (int)Status.Deleted;
                    item1.Cancelled = true;
                    item1.ModUser = userId;
                    item1.CancelDate = DateTime.Now;
                }
            }
            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> DeactivateItemSubstitute(DeactivateItemSubstituteModel model, int userId)
        {

            var item = _context.MasterSPItemSubstitute.Where(e => e.ItemSubstituteID == model.ItemSubstituteID).FirstOrDefault();
            if (item == null)
            {
                throw new ManagerProcessException("000085");
            }
            if (item.Cancelled)
            {
                throw new ManagerProcessException("000071");
            }
            var isValids = _context.MasterLookup.Where(e => e.LookupID == model.Status && e.LookupTypeID == (int)LookupTypes.Status).Any();
            if (!isValids)
            {
                throw new ManagerProcessException("000032");
            }



            item.Status = model.Status;

            item.ModUser = userId;
            item.ModDate = DateTime.Now;
            item.ModTime = DateTime.Now.TimeOfDay;


            var result = await _context.SaveChangesAsync();
            var response = new ApiResponseModel(200, null);


            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<object> GetSupplierMasterItemDetails(string ItemSupNo)
        {
            return null;

            //var data = _context.MasterSPGeneralSupplierItems.Where(e => e.Cancelled == false &&e.ItemSupNo.Contains( ItemSupNo)

            //).Select(e => new
            //{
            //    e.ItemSupID,

            //    ItemSupplier = _context.GettingSPSupplier.Where(x => x.SupplierID == e.ItemSupplierID).Select(x => new { x.SupplierID, x.SupplierName }).FirstOrDefault(),

            //    e.ItemSupNo,
            //    e.ItemSupDesc,
            //    e.ItemSupSubstituteType,
            //    e.ItemSubSubstitute,
            //    e.ItemSupSubstituteItem,
            //    ItemSupUnitofMeasurement = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemSupUnitofMeasurementID).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),

            //    e.ItemSupQuantityPerUnit,
            //    e.ItemSupPrice,

            //    ItemSupPriceCurrency = _context.MasterLookup.Where(x => x.LookupID == e.ItemSupPriceCurrency).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),

            //    e.ItemSupLength,
            //    e.ItemSupWidth,
            //    e.ItemSupHeight,
            //    e.ItemSupSizeM3,
            //    e.ItemSupWeight,
            //    e.ItemSupLexus,
            //    e.ItemSupStart,
            //    e.ItemSupEnd,
            //    e.ItemSupFI_Id,
            //    e.ItemSupPro,
            //    e.ItemSupFlag,

            //    e.ItemSupna2,
            //    e.ItemSupUPQ,
            //    e.ItemSupMaxOrd,
            //    e.ItemSupMinOrd,
            //    e.ItemSupOrigin,

            //    e.ItemSupGroup,
            //    ItemSupFlagSP = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemSupFlagSP).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
            //    ItemSupOrigionMaster =  _context.MasterLookup.Where(x => x.LookupID == e.ItemSupOrigionMaster ).Select(x => new { x.LookupID,x.LookupName }).FirstOrDefault(),
            //   Status = _context.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),


            //    e.EnterUser,
            //    e.EnterDate,
            //    e.EnterTime,
            //    e.ModUser,
            //    e.ModDate,
            //    e.ModTime,
            //    e.MasterFileId,

            //});



            //return data.FirstOrDefault();


        }
        public async ValueTask<object> GetSupplierMasterItemDetailsV2(string ItemSupNo, int? supplierId)
        {

            var data = _context.MasterSPGeneralSupplierItems.Where(e => e.Cancelled == false &&
            e.ItemSupNo.Contains(ItemSupNo) &&
            (supplierId == null || e.ItemSupID == supplierId)

            ).Select(item => new
            {
                item.RecordID,
                ItemSupplier = _context.GettingSPSupplier.Where(x => x.SupplierID == item.ItemSupID).Select(x => new { x.SupplierID, x.SupplierName }).FirstOrDefault(),
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
                item.Status,
                item.Cancelled,
                item.CancelDate,
                item.EnterUser,
                item.EnterDate,
                item.EnterTime,
                item.ModUser,
                item.ModDate,
                item.ModTime

            });



            return data.FirstOrDefault();


        }
        public async ValueTask<ApiResponseModel> UploadCTAItems(UploadSupplierMasterItemsModle2 model, int userId)
        {
            int UploaddedItemsCount = 0;
            int DuplicatedItemsCount = 0;
            int FailedtoUploadItemsCount = 0;
            int MisssingDataCount = 0;
            int InvalidItemNameCount = 0;


            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "UploadCTAItems");
            var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            var dt = ds.Tables[0];

            var MasterLookups = new List<MasterLookup>();

            // int? ItemSupplierID = ((int?)_context.MasterSPItemSupplier.Max(inv => (int?)inv.ItemSupplierID) ?? 0) ;
            // int ItemCategoryID = ((int?)_context.MasterSPGeneralItemCategory.Max(inv => (int?)inv.ItemCategoryID) ?? 0) ;
            //int? ItemNameID = ((int?)_context.MasterSPGeneralItemNames.Max(inv => (int?)inv.ItemNameID) ?? 0) ;
            var MasterSPItemList = new List<MasterSPItem>();
            int newId = ((int?)_context.MasterSPItem.Max(inv => (int?)inv.ItemID) ?? 0);
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    continue;
                }

                try
                {

                    //var itemId = int.Parse(dt.Rows[i][0].ToString());
                    var itemCode = dt.Rows[i][0].ToString();
                    var itemCodeSup = dt.Rows[i][1].ToString();
                    var itemCodeCust = dt.Rows[i][2].ToString();
                    var itemName = dt.Rows[i][3].ToString();
                    if (
                        string.IsNullOrEmpty(itemName) ||
                        string.IsNullOrEmpty(itemCode) ||
                        string.IsNullOrEmpty(itemCodeSup) ||
                        string.IsNullOrEmpty(itemCodeCust)


                        )
                    {
                        MisssingDataCount++;
                        continue;
                    }
                    var itemNameId = _context.MasterSPGeneralItemNames.Where(e => e.ItemNameEn == itemName).Select(e => e.ItemNameID).FirstOrDefault();
                    if (itemNameId == 0 || itemNameId == null)
                    {
                        InvalidItemNameCount++;
                        continue;
                    }

                    var itemCagegory = dt.Rows[i][4].ToString();
                    var itemCagegoryId = _context.MasterSPGeneralItemCategory.Where(e => e.ItemCategoryNameEn == itemCagegory).Select(e => new { e.ItemCategoryID, e.ItemCategoryMinMaxType }).FirstOrDefault();
                    if (itemCagegoryId == null)
                    {
                        FailedtoUploadItemsCount++;
                        continue;
                    }
                    var itemUnitOfMesaure = dt.Rows[i][5].ToString();
                    if (string.IsNullOrEmpty(itemUnitOfMesaure))
                    {
                        MisssingDataCount++;
                        continue;
                    }
                    var itemUnitOfMesaureId = GetMasterLookup(userId, itemUnitOfMesaure, (int)LookupTypes.UnitofMeasurement);
                    var jpmItem = _context.MasterSPGeneralSupplierItems.Where(e => e.ItemSupNo == itemCode).FirstOrDefault();

                    var itemQty = int.Parse(dt.Rows[i][6].ToString());
                    var itemPrice = double.Parse(dt.Rows[i][7].ToString());
                    double? itemLength;
                    if (string.IsNullOrEmpty(dt.Rows[i][8].ToString()) && jpmItem != null)
                    {
                        itemLength = (double)jpmItem.ItemSupLength;
                    }
                    else
                    {
                        itemLength = double.Parse(dt.Rows[i][8].ToString());
                    }
                    double? itemWidth = null;
                    if (string.IsNullOrEmpty(dt.Rows[i][9].ToString()) && jpmItem != null)
                    {
                        itemWidth = (double)jpmItem.ItemSupWidth;
                    }
                    else
                    {
                        itemWidth = double.Parse(dt.Rows[i][9].ToString());
                    }
                    double? itemHeight;
                    if (string.IsNullOrEmpty(dt.Rows[i][10].ToString()) && jpmItem != null)
                    {
                        itemHeight = (double)jpmItem.ItemSupHeight;
                    }
                    else
                    {
                        itemHeight = double.Parse(dt.Rows[i][10].ToString());
                    }

                    double? itemSizeM;
                    if (string.IsNullOrEmpty(dt.Rows[i][11].ToString()) && jpmItem != null)
                    {
                        itemSizeM = (double)jpmItem.ItemSupSizeM3;
                    }
                    else
                    {
                        itemSizeM = double.Parse(dt.Rows[i][11].ToString());
                    }
                    double? itemWeight;
                    if (string.IsNullOrEmpty(dt.Rows[i][12].ToString()) && jpmItem != null)
                    {
                        itemWeight = (double)jpmItem.ItemSupWeight;
                    }
                    else
                    {
                        itemWeight = double.Parse(dt.Rows[i][12].ToString());
                    }





                    var itemMainSupplier = dt.Rows[i][13].ToString();
                    int? itemMainSupplierId = null;
                    if (!string.IsNullOrEmpty(itemMainSupplier))
                    {
                        itemMainSupplierId = _context.GettingSPSupplier
                          .Where(e => e.SupplierName == itemMainSupplier && e.Cancelled == false)
                          .Select(e => e.SupplierID).FirstOrDefault();
                        if (itemMainSupplierId == 0)
                        {
                            FailedtoUploadItemsCount++;
                            continue;
                        }
                    }


                    var itemBarcode = dt.Rows[i][14].ToString();
                    var iemPartCode = dt.Rows[i][15].ToString();
                    if (string.IsNullOrEmpty(iemPartCode))
                    {
                        MisssingDataCount++;
                        continue;
                    }
                    var iemProdCode = dt.Rows[i][16].ToString();
                    var itemFlagSup = dt.Rows[i][17].ToString();
                    if (string.IsNullOrEmpty(itemFlagSup))
                    {
                        MisssingDataCount++;
                        continue;
                    }
                    int itemFlagSupId = GeSPLookup(userId, itemFlagSup, (int)LookupTypes.SupplierFlage);

                    var itemFlagOrder = dt.Rows[i][18].ToString();
                    if (string.IsNullOrEmpty(itemFlagOrder))
                    {
                        MisssingDataCount++;
                        continue;
                    }
                    int itemFlagOrderId = GeSPLookup(userId, itemFlagOrder, (int)LookupTypes.OrderFlage);

                    var itemFlagSales = dt.Rows[i][19].ToString();
                    if (string.IsNullOrEmpty(itemFlagSales))
                    {
                        MisssingDataCount++;
                        continue;
                    }
                    int itemFlagSalesId = GeSPLookup(userId, itemFlagSales, (int)LookupTypes.SalesFlage);

                    var itemMinMax = dt.Rows[i][20].ToString();
                    int? itemMinMaxId = 0;
                    if (string.IsNullOrEmpty(itemMinMax))
                    {
                        itemMinMaxId = itemCagegoryId.ItemCategoryMinMaxType;

                    }
                    else
                    {
                        itemMinMaxId = GeSPLookup(userId, itemMinMax, (int)LookupTypes.MinMaxType);
                    }


                    var itemMinOrder = string.IsNullOrEmpty(dt.Rows[i][21].ToString()) ? 0 : int.Parse(dt.Rows[i][21].ToString());
                    var itemMaxOrder = string.IsNullOrEmpty(dt.Rows[i][22].ToString()) ? 0 : int.Parse(dt.Rows[i][22].ToString());

                    var itemSerialType = dt.Rows[i][23].ToString();
                    int? itemSerialTypeId = null;
                    if (!string.IsNullOrEmpty(itemSerialType))
                    {
                        itemSerialTypeId = GeSPLookup(userId, itemSerialType, (int)LookupTypes.ItemCategorySerialType);
                    }

                    var itemTariff = dt.Rows[i][24].ToString();
                    int? itemTariffId = null;
                    if (!string.IsNullOrEmpty(itemTariff))
                    {
                        itemTariffId = _context.MasterSPGeneralTariff.Where(e => e.TariffCode == itemTariff).Select(e => e.TariffID).FirstOrDefault();
                        if (itemTariffId == 0)
                        {
                            FailedtoUploadItemsCount++;
                            continue;
                        }

                    }
                    var itemTaxType = dt.Rows[i][25].ToString();
                    int? itemTaxTypeId = null;
                    if (!string.IsNullOrEmpty(itemTaxType))
                    {
                        var tax = Math.Truncate(decimal.Parse(itemTaxType) * 100);
                        if (tax > 100 || tax < 0)
                        {
                            FailedtoUploadItemsCount++;
                            continue;
                        }
                        itemTaxType = tax.ToString() + "%";

                        if (!itemTaxType.Contains("%"))
                        {
                            itemTaxType += "%";
                        }
                        itemTaxTypeId = GetMasterLookup(userId, itemTaxType, (int)LookupTypes.TaxType);
                    }


                    var itemDiscountType = dt.Rows[i][26].ToString();
                    int? itemDiscountTypeId = null;
                    if (!string.IsNullOrEmpty(itemDiscountType))
                    {
                        var disc = Math.Truncate(decimal.Parse(itemDiscountType) * 100);
                        if (disc > 100 || disc < 0)
                        {
                            FailedtoUploadItemsCount++;
                            continue;
                        }
                        itemDiscountType = disc.ToString() + "%";

                        if (!itemDiscountType.Contains("%"))
                        {
                            itemDiscountType += "%";
                        }
                        itemDiscountTypeId = GetMasterLookup(userId, itemDiscountType, (int)LookupTypes.DiscountType);
                    }


                    var itemCurrency = dt.Rows[i][27].ToString();
                    int? itemCurrencyId = null;
                    if (!string.IsNullOrEmpty(itemCurrency))
                    {
                        itemCurrencyId = GetMasterLookup(userId, itemCurrency, (int)LookupTypes.Currency);
                    }

                    if (MasterSPItemList.Any(e => e.ItemCode == itemCode))
                    {
                        DuplicatedItemsCount++;
                        continue;
                    }
                    var item = _context.MasterSPItem.Where(e => e.Cancelled == false && e.ItemCode == itemCode).FirstOrDefault();

                    if (item != null)
                    {
                        // item.ItemID = itemId;
                        item.ItemCode = itemCode;
                        item.ItemCodeSup = itemCodeSup;
                        item.ItemCategoryID = itemCagegoryId.ItemCategoryMinMaxType;
                        item.ItemCodeCust = itemCodeCust;
                        item.ItemNameID = itemNameId;
                        item.ItemGroup = (int)Infrastructure.ViewModels.Dtos.Enums.Settings.Group;
                        item.ItemQuantityPerUnit = itemQty;
                        item.ItemUnitofMeasurement = itemUnitOfMesaureId;
                        item.ItemPrice = itemPrice;
                        item.ItemLength = itemLength;
                        item.ItemWidth = itemWidth;
                        item.ItemHeight = itemHeight;
                        item.ItemSizeM3 = itemSizeM;
                        item.ItemWeight = itemWeight;

                        item.ItemMinMaxType = itemMinMaxId;
                        item.ItemProdCode = iemProdCode;
                        item.ItemMainSup = itemMainSupplierId;
                        item.ItemBarcode = itemBarcode;
                        item.ItemPartCode = iemPartCode;
                        item.ItemFlagSup = itemFlagSupId;
                        item.ItemFlagOrder = itemFlagOrderId;
                        item.ItemFlagSales = itemFlagSalesId;
                        item.ItemMinOrder = itemMinOrder;
                        item.ItemMaxOrder = itemMaxOrder;
                        item.ItemSerialType = itemSerialTypeId;
                        item.ItemTariffID = itemTariffId;
                        item.ItemTaxType = itemTaxTypeId;
                        item.ItemDiscountType = itemDiscountTypeId;
                        item.ItemCurrency = itemCurrencyId;


                        item.ModUser = userId;
                        item.ModDate = DateTime.Now;
                        item.ModTime = DateTime.Now.TimeOfDay;

                        DuplicatedItemsCount++;




                        continue;
                    }


                    UploaddedItemsCount++;
                    newId++;
                    item = new MasterSPItem
                    {
                        ItemID = newId,
                        ItemCode = itemCode,
                        ItemCodeSup = itemCodeSup,
                        ItemCategoryID = itemCagegoryId.ItemCategoryID,
                        ItemCodeCust = itemCodeCust,
                        ItemNameID = itemNameId,
                        ItemGroup = (int)Infrastructure.ViewModels.Dtos.Enums.Settings.Group,
                        ItemQuantityPerUnit = itemQty,
                        ItemUnitofMeasurement = itemUnitOfMesaureId,
                        ItemPrice = itemPrice,
                        ItemLength = itemLength,
                        ItemWidth = itemWidth,
                        ItemHeight = itemHeight,
                        ItemSizeM3 = itemSizeM,
                        ItemWeight = itemWeight,
                        ItemCreationDate = DateTime.Now,
                        ItemMinMaxType = itemMinMaxId,
                        ItemProdCode = iemProdCode,
                        ItemMainSup = itemMainSupplierId,
                        ItemBarcode = itemBarcode,
                        ItemPartCode = iemPartCode,
                        ItemFlagSup = itemFlagSupId,
                        ItemFlagOrder = itemFlagOrderId,
                        ItemFlagSales = itemFlagSalesId,
                        ItemMinOrder = itemMinOrder,
                        ItemMaxOrder = itemMaxOrder,
                        ItemSerialType = itemSerialTypeId,
                        ItemTariffID = itemTariffId,
                        ItemTaxType = itemTaxTypeId,
                        ItemDiscountType = itemDiscountTypeId,
                        ItemCurrency = itemCurrencyId,
                        Cancelled = false,
                        Status = (int)Status.Active,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };
                    MasterSPItemList.Add(item);

                    if (model.Save != false && MasterSPItemList.Count >= 1000)
                    {
                        await _context.MasterSPItem.AddRangeAsync(MasterSPItemList);


                        var result = await _context.SaveChangesAsync();
                        MasterSPItemList.Clear();
                    }

                }
                catch
                {

                    FailedtoUploadItemsCount++;
                }

            }
            //  var newId = ((int?)_context.MasterSPGeneralItemCategory.Max(inv => (int?)inv.ItemCategoryID) ?? 0) + 1;

            if (model.Save != false)
            {
                await _context.MasterSPItem.AddRangeAsync(MasterSPItemList);


                var result = await _context.SaveChangesAsync();
            }
            var response = new ApiResponseModel(200, null);
            response.Data = new
            {
                MisssingDataCount,
                UploaddedItemsCount,
                DuplicatedItemsCount,
                FailedtoUploadItemsCount,
                InvalidItemNameCount
            };

            return response;

            throw new ManagerProcessException("000008");
        }

        private int GeSPLookup(int userId, string name, int lookupTypeId)
        {
            var lookupId = _context.MasterSPLookup
               .Where(e => e.Cancelled == false && e.LookupName == name && e.LookupTypeID == lookupTypeId)
               .Select(e => e.LookupID).FirstOrDefault();
            if (lookupId == 0)
            {
                lookupId = MasterSPLookups
                .Where(e => e.LookupName == name && e.LookupTypeID == lookupTypeId)
                .Select(e => e.LookupID).FirstOrDefault();

                lookupId = ((int?)_context.MasterSPLookup.Where(e => e.LookupTypeID == lookupTypeId)
                .Max(inv => (int?)inv.LookupID) ?? lookupTypeId * 1000) + 1;
                var lookup = new MasterSPLookup
                {
                    LookupTypeID = lookupTypeId,
                    LookupID = lookupId,
                    LookupName = name,

                    LookupDefault = false,

                    LookupStatic = false,
                    Cancelled = false,
                    Status = (int)Status.Active,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };

                _context.MasterSPLookup.Add(lookup);
                _context.SaveChanges();





            }

            return lookupId;
        }
        private int GetMasterLookup(int userId, string name, int lookupTypeId)
        {
            var lookupId = _context.MasterLookup
               .Where(e => e.Cancelled == false && e.LookupName == name && e.LookupTypeID == lookupTypeId)
               .Select(e => e.LookupID).FirstOrDefault();
            if (lookupId == 0)
            {

                lookupId = ((int?)_context.MasterLookup.Where(e => e.LookupTypeID == lookupTypeId)
                .Max(inv => (int?)inv.LookupID) ?? lookupTypeId * 1000) + 1;
                var lookup = new MasterLookup
                {
                    LookupTypeID = lookupTypeId,
                    LookupID = lookupId,
                    LookupName = name,

                    LookupDefault = false,

                    LookupStatic = false,
                    Cancelled = false,
                    Status = (int)Status.Active,
                    EnterUser = userId,
                    EnterDate = DateTime.Now,
                    EnterTime = DateTime.Now.TimeOfDay
                };

                _context.MasterLookup.Add(lookup);

                _context.SaveChanges();



            }

            return lookupId;
        }
        public async ValueTask<PaginationDatabaseResponseDto<object>> GetCTAItems(PaginationModel paginationPostModel)
        {

            var data = _context.MasterSPItem.Where(e => e.Cancelled == false
            ).Select(e => new
            {
                e.ItemID,
                e.ItemCode,
                e.ItemCodeSup,
                e.ItemCodeCust,
                ItemName = _context.MasterSPGeneralItemNames.Where(x => x.ItemNameID == e.ItemNameID).Select(x => new { x.ItemNameID, x.ItemNameEn, x.ItemNameAr, x.ItemNameDesc }).FirstOrDefault(),
                ItemCategory = _context.MasterSPGeneralItemCategory.Where(x => x.ItemCategoryID == e.ItemCategoryID).Select(x => new { x.ItemCategoryID, x.ItemCategoryNameAr, x.ItemCategoryNameEn }).FirstOrDefault(),
                ItemUnitofMeasurement = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemUnitofMeasurement).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                e.ItemQuantityPerUnit,

                e.ItemPrice,
                e.ItemLength,
                e.ItemWidth,
                e.ItemHeight,
                e.ItemSizeM3,
                e.ItemWeight,
                ItemMainSupplier = _context.GettingSPSupplier.Where(x => x.SupplierID == e.ItemMainSup).Select(x => new { e.ItemMainSup, x.SupplierName }).FirstOrDefault(),
                e.ItemBarcode,
                e.ItemPartCode,
                e.ItemProdCode,
                ItemFlagSup = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemFlagSup).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemFlagOrder = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemFlagOrder).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemFlagSales = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemFlagSales).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemMinMaxType = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemMinMaxType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                e.ItemMinOrder,
                e.ItemMaxOrder,

                ItemSerialType = _context.MasterSPLookup.Where(x => x.LookupID == e.ItemSerialType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemTariffID = _context.MasterSPGeneralTariff.Where(x => x.TariffID == e.ItemTariffID).Select(x => new { x.TariffID, x.TariffPer, x.TariffCode }).FirstOrDefault(),
                ItemTaxType = _context.MasterLookup.Where(x => x.LookupID == e.ItemTaxType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),
                ItemDiscountType = _context.MasterLookup.Where(x => x.LookupID == e.ItemDiscountType).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),

                ItemCurrency = _context.MasterLookup.Where(x => x.LookupID == e.ItemCurrency).Select(x => new { x.LookupID, x.LookupName }).FirstOrDefault(),

            });



            var result = new PaginationDatabaseResponseDto<object>(paginationPostModel, data);
            return result;


        }
        public async ValueTask<ApiResponseModel> UploadItemsLocations(UploadSupplierMasterItemsModle2 model, int userId)
        {
            int UploaddedItemsCount = 0;
            int DuplicatedItemsCount = 0;
            int InvalidLocationCodesCount = 0;
            int InvalidItemNoCount = 0;
            int FailedtoUploadItemsCount = 0;


            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "ItemsLocations");
            var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            var dt = ds.Tables[0];

            var MasterLookups = new List<MasterLookup>();


            var itemsList = new List<MasterSPItemLocation>();

            var newId = ((int?)_context.MasterSPItemLocation.Max(inv => (int?)inv.ItemLocationID) ?? 0);
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    continue;
                }

                try
                {
                    var itemNo = dt.Rows[i][0].ToString();
                    var locationCode = dt.Rows[i][1].ToString();
                    var itemId = _context.MasterSPItem.Where(e => e.ItemCode == itemNo && e.Status == (int)Status.Active).Select(e => e.ItemID).FirstOrDefault();
                    if (itemId == 0 || itemId == null)
                    {
                        InvalidItemNoCount++;
                        continue;
                    }
                    var locationId = _context.MasterSPGeneralLocations.Where(e => e.LocationCode == locationCode && e.Status == (int)Status.Active)
                        .Select(e => e.LocationID).FirstOrDefault();
                    if (locationId == 0 || locationId == null)
                    {
                        InvalidLocationCodesCount++;
                        continue;
                    }





                    var exist = _context.MasterSPItemLocation.Where(e => e.LocationID == locationId && e.ItemID == itemId && e.Cancelled == false).Any();
                    if (
                        exist ||
                        itemsList.Where(e => e.LocationID == locationId && e.ItemID == itemId).Any()
                       )
                    {
                        DuplicatedItemsCount++;
                        continue;
                    }




                    newId++;
                    var item = new MasterSPItemLocation
                    {
                        ItemLocationID = newId,
                        LocationID = locationId,
                        ItemID = itemId,

                        Cancelled = false,
                        Status = (int)Status.Active,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };

                    UploaddedItemsCount++;
                    itemsList.Add(item);




                }
                catch
                {

                    FailedtoUploadItemsCount++;
                }

            }
            //  var newId = ((int?)_context.MasterSPGeneralItemCategory.Max(inv => (int?)inv.ItemCategoryID) ?? 0) + 1;

            if (model.Save != false)
            {
                await _context.MasterSPItemLocation.AddRangeAsync(itemsList);


                var result = await _context.SaveChangesAsync();
            }
            var response = new ApiResponseModel(200, null);
            response.Data = new { InvalidItemNoCount, InvalidLocationCodesCount, UploaddedItemsCount, DuplicatedItemsCount, FailedtoUploadItemsCount };

            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> UploadItemsSuppliers(UploadSupplierMasterItemsModle2 model, int userId)
        {
            int UploaddedItemsCount = 0;
            int DuplicatedItemsCount = 0;

            int InvalidItemNoCount = 0;
            int FailedtoUploadItemsCount = 0;
            int InvalidSupplierCount = 0;
            int InvalidDiscount = 0;
            int InvalidTax = 0;
            int InvalidValuesforCost = 0;
            int InvalidFOB = 0;
            int InvalidLot = 0;
            int InvalidMaxOrderQty = 0;
            int MissingDataCount = 0;
            int InvalidItemSupplierMaster = 0;


            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "ItemsLocations");
            var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            var dt = ds.Tables[0];

            var MasterLookups = new List<MasterLookup>();


            var itemsList = new List<MasterSPItemSupplier>();


            var newId = ((int?)_context.MasterSPItemSupplier.Max(inv => (int?)inv.ItemSupplierID) ?? 0);
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    continue;
                }

                try
                {
                    var itemNo = dt.Rows[i][0].ToString();
                    var supplierBarcode = dt.Rows[i][1].ToString();
                    var itemSupplierNo = dt.Rows[i][2].ToString();
                    var supplierName = dt.Rows[i][3].ToString();
                    if (string.IsNullOrEmpty(itemNo) ||
                        string.IsNullOrEmpty(supplierName))
                    {
                        MissingDataCount++;
                        continue;
                    }
                    var supplier = _context.GettingSPSupplier.Where(e => e.SupplierName == supplierName && e.Status == (int)Status.Active)
                  .FirstOrDefault();
                    if (supplier == null)
                    {
                        InvalidSupplierCount++;
                        continue;
                    }

                    decimal fob = 0;
                    if (string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
                    {
                        //fob = (decimal)supplier.PriceFactor;
                        fob = _context.MasterSPGeneralSupplierItems
                   .Where(e => e.Cancelled == false && e.ItemSupID == supplier.SupplierID &&
                   e.ItemSupNo == itemNo).Select(e => e.UnitFOBPrice).FirstOrDefault() ?? 0;
                        if (fob == 0)
                        {
                            InvalidItemSupplierMaster++;
                            continue;
                        }
                    }
                    else
                    {
                        fob = decimal.Parse(dt.Rows[i][4].ToString());
                    }
                    decimal AvgCostFactor = 0;
                    if (string.IsNullOrEmpty(dt.Rows[i][5].ToString()))
                    {

                        AvgCostFactor = (decimal)supplier.CostFactor;
                    }
                    else
                    {
                        AvgCostFactor = decimal.Parse(dt.Rows[i][5].ToString());
                    }
                    decimal AvgCost = 0;
                    if (string.IsNullOrEmpty(dt.Rows[i][6].ToString()))
                    {
                        AvgCost = AvgCostFactor * fob;
                    }
                    else
                    {
                        AvgCost = decimal.Parse(dt.Rows[i][6].ToString());
                    }
                    decimal Discount = 0;
                    int? discountType = null;
                    if (string.IsNullOrEmpty(dt.Rows[i][7].ToString()))
                    {
                        discountType = (int)supplier.SupplierDiscount;
                        Discount = decimal.Parse(_context.MasterSPLookup.Where(e => e.LookupID == discountType).Select(e => e.LookupName.Replace("%", "")).FirstOrDefault());

                    }
                    else
                    {
                        Discount = decimal.Parse(dt.Rows[i][7].ToString()) * 100;
                    }
                    decimal Tax = 0;
                    int? taxType = null;
                    if (string.IsNullOrEmpty(dt.Rows[i][8].ToString()))
                    {
                        taxType = supplier.SupplierTaxType;
                        Tax = decimal.Parse(_context.MasterSPLookup.Where(e => e.LookupID == taxType).Select(e => e.LookupName.Replace("%", "")).FirstOrDefault());

                        Tax = (decimal)supplier.SupplierTaxType;
                    }
                    else
                    {
                        Tax = decimal.Parse(dt.Rows[i][8].ToString()) * 100;
                    }

                    var LotSize = int.Parse(dt.Rows[i][9].ToString());
                    var MaxOrderQty = int.Parse(dt.Rows[i][10].ToString());





                    var spItem = _context.MasterSPItem.Where(e => e.ItemCode == itemNo && e.Status == (int)Status.Active).FirstOrDefault();
                    if (spItem == null)
                    {
                        InvalidItemNoCount++;
                        continue;
                    }

                    if (Discount < 0 || Discount > 100)
                    {
                        InvalidDiscount++;
                        continue;
                    }
                    if (Tax < 0 || Tax > 100)
                    {
                        InvalidTax++;
                        continue;
                    }
                    if (AvgCostFactor < 0 || AvgCost < 0)
                    {
                        InvalidValuesforCost++;
                        continue;
                    }
                    if (fob < 0)
                    {
                        InvalidFOB++;
                        continue;
                    }
                    if (LotSize < 0)
                    {
                        InvalidLot++;
                        continue;
                    }
                    if (MaxOrderQty <= 0)
                    {
                        InvalidMaxOrderQty++;
                        continue;
                    }
                    if (discountType == null)
                    {
                        discountType = _lookupService.GetMasterLookup(userId, Discount.ToString() + "%", (int)LookupTypes.DiscountType);
                    }
                    if (taxType == null)
                    {
                        taxType = _lookupService.GetMasterLookup(userId, Tax.ToString() + "%", (int)LookupTypes.TaxType);
                    }
                    var item = _context.MasterSPItemSupplier
                        .Where(e => e.SupplierID == supplier.SupplierID && e.ItemID == spItem.ItemID && e.Cancelled == false).FirstOrDefault();
                    if (item != null)
                    {
                        item.SupplierFOB = fob;
                        item.SupplierID = supplier.SupplierID;
                        item.ItemSupplierBarcode = supplierBarcode;
                        item.ItemSupNumber = itemSupplierNo;

                        item.ModUser = userId;
                        item.ModDate = DateTime.Now;
                        item.ModTime = DateTime.Now.TimeOfDay;
                        item.ItemSupAvgCostFactor = AvgCostFactor;
                        item.ItemSupAvgCost = AvgCost;
                        item.ItemSupDiscountType = discountType;
                        item.ItemSupTaxType = taxType;
                        item.ItemMinQty = LotSize;// spItem.ItemMinOrder;
                        item.ItemMaxQty = MaxOrderQty;
                        DuplicatedItemsCount++;
                        continue;
                    }


                    newId++;
                    item = new MasterSPItemSupplier
                    {

                        ItemSupplierID = newId,
                        ItemID = spItem.ItemID,
                        SupplierFOB = fob,
                        SupplierID = supplier.SupplierID,
                        ItemSupplierBarcode = supplierBarcode,
                        ItemSupNumber = itemSupplierNo,

                        Cancelled = false,
                        Status = (int)Status.Active,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay,
                        ItemSupAvgCostFactor = AvgCostFactor,
                        ItemSupAvgCost = AvgCost,
                        // ItemSupFinalPrice = AvgCost *AvgCostFactor,
                        ItemSupDiscountType = discountType,
                        ItemSupTaxType = taxType,
                        ItemMinQty = LotSize,// spItem.ItemMinOrder,
                        ItemMaxQty = MaxOrderQty,
                    };


                    //await _context.MasterSPItemSupplier.AddAsync(item);

                    UploaddedItemsCount++;
                    itemsList.Add(item);




                }
                catch
                {

                    FailedtoUploadItemsCount++;
                }

            }
            //  var newId = ((int?)_context.MasterSPGeneralItemCategory.Max(inv => (int?)inv.ItemCategoryID) ?? 0) + 1;

            if (model.Save != false)
            {
                await _context.MasterSPItemSupplier.AddRangeAsync(itemsList);


                var result = await _context.SaveChangesAsync();
            }
            var response = new ApiResponseModel(200, null);
            response.Data = new
            {
                InvalidItemNoCount,
                InvalidSupplierCount,
                InvalidDiscount,
                InvalidTax,
                InvalidValuesforCost,
                InvalidFOB,
                InvalidLot,
                InvalidMaxOrderQty,
                UploaddedItemsCount,
                DuplicatedItemsCount,
                FailedtoUploadItemsCount,
                InvalidItemSupplierMaster
            };



            return response;

            throw new ManagerProcessException("000008");
        }
        public async ValueTask<ApiResponseModel> UploadItemsSubstitutes(UploadSupplierMasterItemsModle2 model, int userId)
        {
            int UploaddedItemsCount = 0;
            int DuplicatedItemsCount = 0;
            int InvalidSubstituteTypeount = 0;
            int InvalidItemNoCount = 0;
            int FailedtoUploadItemsCount = 0;
            int InvalidSubstituteItemNoCount = 0;
            int MissingDataCount = 0;


            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "ItemsLocations");
            var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            var dt = ds.Tables[0];

            var MasterLookups = new List<MasterLookup>();


            var itemsList = new List<MasterSPItemSubstitute>();
            var MasterSPItem = new List<MasterSPItem>();

            var newId = ((int?)_context.MasterSPItemSubstitute.Max(inv => (int?)inv.ItemSubstituteID) ?? 0);
            var newIdSpItem = ((int?)_context.MasterSPItem.Max(inv => (int?)inv.ItemID) ?? 0);
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    continue;
                }

                try
                {
                    var itemNo = dt.Rows[i][0].ToString();
                    var substituteType = dt.Rows[i][1].ToString();
                    var substituteCode = dt.Rows[i][2].ToString();
                    var substituteItemNo = dt.Rows[i][3].ToString();
                    var substituteBarcod = dt.Rows[i][4].ToString();
                    if (string.IsNullOrEmpty(itemNo) ||
                        string.IsNullOrEmpty(substituteType) ||
                        string.IsNullOrEmpty(substituteItemNo) ||
                        string.IsNullOrEmpty(substituteCode)
                        )
                    {
                        MissingDataCount++;
                        continue;
                    }
                    var spItem = _context.MasterSPItem.Where(e => e.ItemCode == itemNo && e.Status == (int)Status.Active).FirstOrDefault();
                    if (spItem == null)
                    {
                        InvalidItemNoCount++;
                        continue;
                    }
                    var substituteTypeId = _context.MasterSPLookup.Where(e => e.LookupName == substituteType && e.LookupTypeID == (int)LookupTypes.SubstituteType)
                        .Select(e => e.LookupID).FirstOrDefault();
                    if (substituteTypeId == 0)
                    {
                        InvalidSubstituteTypeount++;
                        continue;
                    }
                    int SubstituteItemId = 0;
                    if (!substituteType.ToLower().Contains("single"))
                    {
                        SubstituteItemId = _context.MasterSPItem.Where(e => e.ItemCode == substituteItemNo && e.Status == (int)Status.Active).Select(e => e.ItemID).FirstOrDefault();
                        if (SubstituteItemId == 0)
                        {
                            InvalidSubstituteItemNoCount++;
                            continue;
                        }
                        var exist = _context.MasterSPItemSubstitute
                     .Where(e => e.ItemID == spItem.ItemID && e.SubstituteID == SubstituteItemId && e.SubstituteType == substituteTypeId && e.Cancelled == false).Any();
                        if (
                            exist ||
                            itemsList.Where(e => e.ItemID == spItem.ItemID && e.SubstituteID == SubstituteItemId && e.SubstituteType == substituteTypeId).Any()
                           )
                        {
                            DuplicatedItemsCount++;
                            continue;
                        }
                    }
                    else
                    {
                        SubstituteItemId = _context.MasterSPItem.Where(e => e.ItemCode == itemNo && e.Status == (int)Status.Active).Select(e => e.ItemID).FirstOrDefault();
                        if (SubstituteItemId == 0)
                        {
                            SubstituteItemId = MasterSPItem.Where(e => e.ItemCode == itemNo).Select(e => e.ItemID).FirstOrDefault();
                        }



                        if (SubstituteItemId == 0)
                        {


                            newIdSpItem++;
                            SubstituteItemId = newIdSpItem;
                            var spItemNew = new MasterSPItem
                            {
                                ItemID = newIdSpItem,
                                ItemCode = substituteItemNo,

                                ItemCodeSup = spItem.ItemCodeSup
                          ,
                                ItemCodeCust = spItem.ItemCodeCust
                          ,
                                ItemNameID = spItem.ItemNameID
                          ,
                                ItemCategoryID = spItem.ItemCategoryID
                          ,
                                ItemUnitofMeasurement = spItem.ItemUnitofMeasurement
                          ,
                                ItemQuantityPerUnit = spItem.ItemQuantityPerUnit

                          ,
                                ItemPrice = spItem.ItemPrice
                          ,
                                ItemLength = spItem.ItemLength
                          ,
                                ItemWidth = spItem.ItemWidth
                          ,
                                ItemHeight = spItem.ItemHeight
                          ,
                                ItemSizeM3 = spItem.ItemSizeM3
                          ,
                                ItemWeight = spItem.ItemWeight
                          ,
                                ItemMainSup = spItem.ItemMainSup
                          ,
                                ItemBarcode = spItem.ItemBarcode
                          ,
                                ItemPartCode = spItem.ItemPartCode
                          ,
                                ItemProdCode = spItem.ItemProdCode
                          ,
                                ItemFlagSup = spItem.ItemFlagSup
                          ,
                                ItemFlagOrder = spItem.ItemFlagOrder
                          ,
                                ItemFlagSales = spItem.ItemFlagSales
                          ,
                                ItemMinMaxType = spItem.ItemMinMaxType
                          ,
                                ItemMinOrder = spItem.ItemMinOrder
                          ,
                                ItemMaxOrder = spItem.ItemMaxOrder
                          ,
                                ItemSerialType = spItem.ItemSerialType
                          ,
                                ItemTariffID = spItem.ItemTariffID
                          ,
                                ItemTaxType = spItem.ItemTaxType
                          ,
                                ItemCreationDate = spItem.ItemCreationDate
                          ,
                                ItemGroup = (int)Infrastructure.ViewModels.Dtos.Enums.Settings.Group,
                                ItemDiscountType = spItem.ItemDiscountType,
                                ItemComments = spItem.ItemComments,
                                Cancelled = false,
                                Status = (int)Status.Active,
                                EnterUser = userId,
                                EnterDate = DateTime.Now,
                                EnterTime = DateTime.Now.TimeOfDay
                            };
                            MasterSPItem.Add(spItemNew);
                        }


                    }
                    var SubstituteCodeId = _lookupService.GeSpLookup(userId, substituteCode, (int)LookupTypes.SubstituteCode, substituteTypeId);

                    var exists = _context.MasterSPItemSubstitute
                          .Where(e => e.Cancelled == false &&
                          e.SubstituteNo == substituteItemNo &&
                          e.ItemID == spItem.ItemID &&
                          e.SubstituteCode == SubstituteCodeId &&
                          e.SubstituteID == SubstituteItemId).Any();
                    if (exists)
                    {
                        DuplicatedItemsCount++;
                        continue;
                    }
                    if (spItem.ItemID == SubstituteItemId && !substituteType.ToLower().Contains("single"))
                    {
                        DuplicatedItemsCount++;
                        continue;
                    }

                    newId++;
                    var item = new MasterSPItemSubstitute
                    {
                        ItemSubstituteID = newId,
                        SubstituteID = SubstituteItemId,
                        ItemID = spItem.ItemID,
                        SubstituteType = substituteTypeId,
                        SubstituteBarcode = substituteBarcod,
                        SubstituteCode = SubstituteCodeId,
                        SubstituteNo = substituteItemNo,
                        Cancelled = false,
                        Status = (int)Status.Active,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };

                    UploaddedItemsCount++;
                    itemsList.Add(item);




                }
                catch
                {

                    FailedtoUploadItemsCount++;
                }

            }

            if (model.Save != false)
            {
                await _context.MasterSPItemSubstitute.AddRangeAsync(itemsList);
                await _context.MasterSPItem.AddRangeAsync(MasterSPItem);


                var result = await _context.SaveChangesAsync();
            }
            var response = new ApiResponseModel(200, null);
            response.Data = new { MissingDataCount, InvalidItemNoCount, InvalidSubstituteItemNoCount, InvalidSubstituteTypeount, UploaddedItemsCount, DuplicatedItemsCount, FailedtoUploadItemsCount };

            return response;

            throw new ManagerProcessException("000008");
        }

        public async ValueTask<ApiResponseModel> UploadOtherSuppliersJPM(UpdateFileModel2 model, int userId)
        {
            int UploaddedItemsCount = 0;
            int DuplicatedItemsCount = 0;
            int FailedtoUploadItemsCount = 0;
            int MissingMandatory = 0;
            int InvalidSupplierCount = 0;
            int TMCSupplierCount = 0;
            string errorCodes = string.Empty;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            FileUploadModel tempFile = await _FileHelper.WriteFile(model.File, "UploadSupplierMasterItems");
            var ds = _FileHelper.ReadExcel(tempFile.ReturnUrl);
            var dt = ds.Tables[0];

            var MasterSPGeneralItemNames = new List<MasterSPGeneralItemName>();
            var MasterSPGeneralItemCategory = new List<MasterSPGeneralItemCategory>();

            int newId = ((int?)_context.MasterSPGeneralSupplierItems.Max(inv => (int?)inv.RecordID) ?? 0);
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                {
                    continue;
                }

                try
                {

                    var supplierName = dt.Rows[i][0].ToString();
                    var PartNo = dt.Rows[i][1].ToString();
                    //  var partNowithdash = dt.Rows[i][2].ToString();
                    var SubstitutionCode = dt.Rows[i][2].ToString();
                    var ItemSupSubstituteItem = dt.Rows[i][3].ToString();
                    var supplierFlage = dt.Rows[i][4].ToString();
                    var UnitFOBPrice = decimal.Parse(dt.Rows[i][5].ToString());
                    var PartName = dt.Rows[i][6].ToString();
                    var ItemSupLength = decimal.Parse(dt.Rows[i][7].ToString());
                    var ItemSupWidth = decimal.Parse(dt.Rows[i][8].ToString());
                    var ItemSupHeight = decimal.Parse(dt.Rows[i][9].ToString());
                    var ItemSupSizeM3 = decimal.Parse(dt.Rows[i][10].ToString());
                    var ItemSupWeight = decimal.Parse(dt.Rows[i][11].ToString());
                    var pn1 = dt.Rows[i][12].ToString();//partcode
                    var minOrderQty = int.Parse(dt.Rows[i][13].ToString());//
                    var barcode = dt.Rows[i][14].ToString();//
                    if (
                        string.IsNullOrEmpty(PartNo) ||
                        //  string.IsNullOrEmpty(partNowithdash) ||
                        string.IsNullOrEmpty(PartName)


                        )
                    {
                        MissingMandatory++;
                        continue;
                    }



                    var supplier = _context.GettingSPSupplier.Where(e => e.SupplierName == supplierName.Trim()).Select(e => new { e.SupplierID, e.TMCSupplier }).FirstOrDefault();

                    if (supplier == null)
                    {
                        InvalidSupplierCount++;
                        continue;
                    }
                    var supplierId = supplier.SupplierID;
                    if (supplier.TMCSupplier == true)
                    {
                        TMCSupplierCount++;

                        continue;
                    }

                    var item = _context.MasterSPGeneralSupplierItems.Where(e => e.ItemSupID == supplierId && e.ItemSupNo == PartNo).FirstOrDefault();


                    if (item != null)
                    {
                        item.ItemSupID = supplierId;
                        item.ItemSupNo = PartNo;
                        //item.partNowithdash = partNowithdash;
                        item.ItemSupFI_Id = supplierFlage;
                        item.ItemSupSubstituteType = SubstitutionCode;
                        item.ItemSupSubstituteItem = ItemSupSubstituteItem;

                        item.UnitFOBPrice = UnitFOBPrice;

                        item.ItemSupDesc = PartName;


                        item.ItemSupLength = ItemSupLength;
                        item.ItemSupWidth = ItemSupWidth;
                        item.ItemSupHeight = ItemSupHeight;
                        item.ItemSupSizeM3 = ItemSupSizeM3;
                        item.ItemSupWeight = ItemSupWeight;
                        item.PNC1 = pn1;

                        item.Parts = barcode;

                        item.Cancelled = false;
                        item.Status = (int)Status.Active;
                        item.ModUser = userId;
                        item.ModDate = DateTime.Now;
                        item.ModTime = DateTime.Now.TimeOfDay;
                        DuplicatedItemsCount++;
                        continue;
                    }
                    //object sub = dt.Rows[i][3];
                    //if(sub != null)
                    //{
                    //    var itemSubstituteCode = _context.MasterSPItemSubstitute.Where(e=>e.SubstituteCode==sub.ToString()).FirstOrDefault();

                    //}
                    UploaddedItemsCount++;
                    newId++;
                    item = new MasterSPGeneralSupplierItem
                    {
                        RecordID = newId,

                        ItemSupID = supplierId,
                        ItemSupNo = PartNo,
                        //  partNowithdash = partNowithdash,
                        ItemSupFI_Id = supplierFlage,

                        ItemSupSubstituteType = SubstitutionCode,
                        ItemSupSubstituteItem = ItemSupSubstituteItem,
                        UnitFOBPrice = UnitFOBPrice,
                        ItemSupDesc = PartName,

                        ItemSupLength = ItemSupLength,
                        ItemSupWidth = ItemSupWidth,
                        ItemSupHeight = ItemSupHeight,
                        ItemSupSizeM3 = ItemSupSizeM3,
                        ItemSupWeight = ItemSupWeight,
                        PNC1 = pn1,
                        Parts = barcode,
                        ItemSupMinOrd = minOrderQty,



                        Cancelled = false,
                        Status = (int)Status.Active,
                        EnterUser = userId,
                        EnterDate = DateTime.Now,
                        EnterTime = DateTime.Now.TimeOfDay
                    };

                    if (model.Save != false)
                    {
                        await _context.MasterSPGeneralSupplierItems.AddAsync(item);
                    }
                }
                catch (Exception ex)
                {

                    FailedtoUploadItemsCount++;
                }

            }

            if (model.Save != false)
            {
                var result = await _context.SaveChangesAsync();
            }


            var response = new ApiResponseModel(200, null);
            response.Data = new
            {
                UploaddedItemsCount,
                DuplicatedItemsCount,
                FailedtoUploadItemsCount,
                MissingMandatory,
                InvalidSupplierCount,
                TMCSupplierCount
            };

            return response;

            throw new ManagerProcessException("000008");
        }
    }
}
