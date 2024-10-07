using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models
{
    public class GetModelModel
    {
        public string? Search { get; set; }
        public int? supplierID { get; set; }
        public int? ModelId { get; set; }
        [SwaggerParameter("API: getbrands")]
        public int? BrandId { get; set; }
        [SwaggerSchema("Lookup type id 5")]
        public int? ModelSegment { get; set; }
        [SwaggerSchema("Lookup type id 6")]
        public int? ModelCustomType { get; set; }
        [SwaggerSchema("Lookup type id 4")]
        public int? ModelType { get; set; }
        public int? EquivalentMarkaziaModelID { get; set; }
        [SwaggerSchema("Lookup type id 1")]
        public int? Status { get; set; }
        [SwaggerParameter("sort 1 default lately added \r\n- 2/3 model name en \r\n- 4/5 segment \r\n- 6/7 brand \r\n- 8 / 9 supplier \r\n- 10 / 11 markazia model \r\n- 12 / 13 markazia equilavent \r\n - 14 / 15 active / inactive")]
        public int Sort { get; set; }
    }
}
