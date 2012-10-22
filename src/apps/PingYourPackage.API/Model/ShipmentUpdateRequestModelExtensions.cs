using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model {

    internal static class ShipmentUpdateRequestModelExtensions {

        internal static Shipment ToShipment(
            this ShipmentUpdateRequestModel requestModel, Shipment existingShipment) {

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