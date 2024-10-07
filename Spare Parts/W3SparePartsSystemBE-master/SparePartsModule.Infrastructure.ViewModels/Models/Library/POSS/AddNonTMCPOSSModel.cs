using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.POSS
{
    public class AddNonTMCPOSSModel
    {
        // public string? DataID { get; set; }
        public int POSSSupplierID { get; set; }
        public string? POSSComments { get; set; }

        public List<AddNonTMCPOSSItemModel> NonTMCPOSSItems { get; set; } = new List<AddNonTMCPOSSItemModel>();

    }
    public class UpdateNonTMCPOSSModel
    {
        // public string? DataID { get; set; }
        public int POSSID { get; set; }
        public int POSSSupplierID { get; set; }
        public string? POSSComments { get; set; }

        public List<AddNonTMCPOSSItemModel> NonTMCPOSSItems { get; set; } = new List<AddNonTMCPOSSItemModel>();

    }
    public class AddNonTMCPOSSItemModel
    {
        public DateTime? ProcDate { get; set; }
        public int? Order_ID { get; set; }
        public string? Ordered_Part { get; set; }
        public int? Ordered_Qty { get; set; }
        public string? Order_No { get; set; }
        public int? Item_No { get; set; }
        public string? Part_No { get; set; }
        public string? Part_Name { get; set; }
        public int? Accepted_Qty { get; set; }
        public int? CancelledQty { get; set; }
        public string? POSSLineComments { get; set; }
    }
}
