using PingYourPackage.API.Model.Dtos;
using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model {
    
    internal static class ShipmentStateExtensions {

        internal static ShipmentStateDto ToShipmentStateDto(this ShipmentState shipmentState) {

            return new ShipmentStateDto { 
                Key = shipmentState.Key,
                ShipmentStatus = shipmentState.ShipmentStatus.ToString(),
                CreatedOn = shipmentState.CreatedOn
            };
        }
    }
}