using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Warehouses
{
    public class AddShelfModel
    {
        [SwaggerSchema("API:GetZones")]
        [Required]
        public int ZoneID { get; set; }
        [Required]
        public string ShelfName { get; set; }
        public string? Comments { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
