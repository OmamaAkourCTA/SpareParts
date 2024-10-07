using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPItemBundle2
    {
        [Key]
        public int BundleLineID { get; set; }
        public int BundleID { get; set; }
        public int ItemID { get; set; }
        public decimal? ItemCost { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemQty { get; set; }
        [Precision(18,3)]
        public decimal TotalItemAmount { get; set; }
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
