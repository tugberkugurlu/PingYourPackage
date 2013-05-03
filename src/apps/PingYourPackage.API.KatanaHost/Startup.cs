using Owin;
using PingYourPackage.API.Config;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Web.Http;

namespace PingYourPackage.API.KatanaHost {

    public partial class Startup {

        public void Configuration(IAppBuilder app) {

            HttpConfiguration config = new HttpConfiguration();
            AutofacWebAPI.Initialize(config);
            RouteConfig.RegisterRoutes(config);
            WebAPIConfig.Configure(config);

            var efMigrationSettings = new PingYourPackage.Domain.Migrations.Configuration();
            var efMigrator = new DbMigrator(efMigrationSettings);
            efMigrator.Update();

            RegisterAddresses(app);
            app.UseWebApi(config);
        }

        private void RegisterAddresses(IAppBuilder app) {

            Uri HostUri = new Uri("http://localhost:16897/");
            Uri HostUriHttps = new Uri("https://localhost:44368/");

            // NOTE: OwinServerFactoryAttribute.Create method and 
            //       OwinHttpListener.Start should give you a hint on how
            //       to register the addresses.
            app.Properties["host.Addresses"] = new List<IDictionary<string, object>> {
                new Dictionary<string, object> { 
                    { "scheme", HostUri.Scheme },
                    { "host", HostUri.Host },
                    { "port", HostUri.Port.ToString()  }
                },

                new Dictionary<string, object> { 
                    { "scheme", HostUriHttps.Scheme },
                    { "host", HostUriHttps.Host },
                    { "port", HostUriHttps.Port.ToString()  }
                }
            };
        }
    }
}