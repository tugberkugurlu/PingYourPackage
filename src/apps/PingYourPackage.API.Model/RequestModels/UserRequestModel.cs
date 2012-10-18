using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model.RequestModels {

    public class UserRequestModel 
        : UserBaseRequestModel {

        [Required]
        [StringLength(30, MinimumLength  = 8)]
        public string Password { get; set; }

        [MinLength(1)]
        public string[] Roles { get; set; }
    }
}