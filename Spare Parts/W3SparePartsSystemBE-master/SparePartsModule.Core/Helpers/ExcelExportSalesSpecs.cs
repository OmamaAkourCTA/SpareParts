using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Domain.Context;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;
using X15 = DocumentFormat.OpenXml.Office2013.Excel;

namespace SparePartsModule.Core.Helpers
{
    public class ExcelExportSalesSpecs
    {

        private readonly VehicleSystemContext _context;
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public ExcelExportSalesSpecs(VehicleSystemContext context, IConfiguration config, IHostingEnvironment env)
        {
            _context = context;
            _config = config;
            _env = env; 

        }
        public void CreateFile()
        {
        
        }
        public string  CreateExcelFile(int ListId, string directory,string language)
        {
          var   fileName = DateTime.Now.Ticks + ".xlsx";
            //Create a new Name for the file due to security reasons.

            var pathBuilt = Path.Combine(_env.ContentRootPath, _config["Settings:Uploads"], directory);

            if (!Directory.Exists(pathBuilt))
            {
                Directory.CreateDirectory(pathBuilt);
            }

            var path = Path.Combine(_env.ContentRootPath, _config["Settings:Uploads"], directory, fileName);


      
         
            
           

            using (SpreadsheetDocument package = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
            {
                CreatePartsForExcel(package, ListId,language);
            }
            return "Uploads/" + directory + "/" + fileName;
        }
        private void CreatePartsForExcel(SpreadsheetDocument document, int ListId,string language)
        {
            SheetData partSheetData = GenerateSheetdataForDetails(ListId,language);

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
            SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.25D,DefaultColumnWidth=50D };

            PageMargins pageMargins1 = new PageMargins() { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D };
            worksheet1.Append(sheetDimension1);
            worksheet1.Append(sheetViews1);
            worksheet1.Append(sheetFormatProperties1);
            worksheet1.Append(sheetData1);
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
        private SheetData GenerateSheetdataForDetails(int ListId,string language)
        {
            SheetData sheetData1 = new SheetData();
            //  sheetData1.Append(CreateHeaderRowForExcel());
            var list = _context.MasterVehilceList.Where(e => e.ListID == ListId).Select(e => new
            {
                e.ListName,
                ModelName = _context.MasterVehicleModel.Where(x => x.ModelID == e.ListModelID).Select(x => new { x.ModelName, x.ModelNameAR }).FirstOrDefault(),
                ModelCode = _context.MasterVehicleModelCode.Where(x => x.ModelCodeID == e.ListModelCodeID).Select(x => new { x.ModelCodeVehicles }).FirstOrDefault(),
            }).FirstOrDefault();
            if (list == null)
            {
                throw new ManagerProcessException("000071");
            }
            Row partsRows = null;
            if (language == "En")
            {
                 partsRows = CreateHeaderRowForExcel("Vehicle Info.", "",1);
                partsRows.StyleIndex = 0;

                sheetData1.Append(partsRows);

                partsRows = GenerateRowForChildPartDetail("Model Name", list.ModelName.ModelName.ToString());
                partsRows.StyleIndex = 1;
                sheetData1.Append(partsRows);

                partsRows = GenerateRowForChildPartDetail("Model Code", list.ModelCode.ModelCodeVehicles.ToString());
                sheetData1.Append(partsRows);


                partsRows = GenerateRowForChildPartDetail("List Name", list.ListName.ToString());
                partsRows.StyleIndex = 2;
                sheetData1.Append(partsRows);
            }
            else
            {
                 partsRows = CreateHeaderRowForExcel("", "معلومات المركبة",1);

                sheetData1.Append(partsRows);

                partsRows = GenerateRowForChildPartDetail( list.ModelName.ModelName.ToString(), "موديل المركبة");
                sheetData1.Append(partsRows);

                partsRows = GenerateRowForChildPartDetail( list.ModelCode.ModelCodeVehicles.ToString(), "رمز المركبة");
                sheetData1.Append(partsRows);


                partsRows = GenerateRowForChildPartDetail( list.ListName.ToString(), "صنف المركبة");
                sheetData1.Append(partsRows);
            }
       

            var categories = _context.MasterVehicleLookup
    .Where(x => x.LookupTypeID == (int)LookupTypes.MasterSpecsCategory && x.Status == (int)Status.Active).Select(x => new { x.LookupID, x.LookupName }).ToList();
            int index = 0;
         
            foreach (var category in categories)
            {
                var genSpecs = _context.MasterVehicleGeneralSpecs.Where(e => e.GeneralSpecsCategory == category.LookupID).OrderBy(e => e.GeneralSpecsCategory).ToList();
                if (genSpecs != null)
                {
                    var isForsales = genSpecs.Where(e => e.GeneralSpecsShowonSalesSheet == true).Any();

                    if (isForsales)
                    {

                        index++;
                        if (language == "En")
                        {

                            partsRows = CreateHeaderRowForExcel(category.LookupName, "", index);
                        }
                        else
                        {
                            partsRows = CreateHeaderRowForExcel("",category.LookupName, index);
                        }
                        sheetData1.Append(partsRows);

                    }


                    foreach (var spec in genSpecs)
                    {
                        if (spec.GeneralSpecsShowonSalesSheet == true)
                        {
                            var specsValues = _context.MasterVehilceListSpecsValues
                             .Where(e => e.ListID == ListId && e.GeneralSpecID == spec.GeneralSpecID)
                             .Select(e => new {
                                 GeneralSpecsItemValue = _context.MasterVehicleGeneralSpecsValues.Where(x => x.GeneralSpecsItemID == e.GeneralSpecItemID)
                             .Select(x => new { x.GeneralSpecsItemValueEN, x.GeneralSpecsItemValueAR }).FirstOrDefault()
                             }
                             )
                             .FirstOrDefault();

                            if (specsValues != null)
                            {
                             


                                if (language == "En")
                                {

                                    partsRows = GenerateRowForChildPartDetail(spec.GeneralSpecsNameEN??"", specsValues.GeneralSpecsItemValue==null?"": specsValues.GeneralSpecsItemValue.GeneralSpecsItemValueEN??"");
                                    sheetData1.Append(partsRows);

                                }
                                else
                                {
                                    partsRows = GenerateRowForChildPartDetail(specsValues.GeneralSpecsItemValue == null ? "" : specsValues.GeneralSpecsItemValue.GeneralSpecsItemValueAR??"", spec.GeneralSpecsNameAR??"");
                                    sheetData1.Append(partsRows);


                                }
                            }

                        }
                    }
                }


            }




            //foreach (TestModel testmodel in data.testData)
            //{
            //    Row partsRows = GenerateRowForChildPartDetail(testmodel);
            //    sheetData1.Append(partsRows);
            //}
            return sheetData1;
        }
        private Row GenerateRowForChildPartDetail(string key,string value)
        {
       
            Row tRow = new Row();
     
            tRow.Append(CreateCell(key));
            tRow.Append(CreateCell(value));
           






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
        private Row CreateHeaderRowForExcel(string key, string value,int index)
        {
            Row workRow = new Row();
          //  MergeCells mergeCells = new MergeCells();
            var cell = CreateCell(key, 2U);
            //  Color color1 = new Color() { Theme = (UInt32Value)1U };

            cell.StyleIndex = 0;


            workRow.Append(cell);
            workRow.Append(CreateCell(value, 2U));
            //  workRow.Append(CreateCell("Test Name", 2U));
            //mergeCells.Append(new MergeCell() { Reference = new StringValue($"A{index}:B{index}") });
            
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
            new Font( // Index 0 - The default font.
            new FontSize() { Val = 11 },
            new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
            new FontName() { Val = "Calibri" }),
            new Font( // Index 1 - The bold font.
            new Bold(),
            new FontSize() { Val = 11 },
            new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                            new FontName() { Val = "Calibri" }),
            new Font( // Index 2 - The Italic font.
            new Italic(),
            new FontSize() { Val = 11 },
            new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
            new FontName() { Val = "Calibri" }),
            new Font( // Index 2 - The Times Roman font. with 16 size
            new FontSize() { Val = 16 },
            new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
            new FontName() { Val = "Times New Roman" })
            ),
            new Fills(
            new Fill( // Index 0 - The default fill.
            new PatternFill() { PatternType = PatternValues.None }),
            new Fill( // Index 1 - The default fill of gray 125 (required)
            new PatternFill() { PatternType = PatternValues.Gray125 }),
            new Fill( // Index 2 - The yellow fill.
            new PatternFill(
            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } }
            )
            { PatternType = PatternValues.Solid })
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
            new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center },
            new CellStyle { Name = "Solid Red", FormatId = 1 }

            )
            { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
            new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true } // Index 6 - Border
            )
            ); // return
          


        }
    }
}
