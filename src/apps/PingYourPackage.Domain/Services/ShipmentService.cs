using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Services {
    
    public class ShipmentService : IShipmentService {

        private readonly IEntityRepository<ShipmentType> _shipmentTypeRepository;

        public ShipmentService(
            IEntityRepository<ShipmentType> shipmentTypeRepository) {

            _shipmentTypeRepository = shipmentTypeRepository;
        }

        public Shipment AddShipment(Guid affiliateKey, Shipment shipment) {
            throw new NotImplementedException();
        }

        public Shipment ChangeShipmentState(Guid shipmentKey, ShipmentStatus shipmentStatus) {
            throw new NotImplementedException();
        }

        // ShipmentType

        public PaginatedList<ShipmentType> GetShipmentTypes(int pageIndex, int pageSize) {

            var shipmentTypes = _shipmentTypeRepository
                .GetAll().ToPaginatedList(pageIndex, pageSize);

            return shipmentTypes;
        }

        public ShipmentType GetShipmentType(Guid key) {

            var shipmentType = _shipmentTypeRepository.GetSingle(key);
            return shipmentType;
        }

        public CreatedShipmentTypeResult AddShipmentType(ShipmentType shipmentType) {

            // If there is already one which has the same name,
            // return unseccessful result back
            if (_shipmentTypeRepository.GetSingleByName(shipmentType.Name) != null) {

                return new CreatedShipmentTypeResult(false);
            }

            shipmentType.Key = Guid.NewGuid();
            shipmentType.CreatedOn = DateTime.Now;

            _shipmentTypeRepository.Add(shipmentType);
            _shipmentTypeRepository.Save();

            return new CreatedShipmentTypeResult(true) { 
                ShipmentType = shipmentType
            };
        }

        public ShipmentType UpdateShipmentType(ShipmentType shipmentType) {

            _shipmentTypeRepository.Edit(shipmentType);
            _shipmentTypeRepository.Save();

            return shipmentType;
        }
    }
}