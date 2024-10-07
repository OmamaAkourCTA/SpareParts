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
    public class RegisterIpAddress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RegisterIPAddressRecordId { get; set; }
        [ForeignKey("Register")]
        public int RegisterId { get; set; }
        public string IpAddress { get; set; }//user
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiyedBy { get; set; }
        public DateTime? ModifiyedAt { get; set; }
        public DateTime? LastSessionStart { get; set; }
        public DateTime? LastSessionEnd { get; set; }

        public Register Register { get; set; }
    }
}
