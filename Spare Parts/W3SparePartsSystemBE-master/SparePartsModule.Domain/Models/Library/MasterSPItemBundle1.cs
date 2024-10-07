using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPItemBundle1
    {
        [Key]
        public int BundleID { get; set; }
        public string BundleCode { get; set; }
        public string BundleNameEn { get; set; }
        public string? BundleNameAr { get; set; }
        public string? BundleDesc { get; set; }
        
        [Precision(18, 3)]
        public decimal? BundleCost { get; set; }
        [Precision(18, 3)]
        public decimal? BundleTaxPercentage { get; set; }
        [Precision(18, 3)]
        public decimal? BundleTaxAmount { get; set; }
        [Precision(18, 3)]
        public decimal? BundleGrandAmount { get; set; }
        [Precision(18,3)]
        public decimal? BundlePrice { get; set; }
        [Precision(18, 3)]
        public decimal? BundleDiscountPercentage { get; set; }
        [Precision(18, 3)]
        public decimal? BundleDiscountAmount { get; set; }
        [Precision(18, 3)]
        public decimal? BundlePriceAfterDiscount { get; set; }
        public string? BundleComments { get; set; }
        public DateTime BundleCreationDate { get; set; }
        public DateTime? BundleValidFrom { get; set; }
        public DateTime? BundleValidTo { get; set; }
        public int? BundleGroup { get; set; }
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
