
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SparePartsModule.Infrastructure.ViewModels.ErrorCode
{
    public static class MarkaziaErrorCodes
    {
        private static Dictionary<string, string> EnErrors = new();
        private static Dictionary<string, string> ArErrors = new();

        public static List<ErrorLangMessage> GetErrorMessage(string statusCode)
        {
          var statusCodes= (statusCode.Contains(",")? statusCode.Substring(0, statusCode.Length - 1).Split(','): statusCode.Split(','));
            var errors = new List<ErrorLangMessage>();
            foreach (var code in statusCodes)
            {
                string enMessage = GetEnError(code);
                string arMessage = GetArError(code);
                errors.Add(new ErrorLangMessage(Convert.ToInt32(code), enMessage ?? "", arMessage ?? ""));
            }
           
            return errors;
        }
        private static string GetEnError(string statusCode)
        {
            if (EnErrors.Count == 0)
            {
                string fileName = "Language/En.Lng";
                string jsonString = File.ReadAllText(fileName);
                EnErrors = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString) ?? new();
            }
            return EnErrors.FirstOrDefault(x => x.Key == statusCode).Value;
        }

        private static string GetArError(string statusCode)
        {
            if (ArErrors.Count == 0)
            {
                string fileName = "Language/Ar.Lng";
                string jsonString = File.ReadAllText(fileName);
                ArErrors = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString) ?? new();

            }
            return ArErrors.FirstOrDefault(x => x.Key == statusCode).Value;
        }
    }


}
