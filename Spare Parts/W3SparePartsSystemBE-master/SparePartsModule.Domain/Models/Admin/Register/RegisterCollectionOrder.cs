using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePartsModule.Domain.Models.Admin;

namespace SparePartsModule.Domain.Models
{
    public class RegisterCollectionOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [ForeignKey("Register")]
        public int RegisterId { get; set; }
        public int OrderId { get; set; }
        public Register Register { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public int? ModifiyedBy { get; set; }
        public DateTime? ModifiyedAt { get; set; }
        public int Status { get; set; } = 2001;
    }
}
