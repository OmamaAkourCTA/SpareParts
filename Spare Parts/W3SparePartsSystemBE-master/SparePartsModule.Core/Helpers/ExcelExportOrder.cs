using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SparePartsModule.Domain.Context;
using Directory = System.IO.Directory;
using Selection = DocumentFormat.OpenXml.Spreadsheet.Selection;
using Workbook = DocumentFormat.OpenXml.Spreadsheet.Workbook;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;
using X15 = DocumentFormat.OpenXml.Office2013.Excel;

namespace SparePartsModule.Core.Helpers
{
    public class ExcelExportOrder
    {


        private readonly SparePartsModuleContext _contextSparePart;
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public ExcelExportOrder(IConfiguration config, IHostingEnvironment env, SparePartsModuleContext contextSparePart)
        {

            _config = config;
            _env = env;
            _contextSparePart = contextSparePart;

        }
        public void CreateFile()
        {

        }

        public string CreateExcelFileForPriceList(int PriceListId, int StatusType, string directory)
        {
            var fileName = DateTime.Now.Ticks + ".xlsx";
            //Create a new Name for the file due to security reasons.

            var pathBuilt = Path.Combine(_env.ContentRootPath, _config["Settings:Uploads"], directory);

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            var path = Path.Combine(_env.ContentRootPath, _config["Settings:Uploads"], directory, fileName);

            GenerateSheetdataForPriceList(PriceListId, "Uploads/" + directory + "/" + fileName);
            return "Uploads/" + directory + "/" + fileName;
        }

        public string CreateExcelFile(int orderId, string directory)
        {
            var fileName = DateTime.Now.Ticks + ".xlsx";
            //Create a new Name for the file due to security reasons.

            var pathBuilt = Path.Combine(_env.ContentRootPath, _config["Settings:Uploads"], directory);

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            var path = Path.Combine(_env.ContentRootPath, _config["Settings:Uploads"], directory, fileName);

            using (SpreadsheetDocument package = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
            {
                CreatePartsForExcel(package, orderId);
            }
            return "Uploads/" + directory + "/" + fileName;
        }

        private void CreatePartsForExcel(SpreadsheetDocument document, int orderId)
        {

            SheetData partSheetData = null;

            partSheetData = GenerateSheetdataForDetailsSparepart(orderId);



            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart1);

            WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            workbookStylesPart1.Stylesheet = GenerateStyleSheet();
            workbookStylesPart1.Stylesheet.Save();
            GenerateWorkbookStylesPartContent(workbookStylesPart1);

            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart1, partSheetData);
        }
        private void GenerateWorkbookPartContent(WorkbookPart workbookPart1)
        {
            Workbook workbook1 = new Workbook();
            Sheets sheets1 = new Sheets();
            Sheet sheet1 = new Sheet() { Name = "Sheet1", SheetId = (UInt32Value)1U, Id = "rId1" };
            sheets1.Append(sheet1);
            workbook1.Append(sheets1);
            workbookPart1.Workbook = workbook1;
        }

        private void GenerateWorksheetPartContent(WorksheetPart worksheetPart1, SheetData sheetData1)
        {
            Worksheet worksheet1 = new Worksheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };



            worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
            SheetDimension sheetDimension1 = new SheetDimension() { Reference = "A1" };

            SheetViews sheetViews1 = new SheetViews();

