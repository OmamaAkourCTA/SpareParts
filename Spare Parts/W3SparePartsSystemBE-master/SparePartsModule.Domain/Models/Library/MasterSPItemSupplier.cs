using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPItemSupplier
    {
        [Key]
        public int ItemSupplierID { get; set; }
        [ForeignKey(nameof(MasterSPItem))]

        public int ItemID { get; set; }
        public int SupplierID { get; set; }
        [Precision(18,3)]
        public decimal? SupplierFOB { get; set; }
        [Precision(18, 3)]
        public decimal? ItemSupAvgCostFactor { get; set; }
        [Precision(18, 3)]
        public decimal? ItemSupAvgCost { get; set; }
        public int? ItemSupDiscountType { get; set; }
        public int? ItemSupTaxType { get; set; }
        public int? ItemMinQty { get; set; }
        public int? ItemMaxQty { get; set; }
        public string? ItemSupplierBarcode { get; set; }
        public string? ItemSupNumber { get; set; }
        public int? ItemSupFlag { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime? EnterDate { get; set; }
        public TimeSpan? EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }

        public MasterSPItem? MasterSPItem { get; set; }
    

    }
}
