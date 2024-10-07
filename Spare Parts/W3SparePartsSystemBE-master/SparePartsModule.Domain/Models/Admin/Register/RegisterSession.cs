using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Domain.Models.Admin;

namespace SparePartsModule.Domain.Models.RegisterSession
{
    public class RegisterSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int RegisterSessionId { get; set; }
        [ForeignKey(nameof(Register))]
        public int RegisterId { get; set; }
        public decimal InitialFund { get; set; }
        public int Status { get; set; }

        public string? RegisterIPAddress { get; set; }
        public string? Notes { get; set; }
        public int? BranchId { get; set; }
        [MaxLength(100)]
        public string? City { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiyedBy { get; set; }
        public DateTime? ModifiyedAt { get; set; }
        // public int? ClosdeBy { get; set; }
        //public DateTime? ClosedAt { get; set; }
        public bool? CantClose { get; set; }
        public Register Register { get; set; }

    }
}
