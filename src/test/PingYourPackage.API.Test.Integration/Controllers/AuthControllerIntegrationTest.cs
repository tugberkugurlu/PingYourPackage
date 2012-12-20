using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Xunit;

namespace PingYourPackage.API.Test.Integration.Controllers {
    
    public class AuthControllerIntegrationTest {

        [Fact, NullCurrentPrincipal]
        public async Task 
            Returns_200_And_Expected_User_For_The_Authed_User() {

            // Arrange
            var mockMemSrv = ServicesMockHelper
                .GetInitialMembershipServiceMock();

            mockMemSrv.Setup(ms =>
                ms.GetUser(Constants.ValidAffiliateUserName)
            ).Returns<string>(
                userName => new UserWithRoles {
                    User = new User {
                        Key = Guid.NewGuid(),
                        Name = userName,
                        Email = "foo@bar.com",
                        IsLocked = false,
                        CreatedOn = DateTime.Now.AddDays(-10),
                        LastUpdatedOn = DateTime.Now.AddDays(-5)
                    },
                    Roles = new List<Role> { 
                        new Role { Key = Guid.NewGuid(), Name = "Affiliate" }
                    }
                }
            );

            var config = IntegrationTestHelper
                .GetInitialIntegrationTestConfig(
                    GetInitialServices(mockMemSrv.Object));

            var request = HttpRequestMessageHelper
                .ConstructRequest(
                    httpMethod: HttpMethod.Get,
                    uri: string.Format(
                        "https://localhost/{0}", "api/auth"),
                    mediaType: "application/json",
                    username: Constants.ValidAffiliateUserName,
                    password: Constants.ValidAffiliatePassword);

            // Act
            var user = await IntegrationTestHelper.GetResponseMessageBodyAsync<UserDto>(config, request, HttpStatusCode.OK);

            // Assert
            Assert.Equal(Constants.ValidAffiliateUserName, user.Name);
            Assert.True(user.Roles.Any(
                role => role.Name.Equals(
                        "Affiliate", 
                        StringComparison.OrdinalIgnoreCase
                    )
                )
            );
        }

        private static IContainer GetInitialServices(
            IMembershipService memSrv) {

            var builder = IntegrationTestHelper
                .GetEmptyContainerBuilder();

            builder.Register(c => memSrv)
                .As<IMembershipService>()
                .InstancePerApiRequest();

            return builder.Build();
        }
    }
}