using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PingYourPackage.API.Test.Integration {
    
    internal static class HttpServerExtensions {

        internal static HttpClient ToHttpClient(
            this HttpServer httpServer) {

            return new HttpClient(httpServer);
        }
    }
}