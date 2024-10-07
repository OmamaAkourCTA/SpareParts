using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SparePartsModule.Domain.Models;
using SparePartsModule.Domain.Models.Admin;
using SparePartsModule.Domain.Models.Branches;
using SparePartsModule.Domain.Models.Menu;
using SparePartsModule.Domain.Models.Permissions;
using SparePartsModule.Domain.Models.RegisterSession;

namespace SparePartsModule.Domain.Context
{
    public class DX_AdmimistrationContext: DbContext
    {
        public readonly string StringConnection = "";
        private readonly IConfiguration _configuration;
        public DX_AdmimistrationContext()
        {
        }
        public DX_AdmimistrationContext(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public DX_AdmimistrationContext(DbContextOptions<DX_AdmimistrationContext> options, IConfiguration configuration)
            : base(options)
        {


            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Lookup> Lookups { get; set; }
        public virtual DbSet<LookupType> LookupTypes { get; set; }
        public virtual DbSet<LookupTypeTranslation> LookupTypeTranslations { get; set; }
        public virtual DbSet<LookupTranslation> LookupTranslations { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<BranchType> BranchTypes { get; set; }
        public virtual DbSet<BranchWorkingHour> BranchWorkingHours { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<PermissionCategory> PermissionCategories { get; set; }
        public virtual DbSet<PermissionSubCategory> PermissionSubCategories { get; set; }
        public virtual DbSet<PermissionItem> PermissionItems { get; set; }
        public virtual DbSet<PermissionItemDetail> PermissionItemDetails { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<UserWorkingHour> UserWorkingHours { get; set; }

        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<Register> Registers { get; set; }

        public virtual DbSet<RegisterEmployee> RegisterEmployees { get; set; }
        public virtual DbSet<RegisterCurrency> RegisterCurrencies { get; set; }

        public virtual DbSet<RegisterSession> RegisterSessions { get; set; }
        //public virtual DbSet<RegisterIpAddress> RegisterIpAddresss { get; set; }

        //public virtual DbSet<UsersResetPasswordsRequest> UsersResetPasswordsRequests { get; set; }
        //public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<MenuCategory> MenuCategories { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        //public virtual DbSet<RegisterBank> RegisterBanks { get; set; }
        //public virtual DbSet<OtherRevenue> OtherRevenues { get; set; }
        //public virtual DbSet<MainFundSageAccount> MainFundSageAccounts { get; set; }
        //public virtual DbSet<PettyCashExpense> PettyCashExpenses { get; set; }
        //public virtual DbSet<SparePartsMasterItem> SparePartsMasterItem { get; set; }
        //public virtual DbSet<OtherRevenueOrder> OtherRevenueOrders { get; set; }
        //public virtual DbSet<PN_Setup> PN_Setup { get; set; }
        public virtual DbSet<Bank> Banks{ get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        //public virtual DbSet<TerminalProvider> TerminalProviders { get; set; }



    }
}

