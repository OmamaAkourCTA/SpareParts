using SparePartsModule.Infrastructure.ViewModels.Models.Library.Items;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models;

namespace SparePartsModule.Interface.Library
{
    public interface ILibraryItemsNamesService
    {
        ValueTask<ApiResponseModel> AddItemName(AddItemNameModel model, int userId);
        ValueTask<ApiResponseModel> EditItemName(EditItemNameModel model, int userId);
        ValueTask<ApiResponseModel> DeleteItemName(string ItemNameID, int userId);
        ValueTask<ApiResponseModel> ItemNameChangeStatus(string ItemNameID, int StatusId, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetItemNames(GetItemNameModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetENItemNames(string? search, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> UploadItemName(UpdateFileModel2 model, int userId);
    }
}
