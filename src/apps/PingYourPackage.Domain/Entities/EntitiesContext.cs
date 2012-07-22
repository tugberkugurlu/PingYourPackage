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

        IDbSet<PackageType> PackageTypes { get; set; }
        IDbSet<PackagePrice> PackagePrices { get; set; }
        IDbSet<PackageSender> PackageSenders { get; set; }
        IDbSet<PackageReceiver> PackageReceivers { get; set; }
        IDbSet<Shipment> Shipments { get; set; }
        IDbSet<ShipmentState> ShipmentStatuses { get; set; }

        IDbSet<User> Users { get; set; }
        IDbSet<Role> Roles { get; set; }
    }

    public class EntitiesContext : DbContext, IEntitiesContext {

        public EntitiesContext() : base("PingYourPackage") { }

        public IDbSet<PackageType> PackageTypes { get; set; }
        public IDbSet<PackagePrice> PackagePrices { get; set; }
        public IDbSet<PackageSender> PackageSenders { get; set; }
        public IDbSet<PackageReceiver> PackageReceivers { get; set; }
        public IDbSet<Shipment> Shipments { get; set; }
        public IDbSet<ShipmentState> ShipmentStatuses { get; set; }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
    }
}