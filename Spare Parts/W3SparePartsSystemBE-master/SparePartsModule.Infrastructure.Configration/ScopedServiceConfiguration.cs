using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SparePartsModule.Core;
using SparePartsModule.Core.Helpers;
using SparePartsModule.Core.Library;
using SparePartsModule.Interface;
using SparePartsModule.Interface.Library;
using SparePartsModule.Interface.Users;

namespace SparePartsModule.Infrastructure.Configration
{
    public static class ScopedServiceConfiguration
    {
        public static IServiceCollection AddScopedService(this IServiceCollection services)
        {

            var mappingConfig = new MapperConfiguration(mapper =>
            {

            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<FileHelper, FileHelper>();
            services.AddScoped<UtilitiesHelper, UtilitiesHelper>();
            services.AddScoped<ILibrarySuppliersService, LibrarySuppliersService>();
            services.AddScoped<ILibraryItemCategoryService, LibraryItemCategoryService>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<ILibraryTariffService, LibraryTariffService>();
            services.AddScoped<ILibraryLocationsService, LibraryLocationsService>();
            services.AddScoped<ILibraryItemsNamesService, LibraryItemsNamesService>();
            services.AddScoped<ILibraryItemsService, LibraryItemsService>();
            services.AddScoped<IVehicleSystemViewsService, VehicleSystemViewsService>();
            services.AddScoped<IItemsBundelsService, ItemsBundelsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILibraryListNamesService, LibraryListNamesService>();
            services.AddScoped<PdfExportSalesSpecs, PdfExportSalesSpecs>();
            services.AddScoped<ExcelExportSalesSpecs, ExcelExportSalesSpecs>();
            services.AddScoped<ISparePartsOrdersService, SparePartsOrdersService>();
            services.AddScoped<IPOSSService, POSSService>();
            services.AddScoped<IWarehousesService, WarehousesService>();
            services.AddScoped<EMailService, EMailService>();
            services.AddScoped<ExcelExportOrder, ExcelExportOrder>();

            
            return services;
        }
    }
    
}
