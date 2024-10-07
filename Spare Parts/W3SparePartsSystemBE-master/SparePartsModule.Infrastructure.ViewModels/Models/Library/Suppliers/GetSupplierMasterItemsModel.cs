using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers
{
    public class GetSupplierMasterItemsModel
    {
        [Required]
        public int SupplierId { get; set; }
        public string? ItemSupplierNumber { get; set; }
    }
}
