using AraibcPdfUnicodeGlyphsResharper;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Domain.Context;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using Column = MigraDoc.DocumentObjectModel.Tables.Column;
using Section = MigraDoc.DocumentObjectModel.Section;
using Style = MigraDoc.DocumentObjectModel.Style;


namespace SparePartsModule.Core
{
    public class PdfExportSalesSpecs
    {
        Document document = new Document();
        private TextFrame addressFrame;
        private MigraDoc.DocumentObjectModel.Tables.Table table;
        private MigraDoc.DocumentObjectModel.Tables.Table tableHeader;
        private readonly VehicleSystemContext _context;


        public PdfExportSalesSpecs(VehicleSystemContext context)
        {
            _context = context;

        }
        public Document CreateDocument(int listId, string language)
        {
            // Create a new MigraDoc document
            this.document = new Document();
            this.document.Info.Title = "Markazia Co.";
            this.document.Info.Subject = "Markazia Co.";
            this.document.Info.Author = "ABC Porgrammers";
            document.DefaultPageSetup.LeftMargin = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);
            document.DefaultPageSetup.RightMargin = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);
            document.DefaultPageSetup.TopMargin = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);
            document.DefaultPageSetup.BottomMargin = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1);


            DefineStyles();

            CreatePage(listId, language);


            return this.document;
        }
        void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = this.document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "arial";
            style.Font.Size = 8;
            style = this.document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = this.document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "arial";
            // style.Font.Name = "Times New Roman";
            style.Font.Size = 8;

            // Create a new style called Reference based on style Normal
            style = this.document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }
        void CreatePage(int listId, string language)
        {
            // Each MigraDoc document needs at least one section.

            Section section = this.document.AddSection();
            //section.AddParagraph();
            //section.AddParagraph();
            //section.AddParagraph();
            this.tableHeader = section.AddTable();
            this.tableHeader.Format.Borders.Visible = false;
            this.tableHeader.AddColumn("6cm");
            this.tableHeader.AddColumn("6cm");
            this.tableHeader.AddColumn("6cm");
            Row row1 = tableHeader.AddRow();
            if (language == "En")
            {
                var img = row1.Cells[0].AddImage("markazia.png");
                img.Width = "6cm";
                img.Height = "2cm";
            }
            else
            {
                var img = row1.Cells[2].AddImage("markazia.png");
                img.Width = "6cm";
                img.Height = "2cm";
                row1.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            }


            //row1.Format.Alignment = ParagraphAlignment.Center;




            // Put a logo in the header

            VechileInfo(listId, section, language);
            section.AddParagraph();
            Specs(listId, section, language);
            // Create footer




            //  CreatePageFooter(section );
            //  section.AddParagraph();


        }

        void VechileInfo(int listId, Section section, string language)
        {
            var list = _context.MasterVehilceList.Where(e => e.ListID == listId).Select(e => new
            {
                e.ListName,
                ModelName = _context.MasterVehicleModel.Where(x => x.ModelID == e.ListModelID).Select(x => new { x.ModelName, x.ModelNameAR }).FirstOrDefault(),
                ModelCode = _context.MasterVehicleModelCode.Where(x => x.ModelCodeID == e.ListModelCodeID).Select(x => new { x.ModelCodeVehicles }).FirstOrDefault(),
            }).FirstOrDefault();

            if (list == null)
            {
                throw new ManagerProcessException("000071");
            }



            if (language == "En")
            {
                var p = section.AddParagraph("Vehicle Info.");
                p.Format.Font.Size = 12;
                p.Format.Font.Bold = true;
                section.AddParagraph();

                this.table = section.AddTable();

                Column columnleft = this.table.AddColumn("9.6cm");
                columnleft.Format.Alignment = ParagraphAlignment.Left;
                Column columnRight = this.table.AddColumn("9.6cm");
                columnleft.Format.Shading.Color = new Color(220, 220, 220);
                var row0 = this.table.AddRow();
                row0.Cells[0].AddParagraph("Model Name");
                row0.Cells[1].AddParagraph(list.ModelName.ModelName.ToString());
                //row0.Cells[0].Format.LeftIndent = 5;
                //row0.Cells[1].Format.LeftIndent = 5;

                row0 = this.table.AddRow();
                row0.Cells[0].AddParagraph("Model Code");
                row0.Cells[1].AddParagraph(list.ModelCode.ModelCodeVehicles.ToString());
                //row0.Cells[0].Format.LeftIndent = 5;
                //row0.Cells[1].Format.LeftIndent = 5;

                row0 = this.table.AddRow();
                row0.Cells[0].AddParagraph("List Name");
                row0.Cells[1].AddParagraph(list.ListName.ToString());
                //row0.Cells[0].Format.LeftIndent = 5;
                //row0.Cells[1].Format.LeftIndent = 5;
            }
            else
            {

                var p = section.AddParagraph("معلومات المركبة".ArabicWithFontGlyphsToPfd());
                p.Format.Alignment = ParagraphAlignment.Right;
                p.Format.Font.Size = 12;
                p.Format.Font.Bold = true;
                section.AddParagraph();

                this.table = section.AddTable();

                Column columnleft = this.table.AddColumn("9.6cm");
                columnleft.Format.Alignment = ParagraphAlignment.Left;
                Column columnRight = this.table.AddColumn("9.6cm");
                columnRight.Format.Shading.Color = new Color(220, 220, 220);
                var row0 = this.table.AddRow();
                row0.Cells[0].AddParagraph(list.ModelName.ModelName.ToString().ArabicWithFontGlyphsToPfd());
                row0.Cells[1].AddParagraph("موديل المركبة".ArabicWithFontGlyphsToPfd());
                row0.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row0.Cells[1].Format.Alignment = ParagraphAlignment.Right;
                //row0.Cells[0].Format.RightIndent = 5;
                //row0.Cells[1].Format.RightIndent = 5;
                row0.Cells[0].Borders.Right.Visible = false;

                row0 = this.table.AddRow();
                row0.Cells[0].AddParagraph(list.ModelCode.ModelCodeVehicles.ToString().ArabicWithFontGlyphsToPfd());
                row0.Cells[1].AddParagraph("رمز المركبة".ArabicWithFontGlyphsToPfd());
                row0.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row0.Cells[1].Format.Alignment = ParagraphAlignment.Right;
                //row0.Cells[0].Format.RightIndent = 5;
                //row0.Cells[1].Format.RightIndent = 5;
                row0.Cells[0].Borders.Right.Visible = false;


                row0 = this.table.AddRow();
                row0.Cells[0].AddParagraph(list.ListName.ToString().ArabicWithFontGlyphsToPfd());
                row0.Cells[1].AddParagraph("صنف المركبة".ArabicWithFontGlyphsToPfd());
                row0.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row0.Cells[1].Format.Alignment = ParagraphAlignment.Right;
                //row0.Cells[0].Format.RightIndent = 5;
                //row0.Cells[1].Format.RightIndent = 5;
                row0.Cells[0].Borders.Right.Visible = false;

            }

            this.table.Format.Borders.Visible = true;
        }
        void Specs(int listId, Section section, string language)
        {
            this.table = section.AddTable();
            Column columnleft = this.table.AddColumn("9.6cm");
            columnleft.Format.Alignment = ParagraphAlignment.Left;
            Column columnRight = this.table.AddColumn("9.6cm");
            this.table.Format.Borders.Visible = true;

            var categories = _context.MasterVehicleLookup
                .Where(x => x.LookupTypeID == (int)LookupTypes.MasterSpecsCategory && x.Status == (int)Status.Active).Select(x => new { x.LookupID, x.LookupName }).ToList();
            foreach (var category in categories)
            {
                var genSpecs = _context.MasterVehicleGeneralSpecs.Where(e => e.GeneralSpecsCategory == category.LookupID).OrderBy(e => e.GeneralSpecsCategory).ToList();
                if (genSpecs != null)
                {
                    var isForsales = genSpecs.Where(e => e.GeneralSpecsShowonSalesSheet == true).Any();

                    if (isForsales)
                    {
                        //    section.AddParagraph(category.LookupName ?? "");
                        //    section.AddParagraph();

                        var row0 = this.table.AddRow();
                        row0.Cells[0].AddParagraph(" " + category.LookupName ?? "");
                        row0.Cells[0].MergeRight = 1;
                        row0.Cells[0].Format.Shading.Color = new Color(169, 169, 169);
                        row0.Cells[0].Format.Font.Size = 11;
                        row0.Cells[0].Format.Font.Bold = true;
                        row0.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                        //if (language != "En")
                        //{
                        //    row0.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                        //}
                        //row0.Cells[0].Format.LeftIndent = 5;
                        //row0.Cells[1].Format.LeftIndent = 5;
                    }


                    foreach (var spec in genSpecs)
                    {
                        if (spec.GeneralSpecsShowonSalesSheet == true)
                        {
                            var specsValues = _context.MasterVehilceListSpecsValues
                             .Where(e => e.ListID == listId && e.GeneralSpecID == spec.GeneralSpecID)
                             .Select(e => new {
                                 GeneralSpecsItemValue = _context.MasterVehicleGeneralSpecsValues.Where(x => x.GeneralSpecsItemID == e.GeneralSpecItemID)
                             .Select(x => new { x.GeneralSpecsItemValueEN, x.GeneralSpecsItemValueAR }).FirstOrDefault()
                             }
                             )
                             .FirstOrDefault();

                            if (specsValues != null)
                            {
                                var row0 = this.table.AddRow();
                                if (language == "En")
                                {
                                    row0.Cells[0].AddParagraph(" " + spec.GeneralSpecsNameEN);
                                    row0.Cells[1].AddParagraph(" " + (specsValues.GeneralSpecsItemValue == null ? "" : specsValues.GeneralSpecsItemValue.GeneralSpecsItemValueEN.ToString()));
                                    row0.Cells[0].Format.Shading.Color = new Color(220, 220, 220);
                                    //row0.Cells[0].Format.LeftIndent = 5;
                                    //row0.Cells[1].Format.LeftIndent = 5;
                                }
                                else
                                {
                                    row0.Cells[1].Format.Shading.Color = new Color(220, 220, 220);
                                    row0.Cells[1].AddParagraph(spec.GeneralSpecsNameAR == null ? "" : spec.GeneralSpecsNameAR.ArabicWithFontGlyphsToPfd() + " ");
                                    row0.Cells[0].AddParagraph(specsValues.GeneralSpecsItemValue == null ? "" : (specsValues.GeneralSpecsItemValue.GeneralSpecsItemValueAR ?? "").ToString().ArabicWithFontGlyphsToPfd() + " ");
                                    row0.Cells[1].Format.Alignment = ParagraphAlignment.Right;
                                    row0.Cells[1].Format.Alignment = ParagraphAlignment.Right;
                                    //row0.Cells[0].Format.RightIndent = 5;
                                    //row0.Cells[1].Format.RightIndent = 5;

                                }
                                row0.Cells[0].Borders.Right.Visible = false;
                            }

                        }
                    }
                }


            }



            //var categorys = _context.MasterVehilceListSpecsValues
            // .Where(e =>e.ListID==listId&& e.MasterVehicleGeneralSpec.GeneralSpecsShowonSalesSheet == true)
            //.Select(e => new
            //{
            //    e.MasterVehicleGeneralSpec.GeneralSpecsNameAR,
            //    e.MasterVehicleGeneralSpec.GeneralSpecsNameEN,
            //    e.MasterVehicleGeneralSpecsValue.GeneralSpecsItemValueEN,
            //    e.MasterVehicleGeneralSpecsValue.GeneralSpecsItemValueAR,
            //    Category = _context.MasterVehicleLookup.Where(x => x.LookupID == e.MasterVehicleGeneralSpec.GeneralSpecsCategory).Select(x => x.LookupName).FirstOrDefault(),
            //}).GroupBy(e => e.Category)
            //.Select(e => new
            //{
            //    Category = e.Key,
            //    Specs = e.Select(x => new {
            //        x.GeneralSpecsNameEN,x.GeneralSpecsNameAR,
            //       x.GeneralSpecsItemValueEN,
            //       x.GeneralSpecsItemValueAR,

            //    }).ToList()

            //}) .ToList();




            //foreach (var category in categorys)
            //{

            //    foreach (var spec in category.Specs)
            //    {
            //        var row0 = this.table.AddRow();
            //        row0.Cells[0].AddParagraph(spec.GeneralSpecsNameEN);
            //        row0.Cells[1].AddParagraph(spec.GeneralSpecsItemValueEN.ToString());
            //    }
            //}






        }

        private void CreatePageFooter(Section section)
        {

            section.PageSetup.OddAndEvenPagesHeaderFooter = true;
            section.PageSetup.StartingNumber = 1;





            section.Footers.Primary.AddParagraph();

            var footerTable = section.Footers.Primary.AddTable();

            Column col1 = footerTable.AddColumn("4.5cm");
            col1.Format.Alignment = ParagraphAlignment.Left;
            Column col11 = footerTable.AddColumn("4.5cm");
            col11.Format.Alignment = ParagraphAlignment.Left;
            Column col2 = footerTable.AddColumn("4.5cm");
            col2.Format.Alignment = ParagraphAlignment.Center;
            Column col3 = footerTable.AddColumn("4.5cm");
            col3.Format.Alignment = ParagraphAlignment.Right;
            // Row row1x = footerTable.AddRow();

        }

    }
}
