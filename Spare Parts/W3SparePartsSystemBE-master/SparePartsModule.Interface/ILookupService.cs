using SparePartsModule.Infrastructure.ViewModels.Models.Lookups;
using SparePartsModule.Infrastructure.ViewModels.Models;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Interface
{
    public interface ILookupService
    {
        ValueTask<PaginationDatabaseResponseDto<object>> GetLookups(GetLookupsModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetLookupsMaster(GetLookupsModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetLookupsTypesMaster(GetLookupsTypeModel model, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> AddLookup(AddLookupModel model, int userId);
        ValueTask<ApiResponseModel> AddLookupType(AddLookupTypeModel model, int userId);
        ValueTask<ApiResponseModel> EditLookupType(EditLookupTypeModel model, int userId);
        ValueTask<ApiResponseModel> DeleteLookupType(int LookupTypeId, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetLookupTypes(GetLookupTypesModel model, PaginationModel paginationPostModel);

        ValueTask<ApiResponseModel> EditLookup(EditLookupModel model, int userId);
        ValueTask<ApiResponseModel> DeleteLookup(int LookupId, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetLookups(GetLookupsModel2 model, PaginationModel paginationPostModel);
        int GeSpLookup(int userId, string name, int lookupTypeId,int? parent=null);
        int GetMasterLookup(int userId, string name, int lookupTypeId);
    
    }
}
