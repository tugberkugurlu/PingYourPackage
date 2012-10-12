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

        internal static Mock<IMembershipService> GetMembershipService(
            string validUserName, string validPassword, string[] userRoles) {

            var principal = new GenericPrincipal(
                new GenericIdentity(validUserName),
                userRoles);

            var membershipServiceMock = new Mock<IMembershipService>();
            membershipServiceMock.Setup(ms =>
                    ms.ValidateUser(validUserName, validPassword))
                .Returns(principal);

            return membershipServiceMock;
        }
    }
}