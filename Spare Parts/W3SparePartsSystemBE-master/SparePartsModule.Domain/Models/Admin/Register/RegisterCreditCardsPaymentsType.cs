using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Domain.Models.Admin;

namespace SparePartsModule.Domain.Models
{
    public class RegisterCreditCardsPaymentsType
    {
        [Key]
        public int RecordId { get; set; }
        [ForeignKey(nameof(Register))]
        public int RegisterId { get; set; }
        public int CardTypeId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? ModifiedBy { get; set; }
        public Register Register { get; set; }
    }
}
