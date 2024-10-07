using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Menu
{
    public class MenuCategory
    {
        [Key]
        public int MenuCategoryId { get; set; }
        public int PortalId { get; set; }
        public int RoleId { get; set; }
        [MaxLength(100)]
        public string MenuCategoryName { get; set; }
        public int OrderNo { get; set; }

        public int Status { get; set; }

    }
}
