using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class Lookup
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LookupId { get; set; }

        public int? LookupTypeId { get; set; }

        public int? ParentId { get; set; }
        public string? LookupTextColor { get; set; }

        public string? LookupBGColor { get; set; }
        public string? Description { get; set; }
        public string? LookupImage { get; set; }

        [ForeignKey("StatusLookup")]
        public int? Status { get; set; }

        public DateTime? CreatedAt { get; set; }
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }

        #region Navigation Properties 
        public User? CreatedByUser { get; set; }
        public Lookup? StatusLookup { get; set; }
        public List<LookupTranslation> Translations { get; set; }   = new List<LookupTranslation>();
        #endregion
    }
}
