using PingYourPackage.API.Model;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.Domain.Services;
using System.Security.Principal;
using System.Web.Http;

namespace PingYourPackage.API.Controllers {
    
    [Authorize]
    public class AuthController : ApiController {

        private readonly IMembershipService _membershipService;

        public AuthController(
            IMembershipService membershipService) {

            _membershipService = membershipService;
        }

        public UserDto GetUser(IPrincipal principal) {

            // It's certain that the user exists and the principal is not null
            // because the request wouldn't arrive here unless the user exists

            UserWithRoles user = _membershipService
                .GetUser(principal.Identity.Name);

            return user.ToUserDto();
        }
    }
}