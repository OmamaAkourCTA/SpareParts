using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Warehouses
{
    public  class AddSubWarehouseModel
    {
        [SwaggerSchema("Api:GetwareHouse")]
        public int WarehouseID { get; set; }
        public string SubWarehouseName { get; set; }
        public string? SubWarehouseAbb { get; set; }
        public string? Comments { get; set; }
        public int Status { get; set; }
    }
}
