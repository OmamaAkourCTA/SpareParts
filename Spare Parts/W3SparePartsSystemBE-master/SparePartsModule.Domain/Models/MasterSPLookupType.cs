using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class MasterSPLookupType
    {
        [Key]
        public int LookupTypeID { get; set; }
        public string LookupTypeName { get; set; }
        public string? Comments { get; set; }
        public int? Status { get; set; }
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
