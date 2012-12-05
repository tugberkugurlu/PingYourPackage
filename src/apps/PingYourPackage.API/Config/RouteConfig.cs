using PingYourPackage.API.Dispatcher;
using PingYourPackage.API.Routing;
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
                "api/affiliates/{key}/shipments/{shipmentKey}",
                defaults: new { controller = "AffiliateShipments", shipmentKey = RouteParameter.Optional },
                constraints: new { key = new GuidRouteConstraint(), shipmentKey = new GuidRouteConstraint() },
                handler: new AffiliateShipmentsDispatcher(config));

            routes.MapHttpRoute(
                "ShipmentStatesHttpRoute",
                "api/shipments/{key}/shipmentstates",
                defaults: new { controller = "ShipmentStates" },
                constraints: new { key = new GuidRouteConstraint() },
                handler: new ShipmentStatesDispatcher(config));

            routes.MapHttpRoute(
                "DefaultHttpRoute",
                "api/{controller}/{key}",
                defaults: new { key = RouteParameter.Optional },
                constraints: new { key = new GuidRouteConstraint() });
        }
    }
}