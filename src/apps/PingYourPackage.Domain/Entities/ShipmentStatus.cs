using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public enum ShipmentStatus {

        Ordered = 1,
        Scheduled = 2,
        InTransit = 3,
        Delivered = 4
    }
}