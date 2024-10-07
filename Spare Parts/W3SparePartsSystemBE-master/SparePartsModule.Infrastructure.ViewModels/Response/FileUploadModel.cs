using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Response
{
    public class FileUploadModel
    {
        public string ReturnUrl { get; set; }
        public string Extension { get; set; }
        public bool IsSucess { get; set; }
    }
}
