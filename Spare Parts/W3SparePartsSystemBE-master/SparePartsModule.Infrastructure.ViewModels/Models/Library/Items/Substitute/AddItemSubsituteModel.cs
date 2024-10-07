using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items.Substitute
{
    public class AddItemSubsituteModel
    {
        [SwaggerSchema("Api:GetItems")]
        public int ItemID { get; set; }

        [SwaggerSchema("master lookup type id 1")]
        public int Status { get; set; }
        public List<SubsituteModel> Subsitutes { get; set; } = new List<SubsituteModel>();
    }
    public class SubsituteModel
    {
        [SwaggerSchema("Api:GetItems")]
        public int? SubstituteID { get; set; }
        [SwaggerSchema("lookup type id 22")]
        public int? SubstituteType { get; set; }
        public int SubstituteCode { get; set; }
        public string? SubstituteNo { get; set; }
        public string? SubstituteBarcode { get; set; }
    }
}
