using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.Domain.Entities;

namespace PingYourPackage.Domain.Services {
    
    public interface IShipmentService {

        void AddShipment(Affiliate affiliate, Shipment shipment);
        void ChangeShipmentState(Guid shipmentKey, ShipmentStatus shipmentStatus);
    }
}