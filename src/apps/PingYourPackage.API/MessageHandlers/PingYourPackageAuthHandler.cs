using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using WebApiDoodle.Web.MessageHandlers;

namespace PingYourPackage.API.MessageHandlers {

    public class PingYourPackageAuthHandler : 
        BasicAuthenticationHandler {

        protected override Task<IPrincipal> AuthenticateUserAsync(
            HttpRequestMessage request, 
            string username, 
            string password, 
            CancellationToken cancellationToken) {

            var membershipService = request.GetMembershipService();

            var validUserCtx = membershipService
                .ValidateUser(username, password);

            return Task.FromResult(validUserCtx.Principal);
        }
    }
}