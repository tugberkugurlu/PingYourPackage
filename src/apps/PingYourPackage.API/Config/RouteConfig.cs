using PingYourPackage.API.Dispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace PingYourPackage.API.Config {

    public class RouteConfig {

        public static void RegisterRoutes(HttpConfiguration config) {

            var routes = config.Routes;

            routes.MapHttpRoute(
                "AffiliateShipmentsHttpRoute",
                "api/affiliates/{key}/shipments",
                new { controller = "AffiliateShipments" },
                constraints: new { },
                handler: new AffiliateShipmentsDispatcher(config));

            routes.MapHttpRoute(
                "ShipmentStatesHttpRoute",
                "api/shipments/{key}/shipmentstates",
                new { controller = "ShipmentStates" });

            routes.MapHttpRoute(
                "DefaultHttpRoute",
                "api/{controller}/{key}",
                new { key = RouteParameter.Optional });
        }
    }
}