using System.ComponentModel.DataAnnotations;

namespace SparePartsModule.Domain.Models
{
    public class MasterVehicleBrand
    {
        [Key]
        public int BrandID { get; set; }
        public string? BrandName { get; set; }
        public string? BrandNameAR { get; set; }
        public int? BrandOrigionCountry { get; set; }
        public string? BrandLogo { get; set; }
        public string? BrandBusinessPlan { get; set; }
        public string? BrandComments { get; set; }
        public int? BrandGroup { get; set; }
  
        public bool? MarkaziaBrand { get; set; }
        public int Status { get; set; }
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
