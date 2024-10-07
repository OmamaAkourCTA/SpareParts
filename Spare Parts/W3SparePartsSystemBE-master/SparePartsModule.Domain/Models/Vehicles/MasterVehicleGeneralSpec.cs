using System.ComponentModel.DataAnnotations;

namespace SparePartsModule.Domain.Models
{
    public class MasterVehicleGeneralSpec
    {
        [Key]
        public int GeneralSpecID { get; set; }
        public string ?GeneralSpecsNameEN { get; set; }
        public string? GeneralSpecsNameAR { get; set; }
        public int? GeneralSpecsCategory { get; set; }
        public string? GeneralSpecsDetails { get; set; }
        public bool? GeneralSpecsShowonPriceOffer { get; set; }
        public bool? GeneralSpecsShowonSalesSheet { get; set; }
        public bool? GeneralSpecsShowonStudyForm { get; set; }
        public bool? GeneralSpecsShowonInspectionForm { get; set; }
        public string? GeneralSpecsRemarks { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime ?CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
    }
}
