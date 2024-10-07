using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items.Substitute;
using Microsoft.AspNetCore.Http;

namespace SparePartsModule.Interface.Library
{
    public interface ILibraryItemsService
    {
        ValueTask<ApiResponseModel> UploadSupplierMasterItems(UploadSupplierMasterItemsModle model, int userId);
        ValueTask<ApiResponseModel> UploadSupplierMasterItemsV2(UpdateFileModel2 model, int userId);
        ValueTask<ApiResponseModel> AddItem(AddItemModel model, int userId);
        ValueTask<ApiResponseModel> EditItem(EditItemModel model, int userId);
        ValueTask<ApiResponseModel> DeleteItem(string ItemId, int userId);
        ValueTask<ApiResponseModel> ItemChangeStatus(string ItemId, int StatusId, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetItems(GetItemsModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetItemInquiryCards(GetItemsModel model, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> AddItemSupplier(AddItemSupplierModel model, int userId);
        ValueTask<ApiResponseModel> EditItemSupplier(EditItemSupplierModel model, int userId);
        ValueTask<ApiResponseModel> DeleteItemSupplier(DeleteItemSupplierModel model, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetItemSupplier(GetItemSupplierModel model, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> AddItemLocation(AddItemLocationModel model, int userId);
        ValueTask<ApiResponseModel> EditItemLocation(EditItemLocationModel model, int userId);
        ValueTask<ApiResponseModel> DeleteItemLocation(int ItemLocationID, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetItemLocations(GetItemLocationsModel model, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> AddItemVehicleModelCode(AddItemVehicleModelCodeModel model, int userId);
        ValueTask<ApiResponseModel> EditItemVehicleModelCode(EditItemVehicleModelCodeModel model, int userId);
        ValueTask<ApiResponseModel> DeleteItemVehicleModelCode(int ItemModelCodeID, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetItemVehicleModelCode(GetItemVehicleModelCodeModel model, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> AddItemSubsitute(AddItemSubsituteModel model, int userId);
        ValueTask<object> GetItemSubstitutes(GetItemSubstitutesModel model);
        ValueTask<ApiResponseModel> DeleteItemSubstitute(int ItemSubstituteID, int userId);
        ValueTask<ApiResponseModel> DeactivateItemSubstitute(DeactivateItemSubstituteModel model, int userId);
        ValueTask<object> GetSupplierMasterItemDetails(string ItemSupNo);
        ValueTask<object> GetSupplierMasterItemDetailsV2(string ItemSupNo, int? supplierId);
        ValueTask<ApiResponseModel> UploadCTAItems(UploadSupplierMasterItemsModle2 model, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetCTAItems( PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> UploadItemsLocations(UploadSupplierMasterItemsModle2 model, int userId);
        ValueTask<ApiResponseModel> UploadItemsSuppliers(UploadSupplierMasterItemsModle2 model, int userId);
        ValueTask<ApiResponseModel> UploadOtherSuppliersJPM(UpdateFileModel2 model, int userId);
        ValueTask<ApiResponseModel> UploadItemsSubstitutes(UploadSupplierMasterItemsModle2 model, int userId);
    }

}
