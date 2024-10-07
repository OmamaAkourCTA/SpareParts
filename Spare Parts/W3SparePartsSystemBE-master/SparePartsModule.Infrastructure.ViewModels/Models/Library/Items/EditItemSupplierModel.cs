using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items
{
    public class EditItemSupplierModel
    {
        [Required]
        public int ItemSupplierID { get; set; }
        [Required]
        [SwaggerSchema("Api:GetItems")]
        public int ItemID { get; set; }
        [Required]
        [SwaggerSchema("Api:Getsuppliers")]
        public int SupplierID { get; set; }
        [Required]
        public decimal SupplierFOB { get; set; }

        public string? ItemBarcode { get; set; }
        public string? ItemSupNumber { get; set; }
        [SwaggerSchema(" lookup typeid 18")]
        public int? ItemSupFlag { get; set; }

        [Required]
        [SwaggerSchema("master lookup typeid 1")]
        public int Status { get; set; }
        public decimal? ItemSupAvgCostFactor { get; set; }
        public decimal? ItemSupCost { get; set; }
        [SwaggerSchema("master  lookup typeid 6")]
        public int? ItemSupDiscountType { get; set; }
        [SwaggerSchema("master lookup typeid 5")]
        public int? ItemSupTaxType { get; set; }
        public int? ItemMinQty { get; set; }
        public int? ItemMaxQty { get; set; }
    }
}
