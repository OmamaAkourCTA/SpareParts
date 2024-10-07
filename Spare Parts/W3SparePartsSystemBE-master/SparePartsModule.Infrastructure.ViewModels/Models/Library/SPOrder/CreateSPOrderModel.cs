using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.SPOrder
{
    public class CreateSPOrderModel
    {
         




        [SwaggerSchema("OrderType:lookup type id 27")]
        public int OrderType { get; set; }
        [SwaggerSchema("OrderMethod:lookup type id 28")]
        public int OrderMethod { get; set; }
        [SwaggerSchema("Api: getsuppliers")]
        public int OrderSupplierId { get; set; }
        [SwaggerSchema("currency lookup id")]
        public int CurrencyLookup { get; set; }
        public decimal OrderCurrencyRate { get; set; }
        public decimal? DiscountPercentage { get; set; }
   
        public decimal DiscountAmount{ get; set; }
        public decimal NetAmount { get; set; }
        public decimal? TaxPercentage { get; set; }
    
        public decimal TaxAmount{ get; set; }
        [SwaggerSchema("lookup type id 29")]
        public int OrderFrieght { get; set; }
        [SwaggerSchema("lookup type id 41")]
        public int? OrderSource { get; set; }
        public string? SourceSequence { get; set; }
        public string? Comments { get; set; }
        public int? OrderBusinessArea { get; set; }
        public List<CreateSPOrderItemsModel>ItemsModel { get; set; }= new List<CreateSPOrderItemsModel>();    


           
    }
}
