using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPGeneralLocation
    {
        [Key]
        public int LocationID { get; set; }
        public string LocationCode { get; set; }
        public int? LocationWarehouse { get; set; }
        public int? LocationWHZone { get; set; }
        public int? LocationWHShelf { get; set; }
        public int? LocationWHSection { get; set; }
        public string? LocationRow { get; set; }
        public string? LocationColumn { get; set; }
        public int? LocationType { get; set; }
        public string? Location { get; set; }
        [Precision(18,3)]
        public decimal? LocationLength { get; set; }
        [Precision(18, 3)]
        public decimal? LocationWidth { get; set; }
        [Precision(18, 3)]
        public decimal? LocationHeight { get; set; }
        [Precision(18, 3)]
        public decimal? LocationSizeM3 { get; set; }
        public string? LocationQR { get; set; }
        public int? LocationGroup { get; set; }
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
