using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.Location
{
    public class EditLocationsModel
    {
        [Required]
        public int LocationId { get; set; }
        public string LocationCode { get; set; }
        [SwaggerSchema("Lookup type id 12")]
        [Required]
        public int LocationWarehouse { get; set; }
        [SwaggerSchema("Lookup type id 13")]
        [Required]
        public int LocationWHZone { get; set; }
        [SwaggerSchema("Lookup type id 14")]
        [Required]
        public int LocationWHShelf { get; set; }
        [SwaggerSchema("Lookup type id 15")]
        [Required]
        public int LocationWHSection { get; set; }
        [Required]
        public string LocationRow { get; set; }
        [Required]
        public string LocationColumn { get; set; }
        [SwaggerSchema("Lookup type id 21")]
        [Required]
        public int? LocationType { get; set; }
        [Required]
        public string? Location { get; set; }
        public decimal? LocationLength { get; set; }
        public decimal? LocationWidth { get; set; }
        public decimal? LocationHeight { get; set; }

        public List<IFormFile>? LocationQR { get; set; }
        //[SwaggerSchema("Lookup type id 6")]
        //[Description("")]
        //public int? LocationGroup { get; set; }
        [SwaggerSchema("Lookup type id 1")]
        [Required]
        public int Status { get; set; }
    }
    public class EditLocationsModelV2
    {
        [Required]
        public int LocationId { get; set; }
        [Required]
        public string LocationCode { get; set; }
        [SwaggerSchema("API:GetWareHouse")]
        [Required]
        public int LocationWarehouse { get; set; }
        [SwaggerSchema("API:GetSubWareHouse")]

        public int? LocationSubWarehouse { get; set; }
        [SwaggerSchema("API:GetZone")]
        [Required]
        public int LocationWHZone { get; set; }
        [SwaggerSchema("API:GetShelf")]
        [Required]
        public int LocationWHShelf { get; set; }
        [SwaggerSchema("API:GetSection")]
        [Required]
        public int LocationWHSection { get; set; }
        [Required]
        public string LocationRow { get; set; }
        [Required]
        public string LocationColumn { get; set; }
        [SwaggerSchema("Lookup type id 21")]
        [Required]
        public int LocationType { get; set; }
        [Required]
        public string? Location { get; set; }
        public decimal LocationLength { get; set; }
        public decimal LocationWidth { get; set; }
        public decimal LocationHeight { get; set; }

        public List<IFormFile>? LocationQR { get; set; }
        //[SwaggerSchema("Lookup type id 6")]
        //[Description("")]
        //public int? LocationGroup { get; set; }
        [SwaggerSchema("Lookup type id 1")]

        [Required]
        public int Status { get; set; }
    }
}
