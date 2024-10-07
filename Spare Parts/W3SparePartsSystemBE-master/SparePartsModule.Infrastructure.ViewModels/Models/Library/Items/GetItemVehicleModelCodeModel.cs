using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items
{
    public class GetItemVehicleModelCodeModel
    {
        public int? BrandID { get; set; }
        public int? ModelID { get; set; }
        public string? ModelCodeVehicle { get; set; }
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }

        public int? LinkTypeID { get; set; }
        public int? ItemId { get; set; }
        public int? Status { get; set; }

       // public int Sort { get; set; }

    }
}
