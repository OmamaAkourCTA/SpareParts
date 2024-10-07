using SparePartsModule.Infrastructure.ViewModels.ErrorCode;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SparePartsModule.Infrastructure.ViewModels
{
    public class ApiResponseModel
    {
        [JsonPropertyName("id")]
        [MaybeNull]
        public int? Id { get; set; }
        [JsonPropertyName("isSuccess")]
        [NotNull]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("statusCode")]
        [NotNull]
        public int StatusCode { get; set; }


        //Not Required 
        [JsonPropertyName("info")]
        [MaybeNull]
        public object? Info { get; set; }

        [JsonPropertyName("data")]
        [MaybeNull]
        public object? Data { get; set; }



        //[JsonPropertyName("error")]
        //[MaybeNull]
        //public ApiResponseErrorModel? Error { get; set; }
        [JsonPropertyName("errors")]
        public List<ErrorLangMessage>? Errors { get; set; }

        [JsonPropertyName("totalPages")]
        [NotNull]
        public int TotalPages { get; set; }

        public ApiResponseModel(int statusCode, object data, int id = 0, int totalPage = 0,int TotalRecordsCount=0, int TotalAllRecordsCount = 0)
        {
            this.StatusCode = statusCode;
            this.Data = data;
            this.IsSuccess = true;
            this.TotalPages = totalPage;
            this.Info = new { TotalRecordsCount, TotalAllRecordsCount };
        }
        public ApiResponseModel(int statusCode, object data,object? info,  int totalPage = 0)
        {
            this.StatusCode = statusCode;
            this.Data = data;
            this.IsSuccess = true;
            this.TotalPages = totalPage;
            this.Info = info;
        }
        public ApiResponseModel(int htppStatusCode, bool isSuccess, string abcErrorStatusCode, string? info = null, string? stackTrace = null, int id = 0, int totalPage = 0)
        {
            this.IsSuccess = isSuccess;
            this.StatusCode = htppStatusCode;
            List<ErrorLangMessage> errorMessages = MarkaziaErrorCodes.GetErrorMessage(abcErrorStatusCode);
            //if(!string.IsNullOrEmpty(info))
            //{
            //    var errors=info.Split(';');
            //    errorMessages.RemoveAll(e=>e.StatusCode==1);
            //    foreach (var error in errors)
            //    {
            //        var msg = new ErrorLangMessage(1, error, error);
            //        errorMessages.Add(msg);
            //    }
            //}
          //  this.Error = new ApiResponseErrorModel(abcErrorStatusCode, errorMessages.ErrorMessageEn, errorMessages.ErrorMessageAr);
            this.Errors = errorMessages;
            this.TotalPages = totalPage;
            this.Info = new { info, stackTrace };
            
        }
    }
}

