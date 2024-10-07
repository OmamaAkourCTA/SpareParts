using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.POSS
{
    public class UploadPOSSModle
    {
   
        [Required]
        public string POSSNo { get; set; }
        [SwaggerSchema("Api:Getsuppliers")]
        [Required]
        public int POSSSupplierID { get; set; }
        [Required]
        public IFormFile File { get; set; }
        public string? POSSComments { get; set; }

    }
}
