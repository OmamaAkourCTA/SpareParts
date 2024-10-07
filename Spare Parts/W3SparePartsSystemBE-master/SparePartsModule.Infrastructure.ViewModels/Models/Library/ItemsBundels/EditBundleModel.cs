using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.ItemsBundels
{
    public class EditBundleModel
    {
        [Required]
        public int BundleID { get; set; }
        [SwaggerSchema("max length 25")]
        [Required]
        public string BundleCode { get; set; }
        [SwaggerSchema("max length 50")]
        [Required]
        public string BundleNameEn { get; set; }
        //[SwaggerSchema("max length 50")]
        //[Required]
        //public string? BundleNameAr { get; set; }
        [SwaggerSchema("max length 100")]
        public string? BundleDesc { get; set; }
        [Required]
        public decimal BundleCost { get; set; }
        [Required]
        public decimal BundlePrice { get; set; }
        [Required]
        public decimal BundleDiscountPercentage { get; set; }
        [Required]
        public decimal BundleDiscountAmount { get; set; }
        [Required]
        public decimal BundlePriceAfterDiscount { get; set; }
        [SwaggerSchema("max length 250")]
        public string? BundleComments { get; set; }
        [SwaggerSchema("date format:yyyy-MM-dd")]
        [Required]
        public DateTime BundleCreationDate { get; set; }
        public DateTime? BundleValidFrom { get; set; }
        public DateTime? BundleValidTo { get; set; }

        [SwaggerSchema("lookup type id 23,24,25")]
        [Required]
        public int Status { get; set; }
        public List<Item> Items { get; set; }
    }
   
}
