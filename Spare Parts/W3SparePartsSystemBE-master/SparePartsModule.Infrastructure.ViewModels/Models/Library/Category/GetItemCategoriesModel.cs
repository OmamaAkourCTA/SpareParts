using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Category
{
    public class GetItemCategoriesModel
    {
        public string? Search { get; set; }
        public string? Code { get; set; }

        [SwaggerParameter("Lookup type id 7")]  
        public string? ItemCategoryType { get; set; }
        [SwaggerParameter("Lookup type id 1")]
       
        public int? Status { get; set; }
        public int Sort { get; set; }
    }
}
