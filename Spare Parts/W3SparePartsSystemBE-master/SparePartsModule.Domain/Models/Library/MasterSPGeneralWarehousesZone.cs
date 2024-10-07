using System.ComponentModel.DataAnnotations;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPGeneralWarehousesZone
    {
        [Key]
        public int ZoneID { get; set; }
        public int WarehouseID { get; set; }
        public int? SubWarehouseID { get; set; }
        public string ZoneName { get; set; }
        public string? Comments { get; set; }
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
