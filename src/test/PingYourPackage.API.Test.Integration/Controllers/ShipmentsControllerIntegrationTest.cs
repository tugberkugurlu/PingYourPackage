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
    
    public class ShipmentsControllerIntegrationTest {

        public class GetShipments {

            [Fact, NullCurrentPrincipal]
            public Task
                Returns_200_And_Shipments_If_Request_Authorized() {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer());

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?page={1}&take={2}",
                            "api/shipments", 1, 2),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                return IntegrationTestHelper
                    .TestForPaginatedDtoAsync<ShipmentDto>(
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

                var shipments = GetDummyShipments(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
                var containerBuilder = GetInitialContainerBuilder();

                Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
                shipmentSrvMock.Setup(ss =>
                    ss.GetShipments(
                        It.IsAny<int>(), It.IsAny<int>()
                    )
                ).Returns<int, int>(
                    (pageIndex, pageSize) =>
                        shipments.AsQueryable()
                            .ToPaginatedList(pageIndex, pageSize)
                );

                containerBuilder.Register(c => shipmentSrvMock.Object)
                    .As<IShipmentService>()
                    .InstancePerApiRequest();

                return containerBuilder.Build();
            }
        }

        public class GetShipment {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_Shipment_If_Request_Authorized_And_Shipment_Exists() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_Shipment_Does_Not_Exist() {

                throw new NotImplementedException();
            }
        }

        public class PostShipment {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_201_And_Shipment_If_Request_Authorized_And_Success() {

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

        public class PutShipment {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_Shipment_Does_Not_Exist() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Shipment_But_Invalid() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_Updated_Shipment_If_Request_Authorized_But_Request_Is_Valid() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Message_Body_Is_Empty() {

                throw new NotImplementedException();
            }
        }

        private static IEnumerable<Shipment> GetDummyShipments(Guid[] keys) {

            var shipmentTypeKeys = new Guid[] { 
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
            };

            List<Shipment> shipments = new List<Shipment>();
            for (int i = 0; i < 3; i++) {

                shipments.Add(new Shipment {
                    Key = keys[i],
                    AffiliateKey = Guid.NewGuid(),
                    ShipmentTypeKey = shipmentTypeKeys[i],
                    Price = 12.23M * i,
                    ReceiverName = string.Format("Receiver {0} Name", i),
                    ReceiverSurname = string.Format("Receiver {0} Surname", i),
                    ReceiverAddress = string.Format("Receiver {0} Address", i),
                    ReceiverCity = string.Format("Receiver {0} City", i),
                    ReceiverCountry = string.Format("Receiver {0} Country", i),
                    ReceiverTelephone = string.Format("Receiver {0} Country", i),
                    ReceiverZipCode = "12345",
                    CreatedOn = DateTime.Now,
                    ShipmentType = new ShipmentType {
                        Key = shipmentTypeKeys[i],
                        Name = "Small",
                        Price = 4.19M,
                        CreatedOn = DateTime.Now,
                    },
                    ShipmentStates = new List<ShipmentState> { 
                        new ShipmentState { Key = Guid.NewGuid(), ShipmentKey = keys[i], ShipmentStatus = ShipmentStatus.Ordered },
                        new ShipmentState { Key = Guid.NewGuid(), ShipmentKey = keys[i], ShipmentStatus = ShipmentStatus.Scheduled }
                    }
                });
            }

            return shipments;
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
    }
}