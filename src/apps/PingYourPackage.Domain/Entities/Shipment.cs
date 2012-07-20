using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class Shipment : IEntity {

        [Key]
        public int Key { get; set; }
        public int PackageSenderKey { get; set; }
        public int PackageReceiverKey { get; set; }

        public PackageSender PackageSender { get; set; }
        public PackageReceiver PackageReceiver { get; set; }
        public PackageType PackageType { get; set; }
        public virtual ICollection<ShipmentStatus> ShipmentStatuses { get; set; }

        public Delivery Delivery { get; set; }

        public Shipment() {

            ShipmentStatuses = new HashSet<ShipmentStatus>();
            Delivery = new Delivery();
        }
    }
}