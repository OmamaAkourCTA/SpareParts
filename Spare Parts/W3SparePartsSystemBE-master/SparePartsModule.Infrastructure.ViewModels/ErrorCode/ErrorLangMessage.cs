using System.Diagnostics.CodeAnalysis;

namespace SparePartsModule.Infrastructure.ViewModels
{

    public class ErrorLangMessage
    {
        public int StatusCode { get; }
        [NotNull]
        public string ErrorMessageEn { get;  }
        [NotNull]
        public string ErrorMessageAr { get;  }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="errorMessageEn"></param>
        /// <param name="errorMessageAr"></param>
        public ErrorLangMessage(int statusCode, string errorMessageEn, string errorMessageAr)
        {
            this.StatusCode = statusCode;
            this.ErrorMessageEn = errorMessageEn;
            this.ErrorMessageAr = errorMessageAr;

        }
    }
}

