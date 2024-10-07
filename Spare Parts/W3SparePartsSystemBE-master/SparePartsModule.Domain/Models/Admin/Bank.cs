using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class Bank
    {
        [Key]
	    public int BankId { get; set; }
        public string BankNameArabic { get; set; }
        public string BankNameEnglish { get; set; }
        public string BankLogo { get; set; }
        public int NumberofAccounts { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
