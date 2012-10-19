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
    
    public class ShipmentTypesControllerIntegrationTest {

        public class GetShipmentTypes {

            [Fact, NullCurrentPrincipal]
            public Task
                Returns_200_And_ShipmentTypes_If_Request_Authorized() {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetContainerForGetShipmentTypesAction());

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?page={1}&take={2}",
                            "api/shipmenttypes", 1, 2),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                return IntegrationTestHelper
                    .TestForPaginatedDtoAsync<ShipmentTypeDto>(
                        config,
                        request,
                        expectedPageIndex: 1,
                        expectedTotalPageCount: 2,
                        expectedCurrentItemsCount: 2,
                        expectedTotalItemsCount: 3,
                        expectedHasNextPageResult: true,
                        expectedHasPreviousPageResult: false);
            }

            private static IContainer GetContainerForGetShipmentTypesAction() {

                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var shipmentTypes = GetDummyShipmentTypes(keys);
                var container = GetInitialContainerBuilder();

                Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
                shipmentSrvMock.Setup(ss =>
                    ss.GetShipmentTypes(
                        It.IsAny<int>(), It.IsAny<int>()
                    )
                ).Returns<int, int>(
                    (pageIndex, pageSize) => 
                        shipmentTypes.AsQueryable()
                            .ToPaginatedList(pageIndex, pageSize)
                );

                container.Register(c => shipmentSrvMock.Object)
                    .As<IShipmentService>()
                    .InstancePerApiRequest();

                return container.Build();
            }
        }

        public class GetShipmentType {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_ShipmentType_If_Request_Authorized_And_ShipmentType_Exists() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_ShipmentType_Does_Not_Exist() { 

                throw new NotImplementedException();
            }
        }

        public class PostShipmentType {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_201_And_ShipmentType_If_Request_Authorized_And_Success() {

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

        public class PutShipmentType {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_ShipmentType_Does_Not_Exist() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Invalid() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_Updated_ShipmentType_If_Request_Authorized_But_Request_Is_Valid() {

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

        private static IEnumerable<ShipmentType> GetDummyShipmentTypes(Guid[] keys) {

            List<ShipmentType> shipmentTypes = new List<ShipmentType> { 
                new ShipmentType { Key = keys[0], Name = "Small", Price = 10.00M, CreatedOn = DateTime.Now },
                new ShipmentType { Key = keys[1], Name = "Medium", Price = 20.00M, CreatedOn = DateTime.Now },
                new ShipmentType { Key = keys[2], Name = "Large", Price = 30.00M, CreatedOn = DateTime.Now }
            };

            return shipmentTypes;
        }
    }
}