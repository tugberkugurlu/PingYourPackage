using Autofac;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Xunit;

namespace PingYourPackage.API.Test.Integration {
    
    internal static class IntegrationAuthTestHelper {

        internal static async Task TestRequestAuthorization(
            HttpMethod httpMethod, 
            string path,
            string mediaType, 
            string testUserName,
            string testPassword,
            HttpStatusCode expectedStatus) {

            // Arrange
            var config = IntegrationTestHelper
                .GetInitialIntegrationTestConfig(GetInitialServices());

            using (var httpServer = new HttpServer(config))
            using (var client = httpServer.ToHttpClient()) {

                var request = HttpRequestMessageHelper.ConstructRequest(
                    httpMethod: httpMethod,
                    uri: string.Format("https://localhost/{0}", path),
                    mediaType: mediaType,
                    username: testUserName,
                    password: testPassword);

                //Act
                var response = await client.SendAsync(request);

                //Assert
                Assert.Equal(expectedStatus, response.StatusCode);
            }
        }

        private static IContainer GetInitialServices() {

            var builder = IntegrationTestHelper.GetInitialContainerBuilder();
            return builder.Build();
        }
    }
}