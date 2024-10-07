using System.ComponentModel.DataAnnotations;

namespace SparePartsModule.Infrastructure.ViewModels.Models.ListNames
{
    public class GetListNameSpecsValuesModel
    {
        [Required]
        public int ListNameID { get; set; }
        public int? GeneralSpecsCategory { get; set; }
        public int? Status { get; set; }
      
    }
}
