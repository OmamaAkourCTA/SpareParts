using SparePartsModule.Domain.Models.Branches;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int? UserNo { get; set; }
        
        [MaxLength(100)]
        public string? FullName { get; set; }
        [MaxLength(20)]

        public string? Mobile { get; set; }
        [MaxLength(100)]

        public string? Email { get; set; }
        [MaxLength(100)]

        public string? Password { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        [MaxLength(250)]

        public string? Token { get; set; }
        [MaxLength(250)]

        public string? DeviceToken { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastLogOut { get; set; }

        public int? Status { get; set; }
        public int BranchId { get; set; }
        public string? City { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public int RoleId { get; set; }
        public bool? Indoor { get; set; }
        public bool? Outdoor { get; set; }
        public string ? ProfileImage { get; set; }
        public int? Invitation { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsLoggedIn { get; set; }
        [Precision(18,3)]
        public decimal MaxAllowedLineDiscount { get; set; }
        [Precision(18, 3)]
        public decimal MaxAllowedInvoiceDiscount { get; set; }
        [Precision(18, 3)]
        public decimal? AllowedChangeDeductionAmount { get; set; }

        public List<UserWorkingHour> UserWorkingHours { get; set; } = new List<UserWorkingHour>();
        public List<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
        
    }
}
