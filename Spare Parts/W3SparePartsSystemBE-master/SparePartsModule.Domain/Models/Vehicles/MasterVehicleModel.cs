using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class MasterVehicleModel
    {
        [Key]
        public int ModelID { get; set; }
        public string? ModelName { get; set; }
        public string? ModelNameAR { get; set; }
        public int? ModelBrandID { get; set; }
        public int? ModelSupplierID { get; set; }
        public int? ModelType { get; set; }
        public int? ModelSegment { get; set; }
        public int? ModelCustomType { get; set; }
        public int? ModelGroup { get; set; }
        public bool? MarkaziaModel { get; set; }
        public int? ModelYear { get; set; }
        public int? EquivalentMarkaziaModelID { get; set; }
        public string? ModelAttachments { get; set; }
        public string? ModelImages { get; set; }
        public string? ModelMarketingImages { get; set; }
        public string? ModelRemarks { get; set; }
        public int Status { get; set; }
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
