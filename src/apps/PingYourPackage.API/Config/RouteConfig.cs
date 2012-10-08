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
                "UserRolesHttpRoute",
                "api/users/{key}/roles",
                new { controller = "userroles" });

            routes.MapHttpRoute(
                "DefaultHttpRoute",
                "api/{controller}/{key}",
                new { key = RouteParameter.Optional });

            //routes.MapHttpRoute(
            //    "UsersPatchHttpRoute",
            //    "api/users/{id}/{action}",
            //    defaults: new { controller = "useroperations" },
            //    constraints: new { httpMethod = new HttpMethodConstraint(new HttpMethod("PATCH")) }
            //);
        }
    }
}