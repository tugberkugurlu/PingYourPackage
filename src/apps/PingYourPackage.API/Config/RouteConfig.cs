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
                "UsersPatchHttpRoute",
                "api/users/{id}/{action}",
                defaults: new { controller = "useroperations" },
                constraints: new { httpMethod = new HttpMethodConstraint(new HttpMethod("PATCH")) }
            );

            routes.MapHttpRoute(
                "DefaultHttpRoute",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }

    public class UserOperationsController : ApiController {

        public HttpResponseMessage Status(StatusRequestCommand cmd) {

            throw new NotImplementedException();
        }

        public HttpResponseMessage Roles(RolesRequestCommand cmd) {

            throw new NotImplementedException();
        }
    }
}