            SheetView sheetView1 = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U };
            Selection selection1 = new Selection() { ActiveCell = "A1", SequenceOfReferences = new ListValue<StringValue>() { InnerText = "A1" } };

            sheetView1.Append(selection1);

            sheetViews1.Append(sheetView1);
            SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.25D, DefaultColumnWidth = 50D };

            DocumentFormat.OpenXml.Drawing.Charts.PageMargins pageMargins1 = new DocumentFormat.OpenXml.Drawing.Charts.PageMargins() { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D };


            // Create custom widths for columns
            //Columns lstColumns = worksheet1.GetFirstChild<Columns>();
            //Boolean needToInsertColumns = false;
            //if (lstColumns == null)
            //{
            //    lstColumns = new Columns();
            //    needToInsertColumns = true;
            //}
            //lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 25, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 9, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 9, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 9, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 5, Max = 5, Width = 13, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 6, Max = 6, Width = 17, CustomWidth = true });
            //lstColumns.Append(new Column() { Min = 7, Max = 7, Width = 12, CustomWidth = true });
            
            // Check if the Columns element exists; if not, create it
            //Columns column = worksheet1.GetFirstChild<Columns>();
            //if (column == null)
            //{
            //    column = new Columns();
            //    worksheet1.InsertAt(column, 0); // Insert Columns as the first child
            //}
            //else
            //{
            //    column.RemoveAllChildren<Column>(); // Clear existing columns
            //}

            //// Define the widths for each column
            //Columns columns = new Columns(
            //    new Column() { Min = 1, Max = 1, Width = 14, CustomWidth = true },  // Column A
            //    new Column() { Min = 2, Max = 2, Width = 14, CustomWidth = true },  // Column B
            //    new Column() { Min = 3, Max = 3, Width = 14, CustomWidth = true },  // Column C
            //    new Column() { Min = 4, Max = 4, Width = 14, CustomWidth = true },  // Column D
            //    new Column() { Min = 5, Max = 5, Width = 14, CustomWidth = true },  // Column E
            //    new Column() { Min = 6, Max = 6, Width = 14, CustomWidth = true },  // Column F
            //    new Column() { Min = 7, Max = 7, Width = 45, CustomWidth = true }   // Column G
            //);
            //worksheet1.Append(columns);

            worksheet1.Append(sheetDimension1);
            worksheet1.Append(sheetViews1);
            worksheet1.Append(sheetFormatProperties1);
            worksheet1.Append(sheetData1);
            //if (needToInsertColumns)
            //worksheet1.InsertAt(lstColumns, 0);
            worksheet1.Append(pageMargins1);

            worksheetPart1.Worksheet = worksheet1;
        }
        private void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart1)
        {
            Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            Fonts fonts1 = new Fonts() { Count = (UInt32Value)2U, KnownFonts = true };

            Font font1 = new Font();
            FontSize fontSize1 = new FontSize() { Val = 11D };
            Color color1 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName1 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

            font1.Append(fontSize1);
            font1.Append(color1);
            font1.Append(fontName1);
            font1.Append(fontFamilyNumbering1);
            font1.Append(fontScheme1);

            Font font2 = new Font();
            Bold bold1 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = 11D };
            Color color2 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName2 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering2 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme2 = new FontScheme() { Val = FontSchemeValues.Minor };

            font2.Append(bold1);
            font2.Append(fontSize2);
            font2.Append(color2);
            font2.Append(fontName2);
            font2.Append(fontFamilyNumbering2);
            font2.Append(fontScheme2);

            fonts1.Append(font1);
            fonts1.Append(font2);

            Fills fills1 = new Fills() { Count = (UInt32Value)2U };

            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };

            fill1.Append(patternFill1);

            Fill fill2 = new Fill();
            PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

            fill2.Append(patternFill2);

            fills1.Append(fill1);
            fills1.Append(fill2);

            Borders borders1 = new Borders() { Count = (UInt32Value)2U };

            Border border1 = new Border();
            LeftBorder leftBorder1 = new LeftBorder();
            RightBorder rightBorder1 = new RightBorder();
            TopBorder topBorder1 = new TopBorder();
            BottomBorder bottomBorder1 = new BottomBorder();
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();

            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);

            Border border2 = new Border();

            LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
            Color color3 = new Color() { Indexed = (UInt32Value)64U };

            leftBorder2.Append(color3);

            RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
            Color color4 = new Color() { Indexed = (UInt32Value)64U };

            rightBorder2.Append(color4);

            TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
            Color color5 = new Color() { Indexed = (UInt32Value)64U };

            topBorder2.Append(color5);

            BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
            Color color6 = new Color() { Indexed = (UInt32Value)64U };

            bottomBorder2.Append(color6);
            DiagonalBorder diagonalBorder2 = new DiagonalBorder();

            border2.Append(leftBorder2);
            border2.Append(rightBorder2);
            border2.Append(topBorder2);
            border2.Append(bottomBorder2);
            border2.Append(diagonalBorder2);

            borders1.Append(border1);
            borders1.Append(border2);

            CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

            cellStyleFormats1.Append(cellFormat1);

            CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)3U };
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyBorder = true };
            CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyFont = true, ApplyBorder = true };

            cellFormats1.Append(cellFormat2);
            cellFormats1.Append(cellFormat3);
            cellFormats1.Append(cellFormat4);

            CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

            cellStyles1.Append(cellStyle1);
            DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
            TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

            StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

            StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
            stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
            X14.SlicerStyles slicerStyles1 = new X14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };

            stylesheetExtension1.Append(slicerStyles1);

            StylesheetExtension stylesheetExtension2 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
            stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");
            X15.TimelineStyles timelineStyles1 = new X15.TimelineStyles() { DefaultTimelineStyle = "TimeSlicerStyleLight1" };

            stylesheetExtension2.Append(timelineStyles1);

            stylesheetExtensionList1.Append(stylesheetExtension1);
            stylesheetExtensionList1.Append(stylesheetExtension2);

            stylesheet1.Append(fonts1);
            stylesheet1.Append(fills1);
            stylesheet1.Append(borders1);
            stylesheet1.Append(cellStyleFormats1);
            stylesheet1.Append(cellFormats1);
            stylesheet1.Append(cellStyles1);
            stylesheet1.Append(differentialFormats1);
            stylesheet1.Append(tableStyles1);
            stylesheet1.Append(stylesheetExtensionList1);

            workbookStylesPart1.Stylesheet = stylesheet1;
        }

        private void GenerateSheetdataForPriceList(int PriceListID, string filePath)
        {

            //var data = _salesOrdersContext.SalesPriceList1.Where(e => e.Cancelled == false && e.PriceListID == PriceListID
            //   ).Select(e => new
            //   {
            //       e.PriceListID,
            //       e.PriceListGroup,
            //       e.PriceListDepartment,
            //       e.PriceListYear,
            //       e.PriceListNo,
            //       e.PriceListSeq,
            //       e.PriceListDate,
            //       e.PriceListType,
            //       //PriceListTypeName = _markaziaMasterContext.MasterLookup.Where(x => x.LookupID == e.PriceListType).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
            //       e.PriceListName,
            //       e.PriceListDesc,
            //       e.PriceListValidFrom,
            //       e.PriceListValidTo,
            //       e.PriceListApproval,
            //       //PricelistItems = _salesOrdersContext.SalesPriceList2.Where(x => x.PriceListID == e.PriceListID)
            //       //.Select(y => new
            //       //{
            //       //    y.PriceListID,
            //       //    y.PriceListLineID,
            //       //    y.PriceListLineSeq,
            //       //    //y.PriceItemBrandName,
            //       //    PriceItemBrandName = _context.MasterVehicleBrand.Where(x => x.BrandID == y.PriceItemBrandName).Select(x => new { x.BrandID, x.BrandName }).FirstOrDefault(),
            //       //    //y.PriceItemModelName,
            //       //    PriceItemModelName = _context.MasterVehicleModel.Where(x => x.ModelID == y.PriceItemModelName).Select(x => new { x.ModelID, x.ModelName, x.ModelYear }).FirstOrDefault(),
            //       //    y.PriceItemYearFrom,
            //       //    y.PriceItemYearTo,
            //       //    y.PriceItemID,
            //       //    y.PriceType,
            //       //    y.Price,
            //       //    y.PriceFactor,
            //       //    y.PriceSystemType,
            //       //})
            //       //.ToList(),
            //   }).FirstOrDefault();

            //   var data2 = _salesOrdersContext.SalesPriceList2.Where(x => x.PriceListID == PriceListID)
            //       .Select(y => new
            //       {
            //           y.PriceListID,
            //           y.PriceListLineID,
            //           y.PriceListLineSeq,
            //           //y.PriceItemBrandName,
            //           PriceItemBrandName = _context.MasterVehicleBrand.Where(x => x.BrandID == y.PriceItemBrandName).Select(x => new { x.BrandID, x.BrandName }).FirstOrDefault(),
            //           //y.PriceItemModelName,
            //           PriceItemModelName = _context.MasterVehicleModel.Where(x => x.ModelID == y.PriceItemModelName).Select(x => new { x.ModelID, x.ModelName, x.ModelYear }).FirstOrDefault(),
            //           y.PriceItemYearFrom,
            //           y.PriceItemYearTo,
            //           y.PriceItemID,
            //           y.PriceType,
            //           y.Price,
            //           y.PriceFactor,
            //           y.PriceSystemType,
            //       })
            //       .ToList();

            //var data = _salesOrdersContext.SalesPriceList1.Where(e => e.Cancelled == false && e.PriceListID == PriceListID
            //  ).Select(e => new
            //  {
            //      e.PriceListID,
            //      e.PriceListGroup,
            //      e.PriceListDepartment,
            //      e.PriceListYear,
            //      e.PriceListNo,
            //      e.PriceListSeq,
            //      e.PriceListDate,
            //      e.PriceListType,
            //      e.PriceListName,
            //      e.PriceListDesc,
            //      e.PriceListValidFrom,
            //      e.PriceListValidTo,
            //      e.PriceListApproval,
            //  });

            //// Fetch additional details from _contextMarkaziaMaster
            //var priceListApprovalIds = data.Select(e => e.PriceListApproval).ToList();
            //var priceListType = data.Select(e => e.PriceListType).ToList();

            //var priceListApprovals = _markaziaMasterContext.MasterLookup
            //    .Where(x => priceListApprovalIds.Contains(x.LookupID))
            //    .Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor })
            //    .ToDictionary(x => x.LookupID);

            //var priceListTypeids = _markaziaMasterContext.MasterLookup
            //    .Where(x => priceListType.Contains(x.LookupID))
            //    .Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor })
            //    .ToDictionary(x => x.LookupID);

            ////PricelistItems
            //var PricelistItems = _salesOrdersContext.SalesPriceList2
            //    .Where(e => e.Cancelled == false && e.PriceListID == PriceListID)
            //    .ToList();

            //var brandIds = PricelistItems.Select(pi => pi.PriceItemBrandName).Distinct().ToList();
            //var modelIds = PricelistItems.Select(pi => pi.PriceItemModelName).Distinct().ToList();
            //var priceIds = PricelistItems.Select(pi => pi.PriceItemID).Distinct().ToList();
            //var pricetypeIds = PricelistItems.Select(pi => pi.PriceType).Distinct().ToList();
            //var priceSystemIds = PricelistItems.Select(pi => pi.PriceSystemType).Distinct().ToList();
            //var customIds = PricelistItems.Select(pi => pi.PriceCustom).Distinct().ToList();

            //var brandNames = _context.MasterVehicleBrand
            //  .Where(b => brandIds.Contains(b.BrandID))
            //  .ToDictionary(b => b.BrandID, b => b.BrandName);

            //var modelNames = _context.MasterVehicleModel
            // .Where(b => modelIds.Contains(b.ModelID))
            // .ToDictionary(b => b.ModelID, b => b.ModelName);

            //var PriceItem = _context.MasterVehilceList
            //   .Where(b => priceIds.Contains(b.ListID))
            //   .ToDictionary(b => b.ListID, b => b.ListName);

            //var PriceCustom = _markaziaMasterContext.MasterLookup
            //   .Where(x => customIds.Contains(x.LookupID))
            //   .Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor })
            //   .ToDictionary(x => x.LookupID);

            //var Pricetype = _markaziaMasterContext.MasterLookup
            //   .Where(x => pricetypeIds.Contains(x.LookupID))
            //   .Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor })
            //   .ToDictionary(x => x.LookupID);

            //var PriceSystem = _markaziaMasterContext.MasterLookup
            //   .Where(x => priceSystemIds.Contains(x.LookupID))
            //   .Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor })
            //   .ToDictionary(x => x.LookupID);

            //var combinedData = data.Select(e => new
            //{
            //    e.PriceListID,
            //    e.PriceListYear,
            //    e.PriceListNo,
            //    e.PriceListSeq,
            //    e.PriceListDate,
            //    PriceListType = e.PriceListType.HasValue && priceListTypeids.ContainsKey(e.PriceListType.Value) ? priceListTypeids[e.PriceListType.Value] : null,
            //    e.PriceListName,
            //    e.PriceListDesc,
            //    e.PriceListValidFrom,
            //    e.PriceListValidTo,
            //    PriceListApprovalDetails = priceListApprovals.ContainsKey(e.PriceListApproval) ? priceListApprovals[e.PriceListApproval] : null,
            //    PricelistItem = PricelistItems.Select(p => new
            //    {
            //        p.PriceListLineID,
            //        p.PriceListID,
            //        p.PriceListLineSeq,
            //        PriceItemBrandName = brandNames.ContainsKey(p.PriceItemBrandName) ? brandNames[p.PriceItemBrandName] : null,
            //        PriceItemModelName = modelNames.ContainsKey(p.PriceItemModelName) ? modelNames[p.PriceItemModelName] : null,
            //        p.PriceItemYearFrom,
            //        p.PriceItemYearTo,
            //        p.PriceItemClassification1,
            //        p.PriceItemClassification2,
            //        p.PriceItemClassification3,
            //        PriceItemID = PriceItem.ContainsKey(p.PriceItemID) ? PriceItem[p.PriceItemID] : null,
            //        PriceCustom = PriceCustom.ContainsKey(p.PriceCustom) ? PriceCustom[p.PriceCustom] : null,
            //        PriceType = Pricetype.ContainsKey(p.PriceType) ? Pricetype[p.PriceType] : null,
            //        p.Price,
            //        p.PriceFactor,
            //        PriceSystemType = p.PriceSystemType.HasValue && PriceSystem.ContainsKey(p.PriceSystemType.Value) ? PriceSystem[p.PriceSystemType.Value] : null,
            //        p.EnterDate,
            //        p.EnterTime,
            //    }).ToList()
            //}).FirstOrDefault();

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Price List"
                };
                sheets.Append(sheet);

                //SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                //// Set header
                //Row headerRow  = new Row();
                //sheetData.Append(CreateCell("Price List", 18, true));
                //sheetData.Append(headerRow);

                //Row row = new Row();
                //row.Append(CreateCell("Brand Name : " + combinedData.PricelistItem.Select(x => x.PriceItemBrandName).FirstOrDefault()));
                //row.Append(CreateCell("Price List Type : " + combinedData.PriceListType.LookupName));
                //row.Append(CreateCell("Price List Seq : " + combinedData.PriceListSeq));
                //row.Append(CreateCell("Price List Name : " + combinedData.PriceListName));
                //row.Append(CreateCell("Export Date: " + DateTime.Now.ToString("dd/MM/yyyy")));
                //sheetData.Append(row);

                //// Set table headers
                //row = new Row();
                //row.Append(
                //    CreateCell("Model"),
                //    CreateCell("List Name"),
                //    CreateCell("Model Year")
                //);

                //int maxPrices = combinedData.PricelistItem
                //.GroupBy(p => new { p.PriceItemModelName, p.PriceItemYearFrom, p.PriceType }).Count();

                //for (int i = 1; i <= maxPrices; i++)
                //{
                //    row.Append(CreateCell($"Price Type {i}"));
                //}
                //sheetData.Append(row);

                //var groupedData = combinedData.PricelistItem.GroupBy(p => new { p.PriceItemModelName, p.PriceItemYearFrom });
                //foreach (var group in groupedData)
                //{
                //    row = new Row();
                //    row.Append(CreateCell(group.Key.PriceItemModelName));
                //    row.Append(CreateCell(combinedData.PriceListName)); // Assuming PriceListName is the same for all
                //    row.Append(CreateCell(group.Key.PriceItemYearFrom.ToString()));
                //    foreach (var item in group)
                //    {
                //        row.Append(CreateCell(item.Price.ToString()));
                //    }
                //    // Append empty cells if there are fewer prices than the maximum number
                //    int remainingCells = maxPrices - group.Count();
                //    for (int i = 0; i < remainingCells; i++)
                //    {
                //        row.Append(CreateCell(""));
                //    }
                //    sheetData.Append(row);
                //}

                workbookPart.Workbook.Save();
            }

            //return sheetData1;
        }

        private Cell CreateCell(string text, double fontSize = 11, bool bold = false)
        {
            Cell cell = new Cell() { CellValue = new CellValue(text), DataType = CellValues.String };

            if (fontSize != 11 || bold)
            {
                cell.StyleIndex = 1;
            }

            return cell;
        }



        private SheetData GenerateSheetdataForDetailsSparepart(int orderId)
        {
            SheetData sheetData1 = new SheetData();

            var order = _contextSparePart.GettingSPOrder1.Where(e => e.Cancelled == false && e.OrderID == orderId
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


             OrderType = _contextSparePart.MasterSPLookup.Where(x => x.LookupID == e.OrderType).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
             OrderMethod = _contextSparePart.MasterSPLookup.Where(x => x.LookupID == e.OrderMethod).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
             OrderFreight = _contextSparePart.MasterSPLookup.Where(x => x.LookupID == e.OrderFreight).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
             Supplier = _contextSparePart.GettingSPSupplier.Where(x => x.SupplierID == e.OrderSupplierID).Select(x => new { x.SupplierID, x.SupplierName, x.SupplierNo }).FirstOrDefault(),
             OrderCurrency = _contextSparePart.MasterLookup.Where(x => x.LookupID == e.OrderCurrency).Select(x => new { x.LookupID, x.LookupName, }).FirstOrDefault(),


             OrderApproval = _contextSparePart.MasterSPLookup.Where(x => x.LookupID == e.OrderApproval).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),

             Status = _contextSparePart.MasterLookup.Where(x => x.LookupID == e.Status).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
             e.EnterDate,
             e.EnterTime,
             e.ModDate,
             e.ModTime,
             EnterUser = _contextSparePart.Users.Where(x => x.UserId == e.EnterUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),
             ModUser = _contextSparePart.Users.Where(x => x.UserId == e.ModUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),
             GettingSPOrder2 = _contextSparePart.GettingSPOrder2.Where(x => x.OrderID == e.OrderID && e.Cancelled == false)
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
                 x.OrderItemDisPer,
                 x.OrderItemDisAmount,
                 x.OrderItemNetAmount,
                 x.OrderItemTaxPer,
                 x.OrderItemTaxAmount,
                 x.OrderItemTotalAmount,
                 OrderItemSupplierFlag = _contextSparePart.MasterSPLookup.Where(y => y.LookupID == x.OrderItemSupplierFlag).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                 OrderItemOrderFlag = _contextSparePart.MasterSPLookup.Where(y => y.LookupID == x.OrderItemOrderFlag).Select(x => new { x.LookupID, x.LookupName, x.LookupBGColor, x.LookupTextColor }).FirstOrDefault(),
                 Item = _contextSparePart.MasterSPItem.Where(z => z.ItemID == x.OrderItemID).Select(w => new
                 {
                     w.ItemID,
                     w.ItemPartCode,
                     w.ItemCode,
                     ItemName = _contextSparePart.MasterSPGeneralItemNames.Where(z => z.ItemNameID == z.ItemNameID)
                .Select(z => new { z.ItemNameID, z.ItemNameDesc, z.ItemNameAr, z.ItemNameEn, w.ItemCode }).FirstOrDefault()
                 }).FirstOrDefault(),


                 x.OrderItemQtyOnHand,
                 x.OrderItemQtyOnOrder,
                 x.OrderItemQtyOrderMin,
                 x.OrderItemQtyOrderMax,
                 x.Status,
                 x.Cancelled,
                 x.CancelDate,
                 // EnterUser = _contextSparePart.Users.Where(y => y.UserId == x.EnterUser).Select(x => new { x.UserId, x.FullName }).FirstOrDefault(),


                 x.EnterDate,
                 x.EnterTime,
                 x.ModUser,
                 x.ModDate,
                 x.ModTime
             }).ToList()



         }).FirstOrDefault();

            Row partsRows = null;

            partsRows = CreateHeaderRowForExcel("Order Id", "Order No", "Order Date", "Supplier", "Currency", "Exchange Rate", "Remark");
            sheetData1.Append(partsRows);

            partsRows = GenerateRowForChildPartDetail(order.OrderID.ToString(), order.OrderNo, order.OrderDate.ToString("yyyy-MM-dd"),
                order.Supplier.SupplierName, order.OrderCurrency == null ? "" : order.OrderCurrency.LookupName, order.OrderCurrencyExchangeRate.ToString(), order.OrderComments ?? "");
            sheetData1.Append(partsRows);





            partsRows = CreateHeaderRowForExcel("Seq", "Item code", "Ordered Qty", "Order Price", "Confirmed Qty", "Confirmed Price", "Supplier flag", "Order Flag");

            sheetData1.Append(partsRows);



            foreach (var item in order.GettingSPOrder2)
            {
                 partsRows = GenerateRowForChildPartDetail(item.OrderLineSeq.ToString(), item.Item.ItemCode,
                   item.OrderItemQty.ToString(),
                   item.OrderItemPrice.ToString(),item.OrderItemQtyConfirmed.ToString(),item.OrderItemPriceConfirmed.ToString(), item.OrderItemSupplierFlag == null ? "" : item.OrderItemSupplierFlag.LookupName, item.OrderItemOrderFlag == null ? "" : item.OrderItemOrderFlag.LookupName);

                //partsRows = GenerateRowForChildPartDetail(item.OrderLineSeq.ToString(), item.Item.ItemCode,
                //    item.OrderItemQtyConfirmed > 0 ? item.OrderItemQtyConfirmed.ToString() : item.OrderItemQty.ToString(),
                //   item.OrderItemPriceConfirmed > 0 ? item.OrderItemPriceConfirmed.ToString() : item.OrderItemPrice.ToString(),item.OrderItemQtyConfirmed.ToString(),item.OrderItemPriceConfirmed.ToString(), item.OrderItemSupplierFlag == null ? "" : item.OrderItemSupplierFlag.LookupName, item.OrderItemOrderFlag == null ? "" : item.OrderItemOrderFlag.LookupName);

                sheetData1.Append(partsRows);

            }


            return sheetData1;
        }

        private Row GenerateRowForChildPartDetail(params string[] keys)
        {
            Row tRow = new Row();
            //  MergeCells mergeCells = new MergeCells();
            foreach (var key in keys)
            {
                var cell = CreateCell(key);
                tRow.Append(cell);
            }


            return tRow;
        }
        private Cell CreateCell(string text)
        {
            Cell cell = new Cell();
            cell.StyleIndex = 1U;
            cell.DataType = ResolveCellDataTypeOnValue(text);
            cell.CellValue = new CellValue(text);
            return cell;
        }
        private Cell CreateCell(string text, uint styleIndex)
        {
            Cell cell = new Cell();
            cell.StyleIndex = styleIndex;
            cell.DataType = ResolveCellDataTypeOnValue(text);
            cell.CellValue = new CellValue(text);

            return cell;
        }
        private Row CreateHeaderRowForExcel(string key, string value, int index)
        {
            Row workRow = new Row();
            //  MergeCells mergeCells = new MergeCells();
            var cell = CreateCell(key, 2U);

            //  cell.StyleIndex = 1;


            workRow.Append(cell);
            workRow.Append(CreateCell(value, 2U));
            //  workRow.Append(CreateCell("Test Name", 2U));
            //mergeCells.Append(new MergeCell() { Reference = new StringValue($"A{index}:B{index}") });

            return workRow;
        }
        private Row CreateHeaderRowForExcel(params string[] keys)
        {
            Row workRow = new Row();
            //  MergeCells mergeCells = new MergeCells();
            foreach (var key in keys)
            {
                var cell = CreateCell(key, 2U);
                workRow.Append(cell);
            }


            return workRow;
        }
        private EnumValue<CellValues> ResolveCellDataTypeOnValue(string text)
        {
            int intVal;
            double doubleVal;
            if (int.TryParse(text, out intVal) || double.TryParse(text, out doubleVal))
            {
                return CellValues.Number;
            }
            else
            {
                return CellValues.String;
            }
        }
        private Stylesheet GenerateStyleSheet()
        {
            return new Stylesheet(
            new Fonts(
            //new Font( // Index 0 - The default font.
            //new FontSize() { Val = 11 },
            //new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
            //new FontName() { Val = "Calibri" }),
            new Font( // Index 1 - The bold font.
            new Bold(),
            new FontSize() { Val = 11 },
            new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                            new FontName() { Val = "Calibri" })//,
            //new Font( // Index 2 - The Italic font.
            //new Italic(),
            //new FontSize() { Val = 11 },
            //new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
            //new FontName() { Val = "Calibri" }),
            //new Font( // Index 2 - The Times Roman font. with 16 size
            //new FontSize() { Val = 16 },
            //new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
            //new FontName() { Val = "Times New Roman" })
            ),
            new Fills(
            //new Fill( // Index 0 - The default fill.
            //new PatternFill() { PatternType = PatternValues.None }),
            new Fill( // Index 1 - The default fill of gray 125 (required)
            new PatternFill() { PatternType = PatternValues.Gray125, BackgroundColor = new BackgroundColor() { Rgb = new HexBinaryValue() { Value = "808080" } } })//,
            //new Fill( // Index 2 - The yellow fill.
            //new PatternFill(
            //new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } }
            //)
            //{ PatternType = PatternValues.Solid })
            ),
            new Borders(
            new Border( // Index 0 - The default border.
            new LeftBorder(),
            new RightBorder(),
            new TopBorder(),
            new BottomBorder(),
            new DiagonalBorder()),
            new Border( // Index 1 - Applies a Left, Right, Top, Bottom border to a cell
            new LeftBorder(
            new Color() { Auto = true }
            )
            { Style = BorderStyleValues.Thin },
            new RightBorder(
            new Color() { Auto = true }
            )
            { Style = BorderStyleValues.Thin },
            new TopBorder(
            new Color() { Auto = true }
            )
            { Style = BorderStyleValues.Thin },
            new BottomBorder(
            new Color() { Auto = true }
            )
            { Style = BorderStyleValues.Thin },
            new DiagonalBorder())
            ),
            new CellFormats(
            new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 }, // Index 0 - The default cell style. If a cell does not have a style index applied it will use this style combination instead
            new CellFormat() { FontId = 1, FillId = 0, BorderId = 0, ApplyFont = true }, // Index 1 - Bold
            new CellFormat() { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true }, // Index 2 - Italic
            new CellFormat() { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true }, // Index 3 - Times Roman
            new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true }, // Index 4 - Yellow Fill
            new CellFormat( // Index 5 - Alignment
            new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
            )
            { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
            new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true } // Index 6 - Border
            )
            ); // return

        }
    }

    public class VehiclePrice
    {
        public string Model { get; set; }
        public string ListName { get; set; }
        public int ModelYear { get; set; }
        public List<decimal> Prices { get; set; }
        public decimal SpecialCampaignPrice { get; set; }
        public decimal SpecialCampaignPriceWithoutOffer { get; set; }
    }
}
