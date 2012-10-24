using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PingYourPackage.API.Controllers {

    [Authorize(Roles = "Admin,Employee")]
    public class ShipmentStatesController : ApiController {

        private readonly IShipmentService _shipmentService;

        public ShipmentStatesController(IShipmentService shipmentService) {

            _shipmentService = shipmentService;
        }

        public IEnumerable<ShipmentState> GetShipmentStates(Guid key) {

            throw new NotImplementedException();
        }

        public HttpResponseMessage PostShipmentState(Guid key) {

            throw new NotImplementedException();
        }
    }
}