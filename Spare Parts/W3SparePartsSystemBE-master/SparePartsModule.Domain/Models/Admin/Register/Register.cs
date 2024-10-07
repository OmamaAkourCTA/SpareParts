using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SparePartsModule.Domain.Models.Branches;

namespace SparePartsModule.Domain.Models.Admin
{
    public class Register
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [MaxLength(30)]
        public string RegistersName { get; set; }
        // public string RegistersNumber { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [ForeignKey(nameof(Branch))]
        public int BranchId { get; set; }

        public bool Cash { get; set; }
        //  public int CurrencyId { get; set; }
        public bool OnAccount { get; set; }
        public bool Cheque { get; set; }
        public bool? OnCustomerAccount { get; set; }
        public bool Cards { get; set; }
        public bool Visa { get; set; }
        public bool Mastercard { get; set; }
        public bool AmericanExpress { get; set; }
        public int? NumberOfSessionsPerDay { get; set; }
        //   public int? NumberOfSessionsPerWeek { get; set; }
        public TimeSpan? AllowedSessionTimeFrom { get; set; }
        public TimeSpan? AllowedSessionTimeTo { get; set; }
        public bool? AllowOpenWithoutSettlement { get; set; }
        //  public int? NumberOfOpeningWithoutSettlementPerDay { get; set; }
        public int? NumberOfOpeningWithoutSettlementPerWeek { get; set; }
        public int CashDepositTypeId { get; set; }
        public int Status { get; set; }
        public bool? CollectForAllBranches { get; set; }
        public bool? CollectForOwnBranch { get; set; }
        public decimal? PettyCashLimit { get; set; }
        public bool DepositCash { get; set; }
        public bool DepositCheque { get; set; }
        public decimal AllProvidedPettyCash { get; set; }
        public decimal AllExpenses { get; set; }
        public decimal CurrentExpenses { get; set; }
        public decimal RemainingPettyCash { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiyedBy { get; set; }
        public DateTime? ModifiyedAt { get; set; }
        public Branch Branch { get; set; }
        public List<RegisterEmployee> RegisterEmployees { get; set; } = new List<RegisterEmployee>();

        public List<RegisterCurrency> RegisterCurrencies { get; set; } = new List<RegisterCurrency>();
        public List<RegisterIpAddress> RegisterIpAddress { get; set; } = new List<RegisterIpAddress>();
        public List<RegisterCollectionOrder> CollectionOrders { get; set; } = new List<RegisterCollectionOrder>();
        public List<RegisterBank> RegisterBanks { get; set; } = new List<RegisterBank>();
        public List<RegisterCreditCardsPaymentsType> RegisterCreditCardsPaymentsTypes { get; set; } = new List<RegisterCreditCardsPaymentsType>();
        public List<RegisterPaymentsTypesForOrder> RegisterPaymentsTypesForOrders { get; set; } = new List<RegisterPaymentsTypesForOrder>();


    }
}
