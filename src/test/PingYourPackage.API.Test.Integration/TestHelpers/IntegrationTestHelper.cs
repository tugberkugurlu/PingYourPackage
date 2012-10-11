using Autofac;
using PingYourPackage.API.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PingYourPackage.API.Test.Integration.TestHelpers {
    
    internal static class IntegrationTestHelper {

        internal static HttpConfiguration GetInitialIntegrationTestConfig() {

            var config = new HttpConfiguration();
            RouteConfig.RegisterRoutes(config.Routes);
            WebAPIConfig.Configure(config);

            return config;
        }

        internal static HttpConfiguration GetInitialIntegrationTestConfig(IContainer container) {

            var config = GetInitialIntegrationTestConfig();
            AutofacWebAPI.Initialize(config, container);

            return config;
        }
    }
}