using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Services {
    
    public class CreatedUserResult {

        public CreatedUserResult(bool isSuccess) {

            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; private set; }
        public UserWithRoles User { get; set; }
    }
}
