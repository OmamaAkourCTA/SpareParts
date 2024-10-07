using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.SPOrder
{
    public class IrregularitiesModel
    {
        public int OrderLineID { get; set; }
        public int OrderLineSeq { get; set; }
        public int OrderItemID { get; set; }
        public string SupplierItemNumber { get; set; }
        public string ItemNameEn { get; set; }
        public object FlagMinMax { get; set; }
        public object FlagHighFOB { get; set; }
        public object FlagHighWeight { get; set; }
        public object FlagHighVolume { get; set; }
        public object FlagMixed { get; set; }
        public decimal OrderItemQty { get; set; }
        public decimal? OrderItemQtyOrderMin { get; set; }
        public decimal? OrderItemQtyOrderMax { get; set; }
        public object Substitute { get; set; }
        public double? ItemWeight { get; set; }
        public double? ItemSizeM3 { get; set; }
        public double? ItemHeight { get; set; }
        public decimal OrderItemPrice { get; set; }
        public decimal TotalLine { get; set; }
    }
}
