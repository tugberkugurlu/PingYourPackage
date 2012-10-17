using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace PingYourPackage.API.Config {

    public class RouteConfig {

        public static void RegisterRoutes(HttpRouteCollection routes) {

            routes.MapHttpRoute(
                "AffiliateShipmentsHttpRoute",
                "api/affiliates/{key}/shipments",
                new { controller = "AffiliateShipments" });

            routes.MapHttpRoute(
                "ShipmentStatesHttpRoute",
                "api/shipments/{key}/shipmentstates",
                new { controller = "ShipmentStates" });

            routes.MapHttpRoute(
                "AffiliateShipmentShipmentStatesHttpRoute",
                "api/affiliates/{key}/shipments/{shipmentKey}/shipmentstates",
                new { controller = "AffiliateShipmentShipmentStates" });

            routes.MapHttpRoute(
                "DefaultHttpRoute",
                "api/{controller}/{key}",
                new { key = RouteParameter.Optional });
        }
    }
}