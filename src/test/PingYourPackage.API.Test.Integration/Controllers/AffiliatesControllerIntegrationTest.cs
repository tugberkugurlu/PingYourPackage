using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PingYourPackage.API.Test.Integration.Controllers {
    
    public class AffiliatesControllerIntegrationTest {

        public class GetAffiliates {

            [Fact, NullCurrentPrincipal]
            public Task
                Returns_200_And_Affiliates_If_Request_Authorized() {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer());

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?page={1}&take={2}",
                            "api/affiliates", 1, 2),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                return IntegrationTestHelper
                    .TestForPaginatedDtoAsync<AffiliateDto>(
                        config,
                        request,
                        expectedPageIndex: 1,
                        expectedTotalPageCount: 2,
                        expectedCurrentItemsCount: 2,
                        expectedTotalItemsCount: 3,
                        expectedHasNextPageResult: true,
                        expectedHasPreviousPageResult: false);
            }

            private static IContainer GetContainer() {

                var affiliates = GetDummyAffiliates(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
                var containerBuilder = GetInitialContainerBuilder();
                
                Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
                shipmentSrvMock.Setup(ss =>
                    ss.GetAffiliates(
                        It.IsAny<int>(), It.IsAny<int>()
                    )
                ).Returns<int, int>(
                    (pageIndex, pageSize) =>
                        affiliates.AsQueryable()
                            .ToPaginatedList(pageIndex, pageSize)
                );

                containerBuilder.Register(c => shipmentSrvMock.Object)
                    .As<IShipmentService>()
                    .InstancePerApiRequest();

                return containerBuilder.Build();
            }
        }

        public class GetAffiliate {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_Affiliate_If_Request_Authorized_And_Affiliate_Exists() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_Affiliate_Does_Not_Exist() {

                throw new NotImplementedException();
            }

            private static IContainer GetContainer() {

                var affiliates = GetDummyAffiliates(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
                var containerBuilder = GetInitialContainerBuilder();

                Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
                // Do the stuff here...

                containerBuilder.Register(c => shipmentSrvMock.Object)
                    .As<IShipmentService>()
                    .InstancePerApiRequest();

                return containerBuilder.Build();
            }
        }

        public class PostAffiliate {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_201_And_Affiliate_If_Request_Authorized_And_Success() {

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

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Message_Body_Is_Empty() {

                throw new NotImplementedException();
            }
        }

        public class PutAffiliate {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_Affiliate_Does_Not_Exist() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Invalid() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_Updated_Affiliate_If_Request_Authorized_But_Request_Is_Valid() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Message_Body_Is_Empty() {

                throw new NotImplementedException();
            }
        }

        private static ContainerBuilder GetInitialContainerBuilder() {

            var builder = IntegrationTestHelper
                .GetEmptyContainerBuilder();

            var mockMemSrv = ServicesMockHelper
                .GetInitialMembershipService();

            builder.Register(c => mockMemSrv.Object)
                .As<IMembershipService>()
                .InstancePerApiRequest();

            return builder;
        }

        private static IEnumerable<Affiliate> GetDummyAffiliates(Guid[] keys) {

            for (int i = 0; i < 3; i++) {

                yield return new Affiliate {
                    Key = keys[i],
                    CompanyName = string.Format("Company {0}", i),
                    TelephoneNumber = string.Format("123-152-182{0}", i),
                    Address = string.Format("Address {0}, 1234{0}", i),
                    CreatedOn = DateTime.Now.AddDays(-5),
                    User = new User {
                        Key = keys[i],
                        Name = string.Format("Comp{0}", i),
                        Email = string.Format("comp{0}@expample.com", i),
                        IsLocked = false,
                        CreatedOn = DateTime.Now.AddDays(-5),
                        LastUpdatedOn = DateTime.Now.AddDays(-1)
                    }
                };
            }
        }
    }
}