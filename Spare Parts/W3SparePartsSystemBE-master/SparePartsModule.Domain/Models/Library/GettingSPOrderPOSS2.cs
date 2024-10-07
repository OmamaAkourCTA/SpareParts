using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class GettingSPOrderPOSS2
    {
        [Key]
        public int POSSLineID { get; set; }
        [ForeignKey(nameof(GettingSPOrderPOSS1))]
        public int POSSID { get; set; }
        public string? DataID { get; set; }
        public DateTime? ProcDate { get; set; }
        public int? Yusou { get; set; }
        public int? DIS_FD { get; set; }
        public DateTime? BO_ReleaseDate { get; set; }
        public string? FL_ID { get; set; }
        public int? Order_ID { get; set; }
        public int? Portion { get; set; }
        public int? BO_Instruction { get; set; }
        public int? BO_CD { get; set; }
        public string? RA_CD { get; set; }
        public string? Exchange_rate { get; set; }
        public string? Ordered_Part { get; set; }
        public int? Ordered_Qty { get; set; }
        public int? CancelledQty { get; set; }
        public string? Order_No { get; set; }
        [ForeignKey(nameof(MasterSPItem))]
        public int? Item_No { get; set; }
        public string? Part_No { get; set; }
        public string? Part_Name { get; set; }
        public string? TRF_CD { get; set; }
        public int? Accepted_Qty { get; set; }
        public double? FOB_UnitPrice { get; set; }
        public string? POSS_Dummy { get; set; }
        //public int? OrderID { get; set; }
        //public int? OrderLineID { get; set; }
        public int? ActionOrderType { get; set; }
        public int? ActionOrderStatus { get; set; }
        public int? ActionItemType { get; set; }
        public int? ActionItemStatus { get; set; }
        public int? ActionManualType { get; set; }
        public int? ActionManualStatus { get; set; }
        public string? POSSLineComments { get; set; }
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
        public GettingSPOrderPOSS1 GettingSPOrderPOSS1 { get; set; }
    }
}
