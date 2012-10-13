using PingYourPackage.API.Model.Dtos;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model {
    
    internal static class UserWithRolesExtensions {

        internal static UserDto ToUserDto(this UserWithRoles userWithRoles) {

            if (userWithRoles == null) {

                throw new ArgumentNullException("userWithRoles");
            }

            if (userWithRoles.User == null) {

                throw new ArgumentNullException("userWithRoles.User");
            }

            return new UserDto { 
                Key = userWithRoles.User.Key,
                Name = userWithRoles.User.Name,
                Email = userWithRoles.User.Email,
                IsLocked = userWithRoles.User.IsLocked,
                CreatedOn = userWithRoles.User.CreatedOn,
                LastUpdatedOn = userWithRoles.User.LastUpdatedOn,
                Roles = userWithRoles.Roles.Select(
                    role => role.ToRoleDto()
                )
            };
        }
    }
}