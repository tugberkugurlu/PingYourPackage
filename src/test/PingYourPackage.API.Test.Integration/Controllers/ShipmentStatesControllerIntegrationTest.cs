using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Xunit;

namespace PingYourPackage.API.Test.Integration.Controllers {

    public class ShipmentStatesControllerIntegrationTest {

        private static readonly string ApiBaseRequestPathFormat = "api/shipments/{0}/shipmentstates";

        public class General {

            [Fact, NullCurrentPrincipal]
            public async Task Returns_404_If_Request_Authanticated_But_Shipment_Does_Not_Exists() {

                // Arrange
                Guid[] shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var invalidshipmentKey = Guid.NewGuid();

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(shipmentKeys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}",
                            string.Format(ApiBaseRequestPathFormat, invalidshipmentKey)),
                        mediaType: "application/json",
                        username: Constants.ValidEmployeeUserName,
                        password: Constants.ValidEmployeePassword);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Does_Not_Return_404_If_Request_Authanticated_And_Shipment_Exists() {

                // Arrange
                Guid[] shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var validshipmentKey = shipmentKeys[2];

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(shipmentKeys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}",
                            string.Format(ApiBaseRequestPathFormat, validshipmentKey)),
                        mediaType: "application/json",
                        username: Constants.ValidEmployeeUserName,
                        password: Constants.ValidEmployeePassword);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
            }

            private static IContainer GetContainer(Guid[] shipmentKeys) {

                var shipments = GetDummyShipments(shipmentKeys);
                var shipmentSrvMock = GetInitialShipmentServiceMock(shipments);
                return GetContainerThroughMock(shipmentSrvMock);
            }
        }

        public class GetShipmentStates {

            [Fact, NullCurrentPrincipal]
            public async Task Returns_200_And_ShipmentStates_If_Request_Authorized() {

                // Arrange
                Guid[] shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var validshipmentKey = shipmentKeys[2];

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(shipmentKeys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}",
                            string.Format(ApiBaseRequestPathFormat, validshipmentKey)),
                        mediaType: "application/json",
                        username: Constants.ValidEmployeeUserName,
                        password: Constants.ValidEmployeePassword);

                // Act
                var shipmentStateDtos = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<IEnumerable<ShipmentStateDto>>(
                        config, request, HttpStatusCode.OK);

                // Assert
                Assert.True(shipmentStateDtos.Count() > 0);
                Assert.True(shipmentStateDtos.Where(x => x.ShipmentKey == validshipmentKey).Count() == shipmentStateDtos.Count());
            }

            private static IContainer GetContainer(Guid[] shipmentKeys) {

                var shipments = GetDummyShipments(shipmentKeys);
                var shipmentSrvMock = GetInitialShipmentServiceMock(shipments);
                shipmentSrvMock.Setup(ss => ss.GetShipmentStates(
                        It.IsAny<Guid>()
                    )
                ).Returns<Guid>(shipmentKey => shipments
                    .SelectMany(x => x.ShipmentStates)
                    .Where(x => x.ShipmentKey == shipmentKey)
                );

                return GetContainerThroughMock(shipmentSrvMock);
            }
        }

        public class PostShipmentState {

            [Fact, NullCurrentPrincipal]
            public async Task Returns_201_And_ShipmentState_If_Request_Authorized_And_Success() {

                // Arrange
                Guid[] shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var validshipmentKey = shipmentKeys[1];

                var shipmentStateRequestModel = new ShipmentStateRequestModel { 
                    ShipmentStatus = "InTransit"
                };

                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, validshipmentKey, shipmentStateRequestModel);

                // Act
                var shipmentStateDto = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<ShipmentStateDto>(
                        configAndRequest.Item1,
                        configAndRequest.Item2,
                        HttpStatusCode.Created);

                // Assert
                Assert.Equal(shipmentStateRequestModel.ShipmentStatus, shipmentStateDto.ShipmentStatus);
                Assert.Equal(validshipmentKey, shipmentStateDto.ShipmentKey);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_409_If_Request_Authorized_But_Conflicted() {

                // Arrange
                Guid[] shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var validshipmentKey = shipmentKeys[1];

                var shipmentStateRequestModel = new ShipmentStateRequestModel {
                    ShipmentStatus = "Ordered"
                };

                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, validshipmentKey, shipmentStateRequestModel);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(configAndRequest.Item1, configAndRequest.Item2);

                // Assert
                Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_400_If_Request_Authorized_But_Invalid() {

                // Arrange
                Guid[] shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var validshipmentKey = shipmentKeys[1];

                var shipmentStateRequestModel = new ShipmentStateRequestModel {
                    ShipmentStatus = "Done"
                };

                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, validshipmentKey, shipmentStateRequestModel);

                // Act
                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        configAndRequest.Item1,
                        configAndRequest.Item2,
                        HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var shipmentStatusModelError = modelState["requestModel.ShipmentStatus"] as string[];

                // Assert
                Assert.NotNull(shipmentStatusModelError);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_400_If_Request_Authorized_But_Message_Body_Is_Empty() {

                // Arrange
                Guid[] shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var validshipmentKey = shipmentKeys[1];

                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, validshipmentKey);

                // Act
                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        configAndRequest.Item1,
                        configAndRequest.Item2,
                        HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var requestModelError = modelState["requestModel"] as string[];

                // Assert
                Assert.NotNull(requestModelError);
            }

            private static Tuple<HttpConfiguration, HttpRequestMessage> GetConfigAndRequestMessage(
                Guid[] shipmentKeys,
                Guid validshipmentKey) {

                return GetConfigAndRequestMessage(shipmentKeys, validshipmentKey, null);
            }

            private static Tuple<HttpConfiguration, HttpRequestMessage> GetConfigAndRequestMessage(
                Guid[] shipmentKeys,
                Guid validshipmentKey,
                ShipmentStateRequestModel requestModel) {

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(shipmentKeys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            string.Format(ApiBaseRequestPathFormat, validshipmentKey)),
                        mediaType: "application/json",
                        username: Constants.ValidEmployeeUserName,
                        password: Constants.ValidEmployeePassword);

                if (requestModel != null) {

                    request.Content = new ObjectContent<ShipmentStateRequestModel>(
                        requestModel, new JsonMediaTypeFormatter());
                }

                return Tuple.Create(config, request);
            }

            private static IContainer GetContainer(Guid[] shipmentKeys) {

                var shipments = GetDummyShipments(shipmentKeys);
                var shipmentSrvMock = GetInitialShipmentServiceMock(shipments);
                shipmentSrvMock.Setup(ss => ss.AddShipmentState(
                        It.IsAny<Guid>(), It.IsAny<ShipmentStatus>()
                    )
                ).Returns<Guid, ShipmentStatus>((shipmentKey, status) => {


                    // All the tests which will use this method have ensured that
                    // the shipment already exists.

                    var shipmentStates = shipments
                        .SelectMany(x => x.ShipmentStates)
                        .Where(x => x.ShipmentKey == shipmentKey);

                    if (!IsShipmentStateInsertable(shipmentStates, shipmentKey, status)) {
                        return new OperationResult<ShipmentState>(false);
                    }

                    var shipmentState = InsertShipmentState(shipmentKey, status);
                    return new OperationResult<ShipmentState>(true) {
                        Entity = shipmentState
                    };
                });

                return GetContainerThroughMock(shipmentSrvMock);
            }

            private static bool IsShipmentStateInsertable(IEnumerable<ShipmentState> shipmentStates, Guid shipmentKey, ShipmentStatus status) {

                var latestState = (from state in shipmentStates
                                   orderby state.ShipmentStatus descending
                                   select state).First();

                return status > latestState.ShipmentStatus;
            }

            private static ShipmentState InsertShipmentState(Guid ShipmentKey, ShipmentStatus status) {

                var shipmentState = new ShipmentState {
                    Key = Guid.NewGuid(),
                    ShipmentKey = ShipmentKey,
                    ShipmentStatus = status,
                    CreatedOn = DateTime.Now
                };

                return shipmentState;
            }
        }

        private static IEnumerable<Shipment> GetDummyShipments(Guid[] shipmentKeys) {

            Guid[] affiliateKeys = IntegrationTestHelper.GetKeys(3);
            Guid[] shipmentTypeKeys = IntegrationTestHelper.GetKeys(3);

            for (int i = 0; i < 9; i++) {

                var states = new List<ShipmentState> { 
                    new ShipmentState { Key = Guid.NewGuid(), ShipmentKey = shipmentKeys[i], ShipmentStatus = ShipmentStatus.Ordered },
                    new ShipmentState { Key = Guid.NewGuid(), ShipmentKey = shipmentKeys[i], ShipmentStatus = ShipmentStatus.Scheduled }
                };

                // Make the first shipment InTransit for each affiliate
                if ((i % 3) == 0) {
                    states.Add(new ShipmentState {
                        Key = Guid.NewGuid(),
                        ShipmentKey = shipmentKeys[i],
                        ShipmentStatus = ShipmentStatus.InTransit
                    });
                }

                yield return new Shipment {
                    Key = shipmentKeys[i],
                    AffiliateKey = affiliateKeys[(i / 3)],
                    ShipmentTypeKey = shipmentTypeKeys[(i / 3)],
                    Price = 12.23M * (i + 1),
                    ReceiverName = string.Format("Receiver {0} Name", i),
                    ReceiverSurname = string.Format("Receiver {0} Surname", i),
                    ReceiverAddress = string.Format("Receiver {0} Address", i),
                    ReceiverCity = string.Format("Receiver {0} City", i),
                    ReceiverCountry = string.Format("Receiver {0} Country", i),
                    ReceiverTelephone = string.Format("Receiver {0} Country", i),
                    ReceiverZipCode = "12345",
                    ReceiverEmail = "foo@example.com",
                    CreatedOn = DateTime.Now,
                    ShipmentType = new ShipmentType {
                        Key = shipmentTypeKeys[(i / 3)],
                        Name = "Small",
                        Price = 4.19M,
                        CreatedOn = DateTime.Now,
                    },
                    ShipmentStates = states
                };
            }
        }

        private static ContainerBuilder GetInitialContainerBuilder() {

            var builder = IntegrationTestHelper
                .GetEmptyContainerBuilder();

            var mockMemSrv = ServicesMockHelper
                .GetInitialMembershipServiceMock();

            builder.Register(c => mockMemSrv.Object)
                .As<IMembershipService>()
                .InstancePerApiRequest();

            return builder;
        }

        private static Mock<IShipmentService> GetInitialShipmentServiceMock(IEnumerable<Shipment> shipments) {

            // These are the operations which are needed 
            // for any request against this controller
            Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
            shipmentSrvMock.Setup(ss => ss.GetShipment(
                    It.IsAny<Guid>()
                )
            ).Returns<Guid>(key => shipments.FirstOrDefault(x => x.Key == key));

            return shipmentSrvMock;
        }

        private static IContainer GetContainerThroughMock(Mock<IShipmentService> shipmentSrvMock) {

            var containerBuilder = GetInitialContainerBuilder();

            containerBuilder.Register(c => shipmentSrvMock.Object)
                .As<IShipmentService>()
                .InstancePerApiRequest();

            return containerBuilder.Build();
        }
    }
}