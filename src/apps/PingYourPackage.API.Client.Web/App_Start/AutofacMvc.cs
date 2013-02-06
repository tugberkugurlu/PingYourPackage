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
            DependencyResolver.SetResolver(new AutofacDependencyResolver(RegisterServices(builder)));
        }

        private static IContainer RegisterServices(ContainerBuilder builder) {

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            string affiliateKey = ConfigurationManager.AppSettings["Api:AffiliateKey"];

            ApiClientContext apiClientContex =
                ApiClientContext.Create(affiliateKey, cfg =>
                    cfg.SetCredentialsFromAppSetting("Api:Username", "Api:Password")
                       .ConnectTo("https://localhost:44306"));

            // Register the client
            builder.Register(c => apiClientContex.GetShipmentsClient())
                .As<IShipmentsClient>()
                .InstancePerHttpRequest();

            return builder.Build();
        }
    }
}