using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class GettingSPOrderHistory2
    {
        [Key]
        public int OrderLineID { get; set; }
        public int OrderID { get; set; }
        public int OrderLineSeq { get; set; }
        public int OrderItemID { get; set; }
        [Precision(18, 3)]
        public decimal OrderItemQty { get; set; }
        [Precision(18, 3)]
        public decimal? OrderItemQtyCancelled { get; set; }
        [Precision(18, 3)]
        public decimal? OrderItemQtyConfirmed { get; set; }
        [Precision(18, 3)]
        public decimal OrderItemPrice { get; set; }
        [Precision(18, 3)]
        public decimal? OrderItemPriceConfirmed { get; set; }
        [Precision(18, 3)]
        public decimal OrderItemAmount { get; set; }
        [Precision(18, 3)]
        public decimal OrderItemDisPer { get; set; }
        [Precision(18, 3)]
        public decimal OrderItemDisAmount { get; set; }
        [Precision(18, 3)]
        public decimal OrderItemNetAmount { get; set; }
        [Precision(18, 3)]
        public decimal OrderItemTaxPer { get; set; }
        [Precision(18, 3)]
        public decimal OrderItemTaxAmount { get; set; }
        [Precision(18, 3)]
        public decimal OrderItemTotalAmount { get; set; }
        public int? OrderItemSupplierFlag { get; set; }
        public int? OrderItemOrderFlag { get; set; }
        [Precision(18, 3)]
        public decimal? OrderItemQtyOnHand { get; set; }
        [Precision(18, 3)]
        public decimal? OrderItemQtyOnOrder { get; set; }
        [Precision(18, 3)]
        public decimal? OrderItemQtyOrderMin { get; set; }
        [Precision(18, 3)]
        public decimal? OrderItemQtyOrderMax { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
    }
}
