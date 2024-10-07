using SparePartsModule.Infrastructure.ViewModels;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.SupplierDelivery;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers;

namespace SparePartsModule.Interface.Library
{
    public interface ILibrarySuppliersService
    {
        ValueTask<ApiResponseModel> AddSupplier(AddSupplierModel model, int userId);
        ValueTask<ApiResponseModel> AddSupplierDeliveryMethod(AddSupplierDeliveryModel model, int userId);
        ValueTask<ApiResponseModel> EditSupplierDeliveryMethod(EditSupplierDeliveryModel model, int userId);
        ValueTask<ApiResponseModel> EditSupplier(EditSupplierModel model, int userId);
        ValueTask<ApiResponseModel> DeleteSupplier(int SupplierID, int userId);
        ValueTask<ApiResponseModel> DeleteSuppliers(string SupplierIDs, int userId);
        ValueTask<ApiResponseModel> SuppliersChangeStatus(string SupplierIDs, int StatusId, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetSuppliers(GetSupplierModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetSupplierMasterItems(GetSupplierMasterItemsModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetSupplierMasterItemsV2(GetSupplierMasterItemsModel model, PaginationModel paginationPostModel);
        ValueTask<object> GetSupplierDeliveryMethod(int SupplierId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetSupplierDeliveryMethod1(int SupplierId, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> DeleteSupplierDeliveryMethod(int SupplierDeliveryMethodID, int userId);
        ValueTask<ApiResponseModel> ImportSupplier(UpdateFileModel2 model, int userId);
    }
}
