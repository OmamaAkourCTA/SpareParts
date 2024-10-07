using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Users
{
    public class CreatePasswordModel
    {
        [Required]
        public string UserId { get; set; }
       // public string? Mobile { get; set; }
        [JsonPropertyName("password")]
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? PasswordConfirm { get; set; }
    }
    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }
        [JsonPropertyName("password")]
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? PasswordConfirm { get; set; }
    }
}
