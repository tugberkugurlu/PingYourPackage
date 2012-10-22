using PingYourPackage.API.Filters;
using PingYourPackage.API.Model;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace PingYourPackage.API.Controllers {
    
    [AffiliateShipmentsAuthorize]
    public class AffiliateShipmentsController : ApiController {

        private readonly IShipmentService _shipmentService;

        public AffiliateShipmentsController(IShipmentService shipmentService) {

            _shipmentService = shipmentService;
        }

        public PaginatedDto<ShipmentDto> GetShipments(Guid key, PaginatedRequestCommand cmd) {

            var shipments = _shipmentService
                .GetShipments(cmd.Page, cmd.Take, affiliateKey: key);

            return shipments.ToPaginatedDto(
                shipments.Select(sh => sh.ToShipmentDto()));
        }

        [EnsureShipmentOwnership]
        public ShipmentDto GetShipment(Guid key, Guid shipmentKey) {

            // we can just get the shipment as the shipment 
            // existance and wwnership has been approved

            var shipment = _shipmentService.GetShipment(key);
            return shipment.ToShipmentDto();
        }

        [EmptyParameterFilter("requestModel")]
        public HttpResponseMessage PostShipment(Guid key, ShipmentByAffiliateRequestModel requestModel) {

            throw new NotImplementedException();
        }

        [EnsureShipmentOwnership]
        [EmptyParameterFilter("requestModel")]
        public ShipmentDto PutShipment(Guid key, Guid shipmentKey, ShipmentByAffiliateRequestModel requestModel) {

            throw new NotImplementedException();
        }

        [EnsureShipmentOwnership]
        public HttpResponseMessage DeleteShipment(Guid key, Guid shipmentKey) {

            throw new NotImplementedException();
        }
    }
}