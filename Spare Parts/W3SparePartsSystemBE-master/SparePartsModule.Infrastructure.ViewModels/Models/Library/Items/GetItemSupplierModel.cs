using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items
{
    public class GetItemSupplierModel
    {
        public int? ItemId { get; set; }
        public string? ItemCode { get; set; }
        public int? SupplierId { get; set; }
        public int Sort { get; set; }
    }
}
