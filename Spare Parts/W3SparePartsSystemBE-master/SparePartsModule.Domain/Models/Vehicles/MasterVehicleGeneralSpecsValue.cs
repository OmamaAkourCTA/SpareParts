using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparePartsModule.Domain.Models
{
    public class MasterVehicleGeneralSpecsValue
    {
        [Key]
        public int GeneralSpecsItemID { get; set; }
        [ForeignKey(nameof(MasterVehilceList))]
        public int GeneralSpecsID { get; set; }
        public string GeneralSpecsItemValueEN { get; set; }
        public string? GeneralSpecsItemValueAR { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
        public MasterVehilceList MasterVehilceList { get; set; }
    }
}
