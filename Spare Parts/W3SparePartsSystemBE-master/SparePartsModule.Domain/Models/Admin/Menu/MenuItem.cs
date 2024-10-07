using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Menu
{
    public class MenuItem
    {
        [Key]
        public int MenuItemId { get; set; }
        public int MenuCategoryId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string Icon { get; set; }
        public string Icon2 { get; set; }
        public string Url { get; set; }
        public int OrderNo { get; set; }
    }
}
