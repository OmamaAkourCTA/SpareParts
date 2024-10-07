using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SparePartsModule.Domain.Models;
using SparePartsModule.Domain.Models.Library;
using SparePartsModule.Domain.Models.Vehicles;
using SparePartsModule.Domain.Models;
using SparePartsModule.Domain.Models.MarkaziaMaster;

namespace SparePartsModule.Domain.Context
{
    public class SparePartsModuleContext : DbContext
    {
        public readonly string StringConnection = "";
        private readonly IConfiguration _configuration;
        public SparePartsModuleContext()
        {
        }
        public SparePartsModuleContext(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public SparePartsModuleContext(DbContextOptions<SparePartsModuleContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        
             public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<GettingSPSupplier> GettingSPSupplier { get; set; }
        public virtual DbSet<GettingSPSupplierDeliveryMethod> GettingSPSupplierDeliveryMethod { get; set; }
        public virtual DbSet<MasterSPLookup> MasterSPLookup { get; set; }
        public virtual DbSet<MasterSPGeneralItemCategory> MasterSPGeneralItemCategory { get; set; }
        public virtual DbSet<MasterSPGeneralTariff> MasterSPGeneralTariff { get; set; }
        //public virtual DbSet<MasterSPGeneralLocation> MasterSPGeneralLocations { get; set; }
        public virtual DbSet<MasterSPGeneralLocationsNew> MasterSPGeneralLocations { get; set; }
        public virtual DbSet<MasterSPGeneralItemName> MasterSPGeneralItemNames { get; set; }
        public virtual DbSet<MasterSPItem> MasterSPItem { get; set; }
        public virtual DbSet<MasterSPItemSubstitute> MasterSPItemSubstitute { get; set; }
        public virtual DbSet<MasterSPItemSupplier> MasterSPItemSupplier { get; set; }
        
        public virtual DbSet<MasterSPGeneralSupplierItem> MasterSPGeneralSupplierItems { get; set; }
        
        public virtual DbSet<MasterLookup> MasterLookup { get; set; }

        public virtual DbSet<MasterSPItemLocation> MasterSPItemLocation { get; set; }


        public virtual DbSet<MasterSPItemModelCode> MasterSPItemModelCode { get; set; }
        public virtual DbSet<MasterSPItemBundle1> MasterSPItemBundle1 { get; set; }
        public virtual DbSet<MasterSPItemBundle2> MasterSPItemBundle2 { get; set; }
        public virtual DbSet<MasterSPLookupType> MasterSPLookupType { get; set; }
        #region Vehilces
        public virtual DbSet<MasterVehicleModelCode> MasterVehicleModelCode { get; set; }
        public virtual DbSet<MasterVehicleBrand> MasterVehicleBrand { get; set; }
        public virtual DbSet<MasterVehicleModel> MasterVehicleModel { get; set; }
        public virtual DbSet<MasterVehilceList> MasterVehilceList { get; set; }
        public virtual DbSet<MasterVehicleGeneralSpec> MasterVehicleGeneralSpecs { get; set; }

        public virtual DbSet<MasterVehicleGeneralSpecsValue> MasterVehicleGeneralSpecsValues { get; set; }

        public virtual DbSet<MasterSPItemBundleApprovalRequest> MasterSPItemBundleApprovalRequest { get; set; }

        #endregion
        public virtual DbSet<GettingSPOrder1> GettingSPOrder1 { get; set; }
        public virtual DbSet<GettingSPOrder2> GettingSPOrder2 { get; set; }
        public virtual DbSet<MasterCurrencyRate> MasterCurrencyRate { get; set; }
        public virtual DbSet<GettingSPOrderHistory1> GettingSPOrderHistory1 { get; set; }

        public virtual DbSet<GettingSPOrderHistory2> GettingSPOrderHistory2 { get; set; }
        public virtual DbSet<GettingSPOrderPOSS1> GettingSPOrderPOSS1 { get; set; }
        public virtual DbSet<GettingSPOrderPOSS2> GettingSPOrderPOSS2 { get; set; }
        public virtual DbSet<MasterApproval> MasterApproval { get; set; }
        public virtual DbSet<MasterSPGeneralWareHouse> MasterSPGeneralWareHouses { get; set; }
        public virtual DbSet<MasterBusinessArea> MasterBusinessArea { get; set; }
        public virtual DbSet<MasterSPGeneralWarehousesSub> MasterSPGeneralWarehousesSub { get; set; }
        public virtual DbSet<MasterSPGeneralWarehousesZone> MasterSPGeneralWarehousesZones { get; set; }
        public virtual DbSet<MasterSPGeneralWarehousesShelf> MasterSPGeneralWarehousesShelfs { get; set; }
        public virtual DbSet<MasterSPGeneralWarehouseSection> MasterSPGeneralWarehouseSection { get; set; }



    }
}
