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
using System.Web.Http.ModelBinding;
using Xunit;

namespace PingYourPackage.API.Test.Integration.Controllers {
    
    public class UsersControllerIntegrationTest {

        public class GetUsers {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_Expected_Users_If_Request_Authorized() {

                // Arrange
                var mockMemSrv = GetInitialMemSrvMockForUsers();

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(mockMemSrv.Object));

                using (var httpServer = new HttpServer(config))
                using (var client = httpServer.ToHttpClient()) {

                    var request = HttpRequestMessageHelper
                        .ConstructRequest(
                            httpMethod: HttpMethod.Get,
                            uri: string.Format(
                                "https://localhost/{0}?page={1}&take={2}", 
                                "api/users", 1, 2),
                            mediaType: "application/json",
                            username: Constants.ValidAdminUserName,
                            password: Constants.ValidAdminPassword);

                    // Act
                    var response = await client.SendAsync(request);
                    var userPaginatedDto = await response.Content.ReadAsAsync<PaginatedDto<UserDto>>();

                    // Assert
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.Equal(1, userPaginatedDto.PageIndex);
                    Assert.Equal(2, userPaginatedDto.TotalPageCount);
                    Assert.Equal(2, userPaginatedDto.Items.Count());
                    Assert.Equal(3, userPaginatedDto.TotalCount);
                    Assert.True(userPaginatedDto.HasNextPage);
                    Assert.False(userPaginatedDto.HasPreviousPage);
                }
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_PageIndex_Parameter_Is_Not_Correct() {

                // Arrange
                var pageIndexParam = 0;
                var pageSizeParam = 20;
                var mockMemSrv = GetInitialMemSrvMockForUsers();

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(mockMemSrv.Object));

                using (var httpServer = new HttpServer(config))
                using (var client = httpServer.ToHttpClient()) {

                    var request = HttpRequestMessageHelper
                        .ConstructRequest(
                            httpMethod: HttpMethod.Get,
                            uri: string.Format(
                                "https://localhost/{0}?page={1}&take={2}",
                                "api/users", 
                                pageIndexParam, 
                                pageSizeParam),
                            mediaType: "application/json",
                            username: Constants.ValidAdminUserName,
                            password: Constants.ValidAdminPassword);

                    // Act
                    var response = await client.SendAsync(request);
                    var httpError = await response.Content.ReadAsAsync<HttpError>();
                    var modelState = (HttpError)httpError["ModelState"];

                    var pageParamError = modelState["Page"] as string[];

                    // Assert
                    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                    Assert.NotNull(pageParamError);
                }
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_PageSize_Parameter_Is_Not_Correct() {

                // Arrange
                var pageIndexParam = 1;
                var pageSizeParam = 51;
                var mockMemSrv = GetInitialMemSrvMockForUsers();

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(mockMemSrv.Object));

                using (var httpServer = new HttpServer(config))
                using (var client = httpServer.ToHttpClient()) {

                    var request = HttpRequestMessageHelper
                        .ConstructRequest(
                            httpMethod: HttpMethod.Get,
                            uri: string.Format(
                                "https://localhost/{0}?page={1}&take={2}",
                                "api/users",
                                pageIndexParam,
                                pageSizeParam),
                            mediaType: "application/json",
                            username: Constants.ValidAdminUserName,
                            password: Constants.ValidAdminPassword);

                    // Act
                    var response = await client.SendAsync(request);
                    var httpError = await response.Content.ReadAsAsync<HttpError>();
                    var modelState = (HttpError)httpError["ModelState"];

                    var takeParamError = modelState["Take"] as string[];

                    // Assert
                    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                    Assert.NotNull(takeParamError);
                }
            }

            private static Mock<IMembershipService> GetInitialMemSrvMockForUsers() {

                var users = GetDummyUsers(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipService();

                mockMemSrv.Setup(ms => ms.GetUsers(
                        It.IsAny<int>(), It.IsAny<int>()
                    )
                ).Returns<int, int>((page, take) =>
                    users.AsQueryable()
                              .ToPaginatedList(page, take)
                );

                return mockMemSrv;
            }
        }

