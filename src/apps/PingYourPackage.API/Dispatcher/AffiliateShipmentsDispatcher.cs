using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace PingYourPackage.API.Dispatcher {

    public class AffiliateShipmentsDispatcher : HttpControllerDispatcher {

        public AffiliateShipmentsDispatcher(HttpConfiguration config) 
            : base(config) { }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken) {

            // We are sure here that the Key route value is supplied
            // So, we can check against it.
            IHttpRouteData routeData = request.GetRouteData();
            Guid affiliateKey = Guid.ParseExact(routeData.Values["key"].ToString(), "D");
            IShipmentService shipmentService = request.GetShipmentService();

            if (shipmentService.GetAffiliate(affiliateKey) == null) {

                return Task.FromResult(
                    request.CreateResponse(HttpStatusCode.NotFound));
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}