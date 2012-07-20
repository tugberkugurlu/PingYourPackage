using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PingYourPackage.API.WebHost {

    public class RouteConfig {

        public static void RegisterRoutes(HttpRouteCollection routes) {

            routes.MapHttpRoute(
                "DefaultHttpRoute",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}