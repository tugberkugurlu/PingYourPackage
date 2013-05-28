using Autofac;
using Autofac.Integration.Mvc;
using PingYourPackage.API.Client.Clients;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PingYourPackage.API.Client.Web {
    
    internal static class AutofacMvc {

        internal static void Initialize() {

            var builder = new ContainerBuilder();
            DependencyResolver.SetResolver(
                new AutofacDependencyResolver(RegisterServices(builder)));
        }

        private static IContainer RegisterServices(ContainerBuilder builder) {

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            ApiClientContext apiClientContex =
                ApiClientContext.Create(cfg =>
                    cfg.SetCredentialsFromAppSetting(
                        "Api:AffiliateKey", "Api:Username", "Api:Password")
                                .ConnectTo("https://localhost:44306"));

            // Register the clients
            builder.Register(c => apiClientContex.GetShipmentsClient())
                .As<IShipmentsClient>().InstancePerHttpRequest();

            builder.Register(c => apiClientContex.GetShipmentTypesClient())
                .As<IShipmentTypesClient>().InstancePerHttpRequest();

            return builder.Build();
        }
    }
}