using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPItem
    {
        [Key]
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string? ItemCodeSup { get; set; }
        public string? ItemCodeCust { get; set; }
        [ForeignKey(nameof(MasterSPGeneralItemName))]
        public int? ItemNameID { get; set; }
        public int? ItemCategoryID { get; set; }
        public int? ItemUnitofMeasurement { get; set; }
        public double? ItemQuantityPerUnit { get; set; }
        public double? ItemAvgCost { get; set; }
        public double? ItemPrice { get; set; }
        public double? ItemLength { get; set; }
        public double? ItemWidth { get; set; }
        public double? ItemHeight { get; set; }
        public double? ItemSizeM3 { get; set; }
        public double? ItemWeight { get; set; }
        public int? ItemMainSup { get; set; }
        public string? ItemBarcode { get; set; }
        public string? ItemQR { get; set; }
        public string? ItemImage { get; set; }
        public string? ItemPartCode { get; set; }
        public string? ItemProdCode { get; set; }
        public int? ItemFlagSup { get; set; }
        public int? ItemFlagOrder { get; set; }
        public int? ItemFlagSales { get; set; }
        public int? ItemMinMaxType { get; set; }
        public int? ItemMinOrder { get; set; }
        public int? ItemMaxOrder { get; set; }
        public int? ItemSerialType { get; set; }
        public int? ItemTariffID { get; set; }
        public int? ItemTaxType { get; set; }
        public int? ItemCurrency { get; set; }
        public int? ItemDiscountType { get; set; }
        public DateTime? ItemCreationDate { get; set; }
        public int? ItemGroup { get; set; }
        public string? ItemComments { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int? EnterUser { get; set; }
        public DateTime? EnterDate { get; set; }
        public TimeSpan? EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
        public MasterSPGeneralItemName MasterSPGeneralItemName { get; set; }
        public List<MasterSPItemSubstitute> MasterSPItemSubstitute { get; set; } = new List<MasterSPItemSubstitute>();
    }
}
