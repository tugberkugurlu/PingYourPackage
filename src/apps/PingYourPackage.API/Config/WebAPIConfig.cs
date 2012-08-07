using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using PingYourPackage.API.MessageHandlers;

namespace PingYourPackage.API.Config {

    public class WebAPIConfig {

        public static void Configure(HttpConfiguration config) { 

            //Message Handlers
            //config.MessageHandlers.Add(new PingYourPackageAuthHandler());

            //Formatters

            //Filters

            //Default Services

            //NOTE: This will come with RTM
            // From DefaultContentNegotiator class:
            // If ExcludeMatchOnTypeOnly is true then we don't match on type only which means
            // that we return null if we can't match on anything in the request. This is useful
            // for generating 406 (Not Acceptable) status codes.
            //config.Services.Replace(
            //    typeof(IContentNegotiator),
            //    new DefaultContentNegotiator(excludeMatchOnTypeOnly: true)
            //);
        }
    }
}