using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.ItemsBundels
{
    public class GetBundlesModel
    {
        public string? BundleCode { get; set; }
        public int? BundleItemID { get; set; }
        public string? BundleName { get; set; }
        public decimal? BundlePriceAfterDiscount { get; set; }
        public DateTime? BundleCreationDate { get; set; }
        public int? Status { get; set; }
        public int Sort { get; set; }
    }
}
