using SparePartsModule.Infrastructure.ViewModels.Models.Library.ItemsBundels;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.SPOrder
{
    public class CreateSPOrderItemsModel
    {
        public int ItemId { get; set; }
        public decimal Qty { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal ItemAmount { get; set; }
        public decimal ItemDiscountPercentage { get; set; }
        public int? OrderItemTax { get; set; }
        public int? OrderItemDis { get; set; }
        public decimal ItemDiscountAmount { get; set; }
        public decimal ItemNetAmount { get; set; }
        public decimal ItemTaxPercentage { get; set; }
      
        public decimal ItemTaxAmount { get; set; }
        public decimal ItemTotal { get; set; }
        public int? SupplierFlag { get; set; }
        public int? OrderItemOrderFlag { get; set; }

        //public int? QtyOrderMax { get; set; }
        //   public int? QtyOrderMin { get; set; }
        // public int Status { get; set; }

    }

}
