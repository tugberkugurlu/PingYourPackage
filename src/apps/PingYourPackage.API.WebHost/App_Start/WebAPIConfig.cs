using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using PingYourPackage.API.MessageHandlers;

namespace PingYourPackage.API.WebHost {

    public class WebAPIConfig {

        public static void Configure(HttpConfiguration config) { 

            //Message Handlers
            config.MessageHandlers.Add(new PingYourPackageAuthHandler());

            //Formatters

            //Filters
            config.Filters.Add(new AuthorizeAttribute());

            //Default Services
        }
    }
}