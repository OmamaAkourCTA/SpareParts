using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class RegisterBank
    {
        [Key]
        public int RegisterBanksId { get; set; }
        [ForeignKey(nameof(RegisterBank))]
        public int RegisterId { get; set; }
        public int SageBankId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public int Status { get; set; }
       // public Register Register { get; set; }
    }
}
