using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Lookups
{
    public class GetLookupsModel
    {
        [Required]
        public int LookupTypeId { get; set; }
        public int? ParentId { get; set; }
        public int? Status { get; set; }
        public string? Name { get; set; }
    }
    public class GetLookupsModel2
    {

        public int? LookupTypeId { get; set; }
        public string? Search { get; set; }
        public int? ParentId { get; set; }
        public int? Status { get; set; }
        public bool? LookupStatic { get; set; }
        public bool? LookupDefault { get; set; }
        public bool? HasParent { get; set; }
        [SwaggerParameter("Default added recently 1 \r\n2/3 lookup name \r\n- 4/5 lookup type id \r\n- 6/7 lookup value \r\n- 8/9 lookup system \r\n- 10/11 lookup static \r\n- 12/13 status ")]
        public int? Sort { get; set; }
    }
}
