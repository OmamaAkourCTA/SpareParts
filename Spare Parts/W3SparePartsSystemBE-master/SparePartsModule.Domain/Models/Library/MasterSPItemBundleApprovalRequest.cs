using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class MasterSPItemBundleApprovalRequest
    {
        [Key]
        public int BundleApprovalRequestID { get; set; }
        public int BundleID { get; set; }
        public int? BundleApprovalSentTo { get; set; }
        public string? BundleApprovalEmailAddress { get; set; }
        public string? BundleApprovalEmailContent { get; set; }
        public int? BundleApprovalRevisionReason { get; set; }
        public string? BundleApprovalRevisionReasonDetails { get; set; }
        public int? BundleApprovalRejectionReason { get; set; }
        public string? BundleApprovalRejectionReasonDetails { get; set; }
        public string? Remarks { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public int? CancelDate { get; set; }
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
    }
}
