using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.POSS
{
    public class GetPOSSModel
    {
        public string? Search { get; set; }
        public string? SupplierId { get; set; }
        public string? SupplierItemNo { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        [SwaggerSchema("1 poss creation date desc\r\n2/3 poss seq \r\n4/5 poss no\r\n6/7 poss upload date \r\n8/9 poss supplier")]
        public int Sort { get; set; }
    }
}
