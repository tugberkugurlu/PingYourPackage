using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class PackagePrice : IEntity {

        [Key]
        public Guid Key { get; set; }
        public Guid PackageTypeKey { get; set; }

        public decimal Price { get; set; }

        [Column(TypeName = "Date")]
        public DateTime ValidFrom { get; set; }
        
        [Column(TypeName = "Date")]
        public DateTime ValidTill { get; set; }
        public DateTime CreatedOn { get; set; }

        public PackageType PackageType { get; set; }
    }
}