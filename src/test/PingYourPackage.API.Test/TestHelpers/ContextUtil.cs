using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace PingYourPackage.API.Test {

    internal static class ContextUtil {

        internal static HttpActionContext GetHttpActionContext(HttpRequestMessage request) {

            HttpActionContext actionContext = CreateActionContext(request: request);
            return actionContext;
        }

        internal static HttpActionContext CreateActionContext(
            HttpControllerContext controllerContext = null, HttpActionDescriptor actionDescriptor = null, HttpRequestMessage request = null) {

            HttpControllerContext controllerCtx = controllerContext ?? CreateControllerContext(request: request);
            HttpActionDescriptor descriptor = actionDescriptor ?? new Mock<HttpActionDescriptor>() { CallBase = true }.Object;

            return new HttpActionContext(controllerCtx, descriptor);
        }

        public static HttpControllerContext CreateControllerContext(
            HttpConfiguration configuration = null, IHttpController controller = null, IHttpRouteData routeData = null, HttpRequestMessage request = null) {

            HttpConfiguration config = configuration ?? new HttpConfiguration();
            IHttpRouteData route = routeData ?? new HttpRouteData(new HttpRoute());
            HttpRequestMessage req = request ?? new HttpRequestMessage();
            req.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            req.Properties[HttpPropertyKeys.HttpRouteDataKey] = route;

            HttpControllerContext context = new HttpControllerContext(config, route, req);
            if (controller != null) {
                context.Controller = controller;
            }
            context.ControllerDescriptor = CreateControllerDescriptor(config);

            return context;
        }

        public static HttpControllerDescriptor CreateControllerDescriptor(HttpConfiguration configuration = null) {

            HttpConfiguration config = configuration ?? new HttpConfiguration();
            HttpControllerDescriptor controllerDescriptor = new HttpControllerDescriptor();
            controllerDescriptor.Configuration = configuration;

            return controllerDescriptor;
        }
    }
}