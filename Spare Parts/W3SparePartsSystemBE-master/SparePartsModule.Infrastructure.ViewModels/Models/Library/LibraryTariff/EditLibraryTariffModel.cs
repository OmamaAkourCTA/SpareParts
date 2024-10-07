﻿using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.LibraryTariff
{
    public class EditLibraryTariffModel
    {
        [Required]
        public int TariffID { get; set; }
        [Required]
        public string TariffCode { get; set; }
        [Required]
        [SwaggerSchema("value between 0 and 100")]
        [Description("")]
        public double TariffPer { get; set; }
        [SwaggerSchema("lookup type id 1")]
        [Description("")]
        public int Status { get; set; }
    }
}
