using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.SupplierDelivery
{
    public class AddSupplierDeliveryModel
    {
        [SwaggerSchema("Lookup type id 5")]
        [Required]
        public int? DeliveryMethodID { get; set; }
        [Required]
        public int SupplierID { get; set; }
        [SwaggerSchema("Lookup type id 1")]
        [Required]
        public int? Status { get; set; }
       
       

        
    }
}
