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
        CreatedResult<ShipmentType> AddShipmentType(ShipmentType shipmentType);
        ShipmentType UpdateShipmentType(ShipmentType shipmentType);

        PaginatedList<Affiliate> GetAffiliates(int pageIndex, int pageSize);
        Affiliate GetAffiliate(Guid key);
        CreatedResult<Affiliate> AddAffiliate(Guid userKey, Affiliate affiliate);
        Affiliate UpdateAffiliate(Affiliate affiliate);
    }
}