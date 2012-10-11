using System.Net.Http;
using System.Security.Principal;
using PingYourPackage.Domain.Services;
using WebAPIDoodle.Http;
using System.Threading;

namespace PingYourPackage.API.MessageHandlers {

    public class PingYourPackageAuthHandler : 
        BasicAuthenticationHandler {

        public PingYourPackageAuthHandler() 
            : base(suppressIfAlreadyAuthenticated: true) { }

        protected override IPrincipal AuthenticateUser(
            HttpRequestMessage request, 
            string username, 
            string password, 
            CancellationToken cancellationToken) {

            var membershipService = (IMembershipService)request
                .GetDependencyScope()
                .GetService(typeof(IMembershipService));

            return membershipService.ValidateUser(
                username, password);
        }

        protected override void HandleUnauthenticatedRequest(UnauthenticatedRequestContext context) {
            
            // Do nothing here. The Autharization 
            // will be handled by the AuthorizeAttribute.
        }
    }
}