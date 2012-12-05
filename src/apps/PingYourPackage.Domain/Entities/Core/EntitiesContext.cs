using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class EntitiesContext : DbContext {

        public EntitiesContext() : base("PingYourPackage") { }

        public IDbSet<ShipmentType> ShipmentTypes { get; set; }
        public IDbSet<Affiliate> Affiliates { get; set; }
        public IDbSet<Shipment> Shipments { get; set; }
        public IDbSet<ShipmentState> ShipmentStates { get; set; }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<UserInRole> UserInRoles { get; set; }
    }
}