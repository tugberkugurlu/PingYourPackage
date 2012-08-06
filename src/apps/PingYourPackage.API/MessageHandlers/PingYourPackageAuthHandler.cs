using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.Domain.Services;

namespace PingYourPackage.API.MessageHandlers {

    public class PingYourPackageAuthHandler : BasicAuthenticationHandler {

        protected override IPrincipal AuthenticateUser(HttpRequestMessage request, string username, string password, System.Threading.CancellationToken cancellationToken) {

            var membershipService = (IMembershipService)request.GetDependencyScope().GetService(typeof(IMembershipService));
            return membershipService.ValidateUser(username, password);
        }
    }
}