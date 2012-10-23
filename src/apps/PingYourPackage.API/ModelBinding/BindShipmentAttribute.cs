using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace PingYourPackage.API.ModelBinding {

    public class BindShipmentAttribute : ParameterBindingAttribute {

        public override HttpParameterBinding GetBinding(
            HttpParameterDescriptor parameter) {

            return new ShipmentParameterBinding(parameter);
        }
    }
}