using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items
{
    public class AddItemNameModel
    {

        [SwaggerSchema("Lookup type id 3")]
        public int? ItemNameCode { get; set; }
        public string? ItemNameEn { get; set; }
        public string? ItemNameAr { get; set; }
        public string? ItemNameDesc { get; set; }
        [SwaggerSchema("Lookup type id 16")]
        [Description("")]
        public int? ItemNameType { get; set; }
        public string? ItemNameComments { get; set; }
        //[SwaggerSchema("Lookup type id 6")]
        //[Description("")]
        //public int? ItemNameGroup { get; set; }
        [SwaggerSchema("Lookup type id 1")]
        [Description("")]
        public int Status { get; set; }
    }
}
