using Autofac;
using PingYourPackage.API.Config;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Threading;

namespace PingYourPackage.API.Test.Integration.Controllers {
    
    public class RolesControllerIntegrationTest : IDisposable {

        private readonly HttpServer _httpServer;

        public RolesControllerIntegrationTest() {

            var config = IntegrationTestHelper.GetInitialIntegrationTestConfig(RegisterServices());
            _httpServer = new HttpServer(config);
        }

        [Fact, NullCurrentPrincipal]
        public async Task Returns_Unauthorized_Response_If_Request_Is_Not_Authorized() {

            // Arrange
            var client = new HttpClient(_httpServer);
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost/api/roles");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //Act
            var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private static IContainer RegisterServices() {

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(
                Assembly.GetAssembly(typeof(WebAPIConfig))).PropertiesAutowired();

            //Repositories

            //services
            builder.RegisterType<CryptoService>()
                .As<ICryptoService>()
                .InstancePerApiRequest();

            return builder.Build();
        }

        public void Dispose() {

            _httpServer.Dispose();
        }
    }
}