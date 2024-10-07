using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SparePartsModule.Domain.Models.MarkaziaMaster;
using SparePartsModule.Domain.Models;

namespace SparePartsModule.Domain.Context
{
    public class MarkaziaMasterContext : DbContext
    {
        public readonly string StringConnection = "";
        private readonly IConfiguration _configuration;
        public MarkaziaMasterContext()
        {
        }
        public MarkaziaMasterContext(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public MarkaziaMasterContext(DbContextOptions<MarkaziaMasterContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public virtual DbSet<MasterLookup> MasterLookup { get; set; }

        public virtual DbSet<MasterLookupType> MasterLookupType { get; set; }
        public virtual DbSet<MasterCurrencyRate> MasterCurrencyRate { get; set; }

        public virtual DbSet<MasterApproval> MasterApproval { get; set; }
        public virtual DbSet<MasterApprovalsEmailsManagement> MasterApprovalsEmailsManagement { get; set; }
        public virtual DbSet<MarkaziaMaster.Domain.Models.MarkaziaMaster.User> Users { get; set; }


    }
}
