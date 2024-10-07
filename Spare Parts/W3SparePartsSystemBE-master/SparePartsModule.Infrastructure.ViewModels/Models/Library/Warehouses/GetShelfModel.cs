using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Warehouses
{
    public class GetShelfModel
    {
        [Required]
        public int ZoneId { get; set; }
        public int? Status { get; set; }
    }
}
