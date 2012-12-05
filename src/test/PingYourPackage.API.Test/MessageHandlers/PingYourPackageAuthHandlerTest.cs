using PingYourPackage.API.MessageHandlers;
using System.Security.Principal;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Dependencies;
using System.Web.Http.Hosting;
using PingYourPackage.Domain.Services;
using Xunit;
using Moq;
using System.Threading.Tasks;

namespace PingYourPackage.API.Test.MessageHandlers {
    
    public class PingYourPackageAuthHandlerTest {

        [Fact]
        public async Task AuthenticateUser_Returns_IPrincipal_If_Valid() {

            string validUsername = "tugberk";
            string validPassword = "12345678";
            var req = SetUpAuthTestInfrastructure(validUsername, validPassword);

            var authHandler = new BasicAuthHandlerTestHelper();
            var returnedPrincipal = await authHandler.RunAuthenticateUserMethodAsync(
                req, validUsername, validPassword, default(CancellationToken));

            Assert.Equal(validUsername, returnedPrincipal.Identity.Name);
        }

        [Fact]
        public async Task AuthenticateUser_Returns_null_If_Not_Valid() {

            string validUsername = "tugberk";
            string validPassword = "12345678";
            string invalidUsername = "tgbrkug";
            string invalidPassword = "13578";

            HttpRequestMessage req = 
                SetUpAuthTestInfrastructure(validUsername, validPassword);

            var authHandler = new BasicAuthHandlerTestHelper();
            var returnedPrincipal = await authHandler.RunAuthenticateUserMethodAsync(
                req, invalidUsername, invalidPassword, default(CancellationToken));

            Assert.Null(returnedPrincipal);
        }

        private HttpRequestMessage SetUpAuthTestInfrastructure(
            string username, string password) {

            var principal = new GenericPrincipal(
                new GenericIdentity(username),
                new[] { "Admin" });

            var request = new HttpRequestMessage();

            var membershipServiceMock = new Mock<IMembershipService>();
            membershipServiceMock.Setup(ms =>
                ms.ValidateUser(It.IsAny<string>(), It.IsAny<string>())
            ).Returns<string, string>((u, p) => 
                (u == username && p == password) ?
                new ValidUserContext { 
                    Principal = principal
                }
                : new ValidUserContext()
            );

            var dependencyScopeMock = new Mock<IDependencyScope>();
            dependencyScopeMock.Setup(
                ds => ds.GetService(typeof(IMembershipService))
            ).Returns(membershipServiceMock.Object);

            request.Properties[HttpPropertyKeys.DependencyScope] =
                dependencyScopeMock.Object;

            return request;
        }

        private class BasicAuthHandlerTestHelper : PingYourPackageAuthHandler {

            public Task<IPrincipal> RunAuthenticateUserMethodAsync(
                HttpRequestMessage request,
                string username,
                string password,
                CancellationToken cancellationToken) {

                return AuthenticateUserAsync(request, username, 
                    password, cancellationToken);
            }
        }
    }
}