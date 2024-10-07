using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items.Substitute
{
    public class GetItemSubstitutesModel
    {
        public int? ItemId { get; set; }
        public int? SubstituteID { get; set; }
        public int? SubstituteCode { get; set; }
        public int? SubstituteType { get; set; }

        public int? Status { get; set; }
        public int? Sort { get; set; }

        
    }
}
