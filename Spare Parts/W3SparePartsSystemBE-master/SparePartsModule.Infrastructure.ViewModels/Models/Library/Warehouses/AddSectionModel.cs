using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Warehouses
{
    public class AddSectionModel
    {
        [SwaggerSchema("API: GetShelfs")]
        public int WarehouseShelfID { get; set; }
        public string SectionName { get; set; }
        public string? Comments { get; set; }
        public int Status { get; set; }
    }
}
