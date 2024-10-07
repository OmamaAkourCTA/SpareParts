using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Models.Library.SPOrder
{
    public class ConfirmOrderModel
    {
        public int OrderId { get; set; }
        public List<ConfirmOrderLinesModel> OrderLines { get; set; }

    }
    public class ConfirmOrderLinesModel
    {
        public int OrderLineId { get; set; }
        public decimal ConfirmedFob { get; set; }
        public int ConfirmedQty { get; set; }
    }
}
