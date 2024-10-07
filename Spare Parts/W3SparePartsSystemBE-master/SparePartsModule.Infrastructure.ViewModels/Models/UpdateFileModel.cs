using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SparePartsModule.Infrastructure.ViewModels.Models
{
    public class UpdateFileModel
    {
        public string? Folder { get; set; }
        public List<IFormFile>? File { get; set; }
    }
    public class UpdateFileModel2
    {
        [Required]
        public IFormFile File { get; set; }

        [DefaultValue(true)]
        public bool Save { get; set; } = true;
    }
}
