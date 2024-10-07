using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models.MarkaziaMaster
{
    public class MasterCurrencyRate
    {
        [Key]
        public string Currency { get; set; }
        public DateTime CurrencyDate { get; set; }

        [Precision(18,10)]
        public decimal CurrencyRate { get; set; }
        public DateTime ERPCurrencyDate { get; set; }
        public double OnlineCurrencyRate { get; set; }
        public DateTime OnlineCurrencyDate { get; set; }
    }
}
