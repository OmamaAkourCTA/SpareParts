using Microsoft.Extensions.Configuration;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Core.Helpers;
using SparePartsModule.Domain.Context;
using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels.Models.ListNames;
using SparePartsModule.Interface;

namespace SparePartsModule.Core
{
    public class LibraryListNamesService: ILibraryListNamesService
    {
        private readonly IConfiguration _config;
        private readonly VehicleSystemContext _context;
        private readonly FileHelper _FileHelper;
        private readonly UtilitiesHelper _utilties;
        private readonly PdfExportSalesSpecs _PdfExportSalesSpecs;
        private readonly ExcelExportSalesSpecs _ExcelExportSalesSpecs;


        public LibraryListNamesService(VehicleSystemContext context, IConfiguration config, FileHelper fileHelper, UtilitiesHelper utilitiesHelper,
            PdfExportSalesSpecs pdfExportSalesSpecs, ExcelExportSalesSpecs excelExportSalesSpecs)
        {

            _config = config;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _FileHelper = fileHelper;
            _utilties = utilitiesHelper;
            _PdfExportSalesSpecs = pdfExportSalesSpecs;
            _ExcelExportSalesSpecs = excelExportSalesSpecs;
        }
       }
}
