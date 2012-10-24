using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model {

    internal static class ShipmentByAffiliateRequestModelExtensions {

        internal static Shipment ToShipment(this ShipmentByAffiliateRequestModel requestModel, Guid affiliateKey) {

            return new Shipment {
                ShipmentTypeKey = requestModel.ShipmentTypeKey.Value,
                AffiliateKey = affiliateKey,
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
    }
}