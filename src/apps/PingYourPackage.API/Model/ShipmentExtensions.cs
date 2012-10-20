using PingYourPackage.API.Model.Dtos;
using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model {
    
    internal static class ShipmentExtensions {

        internal static ShipmentDto ToShipmentDto(this Shipment shipment) {

            return new ShipmentDto { 
                Key = shipment.Key,
                AffiliateKey = shipment.AffiliateKey,
                Price = shipment.Price,
                ReceiverName = shipment.ReceiverName,
                ReceiverSurname = shipment.ReceiverSurname,
                ReceiverAddress = shipment.ReceiverAddress,
                ReceiverZipCode = shipment.ReceiverZipCode,
                ReceiverCity = shipment.ReceiverCity,
                ReceiverCountry = shipment.ReceiverCountry,
                ReceiverTelephone = shipment.ReceiverTelephone,
                ReceiverEmail = shipment.ReceiverEmail,
                CreatedOn = shipment.CreatedOn,
                ShipmentType = shipment.ShipmentType.ToShipmentTypeDto(),
                ShipmentStates = shipment.ShipmentStates.Select(ss => ss.ToShipmentStateDto())
            };
        }
    }
}