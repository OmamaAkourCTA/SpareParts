using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class GettingSPOrderPOSS1
    {
        [Key]
        public int POSSID { get; set; }
        public int POSSSeq { get; set; }
        public string POSSNo { get; set; }
        public DateTime POSSDate { get; set; }
        [ForeignKey(nameof(GettingSPSupplier))]
        public int POSSSupplierID { get; set; }
        public string? POSSFile { get; set; }
        public string? POSSComments { get; set; }
        public int POSSStatus { get; set; }
        public int POSSGroup { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
        public GettingSPSupplier GettingSPSupplier { get; set; }
        public List<GettingSPOrderPOSS2> GettingSPOrderPOSS2 { get; set; } = new List<GettingSPOrderPOSS2>();
    }
}
