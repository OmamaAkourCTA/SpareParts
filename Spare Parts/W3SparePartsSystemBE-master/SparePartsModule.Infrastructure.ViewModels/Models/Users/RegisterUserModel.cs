using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Users
{
    public class RegisterUserModel
    {
        [JsonPropertyName("fullName")]
        [Required]
        public string? FullName { get; set; }


        [JsonPropertyName("mobile")]
        public string? Mobile { get; set; }
        [Required]

        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [Required]
        public int BranchId { get; set; }
        [Required]
        public string City { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        [Required]

        public int? RoleId { get; set; }
        public bool? Indoor { get; set; }
        public bool? Outdoor { get; set; }
        public decimal MaxAllowedInvoiceDiscount { get; set; }
        public decimal MaxAllowedLineDiscount { get; set; }
        public decimal AllowedChangeDeductionAmount { get; set; }
        //   public IFormFile?  ProfileImage { get; set; }

        // public string? UserWorkingHours { get; set; }
        public List<UserPermissionModel>? UserPermissions { get; set; }=new List<UserPermissionModel>();

        public List<UserWorkingHoursModel>? UserWorkingHours { get; set; } = new List<UserWorkingHoursModel>();
      //  [JsonPropertyName("password")]
      //  [Required]
      //  public string? Password { get; set; }
     //   [Required]
      //  public string? PasswordConfirm { get; set; }
        public String? DeviceToken { get; set; }
        public int? Status { get; set; } = 2001;

    }
    public class EditUserModel
    {
        [Required]
        public int UserId { get; set; }
        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }


        [JsonPropertyName("mobile")]
        public string? Mobile { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
        public int? BranchId { get; set; }
        public string? City { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }

        public int? RoleId { get; set; }
        public bool? Indoor { get; set; }
        public bool? Outdoor { get; set; }
        public decimal? MaxAllowedInvoiceDiscount { get; set; }
        public decimal? MaxAllowedLineDiscount { get; set; }
        public decimal? AllowedChangeDeductionAmount { get; set; }
        // public IFormFile? ProfileImage { get; set; }

        //  public string? UserWorkingHours { get; set; }
        //   public string? UserPermissions { get; set; }
        public List<UserPermissionModel>? UserPermissions { get; set; } = new List<UserPermissionModel>();

        public List<UserWorkingHoursModel>? UserWorkingHours { get; set; } = new List<UserWorkingHoursModel>();
        public int? Status { get; set; } = 2001;

    }

    public class UserWorkingHoursModel
    {
        [Required]
        public int DayId { get; set; }
        public string? FromTime { get; set; }
        public string? ToTime { get; set; }
        [Required]
        public int Status { get; set; }

    }
}
