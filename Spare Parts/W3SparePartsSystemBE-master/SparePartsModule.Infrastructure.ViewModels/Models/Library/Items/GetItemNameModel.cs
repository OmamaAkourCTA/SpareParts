using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items
{
    public class GetItemNameModel
    {
        public string? Search { get; set; }
        public string? NameEn { get; set; }
        public string? Code { get; set; }
        public int? Status { get; set; }
        public int Sort { get; set; }
    }
}
