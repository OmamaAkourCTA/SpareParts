using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class RegisterSessionsForceClose
    {
        [Key]
        public int ForceCloseRegisterId { get; set; }
        public int RegisterID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
