using PingYourPackage.API.Model;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PingYourPackage.API.Controllers {

    [Authorize(Roles = "Admin")]
    public class UsersController : ApiController {

        private readonly IMembershipService _membershipService;

        public UsersController(IMembershipService membershipService) {

            _membershipService = membershipService;
        }

        public PaginatedDto<UserDto> GetUsers(
            PaginatedRequestCommand cmd) {

            var users = _membershipService
                .GetUsers(cmd.Page, cmd.Take);

            return users.ToPaginatedDto(
                users.Select(user => user.ToUserDto()));
        }

        public UserDto GetUser(Guid key) {

            var user = _membershipService.GetUser(key);
            if (user == null) {

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return user.ToUserDto();
        }
    }
}