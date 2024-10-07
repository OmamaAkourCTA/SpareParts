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
    public class EditSupplierModel
    {
        [Required]
        public int SupplierID { get; set; }
        public int SupplierNo { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierAbbCode { get; set; }
        [SwaggerSchema("Lookup type id 2")]
        [Description("")]
        public int? OriginalCountry { get; set; }
        public int? SupplierAccountNo { get; set; }
        [SwaggerSchema("Lookup type id 7")]
        [Description("")]
        public int? SupplierCurrency { get; set; }
        [SwaggerSchema("yyyy-MM-dd")]
        [Description("")]
        public DateTime? AgreementStart { get; set; }
        [SwaggerSchema("yyyy-MM-dd")]
        [Description("")]
        public DateTime? AgreementEnd { get; set; }
        public List<IFormFile>? SupplierAttachment { get; set; }
        [SwaggerSchema("Lookup type id 4")]
        [Description("")]
        public int? PaymentMethod { get; set; }
        //[SwaggerSchema("Lookup type id 5")]
        //[Description("")]
        //public int? DeliveryMethod { get; set; }
        //[SwaggerSchema("Local, Gulf, Internaional")]
        //[Description("")]
        [Description("Lookup type id 17")]
        public int? SupplierLocalInternational { get; set; }
        public int? SupplierParent { get; set; }
        [SwaggerSchema("Lookup type id 5")]
        [Description("")]
        public int? SupplierTaxType { get; set; }
        [SwaggerSchema("master lookup type 6")]
        [Description("")]
        public double? Discount { get; set; }
        public string? Comments { get; set; }
        [SwaggerSchema("Lookup type id 1")]
        [Description("")]
        public int? Status { get; set; }
        public bool? TMCSupplier { get; set; }
        public decimal? CostFactor { get; set; }
        public decimal? PriceFactor { get; set; }
    }
}
