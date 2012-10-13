using PingYourPackage.API.Model.Dtos;
using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model {
    
    internal static class RoleExtensions {

        internal static RoleDto ToRoleDto(this Role role) {

            if (role == null) {

                throw new ArgumentNullException("role");
            }

            return new RoleDto { 
                Key = role.Key,
                Name = role.Name
            };
        }
    }
}