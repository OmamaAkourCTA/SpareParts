using SparePartsModule.Infrastructure.ViewModels.Models.Library.Category;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models;

namespace SparePartsModule.Interface.Library
{
    public interface ILibraryItemCategoryService
    {
        ValueTask<ApiResponseModel> AddItemCategory(AddItemCategoryModel model, int userId);
        ValueTask<ApiResponseModel> EditItemCategory(EditItemCategoryModel model, int userId);
        ValueTask<ApiResponseModel> DeleteItemCategory(string ItemCategoryID, int userId);
        ValueTask<ApiResponseModel> ItemCategoryChangeStatus(string ItemCategoryID, int StatusId, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetItemCategories(GetItemCategoriesModel model, PaginationModel paginationPostModel);
    }
}
