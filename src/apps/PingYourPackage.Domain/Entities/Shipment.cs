using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class Shipment : IEntity {

        [Key]
        public Guid Key { get; set; }
        public Guid PackageSenderKey { get; set; }
        public Guid PackageReceiverKey { get; set; }
        public Guid PackageTypeKey { get; set; }
        public Guid UserKey { get; set; }

        public decimal Price { get; set; }

        public PackageSender PackageSender { get; set; }
        public PackageReceiver PackageReceiver { get; set; }
        public PackageType PackageType { get; set; }
        public User User { get; set; }
        public virtual ICollection<ShipmentState> ShipmentStates { get; set; }

        public Shipment() {

            ShipmentStates = new HashSet<ShipmentState>();
        }
    }
}