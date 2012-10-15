using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.Domain.Entities;

namespace PingYourPackage.Domain.Services {
    
    public interface IShipmentService {

        Shipment AddShipment(Guid affiliateKey, Shipment shipment);
        Shipment ChangeShipmentState(Guid shipmentKey, ShipmentStatus shipmentStatus);

        PaginatedList<ShipmentType> GetShipmentTypes(int pageIndex, int pageSize);
        ShipmentType GetShipmentType(Guid key);
        CreatedShipmentTypeResult AddShipmentType(ShipmentType shipmentType);
        ShipmentType UpdateShipmentType(ShipmentType shipmentType);
    }
}