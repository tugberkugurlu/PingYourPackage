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
        public Guid PackageTypeKey { get; set; }

        public decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverName { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverSurname { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverPostCode { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverCity { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverCountry { get; set; }

        [Required]
        [StringLength(50)]
        public string ReceiverTelephone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(320)]
        public string ReceiverEmail { get; set; }

        public PackageSender PackageSender { get; set; }
        public PackageType PackageType { get; set; }
        public virtual ICollection<ShipmentState> ShipmentStates { get; set; }

        public Shipment() {

            ShipmentStates = new HashSet<ShipmentState>();
        }
    }
}