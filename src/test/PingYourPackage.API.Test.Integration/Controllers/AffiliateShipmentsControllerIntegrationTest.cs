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
    
    public class AffiliateShipmentsControllerIntegrationTest {

        private static readonly string ApiBaseRequestPathFormat = "api/affiliates/{0}/shipments";

        public class General {

            [Fact, NullCurrentPrincipal]
            public async Task Returns_404_If_Request_Authanticated_But_Affiliate_Does_Not_Exists() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);
                var invalidAffiliateKey = Guid.NewGuid();

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetContainer(shipmentKeys, affiliateKeys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?page={1}&take={2}",
                            string.Format(ApiBaseRequestPathFormat, invalidAffiliateKey), 1, 2),
                        mediaType: "application/json",
                        username: Constants.ValidAffiliateUserName,
                        password: Constants.ValidAffiliatePassword);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_401_If_Request_Authanticated_But_Affiliate_Is_Not_Related_To_Current_User() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // except for first key, all keys are not 
                // related to current Affiliate user
                var invalidAffiliateKey = affiliateKeys[2];

                // get the mock seperately. We will verify the method calls
                var shipments = GetDummyShipments(shipmentKeys, affiliateKeys);
                var affiliates = GetDummyAffiliates(affiliateKeys);
                var shipmentSrvMock = GetShipmentSrvMock(shipments, affiliates);

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetContainerThroughMock(shipmentSrvMock));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?page={1}&take={2}",
                            string.Format(ApiBaseRequestPathFormat, invalidAffiliateKey), 1, 2),
                        mediaType: "application/json",
                        username: Constants.ValidAffiliateUserName,
                        password: Constants.ValidAffiliatePassword);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                shipmentSrvMock.Verify(ss => ss.IsAffiliateRelatedToUser(invalidAffiliateKey, Constants.ValidAffiliateUserName));
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_200_If_Request_Authanticated_And_Affiliate_Is_Related_To_Current_User() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                // get the mock seperately. We will verify the method calls
                var shipments = GetDummyShipments(shipmentKeys, affiliateKeys);
                var affiliates = GetDummyAffiliates(affiliateKeys);
                var shipmentSrvMock = GetShipmentSrvMock(shipments, affiliates);

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetContainerThroughMock(shipmentSrvMock));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?page={1}&take={2}",
                            string.Format(ApiBaseRequestPathFormat, validAffiliateKey), 1, 2),
                        mediaType: "application/json",
                        username: Constants.ValidAffiliateUserName,
                        password: Constants.ValidAffiliatePassword);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            private static IContainer GetContainer(Guid[] shipmentKeys, Guid[] affiliateKeys) {

                var shipments = GetDummyShipments(shipmentKeys, affiliateKeys);
                var affiliates = GetDummyAffiliates(affiliateKeys);
                var shipmentSrvMock = GetShipmentSrvMock(shipments, affiliates);
                return GetContainerThroughMock(shipmentSrvMock);
            }

            private static Mock<IShipmentService> GetShipmentSrvMock(
                IEnumerable<Shipment> shipments, IEnumerable<Affiliate> affiliates) {

                Mock<IShipmentService> shipmentSrvMock =
                    GetInitialShipmentServiceMock(shipments, affiliates);

                shipmentSrvMock.Setup(ss =>
                    ss.GetShipments(
                        It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()
                    )
                ).Returns<int, int, Guid>(
                    (pageIndex, pageSize, affiliateKey) =>
                        shipments.Where(x => x.AffiliateKey == affiliateKey)
                            .AsQueryable().ToPaginatedList(pageIndex, pageSize)
                );

                return shipmentSrvMock;
            }
        }

        public class GetShipments {

            [Fact, NullCurrentPrincipal]
            public Task Returns_200_And_Shipments_For_Affiliates_If_Request_Authorized() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetContainer(shipmentKeys, affiliateKeys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?page={1}&take={2}",
                            string.Format(ApiBaseRequestPathFormat, validAffiliateKey), 1, 2),
                        mediaType: "application/json",
                        username: Constants.ValidAffiliateUserName,
                        password: Constants.ValidAffiliatePassword);

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

            private static IContainer GetContainer(Guid[] shipmentKeys, Guid[] affiliateKeys) {

                var shipments = GetDummyShipments(shipmentKeys, affiliateKeys);
                var affiliates = GetDummyAffiliates(affiliateKeys);

                Mock<IShipmentService> shipmentSrvMock =
                    GetInitialShipmentServiceMock(shipments, affiliates);

                shipmentSrvMock.Setup(ss =>
                    ss.GetShipments(
                        It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Guid>()
                    )
                ).Returns<int, int, Guid>(
                    (pageIndex, pageSize, affiliateKey) =>
                        shipments.Where(x => x.AffiliateKey == affiliateKey)
                            .AsQueryable().ToPaginatedList(pageIndex, pageSize)
                );

                return GetContainerThroughMock(shipmentSrvMock);
            }
        }

        public class GetShipment {

            [Fact, NullCurrentPrincipal]
            public async Task Returns_200_And_Shipment_If_Request_Authorized_And_Shipment_Exists() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                // There are totatly 9 shipments inside the fake collection.
                // First three belong to affiliateKeys[0]. Take one of them
                var validShipmetKey = shipmentKeys[2];

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, affiliateKeys, validAffiliateKey, validShipmetKey);

                // Act
                var shipmentDto = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<ShipmentDto>(
                        configAndRequest.Item1,
                        configAndRequest.Item2,
                        HttpStatusCode.OK);

                // Assert
                Assert.Equal(validShipmetKey, shipmentDto.Key);
                Assert.Equal(validAffiliateKey, shipmentDto.AffiliateKey);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_404_If_Request_Authorized_But_Shipment_Does_Not_Exist() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                var invalidShipmetKey = Guid.NewGuid();

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, affiliateKeys, validAffiliateKey, invalidShipmetKey);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(
                        configAndRequest.Item1, configAndRequest.Item2);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_401_If_Request_Authorized_But_Requested_Shipment_Is_Not_Related_To_Affiliate() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                // There are totatly 9 shipments inside the fake collection.
                // First three belong to affiliateKeys[0]. Take one of the others
                var invalidShipmetKey = shipmentKeys[5];

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, affiliateKeys, validAffiliateKey, invalidShipmetKey);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(
                        configAndRequest.Item1, configAndRequest.Item2);

                // Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }

            private static Tuple<HttpConfiguration, HttpRequestMessage> GetConfigAndRequestMessage(
                Guid[] shipmentKeys, Guid[] affiliateKeys,
                Guid affiliateRequestKey, Guid shipmentRequestKey) {

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetContainer(shipmentKeys, affiliateKeys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            string.Format(ApiBaseRequestPathFormat, affiliateRequestKey), shipmentRequestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAffiliateUserName,
                        password: Constants.ValidAffiliatePassword);

                return Tuple.Create(config, request);
            }

            private static IContainer GetContainer(Guid[] shipmentKeys, Guid[] affiliateKeys) {

                var shipments = GetDummyShipments(shipmentKeys, affiliateKeys);
                var affiliates = GetDummyAffiliates(affiliateKeys);

                Mock<IShipmentService> shipmentSrvMock =
                    GetInitialShipmentServiceMock(shipments, affiliates);

                shipmentSrvMock.Setup(ss => ss.GetShipment(
                        It.IsAny<Guid>()
                    )
                ).Returns<Guid>(key => shipments.FirstOrDefault(x => x.Key == key));

                return GetContainerThroughMock(shipmentSrvMock);
            }
        }

        public class PostShipment {

            [Fact, NullCurrentPrincipal]
            public async Task Returns_201_And_Shipment_If_Request_Authorized_And_Success() {

                // Arange
                var availableShipmentTypeKeys = IntegrationTestHelper.GetKeys(3);
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                var shipmentRequestModel = new ShipmentByAffiliateRequestModel {
                    ShipmentTypeKey = availableShipmentTypeKeys[1],
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

                var configAndRequest = GetConfigAndRequestMessage(shipmentKeys, 
                    affiliateKeys, availableShipmentTypeKeys, validAffiliateKey, shipmentRequestModel);

                // Act
                var shipmentDto = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<ShipmentDto>(
                        configAndRequest.Item1,
                        configAndRequest.Item2, 
                        HttpStatusCode.Created);

                // Assert
                Assert.Equal(shipmentRequestModel.Price, shipmentDto.Price);
                Assert.Equal(shipmentRequestModel.ReceiverName, shipmentDto.ReceiverName);
                Assert.Equal(shipmentRequestModel.ReceiverEmail, shipmentDto.ReceiverEmail);
                Assert.NotNull(shipmentDto.ShipmentType);
                Assert.True(shipmentDto.ShipmentStates.Count() > 0);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_409_If_Request_Authorized_But_Conflicted() {

                // Arange
                var availableShipmentTypeKeys = IntegrationTestHelper.GetKeys(3);
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                // Put an unavailable ShipmentType
                var shipmentRequestModel = new ShipmentByAffiliateRequestModel {
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

                var configAndRequest = GetConfigAndRequestMessage(shipmentKeys,
                    affiliateKeys, availableShipmentTypeKeys, validAffiliateKey, shipmentRequestModel);

                // Act
                var response = await IntegrationTestHelper.GetResponseAsync(
                    configAndRequest.Item1, configAndRequest.Item2);

                // Assert
                Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_400_If_Request_Authorized_But_Message_Body_Is_Empty() {

                // Arange
                var availableShipmentTypeKeys = IntegrationTestHelper.GetKeys(3);
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                var configAndRequest = GetConfigAndRequestMessage(shipmentKeys,
                    affiliateKeys, availableShipmentTypeKeys, validAffiliateKey);

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
                Guid[] shipmentKeys, Guid[] affiliateKeys,
                Guid[] availableShipmentTypeKeys, Guid affiliateRequestKey) {

                return GetConfigAndRequestMessage(shipmentKeys, affiliateKeys, 
                    availableShipmentTypeKeys, affiliateRequestKey, null);
            }

            private static Tuple<HttpConfiguration, HttpRequestMessage> GetConfigAndRequestMessage(
                Guid[] shipmentKeys, Guid[] affiliateKeys, 
                Guid[] availableShipmentTypeKeys, Guid affiliateRequestKey,
                ShipmentByAffiliateRequestModel requestModel) {

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                            GetContainer(shipmentKeys, affiliateKeys, availableShipmentTypeKeys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            string.Format(ApiBaseRequestPathFormat, affiliateRequestKey)),
                        mediaType: "application/json",
                        username: Constants.ValidAffiliateUserName,
                        password: Constants.ValidAffiliatePassword);

                if (requestModel != null) {

                    request.Content = new ObjectContent<ShipmentByAffiliateRequestModel>(
                        requestModel, new JsonMediaTypeFormatter());
                }

                return Tuple.Create(config, request);
            }

            private static IContainer GetContainer(
                Guid[] shipmentKeys, Guid[] affiliateKeys, Guid[] availableShipmentTypeKeys) {

                var shipments = GetDummyShipments(shipmentKeys, affiliateKeys, availableShipmentTypeKeys);
                var affiliates = GetDummyAffiliates(affiliateKeys);
                var shipmentSrvMock = GetShipmentSrvMock(shipments, affiliates, availableShipmentTypeKeys);

                return GetContainerThroughMock(shipmentSrvMock);
            }

            private static Mock<IShipmentService> GetShipmentSrvMock(
                IEnumerable<Shipment> shipments, 
                IEnumerable<Affiliate> affiliates, 
                Guid[] availableShipmentTypeKeys) {

                Mock<IShipmentService> shipmentSrvMock =
                    GetInitialShipmentServiceMock(shipments, affiliates);

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
            public async Task Returns_404_If_Request_Authorized_But_Shipment_Does_Not_Exist() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                var invalidShipmetKey = Guid.NewGuid();

                var shipmentRequestModel = new ShipmentByAffiliateRequestModel {
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

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, affiliateKeys, validAffiliateKey, invalidShipmetKey, shipmentRequestModel);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(
                        configAndRequest.Item1, configAndRequest.Item2);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_401_If_Request_Authorized_But_Requested_Shipment_Is_Not_Related_To_Affiliate() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                // There are totatly 9 shipments inside the fake collection.
                // First three belong to affiliateKeys[0]. Take one of the others
                var invalidShipmetKey = shipmentKeys[5];

                var shipmentRequestModel = new ShipmentByAffiliateRequestModel {
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

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, affiliateKeys, validAffiliateKey, invalidShipmetKey, shipmentRequestModel);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(
                        configAndRequest.Item1, configAndRequest.Item2);

                // Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_200_And_Updated_Shipment_If_Request_Authorized_And_Request_Is_Valid() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                // There are totatly 9 shipments inside the fake collection.
                // First three belong to affiliateKeys[0]. Take one of them
                var validShipmetKey = shipmentKeys[2];

                var shipmentRequestModel = new ShipmentByAffiliateRequestModel {
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

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(shipmentKeys, 
                    affiliateKeys, validAffiliateKey, validShipmetKey, shipmentRequestModel);

                // Act
                var shipmentDto = await IntegrationTestHelper.
                    GetResponseMessageBodyAsync<ShipmentDto>(
                        configAndRequest.Item1, 
                        configAndRequest.Item2, 
                        HttpStatusCode.OK);

                // Assert
                Assert.Equal(validShipmetKey, shipmentDto.Key);
                Assert.Equal(validAffiliateKey, shipmentDto.AffiliateKey);
                Assert.Equal(shipmentRequestModel.Price, shipmentDto.Price);
                Assert.Equal(shipmentRequestModel.ReceiverEmail, shipmentDto.ReceiverEmail);
            }

            private static Tuple<HttpConfiguration, HttpRequestMessage> GetConfigAndRequestMessage(
                Guid[] shipmentKeys, Guid[] affiliateKeys,
                Guid affiliateRequestKey, Guid shipmentRequestKey) {

                return GetConfigAndRequestMessage(shipmentKeys, affiliateKeys,
                    affiliateRequestKey, shipmentRequestKey, null);
            }

            private static Tuple<HttpConfiguration, HttpRequestMessage> GetConfigAndRequestMessage(
                Guid[] shipmentKeys, Guid[] affiliateKeys,
                Guid affiliateRequestKey, Guid shipmentRequestKey,
                ShipmentByAffiliateRequestModel requestModel) {

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                            GetContainer(shipmentKeys, affiliateKeys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            string.Format(ApiBaseRequestPathFormat, affiliateRequestKey), shipmentRequestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAffiliateUserName,
                        password: Constants.ValidAffiliatePassword);

                if (requestModel != null) {

                    request.Content = new ObjectContent<ShipmentByAffiliateRequestModel>(
                        requestModel, new JsonMediaTypeFormatter());
                }

                return Tuple.Create(config, request);
            }

            private static IContainer GetContainer(Guid[] shipmentKeys, Guid[] affiliateKeys) {

                var shipments = GetDummyShipments(shipmentKeys, affiliateKeys);
                var affiliates = GetDummyAffiliates(affiliateKeys);

                Mock<IShipmentService> shipmentSrvMock =
                    GetInitialShipmentServiceMock(shipments, affiliates);

                shipmentSrvMock.Setup(ss => ss.GetShipment(
                        It.IsAny<Guid>()
                    )
                ).Returns<Guid>(key => shipments.FirstOrDefault(x => x.Key == key));

                shipmentSrvMock.Setup(ss => ss.UpdateShipment(
                        It.IsAny<Shipment>()
                    )
                ).Returns<Shipment>(shipment => shipment);

                return GetContainerThroughMock(shipmentSrvMock);
            }
        }

        public class DeleteShipment {

            [Fact, NullCurrentPrincipal]
            public async Task Returns_404_If_Request_Authorized_But_Shipment_Does_Not_Exist() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                var invalidShipmetKey = Guid.NewGuid();

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, affiliateKeys, validAffiliateKey, invalidShipmetKey);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(
                        configAndRequest.Item1, configAndRequest.Item2);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_401_If_Request_Authorized_But_Requested_Shipment_Is_Not_Related_To_Affiliate() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                // There are totatly 9 shipments inside the fake collection.
                // First three belong to affiliateKeys[0]. Take one of the others
                var invalidShipmetKey = shipmentKeys[5];

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, affiliateKeys, validAffiliateKey, invalidShipmetKey);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(
                        configAndRequest.Item1, configAndRequest.Item2);

                // Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_409_If_Request_Authorized_But_Conflicted() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                // There are totatly 9 shipments inside the fake collection.
                // First three belong to affiliateKeys[0]. Also, first shipment
                // of each affiliate is InTransit (in other words, not deletable
                // according to our test criteria).
                var invalidShipmetKey = shipmentKeys[0];

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, affiliateKeys, validAffiliateKey, invalidShipmetKey);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(
                        configAndRequest.Item1, configAndRequest.Item2);

                // Assert
                Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task Returns_204_If_Request_Authorized_And_Request_Is_Deleted_Sucessfully() {

                // Arrange
                var shipmentKeys = IntegrationTestHelper.GetKeys(9);
                var affiliateKeys = IntegrationTestHelper.GetKeys(3);

                // Except for first key, all keys are not 
                // related to current Affiliate user
                var validAffiliateKey = affiliateKeys[0];

                // There are totatly 9 shipments inside the fake collection.
                // First three belong to affiliateKeys[0]. Also, first shipment
                // of each affiliate is InTransit (in other words, not deletable
                // according to our test criteria).
                var invalidShipmetKey = shipmentKeys[1];

                // Get the config and request
                var configAndRequest = GetConfigAndRequestMessage(
                    shipmentKeys, affiliateKeys, validAffiliateKey, invalidShipmetKey);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(
                        configAndRequest.Item1, configAndRequest.Item2);

                // Assert
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }

            private static Tuple<HttpConfiguration, HttpRequestMessage> GetConfigAndRequestMessage(
                Guid[] shipmentKeys, Guid[] affiliateKeys,
                Guid affiliateRequestKey, Guid shipmentRequestKey) {

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                            GetContainer(shipmentKeys, affiliateKeys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Delete,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            string.Format(ApiBaseRequestPathFormat, affiliateRequestKey), shipmentRequestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAffiliateUserName,
                        password: Constants.ValidAffiliatePassword);

                return Tuple.Create(config, request);
            }

            private static IContainer GetContainer(Guid[] shipmentKeys, Guid[] affiliateKeys) {

                var shipments = GetDummyShipments(shipmentKeys, affiliateKeys);
                var affiliates = GetDummyAffiliates(affiliateKeys);

                Mock<IShipmentService> shipmentSrvMock =
                    GetInitialShipmentServiceMock(shipments, affiliates);

                shipmentSrvMock.Setup(ss => ss.GetShipment(
                        It.IsAny<Guid>()
                    )
                ).Returns<Guid>(key => shipments.FirstOrDefault(x => x.Key == key));

                shipmentSrvMock.Setup(ss => ss.RemoveShipment(
                        It.IsAny<Shipment>()
                    )
                ).Returns<Shipment>(shipment => !IsShipmentRemovable(shipment) ? 
                    new OperationResult(false) : new OperationResult(true)
                );

                return GetContainerThroughMock(shipmentSrvMock);
            }

            private static bool IsShipmentRemovable(Shipment shipment) {

                var latestStatus = (from shipmentState in shipment.ShipmentStates.ToList()
                                    orderby shipmentState.ShipmentStatus descending
                                    select shipmentState).First();

                return latestStatus.ShipmentStatus < ShipmentStatus.InTransit;
            }
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
                        Name = (i == 0) ? 
                               Constants.ValidAffiliateUserName : 
                               string.Format("Comp{0}", i),
                        Email = string.Format("comp{0}@expample.com", i),
                        IsLocked = false,
                        CreatedOn = DateTime.Now.AddDays(-5),
                        LastUpdatedOn = DateTime.Now.AddDays(-1)
                    }
                };
            }
        }

        private static IEnumerable<Shipment> GetDummyShipments(
            Guid[] shipmentKeys, Guid[] affiliateKeys) {

            return GetDummyShipments(
                shipmentKeys, affiliateKeys, IntegrationTestHelper.GetKeys(3));
        }

        private static IEnumerable<Shipment> GetDummyShipments(
            Guid[] shipmentKeys, Guid[] affiliateKeys, Guid[] shipmentTypeKeys) {

            for (int i = 0; i < 9; i++) {
                
                var states = new List<ShipmentState> { 
                    new ShipmentState { Key = Guid.NewGuid(), ShipmentKey = shipmentKeys[i], ShipmentStatus = ShipmentStatus.Ordered },
                    new ShipmentState { Key = Guid.NewGuid(), ShipmentKey = shipmentKeys[i], ShipmentStatus = ShipmentStatus.Scheduled }
                };

                // Make the first shipment InTransit for each affiliate
                if((i % 3) == 0) { 
                    states.Add(new ShipmentState { 
                        Key = Guid.NewGuid(), ShipmentKey = shipmentKeys[i], ShipmentStatus = ShipmentStatus.InTransit
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

        private static Mock<IShipmentService> GetInitialShipmentServiceMock(
            IEnumerable<Shipment> shipments, IEnumerable<Affiliate> affiliates) {

            // These are the operations which are needed 
            // for any request against this controller
            Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
            shipmentSrvMock.Setup(ss => ss.GetAffiliate(
                    It.IsAny<Guid>()
                )
            ).Returns<Guid>(key =>
                affiliates.FirstOrDefault(
                    x => x.Key == key
                )
            );

            shipmentSrvMock.Setup(ss => ss.IsAffiliateRelatedToUser(
                    It.IsAny<Guid>(), It.IsAny<string>()
                )
            ).Returns<Guid, string>((key, username) =>
                affiliates.Any(
                    x => x.Key == key && x.User.Name.Equals(
                        username, StringComparison.OrdinalIgnoreCase
                    )
                )
            );

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