using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SparePartsModule.Domain.Models;
using SparePartsModule.Domain.Models.Vehicles;

namespace SparePartsModule.Domain.Context
{
    public class VehicleSystemContext:DbContext
    {
        public readonly string StringConnection = "";
        private readonly IConfiguration _configuration;
        public VehicleSystemContext()
        {
        }
        public VehicleSystemContext(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public VehicleSystemContext(DbContextOptions<VehicleSystemContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public virtual DbSet<MasterLookup> MasterLookup { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MasterVehicleModel> MasterVehicleModel { get; set; }

        public virtual DbSet<MasterVehicleBrand> MasterVehicleBrand { get; set; }

        public virtual DbSet<MasterVehilceList> MasterVehilceList { get; set; }
        public virtual DbSet<MasterVehicleGeneralSpec> MasterVehicleGeneralSpecs { get; set; }

        public virtual DbSet<MasterVehicleGeneralSpecsValue> MasterVehicleGeneralSpecsValues { get; set; }
        public virtual DbSet<MasterVehicleLookupType> MasterVehicleLookupType { get; set; }
        public virtual DbSet<MasterVehicleLookup> MasterVehicleLookup { get; set; }

        public virtual DbSet<MasterVehicleModelCode> MasterVehicleModelCode { get; set; }
        public virtual DbSet<MasterVehilceListSpecsValue> MasterVehilceListSpecsValues { get; set; }
        public virtual DbSet<GettingVehicleSupplier> GettingVehicleSupplier { get; set; }
        // public virtual DbSet<GettingVehicleSupplier> GettingVehicleSupplier { get; set; }
        //public virtual DbSet<MasterVehicleGeneralSpec> MasterVehicleGeneralSpecs { get; set; }





    }
}
