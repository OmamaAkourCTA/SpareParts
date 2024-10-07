using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Vehicles
{
    public class MasterVehicleModelCode
    {
        [Key]
        public int ModelCodeID { get; set; }
        public int ModelID { get; set; }
        public int? BrandID { get; set; }
        public string? ModelCodeVehicles { get; set; }
        public string? BrandName { get; set; }
        public string? BrandNameAR { get; set; }
        public string? ModelName { get; set; }
        public string? ModelNameAR { get; set; }
        public string? ModelCodeSP { get; set; }
        public string? ModelCodeService { get; set; }
        public string? ModelCodeComments { get; set; }
        public int? ModelCodeValidFrom { get; set; }
        public int? ModelCodeValidTo { get; set; }
        public int? ModelCodeYear { get; set; }
     
        public int? Status { get; set; }
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
