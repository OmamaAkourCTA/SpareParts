using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Warehouses
{
    public class GetWarehousesModel
    {
       
        public string? WarehouseName { get; set; }
   
        [SwaggerParameter(" lookup type id 34")]
        public int? WarehouseType { get; set; }
   
        public int? WarehouseBA { get; set; }
       
        [SwaggerParameter("master lookup type id 1")]
        public int? Status { get; set; }
        [SwaggerParameter("Sort 2,3 Name, 4,5 BA, 6,7 Type, 8,9 status")]
        public int Sort { get; set; }
    }
}
