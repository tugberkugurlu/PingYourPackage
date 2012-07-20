using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class Delivery : IEntity {

        [Key]
        public Guid Key { get; set; }
        public Guid ShipmentKey { get; set; }

        [Required]
        public Shipment Shipment { get; set; }

        public Delivery() {

            Shipment = new Shipment();
        }
    }
}