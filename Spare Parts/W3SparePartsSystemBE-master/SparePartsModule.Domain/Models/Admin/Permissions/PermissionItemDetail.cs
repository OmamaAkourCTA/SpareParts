using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Permissions
{
    public class PermissionItemDetail
    {

        [Key]
        public int PermissionItemDetailsId { get; set; }
        public int PermissionItemId { get; set; }
        [MaxLength(200)]
        public string PerItemDetailsName { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

    }
}
