using Autofac;
using Autofac.Integration.WebApi;
using PingYourPackage.Domain.Services;
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

            var request = HttpRequestMessageHelper.ConstructRequest(
                httpMethod: httpMethod,
                uri: string.Format("https://localhost/{0}", path),
                mediaType: mediaType,
                username: testUserName,
                password: testPassword);

            //Act
            var response = await IntegrationTestHelper.GetResponseAsync(config, request);

            //Assert
            Assert.Equal(expectedStatus, response.StatusCode);
        }

        private static IContainer GetInitialServices() {

            var builder = IntegrationTestHelper.GetEmptyContainerBuilder();

            builder.Register(c =>
                ServicesMockHelper.GetInitialMembershipServiceMock().Object)
                .As<IMembershipService>()
                .InstancePerApiRequest();

            return builder.Build();
        }
    }
}