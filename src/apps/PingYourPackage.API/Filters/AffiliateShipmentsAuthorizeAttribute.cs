using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace PingYourPackage.API.Filters {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AffiliateShipmentsAuthorizeAttribute : AuthorizeAttribute {

        public AffiliateShipmentsAuthorizeAttribute() {

            base.Roles = "Affiliate";
        }

        public override void OnAuthorization(HttpActionContext actionContext) {
            
            base.OnAuthorization(actionContext);

            // If not authorized at all, don't bother checking for the 
            // user - affiliate relation
            if (actionContext.Response == null) { 

                // We are here sure that the request has been authorized and 
                // the user is in the Affiliate role. We also don't need 
                // to check the existence of the affiliate as it has 
                // been also already done by AffiliateShipmentsDispatcher.

                HttpRequestMessage request = actionContext.Request;
                Guid affiliateKey = GetAffiliateKey(request.GetRouteData());
                IPrincipal principal = Thread.CurrentPrincipal;
                IShipmentService shipmentService = request.GetShipmentService();

                if (!shipmentService.IsAffiliateRelatedToUser(affiliateKey, principal.Identity.Name)) {

                    // Set Unauthorized response as the user and 
                    // affiliate isn't related to each other. You might
                    // want to return "404 NotFound" response here if you don't
                    // want to expose the existence of the affiliate.
                    actionContext.Response = request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }

        private static Guid GetAffiliateKey(IHttpRouteData routeData) {

            var affiliateKey = routeData.Values["key"].ToString();
            return Guid.ParseExact(affiliateKey, "D");
        }
    }
}