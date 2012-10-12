using PingYourPackage.API.Model.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model.RequestCommands {

    public class PaginatedRequestCommand : IRequestCommand {

        [Minimum(1)]
        public int Page { get; set; }

        [Minimum(1)]
        public int Take { get; set; }
    }
}