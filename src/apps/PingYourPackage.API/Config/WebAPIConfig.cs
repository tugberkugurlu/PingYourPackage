using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using PingYourPackage.API.MessageHandlers;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;
using PingYourPackage.API.Formatting;
using System.Web.Http.ModelBinding;
using WebAPIDoodle.Filters;

namespace PingYourPackage.API.Config {

    public class WebAPIConfig {

        public static void Configure(HttpConfiguration config) { 

            //Message Handlers
            config.MessageHandlers.Add(new RequireHttpsMessageHandler());
            config.MessageHandlers.Add(new PingYourPackageAuthHandler());

            //Formatters
            var jqueryFormatter = config.Formatters.FirstOrDefault(
                x => x.GetType() == 
                    typeof(JQueryMvcFormUrlEncodedFormatter));

            config.Formatters.Remove(
                config.Formatters.FormUrlEncodedFormatter);

            config.Formatters.Remove(jqueryFormatter);

            // Suppressing the IRequiredMemberSelector for all formatters
            foreach (var formatter in config.Formatters) {

                formatter.RequiredMemberSelector = 
                    new SuppressedRequiredMemberSelector();
            }

            //Filters
            config.Filters.Add(
                new InvalidModelStateFilterAttribute());

            //Default Services

            // If ExcludeMatchOnTypeOnly is true then we don't match on type only which means
            // that we return null if we can't match on anything in the request. This is useful
            // for generating 406 (Not Acceptable) status codes.
            config.Services.Replace(
                typeof(IContentNegotiator),
                new DefaultContentNegotiator(
                    excludeMatchOnTypeOnly: true));

            config.Services.RemoveAll(
                typeof(ModelValidatorProvider),
                validator => !(validator 
                    is DataAnnotationsModelValidatorProvider));
        }
    }
}