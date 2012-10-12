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

        public string[] GetRoles() {

            return new[] { 
                "Admin",
                "Employee",
                "Affiliate"
            };
        }

        public string[] GetRole(Guid key) {

            return new[] { 
                "Admin",
                "Employee",
                "Affiliate"
            };
        }

        public string GetRole(string roleName) {

            return roleName;
        }
    }
}
