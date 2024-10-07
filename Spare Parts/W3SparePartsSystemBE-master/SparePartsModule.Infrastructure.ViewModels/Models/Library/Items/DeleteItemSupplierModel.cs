using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items
{
    public class DeleteItemSupplierModel
    {
        [Required]
        public int ItemSupplierID { get; set; }

    }
}
