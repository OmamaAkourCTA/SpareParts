using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MigraDoc.Rendering;
using SparePartsModule.Infrastructure.ViewModels.Response;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SparePartsModule.Core
{
    public class FileHelper
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;
        private readonly Logger<FileHelper> _looger;

        public FileHelper(IHostingEnvironment env,
            IConfiguration config)
        {
            _config = config;
            _env = env;
        }

        public FileHelper(Logger<FileHelper> looger)
        {
            _looger = looger;
        }

        public static bool CheckIfImageFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".png" || extension == ".jpeg");
        }

        public async Task<FileUploadModel> WriteFile(IFormFile file, string directory)
        {
            FileUploadModel model = new();
            string returnPath = string.Empty;
            string fileName;
            string extension = string.Empty;

            try
            {
                extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];

                fileName = DateTime.Now.Ticks + extension;
                //Create a new Name for the file due to security reasons.

                var pathBuilt = Path.Combine(_env.ContentRootPath, "Uploads", directory);

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                var path = Path.Combine(_env.ContentRootPath, "Uploads", directory, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                model.ReturnUrl = "Uploads/" + directory+"/"+ fileName;
                model.Extension = extension;
            }
            catch (Exception e)
            {
                //log error
                Debug.WriteLine(e.Message);
            }

            return model;
        }

       public bool RemoveFile(string filePath)
        {
            bool isSuccess = false;
            try
            {
                var path = Path.Combine(_env.ContentRootPath, _config["Settings:Uploads"], filePath);

                File.Delete(path);
                isSuccess = true;
            }
            catch (Exception e)
            {
                _looger.LogError(e.Message+"---"+e.StackTrace);
                //log error
                Debug.WriteLine(e.Message);
            }

            return isSuccess;
        }
        public DataSet ReadExcel(string filePaths)
        {
            DataSet ds = new DataSet();

            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(filePaths, false))
            {

                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                foreach (Sheet sheet in sheets)
                {
                    DataTable dt = new DataTable();
                    string relationshipId = sheet.Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                    Worksheet workSheet = worksheetPart.Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Descendants<Row>();
                    string colName = string.Empty;
                    int z = 0;
                    int columnsRowIndex = 0;
                    if (ds.Tables.Count == 1)
                    {
                        columnsRowIndex = 2;
                    }
                    foreach (Cell cell in rows.ElementAt(columnsRowIndex))
                    {
                        z++;
                        colName = "a" + z.ToString(); //GetCellValue(spreadSheetDocument, cell);
                                                      // string columnName = GetColumnName(colName);

                        if (!dt.Columns.Contains(colName))
                        {
                            if (colName.Contains("Date"))
                            {
                                dt.Columns.Add(colName, typeof(DateTime));

                            }
                            else
                            {
                                dt.Columns.Add(colName);

                            }
                        }
                        else
                        {
                            if (!dt.Columns.Contains(cell.LocalName))
                            {
                                dt.Columns.Add(cell.LocalName);
                            }

                        }
                    }

                    foreach (Row row in rows) //this will also include your header row...
                    {

                        DataRow tempRow = dt.NewRow();
                        int columnIndex = 0;

                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            try
                            {


                                //  string value = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                                // Gets the column index of the cell with data
                                int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                                cellColumnIndex--; //zero based index
                                if (columnIndex < cellColumnIndex)
                                {
                                    do
                                    {

                                        tempRow[columnIndex] = ""; //Insert blank data here;
                                        columnIndex++;
                                    }
                                    while (columnIndex < cellColumnIndex);
                                }
                                tempRow[columnIndex] = GetCellValue(spreadSheetDocument, cell);

                                columnIndex++;

                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        dt.Rows.Add(tempRow);
                    }
                    //   dt.Rows.RemoveAt(0); //...so i'm taking it out here.
                    ds.Tables.Add(dt);

                }//end loop sheets

            }
            return ds;

        }

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            string currentcellvalue = string.Empty;
            if (cell.DataType != null)
            {
                if (cell.DataType == CellValues.SharedString)
                {
                    int id;
                    if (Int32.TryParse(cell.InnerText, out id))
                    {
                        SharedStringItem item = document.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                        if (item.Text != null)
                        {
                            //code to take the string value  
                            currentcellvalue = item.Text.Text;
                        }
                        else if (item.InnerText != null)
                        {
                            currentcellvalue = item.InnerText;
                        }
                        else if (item.InnerXml != null)
                        {
                            currentcellvalue = item.InnerXml;
                        }
                    }
                }
            }
            else
            {
                currentcellvalue = cell.CellValue?.InnerText;
            }

            return currentcellvalue;

        }
        private static string GetColumnName(string cellName)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }
        /// <summary>
        /// Given just the column name (no row index), it will return the zero based column index.
        /// Note: This method will only handle columns with a length of up to two (ie. A to Z and AA to ZZ). 
        /// A length of three can be implemented when needed.
        /// </summary>
        /// <param name="columnName">Column Name (ie. A or AB)</param>
        /// <returns>Zero based index if the conversion was successful; otherwise null</returns>
        public static int? GetColumnIndexFromName(string columnName)
        {

            //return columnIndex;
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }
        public async Task<FileUploadModel> WriteFile(MigraDoc.DocumentObjectModel.Document file, string directory)
        {
            FileUploadModel model = new();
            string returnPath = string.Empty;
            string fileName;
            string extension = string.Empty;

            try
            {

                fileName = DateTime.Now.Ticks + ".pdf";
                //Create a new Name for the file due to security reasons.

                var pathBuilt = Path.Combine(_env.ContentRootPath, _config["Settings:Uploads"], directory);

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                var path = Path.Combine(_env.ContentRootPath, _config["Settings:Uploads"], directory, fileName);

                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
                {
                    Document = file
                };

                renderer.PrepareRenderPages();
                renderer.RenderDocument();

                renderer.Save(path);

                model.ReturnUrl = "Uploads/" + directory + "/" + fileName;
                model.Extension = extension;
            }
            catch (Exception e)
            {

                //log error
                Debug.WriteLine(e.Message);
            }

            return model;
        }
    }
}
