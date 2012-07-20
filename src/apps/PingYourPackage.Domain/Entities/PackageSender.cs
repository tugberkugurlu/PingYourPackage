using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class PackageSender : IEntity {

        [Key]
        public Guid Key { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }

        public PackageSender() {

            Shipments = new HashSet<Shipment>();
        }
    }
}