using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;
using PingYourPackage.API.Config;

namespace PingYourPackage.API.WebHost {

    public class Global : System.Web.HttpApplication {

        protected void Application_Start(object sender, EventArgs e) {
            
            var config = GlobalConfiguration.Configuration;

            RouteConfig.RegisterRoutes(config.Routes);
            WebAPIConfig.Configure(config);
            AutofacWebAPI.Initialize(config);
            EFConfig.Initialize();
        }
    }
}