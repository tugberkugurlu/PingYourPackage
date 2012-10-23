using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace PingYourPackage.API.ModelBinding {

    public class ShipmentParameterBinding : HttpParameterBinding {

        private readonly string _parameterName;
        private const string ShipmentDictionaryKey = "__AffiliateShipmentsController_Shipment";

        public ShipmentParameterBinding(HttpParameterDescriptor descriptor) 
            : base(descriptor) {

            _parameterName = descriptor.ParameterName;
        }

        public override Task ExecuteBindingAsync(
            ModelMetadataProvider metadataProvider, 
            HttpActionContext actionContext, 
            CancellationToken cancellationToken) {

            // It is safe to assume that the Shipment instance exists
            // inside the HttpRequestMessage.Properties dictionary
            // because we woulnd't be here if it doesn't.
            var shipment = actionContext.Request
                .Properties[ShipmentDictionaryKey] as Shipment;

            actionContext.ActionArguments.Add(_parameterName, shipment);

            return Task.FromResult(0);
        }
    }
}