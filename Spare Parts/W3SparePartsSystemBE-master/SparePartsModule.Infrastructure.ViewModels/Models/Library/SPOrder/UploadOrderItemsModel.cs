using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.SPOrder
{
    public class UploadOrderItemsModel
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
