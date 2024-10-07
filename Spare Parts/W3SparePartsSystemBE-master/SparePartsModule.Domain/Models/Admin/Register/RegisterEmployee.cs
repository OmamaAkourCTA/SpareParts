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
    public class RegisterEmployee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [ForeignKey("Register")]
        public int RegisterId { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }//user
        public int Userid { get; set; }//user
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiyedBy { get; set; }
        public DateTime? ModifiyedAt { get; set; }
        public Register Register { get; set; }
        public Role Role { get; set; }

    }
}
