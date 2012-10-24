using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Services {
    
    public class ShipmentService : IShipmentService {

        private readonly IEntityRepository<ShipmentType> _shipmentTypeRepository;
        private readonly IEntityRepository<Shipment> _shipmentRepository;
        private readonly IEntityRepository<ShipmentState> _shipmentStateRepository;
        private readonly IEntityRepository<Affiliate> _affiliateRepository;
        private readonly IMembershipService _membershipService;

        public ShipmentService(
            IEntityRepository<ShipmentType> shipmentTypeRepository,
            IEntityRepository<Shipment> shipmentRepository,
            IEntityRepository<ShipmentState> shipmentStateRepository,
            IEntityRepository<Affiliate> affiliateRepository,
            IMembershipService membershipService) {

            _shipmentTypeRepository = shipmentTypeRepository;
            _shipmentRepository = shipmentRepository;
            _shipmentStateRepository = shipmentStateRepository;
            _affiliateRepository = affiliateRepository;
            _membershipService = membershipService;
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

        public OperationResult<ShipmentType> AddShipmentType(ShipmentType shipmentType) {

            // If there is already one which has the same name,
            // return unseccessful result back
            if (_shipmentTypeRepository.GetSingleByName(shipmentType.Name) != null) {

                return new OperationResult<ShipmentType>(false);
            }

            shipmentType.Key = Guid.NewGuid();
            shipmentType.CreatedOn = DateTime.Now;

            _shipmentTypeRepository.Add(shipmentType);
            _shipmentTypeRepository.Save();

            return new OperationResult<ShipmentType>(true) { 
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

        public OperationResult<Affiliate> AddAffiliate(Guid userKey, Affiliate affiliate) {

            var userResult = _membershipService.GetUser(userKey);
            if (userResult == null ||
                !userResult.Roles.Any(
                    role => role.Name.Equals(
                        "affiliate", StringComparison.OrdinalIgnoreCase)) ||
                _affiliateRepository.GetSingle(userKey) != null) {

                return new OperationResult<Affiliate>(false);
            }

            affiliate.Key = userKey;
            affiliate.CreatedOn = DateTime.Now;

            _affiliateRepository.Add(affiliate);
            _affiliateRepository.Save();

            affiliate.User = userResult.User;
            return new OperationResult<Affiliate>(true) {
                Entity = affiliate
            };
        }

        public Affiliate UpdateAffiliate(Affiliate affiliate) {

            _affiliateRepository.Edit(affiliate);
            _affiliateRepository.Save();

            return affiliate;
        }

        // Shipment

        public PaginatedList<Shipment> GetShipments(int pageIndex, int pageSize) {

            var shipments = GetInitialShipments()
                .ToPaginatedList(pageIndex, pageSize);

            return shipments;
        }

        public PaginatedList<Shipment> GetShipments(int pageIndex, int pageSize, Guid affiliateKey) {

            var shipments = _shipmentRepository
                .GetShipmentsByAffiliateKey(affiliateKey)
                .ToPaginatedList(pageIndex, pageSize);

            return shipments;
        }

        public Shipment GetShipment(Guid key) {

            var shipment = GetInitialShipments()
                .FirstOrDefault(x => x.Key == key);

            return shipment;
        }

        public OperationResult<Shipment> AddShipment(Shipment shipment) {

            var affiliate = _affiliateRepository.GetSingle(shipment.AffiliateKey);
            var shipmentType = _shipmentTypeRepository.GetSingle(shipment.ShipmentTypeKey);

            if (affiliate == null || shipmentType == null) {

                return new OperationResult<Shipment>(false);
            }

            shipment.Key = Guid.NewGuid();
            shipment.CreatedOn = DateTime.Now;

            _shipmentRepository.Add(shipment);
            _shipmentRepository.Save();

            // Add the first state for this shipment
            var shipmentState = InsertFirstShipmentState(shipment.Key);

            // Add the down level references manual so that
            // we don't have to have a trip to database to get them
            shipment.ShipmentType = shipmentType;
            shipment.ShipmentStates = new List<ShipmentState> { shipmentState };

            return new OperationResult<Shipment>(true) {
                Entity = shipment
            };
        }

        public Shipment UpdateShipment(Shipment shipment) {

            _shipmentRepository.Edit(shipment);
            _shipmentRepository.Save();

            // Get the shipment seperately so that 
            // we would have down level references such as ShipmentStates.
            var updatedShipment = GetShipment(shipment.Key);

            return shipment;
        }

        public OperationResult RemoveShipment(Shipment shipment) {

            if (IsShipmentRemovable(shipment)) {

                _shipmentRepository.Delete(shipment);
                _shipmentRepository.Save();

                return new OperationResult(true);
            }

            return new OperationResult(false);
        }

        // Private helpers

        private IQueryable<Shipment> GetInitialShipments() {

            return _shipmentRepository.AllIncluding(x => 
                    x.ShipmentType, x => x.ShipmentStates);
        }

        private ShipmentState InsertFirstShipmentState(Guid ShipmentKey) {

            return InsertShipmentState(ShipmentKey, ShipmentStatus.Ordered);
        }

        private ShipmentState InsertShipmentState(Guid ShipmentKey, ShipmentStatus status) {

            var shipmentState = new ShipmentState { 
                Key = Guid.NewGuid(),
                ShipmentKey = ShipmentKey,
                ShipmentStatus = status,
                CreatedOn = DateTime.Now
            };

            _shipmentStateRepository.Add(shipmentState);
            _shipmentStateRepository.Save();

            return shipmentState;
        }

        private bool IsShipmentRemovable(Shipment shipment) {

            var latestStatus = (from shipmentState in shipment.ShipmentStates.ToList()
                                orderby shipmentState.ShipmentStatus descending
                                select shipmentState).First();

            return latestStatus.ShipmentStatus < ShipmentStatus.InTransit;
        }

        // Others

        public bool IsAffiliateRelatedToUser(Guid affiliateKey, string username) {

            var affiliate = GetAffiliate(affiliateKey);

            return affiliate != null && 
                   affiliate.User.Name.Equals(username);
        }
    }
}