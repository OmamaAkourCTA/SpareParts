using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items
{
    public class UploadSupplierMasterItemsModle
    {
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
    public class UploadSupplierMasterItemsModle2
    {

        [Required]
        public IFormFile File { get; set; }
        [DefaultValue(true)]
       
        public bool Save { get; set; } = true;
    }
}
