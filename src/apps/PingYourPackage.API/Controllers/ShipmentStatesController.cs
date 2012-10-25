using PingYourPackage.API.Filters;
using PingYourPackage.API.Model;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Entities;
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
    public class ShipmentStatesController : ApiController {

        // Here, inside this controller, we are sure that the
        // requested shipment exists. As only Admins and Employees 
        // can access this controler, ownership is not a concern.

        private readonly IShipmentService _shipmentService;

        public ShipmentStatesController(IShipmentService shipmentService) {

            _shipmentService = shipmentService;
        }

        public IEnumerable<ShipmentStateDto> GetShipmentStates(Guid key) {

            var shipmentStates = _shipmentService.GetShipmentStates(key);
            return shipmentStates.Select(x => x.ToShipmentStateDto());
        }

        [EmptyParameterFilter("requestModel")]
        public HttpResponseMessage PostShipmentState(Guid key, ShipmentStateRequestModel requestModel) {

            var createdShipmentState = _shipmentService.AddShipmentState(
                key, RetrieveShipmentStatus(requestModel.ShipmentStatus));

            if (!createdShipmentState.IsSuccess) {

                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            var response = Request.CreateResponse(HttpStatusCode.Created,
                createdShipmentState.Entity.ToShipmentStateDto());

            return response;
        }

        private ShipmentStatus RetrieveShipmentStatus(string value) {

            return (ShipmentStatus)Enum
                .Parse(typeof(ShipmentStatus), value, true);
        }
    }
}