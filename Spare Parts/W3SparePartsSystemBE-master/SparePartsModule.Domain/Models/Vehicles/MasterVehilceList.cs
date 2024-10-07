using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparePartsModule.Domain.Models
{
    public class MasterVehilceList
    {
        [Key]
        public int ListID { get; set; }

        public string ListName { get; set; }
        public int? ListBrandID { get; set; }
        [ForeignKey(nameof(MasterVehicleModel))]
        public int? ListModelID { get; set; }
        public int? ListModelCodeID { get; set; }
        public int? ListYear_TBD { get; set; }
        public string? ListSFX { get; set; }
        public int? ListSupplierID { get; set; }
        public int? ListSegment { get; set; }
        public bool? ListMarkazia { get; set; }
        public int? ListMrkaziaEquivalentID { get; set; }
        public int? ListCustomType { get; set; }
        public int? ProductionMonth { get; set; }
        public int? ProductionYear { get; set; }
        public double? CandFPrice { get; set; }
        public int? CandFCurrency { get; set; }
        public int? FuelType { get; set; }
        public string? ListIndex { get; set; }
        public string? ListKCode { get; set; }
        public string? ListSpecsFile { get; set; }
        public int? ListGroup { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
        public MasterVehicleModel MasterVehicleModel { get; set; }
        public List<MasterVehicleGeneralSpecsValue> MasterVehicleGeneralSpecsValues { get; set; }
    }
}
