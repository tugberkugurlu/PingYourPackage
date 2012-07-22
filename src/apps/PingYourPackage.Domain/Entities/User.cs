using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class User : IEntity {

        [Key]
        public Guid Key { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        [Required]
        public string Salt { get; set; }

        public bool IsLocked { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public virtual ICollection<UserInRole> UserInRoles { get; set; }

        public User() {

            UserInRoles = new HashSet<UserInRole>();
        }
    }
}