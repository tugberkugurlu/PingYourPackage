using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class PackagePrice : IEntity {

        [Key]
        public int Key { get; set; }
        public int PackageTypeKey { get; set; }

        [DataType(DataType.Date)]
        public DateTime ValidFrom { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime ValidTill { get; set; }
        public DateTime CreatedOn { get; set; }

        public PackageType PackageType { get; set; }
    }
}