using PingYourPackage.API.Filters;
using PingYourPackage.API.Model;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public PaginatedDto<UserDto> GetUsers(PaginatedRequestCommand cmd) {

            var users = _membershipService.GetUsers(cmd.Page, cmd.Take);

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

        [EmptyParameterFilter("requestModel")]
        public HttpResponseMessage PostUser(UserRequestModel requestModel) {

            var createdUserResult = 
                _membershipService.CreateUser(
                    requestModel.Name, requestModel.Email, 
                    requestModel.Password, requestModel.Roles);

            if (!createdUserResult.IsSuccess) {

                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            var response = Request.CreateResponse(HttpStatusCode.Created,
                createdUserResult.Entity.ToUserDto());

            response.Headers.Location = new Uri(Url.Link("DefaultHttpRoute",
                    new { key = createdUserResult.Entity.User.Key }));

            return response;
        }

        [EmptyParameterFilter("requestModel")]
        public UserDto PutUser(Guid key, UserUpdateRequestModel requestModel) {

            var user = _membershipService.GetUser(key);
            if (user == null) {

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var userWithRoles = _membershipService.UpdateUser(
                user.User, requestModel.Name, requestModel.Email);

            return userWithRoles.ToUserDto();
        }
    }
}