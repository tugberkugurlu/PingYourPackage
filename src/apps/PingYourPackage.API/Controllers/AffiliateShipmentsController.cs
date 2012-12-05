using PingYourPackage.API.Filters;
using PingYourPackage.API.Model;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.API.ModelBinding;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PingYourPackage.API.Controllers {
    
    [AffiliateShipmentsAuthorize]
    public class AffiliateShipmentsController : ApiController {

        // We are OK inside this controller in terms of 
        // Affiliate existance and its relation with the current 
        // authed user has been checked by the handler 
        // and AffiliateShipmentsAuthorizeAttribute.

        // The action method which requests the shipment instance:
        // We can just get the shipment as the shipment 
        // existance and its ownership by the affiliate has been 
        // approved by the EnsureShipmentOwnershipAttribute.
        // The BindShipmentAttribute can bind the shipment from the
        // Properties dictionarty of the HttpRequestMessage instance
        // as it has been put there by the EnsureShipmentOwnershipAttribute.

        private const string RouteName = "AffiliateShipmentsHttpRoute";
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
        public ShipmentDto GetShipment(
            Guid key, 
            Guid shipmentKey, 
            [BindShipment]Shipment shipment) {

            return shipment.ToShipmentDto();
        }

        [EmptyParameterFilter("requestModel")]
        public HttpResponseMessage PostShipment(Guid key, ShipmentByAffiliateRequestModel requestModel) {

            var createdShipmentResult =
                _shipmentService.AddShipment(requestModel.ToShipment(key));

            if (!createdShipmentResult.IsSuccess) {

                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            var response = Request.CreateResponse(HttpStatusCode.Created,
                createdShipmentResult.Entity.ToShipmentDto());

            response.Headers.Location = new Uri(
                Url.Link(RouteName, new { 
                    key = createdShipmentResult.Entity.AffiliateKey,
                    shipmentKey = createdShipmentResult.Entity.Key
                })
            );

            return response;
        }

        [EnsureShipmentOwnership]
        [EmptyParameterFilter("requestModel")]
        public ShipmentDto PutShipment(
            Guid key, 
            Guid shipmentKey,
            ShipmentByAffiliateUpdateRequestModel requestModel,
            [BindShipment]Shipment shipment) {

            var updatedShipment = _shipmentService.UpdateShipment(
                requestModel.ToShipment(shipment));

            return updatedShipment.ToShipmentDto();
        }

        [EnsureShipmentOwnership]
        public HttpResponseMessage DeleteShipment(
            Guid key, 
            Guid shipmentKey,
            [BindShipment]Shipment shipment) {

            var operationResult = _shipmentService.RemoveShipment(shipment);

            if (!operationResult.IsSuccess) {

                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}