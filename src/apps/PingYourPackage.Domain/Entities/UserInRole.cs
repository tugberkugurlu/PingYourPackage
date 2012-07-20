using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class UserInRole : IEntity {

        [Key]
        public Guid Key { get; set; }
        public Guid UserKey { get; set; }
        public Guid RoleKey { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}