using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models
{
    public class AddLookupTypeModel
    {
        [Required]
        public string LookupTypeName { get; set; }
        public string? Comments { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
