using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Response.Users
{
    public class LoginResponseModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }
        [JsonPropertyName("mobile")]
        public string? Mobile { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("expiryDateTime")]
        public DateTime ExpiryDateTime { get; set; }
        public string? ProfileImage { get; set; }
    }
}
