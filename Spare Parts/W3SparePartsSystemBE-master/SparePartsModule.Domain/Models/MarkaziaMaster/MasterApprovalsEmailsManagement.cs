using System.ComponentModel.DataAnnotations;

namespace SparePartsModule.Domain.Models
{
    public class MasterApprovalsEmailsManagement
    {
        [Key]
        public int EmailManagementID { get; set; }
        public int? PortalID { get; set; }
        public int? PortalModuleID { get; set; }
        public int? RequestTypeID { get; set; }
        public int? BusinessAreaID { get; set; }
        // public int? UserID { get; set; }
        public string? ToUserID { get; set; }
        public string? CCUserID { get; set; }
        public string? BccUserID { get; set; }
        public string? Remarks { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int? EnterUser { get; set; }
        public DateTime? EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
    }
}
