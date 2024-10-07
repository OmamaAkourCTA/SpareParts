using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class LookupTranslation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(Lookup))]
        public int LookupId { get; set; }
        public int LanguageId { get; set; }
        [MaxLength(200)]
        public string LookupName { get; set; }
        public Lookup Lookup { get; set; }
    }
}
