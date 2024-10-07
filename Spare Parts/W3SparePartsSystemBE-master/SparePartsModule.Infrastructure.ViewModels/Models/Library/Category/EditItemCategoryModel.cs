using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Category
{
    public class EditItemCategoryModel
    {
        [Required]
        public int ItemCategoryID { get; set; }
        [Required]
        public string? ItemCategoryCode { get; set; }
        [Required]
        public string? ItemCategoryNameEn { get; set; }
        [Required]
        public string? ItemCategoryNameAr { get; set; }
        public string? ItemCategoryDesc { get; set; }
        [SwaggerSchema("Lookup type id 6")]
        [Description("")]
        public int? ItemCategoryType { get; set; }

        [SwaggerSchema("Lookup type id 8")]
        [Description("")]
        public int? ItemCategoryMinMaxType { get; set; }
        [SwaggerSchema("Lookup type id 9")]
        [Description("")]
        public int? ItemCategorySerialType { get; set; }
        public int? ItemCategoryTariffID { get; set; }
        [SwaggerSchema("Lookup type id 10")]
        [Description("")]
        public int? ItemCategoryTaxType { get; set; }
        [SwaggerSchema("master Lookup type id 6")]
        [Description("")]
        public int? ItemCategoryDiscountType { get; set; }
        public string? ItemCategoryComments { get; set; }
        [SwaggerSchema("Lookup type id 1")]
        [Description("")]
        [Required]
        public int? Status { get; set; }
    }
}
