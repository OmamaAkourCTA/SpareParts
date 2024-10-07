using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.MarkaziaMaster
{
    public class MasterApproval
    {
        [Key]
        public int RequestID { get; set; }
        public DateTime RequestDate { get; set; }
        public TimeSpan RequestTime { get; set; }
        public int? RequestPortal { get; set; }
        public int? RequestModule { get; set; }
        public int? RequestType { get; set; }
        public int RequestRecord { get; set; }
       
        public string? EmailAddress { get; set; }
  
        public string? EmailAddressCC { get; set; }
    
        public string? EmailAddressBcc { get; set; }
        public string? EmailContent { get; set; }


    
        public int ApprovalStatus { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
  
        public int? EnterUser { get; set; }
        public DateTime? EnterDate { get; set; }
        public TimeSpan? EnterTime { get; set; }
     
        public string? Attachments { get; set; }


    }
}
