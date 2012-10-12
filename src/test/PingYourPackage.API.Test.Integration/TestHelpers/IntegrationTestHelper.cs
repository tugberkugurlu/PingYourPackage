using Autofac;
using Autofac.Integration.WebApi;
using PingYourPackage.API.Config;
using System.Web.Http;
using System.Reflection;
using PingYourPackage.Domain.Services;

namespace PingYourPackage.API.Test.Integration {
    
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

        internal static ContainerBuilder GetEmptyContainerBuilder() {

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(
                Assembly.GetAssembly(typeof(WebAPIConfig))).PropertiesAutowired();

            return builder;
        }

        internal static ContainerBuilder GetInitialContainerBuilder() {

            var builder = GetEmptyContainerBuilder();

            builder.Register(c => 
                ServicesMockHelper.GetInitialMembershipService().Object)
                .As<IMembershipService>()
                .InstancePerApiRequest();

            return builder;
        }
    }
}