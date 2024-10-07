using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.LibraryTariff
{
    public class GetLibraryTariffModel
    {
        public string? Search { get; set; }
        public string? Code { get; set; }
        public int Sort { get; set; }
        public int? Status { get; set; }
    }
}
