using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Xunit;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Services;
using System.Collections.Generic;
using PingYourPackage.API.Model.Dtos;
using System.Linq;
using Moq;

namespace PingYourPackage.API.Test.Integration.Controllers {

    public class RolesControllerIntegrationTest {

        // GetRoles action method
        public class GetRoles {

            [Fact, NullCurrentPrincipal]
            public Task
                Returns_Unauthorized_Response_If_Request_Is_Not_Authorized() {

                return IntegrationAuthTestHelper.TestRequestAuthorization(
                    HttpMethod.Get,
                    "api/roles",
                    "application/json",
                    Constants.InvalidUserName,
                    Constants.InvalidPassword,
                    HttpStatusCode.Unauthorized);
            }

            [Fact, NullCurrentPrincipal]
            public Task
                Returns_Unauthorized_Response_If_Request_Is_By_Employee() {

                return IntegrationAuthTestHelper.TestRequestAuthorization(
                    HttpMethod.Get,
                    "api/roles",
                    "application/json",
                    Constants.ValidEmployeeUserName,
                    Constants.ValidEmployeePassword,
                    HttpStatusCode.Unauthorized);
            }

            [Fact, NullCurrentPrincipal]
            public Task
                Returns_Unauthorized_Response_If_Request_Is_By_Affiliate() {

                return IntegrationAuthTestHelper.TestRequestAuthorization(
                    HttpMethod.Get,
                    "api/roles",
                    "application/json",
                    Constants.ValidAffiliateUserName,
                    Constants.ValidAffiliatePassword,
                    HttpStatusCode.Unauthorized);
            }

            [Fact, NullCurrentPrincipal]
            public Task
                Does_Not_Returns_Unauthorized_Response_If_Request_Authorized() {

                return IntegrationAuthTestHelper.TestRequestAuthorization(
                    HttpMethod.Get,
                    "api/roles",
                    "application/json",
                    Constants.ValidAdminUserName,
                    Constants.ValidAdminPassword,
                    HttpStatusCode.OK);
            }

            [Fact, NullCurrentPrincipal]
            public async Task 
                Returns_200_And_Expected_Roles_If_Request_Authorized() {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService()));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}",
                            "api/roles"),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                // Act
                var roles = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<IEnumerable<UserDto>>(
                        config, request, HttpStatusCode.OK);

                // Assert
                Assert.Equal(4, roles.Count());
                Assert.True(roles.Any(r => r.Name == "Admin"));
                Assert.True(roles.Any(r => r.Name == "Employee"));
                Assert.True(roles.Any(r => r.Name == "Affiliate"));
                Assert.True(roles.Any(r => r.Name == "Guest"));
            }

            private IMembershipService GetMembershipService() {

                var roles = GetDummyRoles(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipServiceMock();

                mockMemSrv.Setup(ms => ms.GetRoles()).Returns(roles);

                return mockMemSrv.Object;
            }
        }

        public class GetRole_Key {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_Expected_Role_If_Request_Authorized() {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var requestKey = keys[1];

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService(keys)));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/roles",
                            requestKey.ToString()),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                // Act
                var role = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<RoleDto>(
                        config, request, HttpStatusCode.OK);

                // Assert
                Assert.Equal(requestKey, role.Key);
            }

            [Fact, NullCurrentPrincipal]
            public async Task 
                Returns_404_NotFound_If_Not_Exist_With_Key() {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService(keys)));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/roles",
                            Guid.NewGuid().ToString()),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            private IMembershipService GetMembershipService(Guid[] keys) {

                var roles = GetDummyRoles(keys);
                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipServiceMock();

                mockMemSrv.Setup(ms => ms.GetRole(
                        It.Is<Guid>(key =>
                            keys.Contains(key)
                        )
                    )
                ).Returns<Guid>(key => 
                    roles.FirstOrDefault(
                        role => role.Key == key
                    )
                );

                return mockMemSrv.Object;
            }
        }

        public class GetRole_RoleName {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_Expected_Role_If_Request_Authorized() {

                var requestRoleName = "Admin";

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService()));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?roleName={1}",
                            "api/roles",
                            requestRoleName.ToLower()),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                var role = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<RoleDto>(
                        config, request, HttpStatusCode.OK);

                // Assert
                Assert.Equal(requestRoleName, role.Name, StringComparer.OrdinalIgnoreCase);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_Role_Does_Not_Exist() {

                var requestRoleName = "FooBar";

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService()));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?roleName={1}",
                            "api/roles",
                            requestRoleName.ToLower()),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            private IMembershipService GetMembershipService() {

                var roles = GetDummyRoles(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipServiceMock();

                mockMemSrv.Setup(ms => ms.GetRole(
                        It.Is<string>(name =>
                            roles.Any(
                                role => role.Name.Equals(
                                    name, StringComparison.OrdinalIgnoreCase
                                )
                            )
                        )
                    )
                ).Returns<string>(name =>
                    roles.FirstOrDefault(
                        role => role.Name.Equals(
                            name, StringComparison.OrdinalIgnoreCase
                        )
                    )
                );

                return mockMemSrv.Object;
            }
        }

        private static IContainer GetInitialServices(IMembershipService memSrv) {

            var builder = IntegrationTestHelper
                .GetEmptyContainerBuilder();

            builder.Register(c => memSrv)
                .As<IMembershipService>()
                .InstancePerApiRequest();

            return builder.Build();
        }

        private static IEnumerable<Role> GetDummyRoles(Guid[] keys) {

            List<Role> roles = new List<Role> {
                new Role { Key = keys[0], Name = "Admin" },
                new Role { Key = keys[1], Name = "Employee" },
                new Role { Key = keys[2], Name = "Affiliate" },
                new Role { Key = keys[3], Name = "Guest" }
            };

            return roles;
        }
    }
}