using PingYourPackage.API.Model;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

            return _membershipService.GetRole(key).ToRoleDto();
        }

        public RoleDto GetRole(string roleName) {

            return _membershipService.GetRole(roleName).ToRoleDto();
        }
    }
}