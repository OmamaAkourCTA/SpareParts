using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models
{
    public class AddLookupModel
    {
        [Required]
        public int LookupTypeID { get; set; }
        [Required]
        public string LookupName { get; set; }

        [Required]
        public int Status { get; set; }
        public string? LookupDesc { get; set; }
        public double? LookupValue { get; set; }
        public string? LookupSystem { get; set; }
        public bool? LookupStatic { get; set; }
        public bool? LookupDefault { get; set; }
        public int? LookupParent { get; set; }
        public int? LookupAction { get; set; }
        public int? LookupIntegration { get; set; }
        public string? LookupComments { get; set; }
        public List<IFormFile>? LookupImage { get; set; }
        public string? LookupTextColor { get; set; }
        public string? LookupBGColor { get; set; }

    }
}
