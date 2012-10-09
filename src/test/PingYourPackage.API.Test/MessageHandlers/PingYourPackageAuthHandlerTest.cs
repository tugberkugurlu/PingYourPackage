using PingYourPackage.API.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq.Protected;
using Moq;
using WebAPIDoodle.Http;
using System.Security.Principal;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Dependencies;
using System.Web.Http.Hosting;
using PingYourPackage.Domain.Services;
using Xunit;

namespace PingYourPackage.API.Test.MessageHandlers {
    
    public class PingYourPackageAuthHandlerTest {

        [Fact]
        public void AuthenticateUser_Returns_IPrincipal_If_Valid() {

            string validUsername = "tugberk";
            string validPassword = "12345678";
            var req = SetUpAuthTestInfrastructure(validUsername, validPassword);

            var authHandler = new BasicAuthHandlerTestHelper();
            var returnedPrincipal = authHandler.RunAuthenticateUserMethod(
                req, validUsername, validPassword, default(CancellationToken));

            Assert.Equal(validUsername, returnedPrincipal.Identity.Name);
        }

        [Fact]
        public void AuthenticateUser_Returns_null_If_Not_Valid() {

            string validUsername = "tugberk";
            string validPassword = "12345678";
            string invalidUsername = "tgbrkug";
            string invalidPassword = "13578";

            HttpRequestMessage req = 
                SetUpAuthTestInfrastructure(validUsername, validPassword);

            var authHandler = new BasicAuthHandlerTestHelper();
            var returnedPrincipal = authHandler.RunAuthenticateUserMethod(
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
            membershipServiceMock.Setup(ms => ms.ValidateUser(username, password))
                .Returns(principal);

            var dependencyScopeMock = new Mock<IDependencyScope>();
            dependencyScopeMock.Setup(ds => ds.GetService(
                typeof(IMembershipService))
            ).Returns(membershipServiceMock.Object);

            request.Properties[HttpPropertyKeys.DependencyScope] =
                dependencyScopeMock.Object;

            return request;
        }

        private class BasicAuthHandlerTestHelper : PingYourPackageAuthHandler {

            public IPrincipal RunAuthenticateUserMethod(
                HttpRequestMessage request,
                string username,
                string password,
                CancellationToken cancellationToken) {

                return AuthenticateUser(request, username, 
                    password, cancellationToken);
            }
        }
    }
}