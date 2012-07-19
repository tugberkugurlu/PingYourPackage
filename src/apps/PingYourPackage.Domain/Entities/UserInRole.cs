using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class UserInRole : IEntity {

        [Key]
        public int Key { get; set; }

        public int UserKey { get; set; }
        public int RoleKey { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}