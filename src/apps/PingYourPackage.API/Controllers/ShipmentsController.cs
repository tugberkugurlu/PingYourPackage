using PingYourPackage.API.Filters;
using PingYourPackage.API.Model;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PingYourPackage.API.Controllers {

    [Authorize(Roles = "Admin,Employee")]
    public class ShipmentsController : ApiController {

        private readonly IShipmentService _shipmentService;

        public ShipmentsController(IShipmentService shipmentService) {

            _shipmentService = shipmentService;
        }

        public PaginatedDto<ShipmentDto> GetShipments(PaginatedRequestCommand cmd) {

            var shipments = _shipmentService
                .GetShipments(cmd.Page, cmd.Take);

            return shipments.ToPaginatedDto(
                shipments.Select(sh => sh.ToShipmentDto()));
        }

        public ShipmentDto GetShipment(Guid key) {

            var shipment = _shipmentService.GetShipment(key);
            if (shipment == null) {

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return shipment.ToShipmentDto();
        }

        [EmptyParameterFilter("requestModel")]
        public HttpResponseMessage PostShipment(ShipmentRequestModel requestModel) {

            var createdShipmentResult = 
                _shipmentService.AddShipment(requestModel.ToShipment());

            if (!createdShipmentResult.IsSuccess) {

                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            var response = Request.CreateResponse(HttpStatusCode.Created,
                createdShipmentResult.Entity.ToShipmentDto());

            response.Headers.Location = new Uri(Url.Link("DefaultHttpRoute",
                new { key = createdShipmentResult.Entity.Key }));

            return response;
        }

        [EmptyParameterFilter("requestModel")]
        public ShipmentDto PutShipment(Guid key, ShipmentUpdateRequestModel requestModel) {

            var shipment = _shipmentService.GetShipment(key);
            if (shipment == null) {

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var updatedShipment = _shipmentService.UpdateShipment(
                requestModel.ToShipment(shipment));

            return updatedShipment.ToShipmentDto();
        }
    }
}