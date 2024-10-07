using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models
{
    public class GetBrandModel
    {
        [SwaggerParameter("Search on brand,brand AR")]

        public string? Search { get; set; }
        [SwaggerParameter("Lookup type id 2")]
    
        public int? BrandOrigionCountry { get; set; }
        public int? Status { get; set; }
        public bool? MarkaziaBrand { get; set; }
        public int Sort { get; set; }
    }
}
