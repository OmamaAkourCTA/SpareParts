using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Domain.Models.Admin;

namespace SparePartsModule.Domain.Models
{
    public class RegisterCurrency
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [ForeignKey("Register")]
        public int RegisterId { get; set; }
        public int CurrencyId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public int? ModifiyedBy { get; set; }
        public DateTime? ModifiyedAt { get; set; }
        public int Status { get; set; } = 2001;
        public Register Register { get; set; }
    }
}
