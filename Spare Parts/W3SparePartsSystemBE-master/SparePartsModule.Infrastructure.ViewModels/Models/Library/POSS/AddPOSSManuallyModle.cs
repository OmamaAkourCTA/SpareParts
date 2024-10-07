using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.POSS
{
    public class AddPOSSManuallyModle
    {
   
        [Required]
        public string POSSNo { get; set; }
        [SwaggerSchema("Api:Getsuppliers")]
        [Required]
        public int POSSSupplierID { get; set; }
        public string? POSSComments { get; set; }
       public List<POOSItem> POOSItems { get; set; }  = new List<POOSItem>();

    }
    public class POOSItem
    {
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
        public string? Order_No { get; set; }
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
    }
}
