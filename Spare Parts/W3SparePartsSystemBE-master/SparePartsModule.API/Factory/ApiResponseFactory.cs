
using SparePartsModule.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace SparePartsModule.API
{
    public static class ApiResponseFactory
    {
        public static ApiResponseModel CreateSuccessResponse(object data, int id = 0, int totalPage = 0,int totalrecords=0,int TotalAllRecordCount=0)
        {
            return new ApiResponseModel(200, data, id, totalPage, totalrecords, TotalAllRecordCount);
        }
        public static ApiResponseModel CreateSuccessResponse(object data,object info,  int totalPage = 0)
        {
            return new ApiResponseModel(200, data,info, totalPage);
        }
        public static ApiResponseModel CreateManageProcessErrorResponse(string abcErrorStatusCode, string? info = null, string? stackTrace = null, int id = 0, int totalPage = 0)
        {
            return new ApiResponseModel(400, false, abcErrorStatusCode, info: info, stackTrace: stackTrace, id: id, totalPage: totalPage);
        }
        public static ApiResponseModel CreateErrorResponse(string abcErrorStatusCode, string? info = null, string? stackTrace = null, int id = 0, int totalPage = 0)
        {
            return new ApiResponseModel(400, false, abcErrorStatusCode, info: info, stackTrace: stackTrace, id: id, totalPage: totalPage);
        }
        public static ApiResponseModelGeneric<T> CreateErrorResponseGeneric<T>(string abcErrorStatusCode, string? info = null, string? stackTrace = null, int id = 0, int totalPage = 0)
        {
            return new ApiResponseModelGeneric<T>(500, false, abcErrorStatusCode, info: info, stackTrace: stackTrace, id: id, totalPage: totalPage);
        }
        public static ApiResponseModel CreateBadRequestResponse(string abcErrorStatusCode, string? info = null, string? stackTrace = null, int id = 0, int totalPage = 0)
        {
            return new ApiResponseModel(400, false, abcErrorStatusCode, info: info, stackTrace: stackTrace, id: id, totalPage: totalPage);
        }
        public static ApiResponseModel CreateBadRequestResponse(string abcErrorStatusCode,ActionContext context)
        {
            return new ApiResponseModel(400, false, abcErrorStatusCode, string.Join("; ", context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
        }

    }
}

