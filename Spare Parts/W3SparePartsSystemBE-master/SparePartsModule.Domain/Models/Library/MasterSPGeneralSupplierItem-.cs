using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPGeneralSupplierItem0ld
    {
        [Key]
        public int ItemSupID { get; set; }
        public int? MasterFileId { get; set; }
        public int ItemSupplierID { get; set; }
        public string? ItemSupNo { get; set; }
        public string? ItemSupDesc { get; set; }
        public int? ItemSupSubstituteType { get; set; }
        public string? ItemSubSubstitute { get; set; }
        public string? ItemSupSubstituteItem { get; set; }
        public int? ItemSupUnitofMeasurementID { get; set; }
        public double? ItemSupQuantityPerUnit { get; set; }
        public double? ItemSupPrice { get; set; }
        public int? ItemSupPriceCurrency { get; set; }
        public double? ItemSupLength { get; set; }
        public double? ItemSupWidth { get; set; }
        public double? ItemSupHeight { get; set; }
        public double? ItemSupWeight { get; set; }
        public double? ItemSupSizeM3 { get; set; }
        public int? ItemSupLexus { get; set; }
        public int? ItemSupStart { get; set; }
        public int? ItemSupEnd { get; set; }

        public string? ItemSupFI_Id { get; set; }
        public string? ItemSupPro { get; set; }
      
       
        public string? ItemSupFlag { get; set; }
        public int? ItemSupna2 { get; set; }
        public int? ItemSupFlagSP { get; set; }
        //public int? ItemSupna2 { get; set; }
        public int? ItemSupUPQ { get; set; }
        public int? ItemSupMaxOrd { get; set; }
        public int? ItemSupMinOrd { get; set; }
        public string? ItemSupOrigin { get; set; }
        public int? ItemSupOrigionMaster { get; set; }
        public int? ItemSupGroup { get; set; }
        public int? Status { get; set; }
        public bool? Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int? EnterUser { get; set; }
        public DateTime? EnterDate { get; set; }
        public TimeSpan? EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }

    }
}
