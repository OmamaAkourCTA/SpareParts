using SparePartsModule.Domain.Models.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPItemModelCode
    {
        [Key]
        public int ItemModelCodeID { get; set; }
        public int ItemID { get; set; }
        [ForeignKey(nameof(MasterVehicleModelCode))]
        public int ModelCodeID { get; set; }
        public int? ListNameID { get; set; }
        //public int ModelCodeFrom { get; set; }
        //public int ModelCodeTo { get; set; }
        public int ModelCodeLinkType { get; set; }
        public string? ModelCodeSpareParts { get; set; }
        public string? ModelCodeService { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
        public MasterVehicleModelCode MasterVehicleModelCode { get; set; }
    }
}
