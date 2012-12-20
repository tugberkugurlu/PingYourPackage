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

                return GetContainerThroughMock(shipmentSrvMock);
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
                var shipmentDto = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<ShipmentDto>(
                        configAndRequest.Item1, 
                        configAndRequest.Item2, 
                        HttpStatusCode.OK);

                // Assert
                Assert.Equal(requestKey, shipmentDto.Key);
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
                var shipmentSrvMock = GetShipmentSrvMock(shipments);

                return GetContainerThroughMock(shipmentSrvMock);
            }

            private static Mock<IShipmentService> GetShipmentSrvMock(IEnumerable<Shipment> shipments) {

                Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
                shipmentSrvMock.Setup(ss => ss.GetShipment(
                        It.IsAny<Guid>()
                    )
                ).Returns<Guid>(key => shipments.FirstOrDefault(x => x.Key == key));

                return shipmentSrvMock;
            }
        }

        public class PostShipment {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_201_And_Shipment_If_Request_Authorized_And_Success() {

                // Arange
                Guid[] availableShipmentTypeKeys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(availableShipmentTypeKeys));

                var shipmentRequestModel = new ShipmentRequestModel {
                    ShipmentTypeKey = availableShipmentTypeKeys[1],
                    AffiliateKey = Guid.NewGuid(),
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
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            ApiBaseRequestPath),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentRequestModel>(
                    shipmentRequestModel, new JsonMediaTypeFormatter());

                // Act
                var shipmentDto = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<ShipmentDto>(
                        config, request, HttpStatusCode.Created);

                // Assert
                Assert.Equal(shipmentRequestModel.Price, shipmentDto.Price);
                Assert.Equal(shipmentRequestModel.ReceiverName, shipmentDto.ReceiverName);
                Assert.Equal(shipmentRequestModel.ReceiverEmail, shipmentDto.ReceiverEmail);
                Assert.NotNull(shipmentDto.ShipmentType);
                Assert.True(shipmentDto.ShipmentStates.Count() > 0);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_409_If_Request_Authorized_But_Conflicted() {

                // Arange
                Guid[] availableShipmentTypeKeys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(availableShipmentTypeKeys));

                var shipmentRequestModel = new ShipmentRequestModel {
                    ShipmentTypeKey = Guid.NewGuid(),
                    AffiliateKey = Guid.NewGuid(),
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
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            ApiBaseRequestPath),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentRequestModel>(
                    shipmentRequestModel, new JsonMediaTypeFormatter());

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Invalid() {

                // Arange
                Guid[] availableShipmentTypeKeys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(availableShipmentTypeKeys));

                var shipmentRequestModel = new ShipmentRequestModel {
                    ReceiverName = "ANameWhichIsMoreThan50CharsANameWhichIsMoreThan50Chars",
                    ReceiverSurname = "ASurnameWhichIsMoreThan50CharsASurnameWhichIsMoreThan50Chars",
                    ReceiverAddress = "AnAddressWhichIsMoreThan50CharsAnAddressWhichIsMoreThan50Chars",
                    ReceiverCity = "ACityWhichIsMoreThan50CharsACityWhichIsMoreThan50Chars",
                    ReceiverTelephone = "ATelephoneWhichIsMoreThan50CharsATelephoneWhichIsMoreThan50Chars",
                    ReceiverEmail = "fooexample.com"
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            ApiBaseRequestPath),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentRequestModel>(
                    shipmentRequestModel, new JsonMediaTypeFormatter());

                var httpError = await IntegrationTestHelper.
                    GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var affiliateKeyError = modelState["requestModel.AffiliateKey"] as string[];
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
                Assert.NotNull(affiliateKeyError);
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
                Returns_400_If_Request_Authorized_But_Message_Body_Is_Empty() {

                // Arange
                Guid[] availableShipmentTypeKeys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(availableShipmentTypeKeys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            ApiBaseRequestPath),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var requestModelError = modelState["requestModel"] as string[];

                // Assert
                Assert.NotNull(requestModelError);
            }

            private static IContainer GetContainer() {

                return GetContainer(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
            }

            private static IContainer GetContainer(Guid[] availableShipmentTypeKeys) {

                var shipments = GetDummyShipments(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
                var shipmentSrvMock = GetShipmentSrvMock(shipments, availableShipmentTypeKeys);

                return GetContainerThroughMock(shipmentSrvMock);
            }

            private static Mock<IShipmentService> GetShipmentSrvMock(
                IEnumerable<Shipment> shipments) {

                return GetShipmentSrvMock(shipments, availableShipmentTypeKeys: new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
            }

            private static Mock<IShipmentService> GetShipmentSrvMock(
                IEnumerable<Shipment> shipments, Guid[] availableShipmentTypeKeys) {

                Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
                shipmentSrvMock.Setup(ss => ss.GetShipment(
                        It.IsAny<Guid>()
                    )
                ).Returns<Guid>(key => shipments.FirstOrDefault(x => x.Key == key));

                // For the valid result
                shipmentSrvMock.Setup(ss => ss.AddShipment(
                        It.IsAny<Shipment>()
                    )
                ).Returns<Shipment>(shipment => {
                    
                    shipment.Key = Guid.NewGuid();
                    shipment.CreatedOn = DateTime.Now;
                    shipment.ShipmentType = new ShipmentType {
                        Key = availableShipmentTypeKeys[1],
                        Name = "Small",
                        Price = 4.19M,
                        CreatedOn = DateTime.Now,
                    };

                    shipment.ShipmentStates = new List<ShipmentState> { 
                        new ShipmentState { Key = Guid.NewGuid(), ShipmentKey = shipment.Key, ShipmentStatus = ShipmentStatus.Ordered }
                    };

                    return new OperationResult<Shipment>(true) {
                        Entity = shipment
                    };
                });

                // For the invalid result
                shipmentSrvMock.Setup(ss => ss.AddShipment(
                        It.Is<Shipment>(s => 
                            !availableShipmentTypeKeys.Contains(
                                s.ShipmentTypeKey
                            )
                        )
                    )
                ).Returns(new OperationResult<Shipment>(false));

                return shipmentSrvMock;
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
                Returns_200_And_Updated_Shipment_If_Request_Authorized_And_Request_Is_Valid() {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };
                var requestKey = keys[1];
                var shipments = GetDummyShipments(keys);
                var shipmentSrvMock = GetShipmentSrvMock(shipments);
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetContainerThroughMock(shipmentSrvMock));

                var shipmentRequestModel = new ShipmentUpdateRequestModel {
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

                return GetContainerThroughMock(shipmentSrvMock);
            }

            private static Mock<IShipmentService> GetShipmentSrvMock(IEnumerable<Shipment> shipments) {

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

            for (int i = 0; i < 3; i++) {

                yield return new Shipment {
                    Key = keys[i],
                    AffiliateKey = Guid.NewGuid(),
                    ShipmentTypeKey = shipmentTypeKeys[i],
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
                        Key = shipmentTypeKeys[i],
                        Name = "Small",
                        Price = 4.19M,
                        CreatedOn = DateTime.Now,
                    },
                    ShipmentStates = new List<ShipmentState> { 
                        new ShipmentState { Key = Guid.NewGuid(), ShipmentKey = keys[i], ShipmentStatus = ShipmentStatus.Ordered },
                        new ShipmentState { Key = Guid.NewGuid(), ShipmentKey = keys[i], ShipmentStatus = ShipmentStatus.Scheduled }
                    }
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

        private static IContainer GetContainerThroughMock(Mock<IShipmentService> shipmentSrvMock) {

            var containerBuilder = GetInitialContainerBuilder();

            containerBuilder.Register(c => shipmentSrvMock.Object)
                .As<IShipmentService>()
                .InstancePerApiRequest();

            return containerBuilder.Build();
        }
    }
}