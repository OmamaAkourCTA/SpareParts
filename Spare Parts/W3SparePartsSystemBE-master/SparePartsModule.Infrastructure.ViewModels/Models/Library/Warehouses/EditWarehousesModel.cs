using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Warehouses
{
    public class EditWarehousesModel
    {
        [Required]
        public int WarehouseID { get; set; }
        [Required]
        public string WarehouseName { get; set; }
        [Required]
        [SwaggerSchema(" lookup type id 34")]
        public int WarehouseType { get; set; }
        [Required]
        public int WarehouseBA { get; set; }
        public string? WarehouseAbb { get; set; }
        [Required]
        [SwaggerSchema("master lookup type id 1")]
        public int Status { get; set; }
    }
}
