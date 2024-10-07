using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers
{
    public class GetSupplierModel
    {
        public string? Search { get; set; }
        public string? Code { get; set; }
        [SwaggerParameter("Lookup type id 17")]
  
        public string? SupplierLocalInternational { get; set; }
        [SwaggerParameter("Lookup type id 5")]     
        public string? SupplierDeliveryMethod { get; set; }
        public bool? IsMainSupplier { get; set; }

        public int Sort { get; set; }
        public int? Status { get; set; }
    }
}
