using Moq;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Test.Integration {
    
    internal static class ServicesMockHelper {

        internal static Mock<IMembershipService> GetInitialMembershipService() {

            var membershipServiceMock = new Mock<IMembershipService>();

            var adminPrincipal = new GenericPrincipal(
                new GenericIdentity(Constants.ValidAdminUserName),
                new[] { "Admin" });

            membershipServiceMock.Setup(ms => ms.ValidateUser(
                Constants.ValidAdminUserName,
                Constants.ValidAdminPassword))
                .Returns(new ValidUserContext {
                    Principal = adminPrincipal
                });

            var employeePrincipal = new GenericPrincipal(
                new GenericIdentity(Constants.ValidEmployeeUserName),
                new[] { "Employee" });

            membershipServiceMock.Setup(ms => ms.ValidateUser(
                Constants.ValidEmployeeUserName,
                Constants.ValidEmployeePassword))
                .Returns(new ValidUserContext {
                    Principal = employeePrincipal
                });

            var affiliatePrincipal = new GenericPrincipal(
                new GenericIdentity(Constants.ValidAffiliateUserName),
                new[] { "Affiliate" });

            membershipServiceMock.Setup(ms => ms.ValidateUser(
                Constants.ValidAffiliateUserName,
                Constants.ValidAffiliatePassword))
                .Returns(new ValidUserContext {
                    Principal = affiliatePrincipal
                });

            // For invalid user
            membershipServiceMock.Setup(ms => ms.ValidateUser(
                Constants.InvalidUserName,
                Constants.InvalidPassword))
                .Returns(new ValidUserContext());

            return membershipServiceMock;
        }
    }
}