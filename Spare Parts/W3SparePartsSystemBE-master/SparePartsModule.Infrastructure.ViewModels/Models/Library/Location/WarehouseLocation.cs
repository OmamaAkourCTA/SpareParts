using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Location
{
    public class WarehouseLocation
    {
        public string WarehouseName { get; set; }
        public string WarehouseAbb { get; set; }
        public string Subwarehouse { get; set; }
        public string SubWarehouseAbb { get; set; }
        public string Zone { get; set; }
        public string Shelf { get; set; }
        public string Section { get; set; }
        public string LocationType { get; set; }
        public string Row { get; set; }
        public string Column { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Length { get; set; }
    }

}
