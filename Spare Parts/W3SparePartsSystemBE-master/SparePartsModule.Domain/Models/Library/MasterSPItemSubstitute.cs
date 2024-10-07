using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPItemSubstitute
    {
        [Key]
        public int ItemSubstituteID { get; set; }
        [ForeignKey(nameof(MasterSPItem))]
        public int ItemID { get; set; }
       
        public int? SubstituteID { get; set; }
        public int? SubstituteType { get; set; }
        public int? SubstituteCode { get; set; }
        public string? SubstituteNo { get; set; }
        public string? SubstituteBarcode { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
        public MasterSPItem MasterSPItem { get; set; }
      
    }
}
