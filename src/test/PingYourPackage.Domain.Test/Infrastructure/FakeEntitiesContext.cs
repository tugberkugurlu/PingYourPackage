using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.Domain.Entities;

namespace PingYourPackage.Domain.Test.Infrastructure {

    public class FakeEntitiesContext : IEntitiesContext {

        public FakeEntitiesContext() {

            PackageTypes = new FakeDbSet<PackageType>();
            PackagePrices = new FakeDbSet<PackagePrice>();
            PackageSenders = new FakeDbSet<Affiliate>();
            Shipments = new FakeDbSet<Shipment>();
            ShipmentStates = new FakeDbSet<ShipmentState>();
            
            Users = new FakeDbSet<User>();
            Roles = new FakeDbSet<Role>();
            UserInRoles = new FakeDbSet<UserInRole>();
        }

        public IDbSet<PackageType> PackageTypes { get; set; }
        public IDbSet<PackagePrice> PackagePrices { get; set; }
        public IDbSet<Affiliate> PackageSenders { get; set; }
        public IDbSet<Shipment> Shipments { get; set; }
        public IDbSet<ShipmentState> ShipmentStates { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<UserInRole> UserInRoles { get; set; }

        public int SaveChanges() {

            return 0;
        }

        public void Dispose() { }
    }
}
