using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Services {

    public interface IMembershipService {

        IPrincipal ValidateUser(string username, string password);

        void CreateUser(string username, string email, string password);
        void CreateUser(string username, string email, string password, string role);
        void CreateUser(string username, string email, string password, string[] roles);

        bool ChangePassword(string username, string oldPassword, string newPassword);

        bool AddToRole(string username, string role);
        bool RemoveFromRole(string username, string role);
    }
}