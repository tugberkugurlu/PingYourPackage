using Autofac;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using Xunit;

namespace PingYourPackage.API.Test.Integration.Controllers {

    public class RolesControllerIntegrationTest {

        // GetRoles action method
        public class GetRolesActionTest {

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
                Returns_Unauthorized_Response_If_Request_Is_Not_By_Admin() {

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
                Does_Not_Returns_Unauthorized_Response_If_Request_Authorized() {

                return IntegrationAuthTestHelper.TestRequestAuthorization(
                    HttpMethod.Get,
                    "api/roles",
                    "application/json",
                    Constants.ValidAdminUserName,
                    Constants.ValidAdminPassword,
                    HttpStatusCode.OK);
            }

            private static IContainer GetInitialServices() {

                var builder = IntegrationTestHelper.GetInitialContainerBuilder();
                return builder.Build();
            }
        }

        public class GetRoleActionTest {
        }

        public class PostRoleActionTest {
        }
    }
}