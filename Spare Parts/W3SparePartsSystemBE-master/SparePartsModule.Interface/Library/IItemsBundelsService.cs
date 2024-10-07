using SparePartsModule.Infrastructure.ViewModels.Models.Library.ItemsBundels;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models;

namespace SparePartsModule.Interface.Library
{
    public interface IItemsBundelsService
    {
        ValueTask<ApiResponseModel> AddBundle(AddBundleModel model, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetBundles(GetBundlesModel model, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> DeleteBundle(string  BundleID, int userId);
        ValueTask<ApiResponseModel> BundlesChangeStatus(string BundleIDs, int StatusId, int userId);
        ValueTask<ApiResponseModel> AddBundleItems(AddBundleItemsModel model, int userId);
        ValueTask<ApiResponseModel> DeleteBundleItem(int BundleLineID, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetBundlesItems(GetBundlesItemsModel model, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> EditBundle(EditBundleModel model, int userId);
        ValueTask<ApiResponseModel> SendBundleForApproval(int BundleID, int userId);
        ValueTask<ApiResponseModel> HandleBundleApprovalRequest(HandleBundleApprovalRequestModel model, int userId);
        ValueTask<ApiResponseModel> AddBundleComplete(AddBundleModelComplete model, int userId);
        ValueTask<ApiResponseModel> ImportBundle(UpdateFileModel2 model, int userId);

    }
}
