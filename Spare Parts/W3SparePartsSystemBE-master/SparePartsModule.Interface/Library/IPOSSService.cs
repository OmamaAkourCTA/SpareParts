using SparePartsModule.Infrastructure.ViewModels.Models.Library.POSS;
using SparePartsModule.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Models;

namespace SparePartsModule.Interface.Library
{
    public interface IPOSSService
    {
        ValueTask<ApiResponseModel> UploadPOSS(UploadPOSSModle model, int userId);
        ValueTask<PaginationDatabaseResponseDto<object>> GetPOSS(GetPOSSModel model, PaginationModel paginationPostModel);
        ValueTask<PaginationDatabaseResponseDto<object>> GetPOSSRawViewDetails(int POSSID, PaginationModel paginationPostModel);
        ValueTask<ApiResponseModel> AddPOSSManually(AddPOSSManuallyModle model, int userId);
        ValueTask<object> GetPOSSTMCSummeryView(int POSSID);
        ValueTask<ApiResponseModel> AddNonTMCPOSS(AddNonTMCPOSSModel model, int userId);
        ValueTask<ApiResponseModel> UpdateNonTMCPOSS(UpdateNonTMCPOSSModel model, int userId);
        ValueTask<object> GetOrderPOSSSummeryView(int OrderId);
    }
}
