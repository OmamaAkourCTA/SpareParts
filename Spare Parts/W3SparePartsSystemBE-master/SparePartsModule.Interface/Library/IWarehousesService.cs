using SparePartsModule.Infrastructure.ViewModels.Models.Library.Warehouses;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models;

namespace SparePartsModule.Interface.Library
{
    public interface IWarehousesService
    {
        ValueTask<ApiResponseModel> AddWarehouses(AddWarehousesModel model, int userId);
        ValueTask<ApiResponseModel> EditWarehouses(EditWarehousesModel model, int userId);
        ValueTask<ApiResponseModel> DeleteWarehouses(string WarehouseID, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetWarehouses(GetWarehousesModel model, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> AddSubWarehouse(AddSubWarehouseModel model, int userId);
        ValueTask<ApiResponseModel> AddZone(AddZoneModel model, int userId);
        ValueTask<ApiResponseModel> AddShelf(AddShelfModel model, int userId);
        ValueTask<ApiResponseModel> AddSection(AddSectionModel model, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetSubwarehouse(GetSubwarehouseModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetZones(GetZonesModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetShelf(GetShelfModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetSection(GetSectionModel model, PaginationModel paginationPostModel);
    }
}
