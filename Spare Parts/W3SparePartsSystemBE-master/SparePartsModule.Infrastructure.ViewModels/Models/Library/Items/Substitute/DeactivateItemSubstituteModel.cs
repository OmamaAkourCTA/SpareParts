using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items.Substitute
{
    public class DeactivateItemSubstituteModel
    {

        [Required]
        public int ItemSubstituteID { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
