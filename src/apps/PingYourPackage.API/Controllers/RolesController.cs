using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PingYourPackage.API.Controllers {

    [Authorize]
    public class RolesController : ApiController {

        public string Get() {

            return "OK";
        }
    }
}
