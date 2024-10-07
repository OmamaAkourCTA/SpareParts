using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SparePartsModule.Infrastructure.ViewModels
{
    public class ApiResponseErrorModel
    {
        [JsonPropertyName("errorCode")]
        [NotNull]
        public string ErrorCode { get; }

        [JsonPropertyName("messageEn")]
        [NotNull]
        public string MessageEn { get; }

        [JsonPropertyName("messageAr")]
        public string? MessageAr { get; set; }

        [JsonPropertyName("stackTrace")]
        public string? StackTrace { get; set; }

        public ApiResponseErrorModel(string errorCode, string messageEn, string messageAr, string? stackTrace = null)
        {
            ErrorCode = errorCode;
            MessageEn = messageEn;
            MessageAr = messageAr;
            StackTrace = stackTrace;
        }
       

    }
}

