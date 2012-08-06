using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.Domain.Entities;

namespace PingYourPackage.Domain.Services {

    public class MembershipService : IMembershipService {

        private readonly IEntityRepository<User> _userRepository;
        private readonly IEntityRepository<Role> _roleRepository;
        private readonly IEntityRepository<UserInRole> _userInRoleRepository;
        private readonly ICryptoService _cryptoService;

        public MembershipService(
            IEntityRepository<User> userRepository, 
            IEntityRepository<Role> roleRepository,
            IEntityRepository<UserInRole> userInRoleRepository,
            ICryptoService cryptoService) {

            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userInRoleRepository = userInRoleRepository;
            _cryptoService = cryptoService;

        }

        public IPrincipal ValidateUser(string username, string password) {

            var user = _userRepository.GetSingleByUsername(username);

            if (user != null && isPasswordValid(user, password)) {
                
                var identity = new GenericIdentity(user.Name);
                var userInRoles = _userInRoleRepository.FindBy(x => x.UserKey == user.Key);

                var userRoleNames = new string[] { };
                if (userInRoles != null) {

                    var userRoleKeys = userInRoles.Select(x => x.RoleKey).ToArray();
                    userRoleNames = _roleRepository
                        .FindBy(x => userRoleKeys.Contains(x.Key))
                            .Select(x => x.Name).ToArray();
                }

                return new GenericPrincipal(identity, userRoleNames);
            }

            return null;
        }

        public void CreateUser(string username, string email, string password) {

            CreateUser(username, password, email, roles: null);
        }

        public void CreateUser(
            string username, string email, string password, string role) {

            CreateUser(username, password, email, roles: new[] { role });
        }

        //TODO: User doesn't contain E-mail column. Add one.
        public void CreateUser(
            string username, string email, string password, string[] roles) {

            var passwordSalt = _cryptoService.GenerateSalt();

            var user = new User() { 
                Name = username,
                Salt = passwordSalt,
                HashedPassword = _cryptoService.EncryptPassword(password, passwordSalt),
                CreatedOn = DateTime.Now
            };

            _userRepository.Add(user);
            _userRepository.Save();

            if (roles != null || roles.Length > 0) { 

                foreach (var roleName in roles) {

                    addUserToRole(user, roleName);
	            }
            }
        }

        public bool ChangePassword(
            string username, string oldPassword, string newPassword) {

            var user = _userRepository.GetSingleByUsername(username);

            if (user != null && isPasswordValid(user, oldPassword)) {

                user.HashedPassword = 
                    _cryptoService.EncryptPassword(newPassword, user.Salt);

                _userRepository.Edit(user);
                _userRepository.Save();

                return true;
            }

            return false;
        }

        public bool AddToRole(string username, string role) {

            var user = _userRepository.GetSingleByUsername(username);
            if (user != null) {

                addUserToRole(user, role);
                return true;
            }

            return false;
        }

        public bool RemoveFromRole(string username, string role) {

            var user = _userRepository.GetSingleByUsername(username);
            var roleEntity = _roleRepository.GetSingleByRoleName(role);

            if (user != null && roleEntity != null) {

                var userInRole = _userInRoleRepository.GetAll()
                    .FirstOrDefault(x => x.RoleKey == roleEntity.Key 
                        && x.UserKey == user.Key);

                if (userInRole != null) {

                    _userInRoleRepository.Delete(userInRole);
                    _userInRoleRepository.Save();
                }
            }

            return false;
        }

        private void addUserToRole(User user, string roleName) {

            var role = _roleRepository.GetSingleByRoleName(roleName);
            if (role == null) {

                var tempRole = new Role() {
                    Name = roleName
                };

                _roleRepository.Add(tempRole);
                _roleRepository.Save();
                role = tempRole;
            }

            var userInRole = new UserInRole() {
                RoleKey = role.Key,
                UserKey = user.Key
            };

            _userInRoleRepository.Add(userInRole);
            _userInRoleRepository.Save();
        }

        private bool isPasswordValid(User user, string password) {

            return string.Equals(
                    _cryptoService.EncryptPassword(
                        password, user.Salt), user.HashedPassword);
        }
    }
}