using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class GettingVehicleSupplier
    {
        [Key]
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string? SupplierCode { get; set; }
        public int? SupplierOriginCountry { get; set; }
        public int? SupplierCurrency { get; set; }
        public DateTime? SupplierAgreementStartDate { get; set; }
        public DateTime? SupplierAgreementEndDate { get; set; }
        public string? SupplierAttachments { get; set; }
        public int? SupplierPaymentMethod { get; set; }
     
        public string? SupplierComments { get; set; }
        public bool? MainSupplier { get; set; }
        public int? SupplierParent { get; set; }
        public string? SupplierAccountNo { get; set; }
        public int? SupplierGroup { get; set; }
        public int? SupplierCompany { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        public int? EnterUser { get; set; }
        public DateTime? EnterDate { get; set; }
        public TimeSpan? EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
    }
}
