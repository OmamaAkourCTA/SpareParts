using SparePartsModule.Infrastructure.ViewModels.Models.Library.Location;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Interface.Library
{
    public interface ILibraryLocationsService
    {
        ValueTask<ApiResponseModel> AddLocations(AddLocationsModel model, int userId);
        ValueTask<ApiResponseModel> EditLocations(EditLocationsModel model, int userId);
        ValueTask<ApiResponseModel> DeleteLocations(string LocationId, int userId);
        ValueTask<ApiResponseModel> LocationsChangeStatus(string LocationId, int StatusId, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetLocations(GetLocationsModel model, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> ImportLocations(UpdateFileModel2 model, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetLocationsV2(GetLocationsModelV2 model, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> AddLocationsV2(AddLocationsModelV2 model, int userId);
        ValueTask<ApiResponseModel> EditLocationsV2(EditLocationsModelV2 model, int userId);
    }
}