        public class GetUser {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_Expected_User_If_Request_Authorized_And_User_Exists() {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };
                var mockMemSrv = GetInitialMemSrvMockForUsers(keys);

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(mockMemSrv.Object));

                using (var httpServer = new HttpServer(config))
                using (var client = httpServer.ToHttpClient()) {

                    var request = HttpRequestMessageHelper
                        .ConstructRequest(
                            httpMethod: HttpMethod.Get,
                            uri: string.Format(
                                "https://localhost/{0}/{1}",
                                "api/users", keys[1]),
                            mediaType: "application/json",
                            username: Constants.ValidAdminUserName,
                            password: Constants.ValidAdminPassword);

                    // Act
                    var response = await client.SendAsync(request);
                    var userDto = await response.Content.ReadAsAsync<UserDto>();

                    // Assert
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.Equal(keys[1], userDto.Key);
                }
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_User_Does_Not_Exist() {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };
                var mockMemSrv = GetInitialMemSrvMockForUsers(keys);

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(mockMemSrv.Object));

                using (var httpServer = new HttpServer(config))
                using (var client = httpServer.ToHttpClient()) {

                    var request = HttpRequestMessageHelper
                        .ConstructRequest(
                            httpMethod: HttpMethod.Get,
                            uri: string.Format(
                                "https://localhost/{0}/{1}",
                                "api/users", Guid.NewGuid()),
                            mediaType: "application/json",
                            username: Constants.ValidAdminUserName,
                            password: Constants.ValidAdminPassword);

                    // Act
                    var response = await client.SendAsync(request);

                    // Assert
                    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                }
            }

            private static Mock<IMembershipService> GetInitialMemSrvMockForUsers(Guid[] keys) {

                var users = GetDummyUsers(keys);
                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipService();

                mockMemSrv.Setup(ms => ms.GetUser(
                        It.Is<Guid>(
                            key => keys.Contains(key)
                        )
                    )
                ).Returns<Guid>(key => 
                    users.FirstOrDefault(x =>
                        x.User.Key == key
                    )
                );

                return mockMemSrv;
            }
        }

        public class PostUser {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_201_And_User_If_Request_Authorized_And_Success() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_409_If_Request_Authorized_But_Conflicted() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Invalid() {

                throw new NotImplementedException();
            }
        }

        public class PutUser {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_User_Does_Not_Exist() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Invalid() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_User_If_Request_Authorized_But_Request_Is_Valid() {

                throw new NotImplementedException();
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

        private static List<UserWithRoles> GetDummyUsers(Guid[] keys) {

            #region Dummy User List
            List<UserWithRoles> users = new List<UserWithRoles> { 
                new UserWithRoles { 
                    User = new User { 
                        Key = keys[0],
                        Name = Constants.ValidAdminUserName,
                        Email = "fooAdmin@example.com",
                        IsLocked = false,
                        CreatedOn = DateTime.Now.AddDays(-10),
                        LastUpdatedOn = DateTime.Now.AddDays(-5)
                    },
                    Roles = new List<Role> { 
                        new Role { 
                            Key = Guid.NewGuid(), Name = "Admin"
                        }
                    }
                },

                new UserWithRoles { 
                    User = new User { 
                        Key = keys[1],
                        Name = Constants.ValidAffiliateUserName,
                        Email = "fooAffiliate@example.com",
                        IsLocked = false,
                        CreatedOn = DateTime.Now.AddDays(-10),
                        LastUpdatedOn = DateTime.Now.AddDays(-5)
                    },
                    Roles = new List<Role> { 
                        new Role { 
                            Key = Guid.NewGuid(), Name = "Affiliate"
                        }
                    }
                },

                new UserWithRoles { 
                    User = new User { 
                        Key = keys[2],
                        Name = Constants.ValidEmployeeUserName,
                        Email = "fooEmployee@example.com",
                        IsLocked = false,
                        CreatedOn = DateTime.Now.AddDays(-10),
                        LastUpdatedOn = DateTime.Now.AddDays(-5)
                    },
                    Roles = new List<Role> { 
                        new Role { 
                            Key = Guid.NewGuid(), Name = "Employee"
                        }
                    }
                }
            };
            #endregion

            return users;
        }
    }
}