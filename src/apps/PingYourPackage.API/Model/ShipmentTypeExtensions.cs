using PingYourPackage.API.Model.Dtos;
using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model {
    
    internal static class ShipmentTypeExtensions {

        internal static ShipmentTypeDto ToShipmentTypeDto(this ShipmentType shipmentType) {

            return new ShipmentTypeDto { 

                Key = shipmentType.Key,
                Name = shipmentType.Name,
                Price = shipmentType.Price,
                CreatedOn = shipmentType.CreatedOn
            };
        }
    }
}