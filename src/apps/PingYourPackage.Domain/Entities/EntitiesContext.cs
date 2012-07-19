using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public interface IEntitiesContext : IDisposable {

        int SaveChanges();
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry Entry(object entry);

        DbSet<PackageType> PackageTypes { get; set; }
        DbSet<PackagePrice> PackagePrices { get; set; }
        DbSet<PackageSender> PackageSenders { get; set; }
        DbSet<PackageReceiver> PackageReceivers { get; set; }
        DbSet<Shipment> Shipments { get; set; }
        DbSet<ShipmentStatus> ShipmentStatuses { get; set; }

        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
    }

    public class EntitiesContext : DbContext, IEntitiesContext {

        public EntitiesContext() : base("PingYourPackage") { }

        public DbSet<PackageType> PackageTypes { get; set; }
        public DbSet<PackagePrice> PackagePrices { get; set; }
        public DbSet<PackageSender> PackageSenders { get; set; }
        public DbSet<PackageReceiver> PackageReceivers { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentStatus> ShipmentStatuses { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}