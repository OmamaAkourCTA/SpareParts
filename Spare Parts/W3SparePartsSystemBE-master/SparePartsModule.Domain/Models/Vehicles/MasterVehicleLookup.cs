using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class MasterVehicleLookup
    {
        [Key]
        public int LookupID { get; set; }
        public int LookupTypeID { get; set; }
       
        public string LookupName { get; set; }

        public string? LookupNameAR { get; set; }
        public string? LookupDesc { get; set; }
        public double? LookupValue { get; set; }
        public string? LookupSystem { get; set; }
        public bool? LookupStatic { get; set; }
        public bool? LookupDefault { get; set; }
        public int? LookupParent { get; set; }
        public int? LookupAction { get; set; }
        public int? LookupIntegration { get; set; }
        public string? LookupComments { get; set; }
        public string? LookupImage { get; set; }
        public string? LookupTextColor { get; set; }
        public string? LookupBGColor { get; set; }
        public int Status { get; set; }
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
