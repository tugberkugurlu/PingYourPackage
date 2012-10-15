using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model {
    
    internal static class ShipmentTypeRequestModelExtensions {

        internal static ShipmentType ToShipmentType(
            this ShipmentTypeRequestModel requestModel) {

            return new ShipmentType { 
                Name = requestModel.Name,
                Price = requestModel.Price.Value
            };
        }

        internal static ShipmentType ToShipmentType(
            this ShipmentTypeRequestModel requestModel, 
            ShipmentType existingShipmentType) {

            existingShipmentType.Name = requestModel.Name;
            existingShipmentType.Price = requestModel.Price.Value;

            return existingShipmentType;
        }
    }
}