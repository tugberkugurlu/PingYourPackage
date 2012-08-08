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

        IDbSet<ShipmentType> PackageTypes { get; set; }
        IDbSet<Affiliate> PackageSenders { get; set; }
        IDbSet<Shipment> Shipments { get; set; }
        IDbSet<ShipmentState> ShipmentStates { get; set; }

        IDbSet<User> Users { get; set; }
        IDbSet<Role> Roles { get; set; }
        IDbSet<UserInRole> UserInRoles { get; set; }
    }

    public class EntitiesContext : DbContext, IEntitiesContext {

        public EntitiesContext() : base("PingYourPackage") { }

        public IDbSet<ShipmentType> PackageTypes { get; set; }
        public IDbSet<Affiliate> PackageSenders { get; set; }
        public IDbSet<Shipment> Shipments { get; set; }
        public IDbSet<ShipmentState> ShipmentStates { get; set; }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<UserInRole> UserInRoles { get; set; }
    }
}