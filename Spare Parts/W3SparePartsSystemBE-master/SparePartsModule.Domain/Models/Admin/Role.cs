using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }
        public bool Indoor { get; set; }
        public bool Outdoor { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiyedBy { get; set; }
        public DateTime? ModifiyedAt { get; set; }
        public int Status { get; set; }
        public int NoofUsers { get; set; }
        public bool CanModifiy { get; set; }
        public decimal MaxAllowedLineDiscount { get; set; }
        public decimal MaxAllowedInvoiceDiscount { get; set; }
        public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();



    }
}
