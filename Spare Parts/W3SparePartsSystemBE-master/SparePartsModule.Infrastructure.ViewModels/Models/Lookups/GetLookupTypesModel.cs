using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models
{
    public class GetLookupTypesModel
    {

        public string? Name { get; set; }

        public int? Status { get; set; }
        [SwaggerParameter("default lookup type id asc - \r\n           2/3 lookup type id asc / desc \r\n           4/5 lookup name asc / desc \r\n           6/7 created by \r\n           8/9 created at \r\n           10/11 modified by\r\n            12 / 13 modified at  \r\n           14/15 status asc / desc")]
        public int Sort { get; set; }
    }
}
