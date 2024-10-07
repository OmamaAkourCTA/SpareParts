using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Branches
{
    public class BranchType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public int BranchTypeId { get; set; }
        public Branch? Branch { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiyedBy { get; set; }
        public DateTime? ModifiyedAt { get; set; }
        public int Status { get; set; }

    }
}
