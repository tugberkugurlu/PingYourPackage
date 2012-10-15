using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Services {

    public class CreatedShipmentTypeResult : CreatedResult {

        public CreatedShipmentTypeResult(bool isSuccess) 
            : base(isSuccess) { 
        }

        public ShipmentType ShipmentType { get; set; }
    }
}