using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.Library
{
    public class GettingSPOrder1
    {
        [Key]
        public int OrderID { get; set; }
        public int OrderSeq { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderSupplierID { get; set; }
        public int OrderType { get; set; }
        public int OrderMethod { get; set; }
        public int OrderFreight { get; set; }
        [Precision(18,3)]
        public decimal OrderAmount { get; set; }
        [Precision(18, 3)]
        public decimal? OrderDisPer { get; set; }
    
        [Precision(18, 3)]
        public decimal OrderDisAmount { get; set; }
        [Precision(18, 3)]
        public decimal OrderNetAmount { get; set; }
        [Precision(18, 3)]
        public decimal? OrderTaxPer { get; set; }

        [Precision(18, 3)]
        public decimal OrderTaxAmount { get; set; }

        [Precision(18, 3)]
        public decimal OrderTotalAmountCur { get; set; }
        public int? OrderCurrency { get; set; }
        public DateTime OrderCurrencyExchangeDate { get; set; }
        [Precision(18, 3)]
        public decimal OrderCurrencyExchangeRate { get; set; }
        [Precision(18, 3)]
        public decimal OrderTotalAmountJOD { get; set; }
        public string? OrderComments { get; set; }
        public int? OrderApproval { get; set; }
        public int OrderGroup { get; set; }
       
        public int? OrderSource { get; set; }
        public string? SourceSequence { get; set; }
        public int? OrderBusinessArea { get; set; }
        public int Status { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelDate { get; set; }
        [ForeignKey(nameof(User))]
        public int EnterUser { get; set; }
        public DateTime EnterDate { get; set; }
        public TimeSpan EnterTime { get; set; }
        public int? ModUser { get; set; }
        public DateTime? ModDate { get; set; }
        public TimeSpan? ModTime { get; set; }
        public User User { get; set; }
        public List<GettingSPOrder2> GettingSPOrder2 { get; set; }
    }
}
