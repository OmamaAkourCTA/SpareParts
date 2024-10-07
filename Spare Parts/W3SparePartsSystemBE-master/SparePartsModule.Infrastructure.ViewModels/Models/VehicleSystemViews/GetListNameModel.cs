using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.ListNames
{
    public class GetListNameModel
    {
        public string? Search { get; set; }
        public string? BrandId { get; set; }
        public string? ModelId { get; set; }
        public string? ModelCode { get; set; }
        public string? SupplierId { get; set; }
        public string? SegmentId { get; set; }
        public bool? MarkaziaList { get; set; }
        public string? ModelYear { get; set; }
        public string? ProductionCode { get; set; }
        public int? GeneralSpecItemID { get; set; }
        public int? Status { get; set; }
        public int Sort { get; set; }
    }
}
