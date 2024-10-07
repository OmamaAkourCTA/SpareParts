using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class MasterVehilceListSpecsValue
    {
        [Key]
        public int ListSpecID { get; set; }
        public int? ListID { get; set; }
        [ForeignKey(nameof(MasterVehicleGeneralSpec))]
        public int? GeneralSpecID { get; set; }
        [ForeignKey(name: nameof(MasterVehicleGeneralSpecsValue))]
        public int? GeneralSpecItemID { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
        public MasterVehicleGeneralSpec MasterVehicleGeneralSpec { get; set; }
        public MasterVehicleGeneralSpecsValue MasterVehicleGeneralSpecsValue { get; set; }
    }
}
