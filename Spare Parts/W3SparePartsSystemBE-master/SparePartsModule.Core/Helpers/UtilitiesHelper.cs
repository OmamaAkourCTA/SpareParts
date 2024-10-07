using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SparePartsModule.Core.Helpers
{
    public class UtilitiesHelper
    {
        public virtual string IsValidEmail(string email)
        {
            string valid = string.Empty;

            try
            {
                var emailAddress = new MailAddress(email);
                string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                return regex.IsMatch(email)?"": "000016,";
            }
            catch
            {
                valid = "000016,";
            }

            return valid;
        }
        public virtual bool ValidateLongitude(string value)
        {
            decimal _value;
            var isValid = decimal.TryParse(value, out _value);
            if (_value < -180 || _value > 180 || isValid == false)
            {
                return false;
            }
            return true;
        }
        public virtual bool ValidateLatitude(string value)
        {
            decimal _value;
          var isValid=  decimal.TryParse(value, out _value);
            if (_value < -90 || _value > 90|| isValid==false)
            {
                return false;
            }
            return true;
        }
        public virtual bool HasSpecialChars(string yourString)
        {
             char[] SpecialChars = "!@#$%^&*()[]{}\\//+-*".ToCharArray();
            int indexOf = yourString.IndexOfAny(SpecialChars);

            return indexOf >= 0;   
        }
        public virtual string ValidateMobile(string mobile)
        {
            if (mobile.Length > 10 || mobile.Length < 8)
            {
                return "000012,";
            }
            return string.Empty ;
        }
        public virtual string ValidateStatus(int status)
        {
            if (status< 2001|| status>=3000)
            {
                return "000041,";

            }
            return string.Empty;
        }
        public virtual string HashPassword(string password)
        {
            string saltKey = "HackMeIfYouCanDoIt";
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            byte[] salt = Encoding.ASCII.GetBytes(saltKey);

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                        password: password!,
                                        salt: salt,
                                        prf: KeyDerivationPrf.HMACSHA256,
                                        iterationCount: 100000,
                                        numBytesRequested: 256 / 8));
        }
        public virtual string UnHashString(string password)
        {
            string saltKey = "w2gD2U0Nvi";
            password=password.Replace(saltKey, string.Empty)+"=";
            byte[] data = Convert.FromBase64String(password);

            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
        public virtual string HashString(string password)
        {
            string saltKey = "w2gD2U0Nvi";

            byte[] salt = Encoding.ASCII.GetBytes(password);

            return saltKey+Convert.ToBase64String(salt).Replace("=","");
        }
        public virtual  bool IsIpValid(string ip) =>
       new Regex("^(?!0)(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?!0)(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?!0)(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?!0)(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")
           .IsMatch(ip);
        public virtual DateTime StartOfWeek( DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public virtual decimal ConvertToJod(decimal value,int currencyId)
        {
            if(currencyId== 9002)//dollar
            {
               value = value * 0.71M;
               value = decimal.Round(value, 3, MidpointRounding.AwayFromZero);

            }
            if (currencyId == 9003)//Euro
            {
                value = value * 0.76M;
                value = decimal.Round(value, 3, MidpointRounding.AwayFromZero);

            }
            return value;
        }

        public virtual bool IsLettersOnly(string value)
        {

            bool result = value.All(e => Char.IsLetter(e) || Char.IsWhiteSpace(e));
            return result;

        }
        public virtual bool IsRtl( string input)
        {
            return Regex.IsMatch(input, @"\p{IsArabic}");
        }
  
        public virtual bool IsArabic(string input)
        {
            Regex regex = new Regex("[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]");
            return regex.IsMatch(input);
        }
        internal bool HasEnglishCharacters(string text)
        {



            return Regex.IsMatch(text, "^[a-zA-Z0-9]*$");
        }
        public virtual bool IsValidColorHexa(string hc)
        {

            return Regex.IsMatch(hc, @"[#][0-9A-Fa-f]{6}\b");
        }
        public virtual bool EnglishCharNumber(string hc)
        {

            return Regex.IsMatch(hc, @"^[a-zA-Z0-9]*$");
        }
        public virtual bool EnglishChar(string hc)
        {

            return Regex.IsMatch(hc, @"^[a-zA-Z]*$");
        }
    }
}
