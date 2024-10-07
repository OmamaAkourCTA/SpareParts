using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items
{
    public class AddItemLocationModel
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
