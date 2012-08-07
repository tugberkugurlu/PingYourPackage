using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PingYourPackage.API.Test {
    
    internal static class TestHelper {

        internal static Task<HttpResponseMessage> InvokeMessageHandler(HttpRequestMessage request, DelegatingHandler handler, CancellationToken cancellationToken = default(CancellationToken)) {

            handler.InnerHandler = new DummyHandler();
            var invoker = new HttpMessageInvoker(handler);
            return invoker.SendAsync(request, cancellationToken);
        }

        private class DummyHandler : DelegatingHandler {

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                return Task.FromResult(response);
            }
        }
    }
}