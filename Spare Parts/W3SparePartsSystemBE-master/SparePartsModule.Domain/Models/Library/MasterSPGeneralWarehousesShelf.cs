using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPGeneralWarehousesShelf
    {
        [Key]
        public int ShelfID { get; set; }
        public int ZoneID { get; set; }
        public string ShelfName { get; set; }
        public string Comments { get; set; }
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
