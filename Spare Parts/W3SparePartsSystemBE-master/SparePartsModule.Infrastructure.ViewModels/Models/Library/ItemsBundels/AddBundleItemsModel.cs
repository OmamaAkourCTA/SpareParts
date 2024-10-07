using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.ItemsBundels
{
    public class AddBundleItemsModel
    {
        [Required]
        [SwaggerSchema("api:getbundels")]
        public int BundleID { get; set; }
      
        [Required]
        [SwaggerSchema("master lookup type id 1")]
        public int Status { get; set; }
        public List<Item>  Items { get; set; }
    }
    public class Item
    {
        [Required]
        [SwaggerSchema("api:getitems")]
        public int ItemID { get; set; }
       
        public decimal? ItemCost { get; set; }
        [Required]
        public decimal ItemPrice { get; set; }
        [Required]
        public int ItemQty { get; set; }
    }
}
