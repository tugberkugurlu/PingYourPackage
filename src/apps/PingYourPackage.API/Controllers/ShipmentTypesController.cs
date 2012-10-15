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
    public class ShipmentTypesController : ApiController {

        private readonly IShipmentService _shipmentService;

        public ShipmentTypesController(
            IShipmentService shipmentService) {

            _shipmentService = shipmentService;
        }

        public PaginatedDto<ShipmentTypeDto> GetShipmentTypes(
            PaginatedRequestCommand cmd) {

            var shipmentTypes = _shipmentService.GetShipmentTypes(
                cmd.Page, cmd.Take);

            return shipmentTypes.ToPaginatedDto(
                shipmentTypes.Select(st => st.ToShipmentTypeDto()));
        }

        public ShipmentTypeDto GetShipmentType(Guid key) {

            var shipmetType = _shipmentService.GetShipmentType(key);
            if (shipmetType == null) {

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return shipmetType.ToShipmentTypeDto();
        }

        public HttpResponseMessage PostShipmentType(
            ShipmentTypeRequestModel requestModel) {

            var createdShipmentTypeResult = _shipmentService
                .AddShipmentType(requestModel.ToShipmentType());

            if (!createdShipmentTypeResult.IsSuccess) {

                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            var response = Request.CreateResponse(HttpStatusCode.Created,
                createdShipmentTypeResult.ShipmentType.ToShipmentTypeDto());

            response.Headers.Location = new Uri(Url.Link("DefaultHttpRoute",
                    new { key = createdShipmentTypeResult.ShipmentType.Key }));

            return response;
        }

        public ShipmentTypeDto PutShipmentType(
            Guid key,
            ShipmentTypeRequestModel requestModel) {

            var shipmentType = _shipmentService.GetShipmentType(key);
            if (shipmentType == null) {

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var updatedShipmentType = _shipmentService.UpdateShipmentType(shipmentType);
            return updatedShipmentType.ToShipmentTypeDto();
        }
    }
}