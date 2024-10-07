using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Warehouses
{
    public class AddZoneModel
    {
        [SwaggerSchema("API:Getwarehouse")]
        [Required]
        public int WarehouseID { get; set; }
        [SwaggerSchema("API:Getsubwarehouse")]
        public int? SubWarehouseID { get; set; }
        [Required]
        public string ZoneName { get; set; }
        public string? Comments { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
