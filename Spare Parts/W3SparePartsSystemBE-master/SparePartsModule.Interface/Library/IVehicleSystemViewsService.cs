using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models.ListNames;

namespace SparePartsModule.Interface.Library
{
    public interface IVehicleSystemViewsService
    {
        ValueTask<PaginationDatabaseResponseDto<object>> GetBrands(GetBrandModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetModelCode(GetModelCodeModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetModels(GetModelModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetListName(GetListNameModel model, PaginationModel paginationPostModel);

        ValueTask<ApiResponseModel> ExportSalesSpecsSheet(int ListId);

        ValueTask<PaginationDatabaseResponseDto<object>> GetListNameSpecsValues(GetListNameSpecsValuesModel model, PaginationModel paginationPostModel);

    }
}
