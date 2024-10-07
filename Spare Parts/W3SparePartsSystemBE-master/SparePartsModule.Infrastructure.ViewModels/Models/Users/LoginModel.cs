using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Users
{
    public class LoginModel
    {

        [JsonPropertyName("identity")]
        public string? Identity { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        public string? DeviceToken { get; set; }
    }
}
