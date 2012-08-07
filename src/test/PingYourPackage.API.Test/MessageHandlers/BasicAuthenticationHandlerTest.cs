using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PingYourPackage.API.MessageHandlers;
using PingYourPackage.API.Test.xUnit;
using Xunit;

namespace PingYourPackage.API.Test.MessageHandlers {

    public class BasicAuthenticationHandlerTest {

        private const string UserName = "foo";
        private const string Password = "bar";

        [Fact]
        public Task BasicAuthenticationHandler_ReturnsUnauthorizedIfAuthorizationHeaderIsNotSupplied() {

            //Arange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            var customBasicAuthHandler = new CustomBasicAuthHandler();

            //Act
            return TestHelper.InvokeMessageHandler(request, customBasicAuthHandler)

                .ContinueWith(task => {
                    
                    //Assert
                    Assert.Equal(TaskStatus.RanToCompletion, task.Status);
                    Assert.Equal(HttpStatusCode.Unauthorized, task.Result.StatusCode);
                });
        }

        [Fact]
        public Task BasicAuthenticationHandler_ReturnsUnauthorizedWhenAuthorizationHeaderIsNotVerified() {

            //Arange
            string usernameAndPassword = string.Format("{0}:{1}", "guydy", "efuıry");
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", EncodeToBase64(usernameAndPassword));
            var basicAuthHandler = new CustomBasicAuthHandler();

            //Act
            return TestHelper.InvokeMessageHandler(request, basicAuthHandler)

                .ContinueWith(task => {

                    //Assert
                    Assert.Equal(TaskStatus.RanToCompletion, task.Status);
                    Assert.Equal(HttpStatusCode.Unauthorized, task.Result.StatusCode);
                });
        }

        [Fact]
        public Task BasicAuthenticationHandler_Returns200WhenAuthorizationHeaderIsVerified() {

            //Arange
            string usernameAndPassword = string.Format("{0}:{1}", UserName, Password);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", EncodeToBase64(usernameAndPassword));
            var basicAuthHandler = new CustomBasicAuthHandler();

            //Act
            return TestHelper.InvokeMessageHandler(request, basicAuthHandler)

                .ContinueWith(task => {

                    //Assert
                    Assert.Equal(TaskStatus.RanToCompletion, task.Status);
                    Assert.Equal(HttpStatusCode.OK, task.Result.StatusCode);
                });
        }

        [Fact]
        public Task BasicAuthenticationHandler_SetsCurrentThreadPrincipalWhenAuthorizationHeaderIsVerified() {

            //Arange
            string usernameAndPassword = string.Format("{0}:{1}", UserName, Password);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", EncodeToBase64(usernameAndPassword));
            var basicAuthHandler = new CustomBasicAuthHandler();

            var principal = Thread.CurrentPrincipal;

            //Act
            return TestHelper.InvokeMessageHandler(request, basicAuthHandler)

                .ContinueWith(task => {

                    //Assert
                    Assert.Equal(TaskStatus.RanToCompletion, task.Status);
                    Assert.NotNull(Thread.CurrentPrincipal);
                    Assert.IsType<GenericPrincipal>(Thread.CurrentPrincipal);
                });
        }

        private static string EncodeToBase64(string value) {

            byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(value);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public class CustomBasicAuthHandler : BasicAuthenticationHandler {

            protected override IPrincipal AuthenticateUser(HttpRequestMessage request, string username, string password, CancellationToken cancellationToken) {

                if (username == UserName && password == Password) {

                    var identity = new GenericIdentity(username);
                    return new GenericPrincipal(identity, null);
                }

                return null;
            }
        }
    }
}
