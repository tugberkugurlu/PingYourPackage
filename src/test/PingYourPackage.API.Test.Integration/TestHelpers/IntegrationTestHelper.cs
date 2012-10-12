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

        internal static ContainerBuilder GetInitialContainerBuilder() {

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(
                Assembly.GetAssembly(typeof(WebAPIConfig))).PropertiesAutowired();

            return builder;
        }

        internal static ContainerBuilder GetInitialContainerBuilder(
            string validUserName,string validPassword, string[] userRoles) {

            var builder = GetInitialContainerBuilder();

            builder.Register(c => 
                ServicesMockHelper.GetMembershipService(
                    validUserName, validPassword, userRoles).Object)
                .As<IMembershipService>()
                .InstancePerApiRequest();

            return builder;
        }
    }
}