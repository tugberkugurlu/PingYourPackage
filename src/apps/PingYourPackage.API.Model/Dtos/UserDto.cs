using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model.Dtos {

    public class UserDto : IDto {

        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public IEnumerable<RoleDto> Roles { get; set; }
    }
}