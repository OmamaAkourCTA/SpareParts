using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Permissions
{
    public class PermissionCategory
    {
        [Key]
        public int PermissionCatId { get; set; }
        [MaxLength(200)]
        public string PerCatName { get; set; }
        public int PermissionPortalId { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int OrderNo { get; set; }

    }
}
