using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using PingYourPackage.Domain.Entities;

namespace PingYourPackage.API.Controllers {

    public class PackageSendersController : ApiController {

        private readonly IEntityRepository<PackageSender> _packageSenderRepository;

        public PackageSendersController(IEntityRepository<PackageSender> packageSenderRepository) {

            _packageSenderRepository = packageSenderRepository;
        }

        public string[] Get() {

            return new[] { 
                "1",
                "2",
                "3"
            };
        }
    }
}
