using PingYourPackage.API.Model;
using PingYourPackage.API.Model.Dtos;
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
    public class RolesController : ApiController {

        private readonly IMembershipService _membershipService;

        public RolesController(IMembershipService membershipService) {

            _membershipService = membershipService;
        }

        public IEnumerable<RoleDto> GetRoles() {

            return _membershipService
                   .GetRoles()
                   .Select(role => role.ToRoleDto());
        }

        public RoleDto GetRole(Guid key) {

            var role = _membershipService.GetRole(key);
            if (role == null) {

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return role.ToRoleDto();
        }

        public RoleDto GetRole(string roleName) {

            var role = _membershipService.GetRole(roleName);
            if (role == null) {

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return role.ToRoleDto();
        }
    }
}