using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Suppliers
{
    public class AddSupplierModel
    {
        [Required]
        public string SupplierName { get; set; }
        public string? SupplierAbbCode { get; set; }
      //  [SwaggerSchema("Lookup type id 2")]
        [SwaggerSchema("Lookup type id 2")]
        public int? OriginalCountry { get; set; }
        public int? SupplierAccountNo { get; set; }
       // [SwaggerSchema("")]
        [SwaggerSchema("Lookup type id 7")]
        public int? SupplierCurrency { get; set; }
       // [SwaggerSchema("")]
        [SwaggerSchema("yyyy-MM-dd")]
        public DateTime? AgreementStart { get; set; }
       // [SwaggerSchema("")]
        [SwaggerSchema("yyyy-MM-dd")]
        public DateTime? AgreementEnd { get; set; }
        public List<IFormFile>? SupplierAttachment { get; set; }
       // [SwaggerSchema("")]
        [SwaggerSchema("Lookup type id 4")]
        public int? PaymentMethod { get; set; }
       // [SwaggerSchema("")]
        //[SwaggerSchema("Lookup type id 5")]
        //public int? DeliveryMethod { get; set; }
        // [SwaggerSchema("")]
        // [SwaggerSchema("Local, Gulf, Internaional")]
        [SwaggerSchema("Lookup type id 17")]
        public int? SupplierLocalInternational { get; set; }
        public int? SupplierParent { get; set; }
       // [SwaggerSchema("")]
        [SwaggerSchema("Lookup type id 5")]
    
        public int? SupplierTaxType { get; set; }
        //[SwaggerSchema("")]
        [SwaggerSchema("master lookup type 6")]
        public int? Discount { get; set; }
        public string? Comments { get; set; }
        [SwaggerSchema("Lookup type id 1")]
       // [SwaggerSchema("")]
        public int Status { get; set; }
        public bool? TMCSupplier { get; set; }
        public decimal? CostFactor { get; set; }
        public decimal? PriceFactor { get; set; }
    }
}
