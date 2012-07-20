using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class PackageType : IEntity {

        [Key]
        public int Key { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<PackagePrice> PackagePrices { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }

        public PackageType() {

            PackagePrices = new HashSet<PackagePrice>();
            Shipments = new HashSet<Shipment>();
        }
    }
}