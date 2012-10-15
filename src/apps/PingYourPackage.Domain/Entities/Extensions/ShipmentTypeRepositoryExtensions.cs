using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {
    
    public static class ShipmentTypeRepositoryExtensions {

        public static ShipmentType GetSingleByName(
            this IEntityRepository<ShipmentType> shipmentTypeRepository,
            string name) {

            return shipmentTypeRepository
                .FindBy(x => x.Name == name).FirstOrDefault();
        }
    }
}
