using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Domain.Models
{
    public class BankAccount
    {
        [Key]
        public int AccountId { get; set; }
        [ForeignKey(nameof(Bank))]
        public int BankId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public int SageBankAccountId { get; set; }
        public int AccountType { get; set; }
        public int CurrencyId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Used { get; set; }
        public Bank Bank { get; set; }
    }
}
