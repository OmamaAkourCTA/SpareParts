using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Location
{
    public class GetLocationsModel
    {
        public string? Search { get; set; }
        public string? Code { get; set; }
        public int Sort { get; set; }
        [SwaggerParameter("Lookup type id 12")]
       
        public string? LocationWarehouse { get; set; }
        [SwaggerParameter("Lookup type id 13")]
        public int? LocationWHZone { get; set; }
        [SwaggerParameter("Lookup type id 14")]
        public int? LocationWHShelf { get; set; }
        [SwaggerParameter("Lookup type id 15")]
        public int? LocationWHSection { get; set; }
        [SwaggerParameter("Lookup type id 21")]
        public string? LocationType { get; set; }

    }
    public class GetLocationsModelV2
    {
        public string? Search { get; set; }
        public string? Code { get; set; }
        public int Sort { get; set; }
        [SwaggerParameter("API:GetwareHouse")]
        public string? LocationWarehouse { get; set; }
        [SwaggerParameter("API:GetwareHouse")]
        public string? LocationSubWarehouse { get; set; }
        [SwaggerParameter("API:GetZone")]
        public int? LocationWHZone { get; set; }
        [SwaggerParameter("API:GetShelf")]
        public int? LocationWHShelf { get; set; }
        [SwaggerParameter("API:GetSection")]
        public int? LocationWHSection { get; set; }
        [SwaggerParameter("Lookup type id 21")]
        public string? LocationType { get; set; }


    }
}
