using SparePartsModule.Infrastructure.ViewModels.Dtos.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Items
{
    public class AddItemVehicleModelCodeModel
    {

        [Required]
        public int ModelCodeID { get; set; }
        [Required]
        public int ItemID { get; set; }
        //   [SwaggerSchema("year between 1900,2100")]
        //  public int FromYear  { get; set; }
        //  [SwaggerSchema("year between 1900,2100")]
        //public int ToYear { get; set; }
        // [SwaggerSchema("lookup type id=2")]
        [Required]
        public int? ListNameID { get; set; }
        public int ModelCode  { get; set; }
        public string? ModelCodeSP { get; set; }
        public string? ModelCodeService { get; set; }
        [Required]
        public int Status { get; set; }


    }
}
