using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPGeneralSupplierItem
    {
        [Key]
        public int? RecordID { get; set; }
        public int? ItemSupID { get; set; }
        public string? ItemSupNo { get; set; }
        public string? partNowithdash { get; set; }
        public string? ItemSupFI_Id { get; set; }
        public int? Non_Usable { get; set; }
        public int? Non_Usage { get; set; }
        public int? UsagePostponed { get; set; }
        public int? Allotment { get; set; }
        public int? TMCPartsCatalogueError { get; set; }
        public int? ExportDestinationCode { get; set; }
        public int? SpecialStorageFollowParts { get; set; }
        public int? ManufactureDiscontinuation { get; set; }
        public int? NoService { get; set; }
        public int? Discontinuation { get; set; }
        public int? All_time_buypartcode { get; set; }
        public int? ItemSupMaxOrd { get; set; }
        public string? ItemSupSubstituteType { get; set; }
        public string? ItemSupSubstituteItem { get; set; }
        public int? QuantityinUse { get; set; }
        [Precision(18,3)]
        public decimal? UnitFOBPrice { get; set; }
        [Precision(18, 3)]
        public decimal? Corrunitprice { get; set; }
        public int? PriceClass { get; set; }
        public string? ItemSupPro { get; set; }
        public string? ItemSupDesc { get; set; }
        public int? ItemSupStart { get; set; }
        public int? ItemSupEnd { get; set; }
        public string? ItemSupPrice { get; set; }
        public int? ItemSupUPQ { get; set; }
        public int? Qty_per_TMC { get; set; }
        public int? DistributionPackage1 { get; set; }
        public int? DistributionPackage2 { get; set; }
        public int? DistributionPackage3 { get; set; }
        [Precision(18, 3)]
        public decimal? ItemSupLength { get; set; }
        [Precision(18, 3)]
        public decimal? ItemSupWidth { get; set; }
        [Precision(18, 3)]
        public decimal? ItemSupHeight { get; set; }
        [Precision(18, 3)]
        public decimal? ItemSupSizeM3 { get; set; }
        [Precision(18, 3)]
        public decimal? ItemSupWeight { get; set; }
        public string? Parts { get; set; }
        public int? TMCStockCode { get; set; }
        public int? ItemSupLexus { get; set; }
        public int? MultiplePNCCode { get; set; }
        public string? PNC1 { get; set; }
        public string? PNC2 { get; set; }
        public string? TMCPartsCenterCode { get; set; }
        public int? ItemSupMinOrd { get; set; }
        public string? Filler { get; set; }
        public string? ItemSupOrigin { get; set; }
        public int? Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int? EnterUser { get; set; }
        public DateTime? EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }

    }
}
