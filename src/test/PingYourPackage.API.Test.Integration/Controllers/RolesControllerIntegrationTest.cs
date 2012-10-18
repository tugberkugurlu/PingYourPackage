using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using Xunit;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Services;
using System.Collections.Generic;
using PingYourPackage.API.Model.Dtos;
using System.Linq;
using Moq;
using System.Globalization;

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
            public async Task Returns_Expected_Roles() {

                // Arrange
                var mockMemSrv = ServicesMockHelper.GetInitialMembershipService();
                mockMemSrv.Setup(ms => ms.GetRoles()).Returns(() => {

                    return new[] { 
                        new Role { Key = Guid.NewGuid(), Name = "Admin" },
                        new Role { Key = Guid.NewGuid(), Name = "Employee" },
                        new Role { Key = Guid.NewGuid(), Name = "Affiliate" },
                        new Role { Key = Guid.NewGuid(), Name = "Guest" }
                    };
                });

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetInitialServices(mockMemSrv.Object));

                using (var httpServer = new HttpServer(config))
                using (var client = httpServer.ToHttpClient()) {

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
                    var response = await client.SendAsync(request);
                    var roles = await response.Content.ReadAsAsync<IEnumerable<RoleDto>>();

                    // Assert
                    Assert.Equal(4, roles.Count());
                    Assert.True(roles.Any(r => r.Name == "Admin"));
                    Assert.True(roles.Any(r => r.Name == "Employee"));
                    Assert.True(roles.Any(r => r.Name == "Affiliate"));
                    Assert.True(roles.Any(r => r.Name == "Guest"));
                }
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

        public class GetRole {

            [Fact, NullCurrentPrincipal]
            public async Task 
                Returns_Expected_Role_With_Key() {

                // Arrange
                Guid key1 = Guid.NewGuid(),
                     key2 = Guid.NewGuid(),
                     key3 = Guid.NewGuid(),
                     key4 = Guid.NewGuid();

                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipService();

                mockMemSrv.Setup(ms => ms.GetRole(
                        It.Is<Guid>(k =>
                            k == key1 || k == key2 || 
                            k == key3 || k == key4
                        )
                    )
                ).Returns<Guid>(key => new Role { 
                    Key = key, Name = "FooBar"
                });

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetInitialServices(mockMemSrv.Object));

                using (var httpServer = new HttpServer(config))
                using (var client = httpServer.ToHttpClient()) {

                    var request = HttpRequestMessageHelper
                        .ConstructRequest(
                            httpMethod: HttpMethod.Get,
                            uri: string.Format(
                                "https://localhost/{0}/{1}", 
                                "api/roles", 
                                key2.ToString()),
                            mediaType: "application/json",
                            username: Constants.ValidAdminUserName,
                            password: Constants.ValidAdminPassword);

                    // Act
                    var response = await client.SendAsync(request);
                    var role = await response.Content.ReadAsAsync<RoleDto>();

                    // Assert
                    Assert.Equal(key2, role.Key);
                    Assert.Equal("FooBar", role.Name);
                }
            }

            [Fact, NullCurrentPrincipal]
            public async Task 
                Returns_404_NotFound_If_Not_Exist_With_Key() {

                // Arrange
                Guid key1 = Guid.NewGuid(),
                     key2 = Guid.NewGuid(),
                     key3 = Guid.NewGuid(),
                     key4 = Guid.NewGuid();

                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipService();

                mockMemSrv.Setup(ms => ms.GetRole(
                        It.Is<Guid>(k =>
                            k == key1 || k == key2 ||
                            k == key3 || k == key4
                        )
                    )
                ).Returns<Guid>(key => new Role {
                    Key = key,
                    Name = "FooBar"
                });

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetInitialServices(mockMemSrv.Object));

                using (var httpServer = new HttpServer(config))
                using (var client = httpServer.ToHttpClient()) {

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
                    var response = await client.SendAsync(request);

                    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                }
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

        public class PostRole {

            [Fact, NullCurrentPrincipal]
            public async Task 
                Returns_Expected_Role_With_RoleName() {

                var key = Guid.NewGuid();
                var roleNames = new[] { 
                    "Admin", "Employee", "Guest"
                };

                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipService();

                mockMemSrv.Setup(ms => ms.GetRole(
                        It.Is<string>(name =>
                            roleNames.Contains(
                                name, StringComparer.OrdinalIgnoreCase)
                        )
                    )
                ).Returns<string>(name => new Role {
                    Key = key,
                    Name = name
                });

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetInitialServices(mockMemSrv.Object));

                using (var httpServer = new HttpServer(config))
                using (var client = httpServer.ToHttpClient()) {

                    var request = HttpRequestMessageHelper
                        .ConstructRequest(
                            httpMethod: HttpMethod.Get,
                            uri: string.Format(
                                "https://localhost/{0}?roleName={1}",
                                "api/roles",
                                roleNames[2].ToLower()),
                            mediaType: "application/json",
                            username: Constants.ValidAdminUserName,
                            password: Constants.ValidAdminPassword);

                    // Act
                    var response = await client.SendAsync(request);
                    var role = await response.Content.ReadAsAsync<RoleDto>();

                    // Assert
                    Assert.Equal(key, role.Key);
                    Assert.Equal(roleNames[2], role.Name, StringComparer.OrdinalIgnoreCase);
                }
            }

            [Fact, NullCurrentPrincipal]
            public async Task 
                Returns_404_NotFound_If_Not_Exist_With_RoleName() {

                var key = Guid.NewGuid();
                var roleNames = new[] { 
                    "Admin", "Employee", "Guest"
                };

                var invalidRoleName = "FooBar";

                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipService();

                mockMemSrv.Setup(ms => ms.GetRole(
                        It.Is<string>(name =>
                            roleNames.Contains(
                                name, StringComparer.OrdinalIgnoreCase)
                        )
                    )
                ).Returns<string>(name => new Role {
                    Key = key,
                    Name = name
                });

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetInitialServices(mockMemSrv.Object));

                using (var httpServer = new HttpServer(config))
                using (var client = httpServer.ToHttpClient()) {

                    var request = HttpRequestMessageHelper
                        .ConstructRequest(
                            httpMethod: HttpMethod.Get,
                            uri: string.Format(
                                "https://localhost/{0}?roleName={1}",
                                "api/roles",
                                invalidRoleName),
                            mediaType: "application/json",
                            username: Constants.ValidAdminUserName,
                            password: Constants.ValidAdminPassword);

                    // Act
                    var response = await client.SendAsync(request);

                    // Assert
                    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                }
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
}