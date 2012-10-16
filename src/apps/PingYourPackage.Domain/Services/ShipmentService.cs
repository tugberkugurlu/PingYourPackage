using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Services {
    
    public class ShipmentService : IShipmentService {

        private readonly IEntityRepository<ShipmentType> _shipmentTypeRepository;
        private readonly IEntityRepository<Affiliate> _affiliateRepository;
        private readonly IMembershipService _membershipService;

        public ShipmentService(
            IEntityRepository<ShipmentType> shipmentTypeRepository,
            IEntityRepository<Affiliate> affiliateRepository,
            IMembershipService membershipService) {

            _shipmentTypeRepository = shipmentTypeRepository;
            _affiliateRepository = affiliateRepository;
            _membershipService = membershipService;
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

        public CreatedResult<ShipmentType> AddShipmentType(ShipmentType shipmentType) {

            // If there is already one which has the same name,
            // return unseccessful result back
            if (_shipmentTypeRepository.GetSingleByName(shipmentType.Name) != null) {

                return new CreatedResult<ShipmentType>(false);
            }

            shipmentType.Key = Guid.NewGuid();
            shipmentType.CreatedOn = DateTime.Now;

            _shipmentTypeRepository.Add(shipmentType);
            _shipmentTypeRepository.Save();

            return new CreatedResult<ShipmentType>(true) { 
                Entity = shipmentType
            };
        }

        public ShipmentType UpdateShipmentType(ShipmentType shipmentType) {

            _shipmentTypeRepository.Edit(shipmentType);
            _shipmentTypeRepository.Save();

            return shipmentType;
        }

        // Affiliate

        public PaginatedList<Affiliate> GetAffiliates(int pageIndex, int pageSize) {

            var affiliates = _affiliateRepository
                .AllIncluding(x => x.User).ToPaginatedList(pageIndex, pageSize);

            return affiliates;
        }

        public Affiliate GetAffiliate(Guid key) {

            var affiliate = _affiliateRepository
                .AllIncluding(x => x.User)
                .FirstOrDefault(x => x.Key == key);

            return affiliate;
        }

        public CreatedResult<Affiliate> AddAffiliate(
            Guid userKey, Affiliate affiliate) {

            var userResult = _membershipService.GetUser(userKey);
            if (userResult == null ||
                !userResult.Roles.Any(
                    role => role.Name.Equals(
                        "affiliate", StringComparison.OrdinalIgnoreCase)) ||
                _affiliateRepository.GetSingle(userKey) != null) {

                return new CreatedResult<Affiliate>(false);
            }

            affiliate.Key = userKey;
            affiliate.CreatedOn = DateTime.Now;

            _affiliateRepository.Add(affiliate);
            _affiliateRepository.Save();

            affiliate.User = userResult.User;
            return new CreatedResult<Affiliate>(true) {
                Entity = affiliate
            };
        }

        public Affiliate UpdateAffiliate(Affiliate affiliate) {

            _affiliateRepository.Edit(affiliate);
            _affiliateRepository.Save();

            return affiliate;
        }
    }
}