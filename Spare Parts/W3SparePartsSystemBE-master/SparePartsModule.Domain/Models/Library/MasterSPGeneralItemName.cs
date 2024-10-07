using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPGeneralItemName
    {
        [Key]
        public int? ItemNameID { get; set; }
        [ForeignKey(nameof(MasterSPLookup))]
        public int? ItemNameCode { get; set; }
        public string? ItemNameEn { get; set; }
        public string? ItemNameAr { get; set; }
        public string? ItemNameDesc { get; set; }
        public int? ItemNameType { get; set; }
        public string? ItemNameComments { get; set; }
        public int? ItemNameGroup { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
        public MasterSPLookup MasterSPLookup { get; set; }
    }
}
