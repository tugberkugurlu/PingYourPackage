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
    
    public class ShipmentsControllerIntegrationTest {

        private static readonly string ApiBaseRequestPath = "api/shipments";

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
                            ApiBaseRequestPath, 1, 2),
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

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };
                var requestKey = keys[1];

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(keys, requestKey);

                // Act
                var userDto = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<ShipmentDto>(
                        configAndRequest.Item1, 
                        configAndRequest.Item2, 
                        HttpStatusCode.OK);

                // Assert
                Assert.Equal(requestKey, userDto.Key);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_Shipment_Does_Not_Exist() {
                
                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };
                var requestKey = Guid.NewGuid();

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(keys, requestKey);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(
                        configAndRequest.Item1, configAndRequest.Item2);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            private static Tuple<HttpConfiguration, HttpRequestMessage> 
                GetConfigAndRequestMessage(Guid[] keys, Guid requestKey) {

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(keys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            ApiBaseRequestPath, requestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                return Tuple.Create(config, request);
            }

            private static IContainer GetContainer(Guid[] keys) {

                var shipments = GetDummyShipments(keys);
                var containerBuilder = GetInitialContainerBuilder();

                Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
                shipmentSrvMock.Setup(ss => ss.GetShipment(
                        It.IsAny<Guid>()
                    )
                ).Returns<Guid>(key => shipments.FirstOrDefault(x => x.Key == key));

                containerBuilder.Register(c => shipmentSrvMock.Object)
                    .As<IShipmentService>()
                    .InstancePerApiRequest();

                return containerBuilder.Build();
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

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };
                var requestKey = Guid.NewGuid();
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(keys));

                var shipmentRequestModel = new ShipmentUpdateRequestModel {
                    ShipmentTypeKey = Guid.NewGuid(),
                    Price = 12.23M,
                    ReceiverName = "Receiver 1 Name",
                    ReceiverSurname = "Receiver 1 Surname",
                    ReceiverAddress = "Receiver 1 Address",
                    ReceiverCity = "Receiver 1 City",
                    ReceiverCountry = "Receiver 1 Country",
                    ReceiverTelephone = "Receiver 1 Country",
                    ReceiverZipCode = "12345",
                    ReceiverEmail = "foo@example.com"
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            ApiBaseRequestPath, requestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentUpdateRequestModel>(
                    shipmentRequestModel, new JsonMediaTypeFormatter());

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Shipment_But_Invalid() {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };
                var requestKey = Guid.NewGuid();
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(keys));

                var shipmentRequestModel = new ShipmentUpdateRequestModel {
                    ReceiverName = "ANameWhichIsMoreThan50CharsANameWhichIsMoreThan50Chars",
                    ReceiverSurname = "ASurnameWhichIsMoreThan50CharsASurnameWhichIsMoreThan50Chars",
                    ReceiverAddress = "AnAddressWhichIsMoreThan50CharsAnAddressWhichIsMoreThan50Chars",
                    ReceiverCity = "ACityWhichIsMoreThan50CharsACityWhichIsMoreThan50Chars",
                    ReceiverTelephone = "ATelephoneWhichIsMoreThan50CharsATelephoneWhichIsMoreThan50Chars",
                    ReceiverEmail = "fooexample.com"
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            ApiBaseRequestPath, requestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentUpdateRequestModel>(
                    shipmentRequestModel, new JsonMediaTypeFormatter());

                var httpError = await IntegrationTestHelper.
                    GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var shipmentTypeKeyError = modelState["requestModel.ShipmentTypeKey"] as string[];
                var priceError = modelState["requestModel.Price"] as string[];
                var receiverNameError = modelState["requestModel.ReceiverName"] as string[];
                var receiverSurnameError = modelState["requestModel.ReceiverSurname"] as string[];
                var receiverAddressError = modelState["requestModel.ReceiverAddress"] as string[];
                var receiverCityError = modelState["requestModel.ReceiverCity"] as string[];
                var receiverCountryError = modelState["requestModel.ReceiverCountry"] as string[];
                var receiverTelephoneError = modelState["requestModel.ReceiverTelephone"] as string[];
                var receiverEmailError = modelState["requestModel.ReceiverEmail"] as string[];
                var receiverZipCodeError = modelState["requestModel.ReceiverZipCode"] as string[];

                // Assert
                Assert.NotNull(shipmentTypeKeyError);
                Assert.NotNull(priceError);
                Assert.NotNull(receiverNameError);
                Assert.NotNull(receiverSurnameError);
                Assert.NotNull(receiverAddressError);
                Assert.NotNull(receiverCityError);
                Assert.NotNull(receiverCountryError);
                Assert.NotNull(receiverTelephoneError);
                Assert.NotNull(receiverEmailError);
                Assert.NotNull(receiverZipCodeError);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_Updated_Shipment_If_Request_Authorized_But_Request_Is_Valid() {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };
                var requestKey = keys[1];
                var shipments = GetDummyShipments(keys);
                var shipmentSrvMock = GetShipmentSrvMock(shipments);
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(keys, shipmentSrvMock));

                var shipmentRequestModel = new ShipmentUpdateRequestModel {
                    ShipmentTypeKey = Guid.NewGuid(),
                    Price = 12.23M,
                    ReceiverName = "Receiver 1 Name",
                    ReceiverSurname = "Receiver 1 Surname",
                    ReceiverAddress = "Receiver 1 Address",
                    ReceiverCity = "Receiver 1 City",
                    ReceiverCountry = "Receiver 1 Country",
                    ReceiverTelephone = "Receiver 1 Country",
                    ReceiverZipCode = "12345",
                    ReceiverEmail = "foobar@example.com"
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            ApiBaseRequestPath, requestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentUpdateRequestModel>(
                    shipmentRequestModel, new JsonMediaTypeFormatter());

                var shipmentDto = await IntegrationTestHelper.
                    GetResponseMessageBodyAsync<ShipmentDto>(
                        config, request, HttpStatusCode.OK);

                // Assert
                shipmentSrvMock.Verify(
                    ss => ss.UpdateShipment(
                        It.IsAny<Shipment>()), Times.Once()
                );

                Assert.Equal(requestKey, shipmentDto.Key);
                Assert.Equal(shipmentRequestModel.Price, shipmentDto.Price);
                Assert.Equal(shipmentRequestModel.ReceiverEmail, shipmentDto.ReceiverEmail);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Message_Body_Is_Empty() {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };
                var requestKey = Guid.NewGuid();
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(keys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            ApiBaseRequestPath, requestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                // Act
                var httpError = await IntegrationTestHelper.
                    GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var requestModelError = modelState["requestModel"] as string[];

                // Assert
                Assert.NotNull(requestModelError);
            }

            private static IContainer GetContainer(Guid[] keys) {

                var shipments = GetDummyShipments(keys);
                var shipmentSrvMock = GetShipmentSrvMock(shipments);

                return GetContainer(keys, shipmentSrvMock);
            }

            private static IContainer GetContainer(
                Guid[] keys, Mock<IShipmentService> shipmentSrvMock) {

                var containerBuilder = GetInitialContainerBuilder();

                containerBuilder.Register(c => shipmentSrvMock.Object)
                    .As<IShipmentService>()
                    .InstancePerApiRequest();

                return containerBuilder.Build();
            }

            private static Mock<IShipmentService> GetShipmentSrvMock(
                IEnumerable<Shipment> shipments) {

                Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
                shipmentSrvMock.Setup(ss => ss.GetShipment(
                        It.IsAny<Guid>()
                    )
                ).Returns<Guid>(key => shipments.FirstOrDefault(x => x.Key == key));

                shipmentSrvMock.Setup(ss => ss.UpdateShipment(
                        It.IsAny<Shipment>()
                    )
                ).Returns<Shipment>(shipment => shipment);

                return shipmentSrvMock;
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
                    ReceiverEmail = "foo@example.com",
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