using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class LookupType
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int LookupTypeId { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public int? ModifiedBy { get; set; }
        public string? Description { get; set; }

    }
}
