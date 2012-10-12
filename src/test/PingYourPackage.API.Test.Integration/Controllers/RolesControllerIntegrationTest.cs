using Autofac;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using Xunit;

namespace PingYourPackage.API.Test.Integration.Controllers {
    
    public class RolesControllerIntegrationTest {

        private const string ValidUserName = "tugberk";
        private const string ValidPassword = "12345678";
        private const string InvalidUserName = "tgbrk";
        private const string InvalidPassword = "87654321";

        [Fact, NullCurrentPrincipal]
        public async Task 
            Returns_Unauthorized_Response_If_Request_Is_Not_Authorized() {

            // Arrange
            var config = IntegrationTestHelper
                .GetInitialIntegrationTestConfig(
                    GetInitialServices(
                        ValidUserName, ValidPassword, new[] { "Admin" }));

            using (var httpServer = new HttpServer(config))
            using (var client = httpServer.ToHttpClient()) {

                var request = HttpRequestMessageHelper.ConstructRequest(
                    httpMethod: HttpMethod.Get,
                    uri: "https://localhost/api/roles",
                    mediaType: "application/json",
                    username: InvalidUserName,
                    password: InvalidPassword);

                //Act
                var response = await client.SendAsync(request);

                //Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Fact, NullCurrentPrincipal]
        public async Task 
            Returns_Unauthorized_Response_If_Request_Is_Not_By_Admin() {

            // Arrange
            var config = IntegrationTestHelper
                .GetInitialIntegrationTestConfig(
                    GetInitialServices(
                        ValidUserName, ValidPassword, new[] { "Employee" }));

            using (var httpServer = new HttpServer(config))
            using (var client = httpServer.ToHttpClient()) {

                var request = HttpRequestMessageHelper.ConstructRequest(
                    httpMethod: HttpMethod.Get,
                    uri: "https://localhost/api/roles",
                    mediaType: "application/json",
                    username: ValidUserName,
                    password: ValidPassword);

                //Act
                var response = await client.SendAsync(request);

                //Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Fact, NullCurrentPrincipal]
        public async Task 
            Does_Not_Returns_Unauthorized_Response_If_Request_Authorized() {
            
            // Arrange
            var config = IntegrationTestHelper
                .GetInitialIntegrationTestConfig(
                    GetInitialServices(
                        ValidUserName, ValidPassword, new[] { "Admin" }));

            using (var httpServer = new HttpServer(config))
            using (var client = httpServer.ToHttpClient()) {

                var request = HttpRequestMessageHelper.ConstructRequest(
                    httpMethod: HttpMethod.Get,
                    uri: "https://localhost/api/roles",
                    mediaType: "application/json",
                    username: ValidUserName,
                    password: ValidPassword);

                //Act
                var response = await client.SendAsync(request);

                //Assert
                Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        public class GetRolesActionTest { 
        }

        public class GetRoleActionTest {
        }

        public class PostRoleActionTest {
        }

        private static IContainer GetInitialServices(
            string validUserName,
            string validPassword,
            string[] userRoles) {

            var builder = IntegrationTestHelper.GetInitialContainerBuilder(
                validUserName, validPassword, userRoles);

            return builder.Build();
        }
    }
}