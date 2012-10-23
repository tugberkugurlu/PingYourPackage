using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model {

    internal static class ShipmentByAffiliateRequestModelExtensions {

        internal static Shipment ToShipment(this ShipmentByAffiliateRequestModel requestModel) {

            return new Shipment {
                ShipmentTypeKey = requestModel.ShipmentTypeKey.Value,
                Price = requestModel.Price.Value,
                ReceiverName = requestModel.ReceiverName,
                ReceiverSurname = requestModel.ReceiverSurname,
                ReceiverAddress = requestModel.ReceiverAddress,
                ReceiverZipCode = requestModel.ReceiverZipCode,
                ReceiverCity = requestModel.ReceiverCity,
                ReceiverCountry = requestModel.ReceiverCountry,
                ReceiverTelephone = requestModel.ReceiverTelephone,
                ReceiverEmail = requestModel.ReceiverEmail
            };
        }

        internal static Shipment ToShipment(this ShipmentByAffiliateRequestModel requestModel, Shipment existingShipment) {

            existingShipment.ShipmentTypeKey = requestModel.ShipmentTypeKey.Value;
            existingShipment.Price = requestModel.Price.Value;
            existingShipment.ReceiverName = requestModel.ReceiverName;
            existingShipment.ReceiverSurname = requestModel.ReceiverSurname;
            existingShipment.ReceiverAddress = requestModel.ReceiverAddress;
            existingShipment.ReceiverZipCode = requestModel.ReceiverZipCode;
            existingShipment.ReceiverCity = requestModel.ReceiverCity;
            existingShipment.ReceiverCountry = requestModel.ReceiverCountry;
            existingShipment.ReceiverTelephone = requestModel.ReceiverTelephone;
            existingShipment.ReceiverEmail = requestModel.ReceiverEmail;

            return existingShipment;
        }
    }
}