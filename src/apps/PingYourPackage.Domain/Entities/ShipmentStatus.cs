using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class ShipmentStatus : IEntity {

        [Key]
        public Guid Key { get; set; }
        public Guid ShipmentKey { get; set; }

        public Shipment Shipment { get; set; }
    }
}
