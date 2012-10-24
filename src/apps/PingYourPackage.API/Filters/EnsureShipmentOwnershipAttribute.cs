using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Routing;
using PingYourPackage.Domain.Services;
using System.Threading;
using System.Security.Principal;
using System.Net;
using PingYourPackage.Domain.Entities;

namespace PingYourPackage.API.Filters {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class EnsureShipmentOwnershipAttribute : Attribute, IAuthorizationFilter {

        private const string ShipmentDictionaryKey = "__AffiliateShipmentsController_Shipment";
        public bool AllowMultiple { get { return false; } }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation) {

            // We are here sure that the user is authanticated and request 
            // can be kept executing because the AuthorizeAttribute has 
            // been invoked before this filter's OnActionExecuting method.
            // Also, we are sure that the affiliate is associated with
            // the currently authanticated user as the previous action filter 
            // has checked against this.
            IHttpRouteData routeData = actionContext.Request.GetRouteData();
            Uri requestUri = actionContext.Request.RequestUri;

            Guid affiliateKey = GetAffiliateKey(routeData);
            Guid shipmentKey = GetShipmentKey(routeData, requestUri);

            // Check if the affiliate really owns the shipment
            // whose key came from the request. We don't need to check the 
            // existence of the affiliate as this check has been already 
            // performed by the AffiliateShipmentsDispatcher.
            IShipmentService shipmentService = actionContext.Request.GetShipmentService();
            Shipment shipment = shipmentService.GetShipment(shipmentKey);

            // Check the shipment existance
            if (shipment == null) {

                return Task.FromResult(
                    new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            // Check the shipment ownership
            if (shipment.AffiliateKey != affiliateKey) {

                // You might want to return "404 NotFound" response here 
                // if you don't want to expose the existence of the shipment.
                return Task.FromResult(
                    new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }

            // Stick the shipment inside the Properties dictionary so 
            // that we won't need to have another trip to database.
            // The ShipmentParameterBinding will bind the Shipment param
            // if needed.
            actionContext.Request
                .Properties[ShipmentDictionaryKey] = shipment;

            // The request is legit, continue executing.
            return continuation();
        }

        private static Guid GetAffiliateKey(IHttpRouteData routeData) {

            var affiliateKey = routeData.Values["key"].ToString();
            return Guid.ParseExact(affiliateKey, "D");
        }

        private static Guid GetShipmentKey(IHttpRouteData routeData, Uri requestUri) {

            // We are sure at this point that the shipmentKey value has been
            // supplied (either through route or quesry string) because it 
            // wouldn't be possible for the request to arrive here if it wasn't.
            object shipmentKeyString;
            if (routeData.Values.TryGetValue("shipmentKey", out shipmentKeyString)) {

                return Guid.ParseExact(shipmentKeyString.ToString(), "D");
            }

            // It's now sure that query string has the shipmentKey value
            var quesryString = requestUri.ParseQueryString();
            return Guid.ParseExact(quesryString["shipmentKey"], "D");
        }
    }
}