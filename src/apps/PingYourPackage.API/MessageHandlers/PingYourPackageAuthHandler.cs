using System.Net.Http;
using System.Security.Principal;
using PingYourPackage.Domain.Services;
using WebAPIDoodle.Http;
using System.Threading;

namespace PingYourPackage.API.MessageHandlers {

    public class PingYourPackageAuthHandler : BasicAuthenticationHandler {

        protected override IPrincipal AuthenticateUser(
            HttpRequestMessage request, 
            string username, 
            string password, 
            CancellationToken cancellationToken) {

            IMembershipService membershipService = (IMembershipService)request
                .GetDependencyScope()
                .GetService(typeof(IMembershipService));

            return membershipService.ValidateUser(username, password);
        }
    }
}