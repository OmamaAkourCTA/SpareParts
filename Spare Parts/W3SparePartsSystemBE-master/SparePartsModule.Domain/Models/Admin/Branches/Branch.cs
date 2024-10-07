using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Branches
{
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }
        [MaxLength(30)]
        [MinLength(6,ErrorMessage ="Min 6 char")]
        public string BranchName { get; set; }
        [MaxLength(20)]
        public string? Phone { get; set; }
        [MaxLength(50)]
        public string? Email { get; set; }
        [MaxLength(50)]
        public string? Country { get; set; }
        [MaxLength(50)]
        public string? City { get; set; }
        [MaxLength(100)]
        public string? BranchArea { get; set; }
        [MaxLength(500)]
        public string? Address { get; set; }
        [MaxLength(50)]
        public string? Longitude { get; set; }
        [MaxLength(50)]
        public string? latitude { get; set; }
        public int? AdminUser { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiyedBy { get; set; }
        public DateTime? ModifiyedAt { get; set; }
        public int Status { get; set; }
        public bool IsDefaultBranch { get; set; }
        public List<BranchType> BranchTypes { get; set; } = new List<BranchType>();
        public List<BranchWorkingHour> BranchWorkingHours { get; set; } = new List<BranchWorkingHour>();




    }
}
