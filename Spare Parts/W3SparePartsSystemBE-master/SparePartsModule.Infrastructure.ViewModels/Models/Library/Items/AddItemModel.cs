using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items
{
    public class AddItemModel
    {

        [SwaggerSchema("Max length:25")]
        [Required]
        public string ItemCode { get; set; }
        [SwaggerSchema("Max length:25")]
        public string? ItemCodeSup { get; set; }
        [SwaggerSchema("Max length:25")]
        public string? ItemCodeCust { get; set; }
        [SwaggerSchema("API:GetItemNames")]
        public int? ItemNameID { get; set; }
        [SwaggerSchema("API:GetItemCategories")]
        public int? ItemCategoryID { get; set; }
        [SwaggerSchema("Master: Lookup type id 8")]
        public int? ItemUnitofMeasurement { get; set; }
        public double? ItemQuantityPerUnit { get; set; }
     
        public double? ItemPrice { get; set; }
        public double? ItemLength { get; set; }
        public double? ItemWidth { get; set; }
        public double? ItemHeight { get; set; }
        public double? ItemSizeM3 { get; set; }
        public double? ItemWeight { get; set; }
        [SwaggerSchema("API:GetSuppliers")]
        public int? ItemMainSup { get; set; }
        public string? ItemBarcode { get; set; }
        public IFormFile? ItemQR { get; set; }
        public string? ItemPartCode { get; set; }
        public string? ItemProdCode { get; set; }
        [SwaggerSchema("Lookup type id 18")]
        public int? ItemFlagSup { get; set; }
        [SwaggerSchema("Lookup type id 19")]
        public int? ItemFlagOrder { get; set; }
        [SwaggerSchema("Lookup type id 20")]
        public int? ItemFlagSales { get; set; }
        [SwaggerSchema("Lookup type id 8")]
        public int? ItemMinMaxType { get; set; }
        public int? ItemMinOrder { get; set; }
        public int? ItemMaxOrder { get; set; }
        [SwaggerSchema("Lookup type id 8")]
        public int? ItemSerialType { get; set; }
        [SwaggerSchema("API:GetLibraryTarifs")]
        public int? ItemTariffID { get; set; }
        [SwaggerSchema("Master: Lookup type id 5")]
        public int? ItemTaxType { get; set; }
        [SwaggerSchema("Date format yyyy-MM-dd")]
        public DateTime? ItemCreationDate { get; set; }
        //[SwaggerSchema("Master: Lookup type id 3")]
        //public int? ItemGroup { get; set; }
        [SwaggerSchema("Master: Lookup type id 1")]
        [Required]
        public int Status { get; set; }
        [SwaggerSchema("Master: Lookup type id 6")]
        public int? ItemDiscountType { get; set; }
        //[SwaggerSchema("Master: Lookup type id 7")]
        //public int? ItemCurrency { get; set; }
        public IFormFile? ItemImage { get; set; }
        public string? ItemComments { get; set; }
    }
}
