using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items
{
    public class GetItemsModel
    {
        public int? ItemID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemCodeSup { get; set; }
        public string? ItemCodeCust { get; set; }
        public string? ItemNameID { get; set; }
        public int? ItemNameCode { get; set; }
        public int? ItemNameNo { get; set; }
        public DateTime? CreationDateFrom { get; set; }
        public DateTime? CreationDateTo { get; set; }
        [SwaggerParameter("lookuptype id 7")]
        public string? ItemCategoryID { get; set; }
        public string? ItemMainSup { get; set; }


        public string? ItemPartCode { get; set; }
        public string? ItemProdCode { get; set; }

        public int? Status { get; set; }
        [SwaggerParameter("lookuptype id 18 ")]
        public string? SupplierFlag { get; set; }
        [SwaggerParameter("lookuptype id 19")]
        public string? ItemFlagOrder { get; set; }
        [SwaggerParameter("lookuptype id 20")]
        public string? ItemFlagSales { get; set; }



        public int Sort { get; set; }
    }

    public class GetItemInquiryCards
    {
        public int? ItemID { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemCodeCust { get; set; }
        public string? ItemCodeSup { get; set; }
        public decimal? ItemPrice { get; set; }
        public int? ItemQtyUserLocation { get; set; }
        public int? ItemQtyAllWarehouses { get; set; }
        public decimal? ItemQuantityPerUnit { get; set; }
        public string? ItemImage { get; set; }
        public decimal? ItemPriceWithTax { get; set; }
        public int? ItemSectionAvailable { get; set; }
        public int? ItemSectionReserved { get; set; }
        public decimal? ItemLength { get; set; }
        public decimal? ItemWidth { get; set; }
        public decimal? ItemHeight { get; set; }
        public decimal? ItemSizeM3 { get; set; }
        public decimal? ItemWeight { get; set; }
        public int? ItemMinOrder { get; set; }
        public int? ItemMaxOrder { get; set; }
        public string? ItemBarcode { get; set; }
        public string? ItemQR { get; set; }
        public string? ItemPartCode { get; set; }
        public string? ItemProdCode { get; set; }
        public string? ItemComments { get; set; }
        public decimal? ItemAvgCost { get; set; }
        public DateTime? ItemCreationDate { get; set; }
        public DateTime? EnterDate { get; set; }
        public TimeSpan? EnterTime { get; set; }
        public int? ItemNumberOfSubstitutes { get; set; }
        public int? ItemNumberOfModelCodes { get; set; }
        public int? ItemNumberOfBundles { get; set; }
        public int? ItemNumberOfConfirmedOrders { get; set; }
        public int? ItemNumberOfSuppliers { get; set; }
        public int? ItemCategoryID { get; set; }
        public string? ItemCategoryName { get; set; }
        public int? ItemUnitofMeasurementID { get; set; }
        public string? ItemUnitofMeasurementName { get; set; }
        public int? ItemFlagSupID { get; set; }
        public string? ItemFlagSupName { get; set; }
        public int? ItemFlagSalesID { get; set; }
        public string? ItemFlagSalesName { get; set; }
        public int? ItemFlagOrderID { get; set; }
        public string? ItemFlagOrderName { get; set; }
        public int? ItemMinMaxTypeID { get; set; }
        public string? ItemMinMaxTypeName { get; set; }
        public int? ItemSerialTypeID { get; set; }
        public string? ItemSerialTypeName { get; set; }
        public int? ItemTaxTypeID { get; set; }
        public string? ItemTaxTypeName { get; set; }
        public int? ItemDiscountTypeID { get; set; }
        public string? ItemDiscountTypeName { get; set; }
        public int? ItemCurrencyID { get; set; }
        public string? ItemCurrencyName { get; set; }
        public int? ItemGroupID { get; set; }
        public string? ItemGroupName { get; set; }
        public int? StatusID { get; set; }
        public string? StatusName { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int? TariffID { get; set; }
        public string? TariffCode { get; set; }
        public decimal? TariffPer { get; set; }
        public int? ItemNameID { get; set; }
        public string? ItemNameEn { get; set; }
        public string? ItemNameAr { get; set; }
        public int? ItemNameCode { get; set; }
        public string? ItemNameDesc { get; set; }
    }
}
