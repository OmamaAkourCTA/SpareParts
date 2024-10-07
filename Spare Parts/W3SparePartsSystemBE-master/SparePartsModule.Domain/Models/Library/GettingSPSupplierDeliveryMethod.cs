using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class GettingSPSupplierDeliveryMethod
    {

        [Key]
        public int SupplierDeliveryMethodID { get; set; }
        [ForeignKey(nameof(GettingSPSupplier))]

        public int SupplierID { get; set; }
        [ForeignKey(nameof(MasterSPLookup))]
        public int? SupplierDeliveryMethod { get; set; }
        public double? SupplierAverageLeadTime { get; set; }
        public int? Status { get; set; }
        public bool? Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
        public GettingSPSupplier GettingSPSupplier { get; set; }
        public MasterSPLookup MasterSPLookup { get; set; }
    }
}
