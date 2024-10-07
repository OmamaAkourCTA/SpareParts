using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.ItemsBundels
{
    public class HandleBundleApprovalRequestModel
    {
        [Required]
        public int BundleID { get; set; }
        [Required]
        [SwaggerSchema("lookup id for approved or rejected or need revise ")]
        public int Status { get; set; }
        [SwaggerSchema("lookup type id 24")]
        public int? RejectionResonID { get; set; }
        public string? RrejectionReasonDetails { get; set; }
        [SwaggerSchema("lookup type id 25")]
        public int? ReviseReasonID { get; set; }
        public string? ReviseReasonDetails { get; set; }
        public string? Remarks { get; set; }
    
    }
}
