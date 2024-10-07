using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Users
{
    public class AddRoleModel
    {
        [Required]

        public string? Name { get; set; }
        public bool? Indoor { get; set; }

        public bool? Outdoor { get; set; }
        public decimal MaxAllowedInvoiceDiscount { get; set; }
        public decimal MaxAllowedLineDiscount { get; set; }
        [Required]
        public int Status { get; set; }
        public List<RolePermissionModel> RolePermissions { get; set; }=new List<RolePermissionModel>();

    }
    public class UpdateRoleModel
    {
        [Required]
        public int RoleId { get; set; }


        public string? Name { get; set; }
        public bool? Indoor { get; set; } = false;
        public decimal? MaxAllowedInvoiceDiscount { get; set; }
        public decimal? MaxAllowedLineDiscount { get; set; }
        public bool? Outdoor { get; set; } = false;
        public int? Status { get; set; }
        public List<RolePermissionModel>? RolePermissions { get; set; } = new List<RolePermissionModel>();
    }
}
