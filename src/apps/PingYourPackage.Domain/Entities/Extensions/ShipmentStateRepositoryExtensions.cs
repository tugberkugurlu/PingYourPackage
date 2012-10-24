using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {
    
    public static class ShipmentStateRepositoryExtensions {

        public static IQueryable<ShipmentState> GetAllByShipmentKey(
            this IEntityRepository<ShipmentState> shipmentStateRepository, Guid shipmentKey) {

            return shipmentStateRepository.GetAll()
                .Where(x => x.ShipmentKey == shipmentKey);
        }
    }
}