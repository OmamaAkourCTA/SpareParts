using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models
{
    public class GetModelCodeModel
    {
        public string?  Search { get; set; }
        public int? ModelId { get; set; }
        public int? Status { get; set; }
        public int Sort { get; set; }
    }
}
