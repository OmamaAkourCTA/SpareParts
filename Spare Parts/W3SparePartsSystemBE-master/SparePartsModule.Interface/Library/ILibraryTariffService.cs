using SparePartsModule.Infrastructure.ViewModels.Models.Library.LibraryTariff;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models;

namespace SparePartsModule.Interface.Library
{
    public interface ILibraryTariffService
    {
        ValueTask<ApiResponseModel> AddLibraryTariff(AddLibraryTariffModel model, int userId);
        ValueTask<ApiResponseModel> EditLibraryTariff(EditLibraryTariffModel model, int userId);
        ValueTask<ApiResponseModel> DeleteLibraryTariff(string TariffID, int userId);
        ValueTask<ApiResponseModel> ItemNameChangeStatus(string TariffID, int StatusId, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetLibraryTarifs(GetLibraryTariffModel model, PaginationModel paginationPostModel);
    }
}
