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
    
    public class ShipmentTypesControllerIntegrationTest {

        public class GetShipmentTypes {

            [Fact, NullCurrentPrincipal]
            public Task
                Returns_200_And_ShipmentTypes_If_Request_Authorized() {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetContainer());

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

            private static IContainer GetContainer() {

                var shipmentTypes = GetDummyShipmentTypes(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
                var containerBuilder = GetInitialContainerBuilder();

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

                containerBuilder.Register(c => shipmentSrvMock.Object)
                    .As<IShipmentService>()
                    .InstancePerApiRequest();

                return containerBuilder.Build();
            }
        }

        public class GetShipmentType {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_ShipmentType_If_Request_Authorized_And_ShipmentType_Exists() {

                // Arrange
                Guid[] keys = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
                var shipmentTypes = GetDummyShipmentTypes(keys);
                var requestKey = keys[1];
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetContainer(keys, shipmentTypes));

                var expectedShipmentType = shipmentTypes.FirstOrDefault(
                    st => st.Key == requestKey);

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/shipmenttypes", requestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                // Act
                var shipmentTypeDto = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<ShipmentTypeDto>(
                        config, request, HttpStatusCode.OK);

                // Assert
                Assert.Equal(expectedShipmentType.Key, shipmentTypeDto.Key);
                Assert.Equal(expectedShipmentType.Name, shipmentTypeDto.Name);
                Assert.Equal(expectedShipmentType.Price, shipmentTypeDto.Price);
                Assert.Equal(expectedShipmentType.CreatedOn, shipmentTypeDto.CreatedOn);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_ShipmentType_Does_Not_Exist() {

                // Arrange
                Guid[] keys = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
                var shipmentTypes = GetDummyShipmentTypes(keys);
                var invalidRequestKey = Guid.NewGuid();
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetContainer(keys, shipmentTypes));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/shipmenttypes", invalidRequestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            private static IContainer GetContainer(Guid[] keys, 
                IEnumerable<ShipmentType> shipmentTypes) {

                var containerBuilder = GetInitialContainerBuilder();

                Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
                shipmentSrvMock.Setup(ss => ss.GetShipmentType(
                        It.Is<Guid>(
                            key => keys.Contains(key)
                        )
                    )
                ).Returns<Guid>(key =>
                    shipmentTypes.FirstOrDefault(
                        st => st.Key == key
                    )
                );

                containerBuilder.Register(c => shipmentSrvMock.Object)
                    .As<IShipmentService>()
                    .InstancePerApiRequest();

                return containerBuilder.Build();
            }
        }

        public class PostShipmentType {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_201_And_ShipmentType_If_Request_Authorized_And_Success() {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer());

                var shipmentTypeRequestModel = new ShipmentTypeRequestModel {
                    Name = "X-Large",
                    Price = 40.00M
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            "api/shipmenttypes"),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentTypeRequestModel>(
                    shipmentTypeRequestModel, new JsonMediaTypeFormatter());

                // Act
                var shipmentTypeDto = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<ShipmentTypeDto>(
                        config, request, HttpStatusCode.Created);

                // Assert
                Assert.Equal(shipmentTypeRequestModel.Name, shipmentTypeDto.Name);
                Assert.Equal(shipmentTypeRequestModel.Price, shipmentTypeDto.Price);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_409_If_Request_Authorized_But_Conflicted() {

                // Arrange
                var shipmentTypes = GetDummyShipmentTypes(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() });
                var targetShipmentType = shipmentTypes.FirstOrDefault();
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetContainer(shipmentTypes));

                var shipmentTypeRequestModel = new ShipmentTypeRequestModel {
                    Name = targetShipmentType.Name,
                    Price = 40.00M
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            "api/shipmenttypes"),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentTypeRequestModel>(
                    shipmentTypeRequestModel, new JsonMediaTypeFormatter());

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Invalid() {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer());

                var shipmentTypeRequestModel = new ShipmentTypeRequestModel {
                    Name = "ANameWhichIsMoreThan50CharsANameWhichIsMoreThan50Chars",
                    Price = 40.00M
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            "api/shipmenttypes"),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentTypeRequestModel>(
                    shipmentTypeRequestModel, new JsonMediaTypeFormatter());

                // Act
                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var nameError = modelState["requestModel.Name"] as string[];

                // Assert
                Assert.NotNull(nameError);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Message_Body_Is_Empty() {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer());

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            "api/shipmenttypes"),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                // Act
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
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() });
            }

            private static IContainer GetContainer(Guid[] keys) {

                var shipmentTypes = GetDummyShipmentTypes(keys);
                return GetContainer(shipmentTypes);
            }

            private static IContainer GetContainer(IEnumerable<ShipmentType> shipmentTypes) {

                var containerBuilder = GetInitialContainerBuilder();

                Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
                shipmentSrvMock.Setup(ss => ss.AddShipmentType(
                        It.IsAny<ShipmentType>()
                    )
                ).Returns<ShipmentType>(st => {

                    st.Key = Guid.NewGuid();
                    st.CreatedOn = DateTime.Now;

                    return new CreatedResult<ShipmentType>(true) { Entity = st };
                });

                shipmentSrvMock.Setup(ss => ss.AddShipmentType(
                        It.Is<ShipmentType>(
                            st => shipmentTypes.Any(
                                x => x.Name.Equals(
                                    st.Name, StringComparison.OrdinalIgnoreCase
                                )
                            )
                        )
                    )
                ).Returns(new CreatedResult<ShipmentType>(false));

                containerBuilder.Register(c => shipmentSrvMock.Object)
                    .As<IShipmentService>()
                    .InstancePerApiRequest();

                return containerBuilder.Build();
            }
        }

        public class PutShipmentType {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_ShipmentType_Does_Not_Exist() {

                // Arrange
                Guid[] keys = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
                var invalidRequestKey = Guid.NewGuid();
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(keys));

                var shipmentTypeRequestModel = new ShipmentTypeRequestModel { 
                    Name = "X-Large",
                    Price = 40.00M
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/shipmenttypes", invalidRequestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentTypeRequestModel>(
                    shipmentTypeRequestModel, new JsonMediaTypeFormatter());

                // Act
                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Invalid() {

                // Arrange
                Guid[] keys = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
                var requestKey = keys[1];
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(keys));

                var shipmentTypeRequestModel = new ShipmentTypeRequestModel {
                    Name = "ANameWhichIsMoreThan50CharsANameWhichIsMoreThan50Chars",
                    Price = 40.00M
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/shipmenttypes", requestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentTypeRequestModel>(
                    shipmentTypeRequestModel, new JsonMediaTypeFormatter());

                // Act
                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var nameError = modelState["requestModel.Name"] as string[];

                // Assert
                Assert.NotNull(nameError);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_Updated_ShipmentType_If_Request_Authorized_But_Request_Is_Valid() {

                // Arrange
                Guid[] keys = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
                var requestKey = keys[1];
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(keys));

                var shipmentTypeRequestModel = new ShipmentTypeRequestModel {
                    Name = "X-Large",
                    Price = 40.00M
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/shipmenttypes", requestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<ShipmentTypeRequestModel>(
                    shipmentTypeRequestModel, new JsonMediaTypeFormatter());

                // Act
                var shipmentTypeDto = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<ShipmentTypeDto>(
                        config, request, HttpStatusCode.OK);

                // Assert
                Assert.Equal(requestKey, shipmentTypeDto.Key);
                Assert.Equal(shipmentTypeRequestModel.Name, shipmentTypeDto.Name);
                Assert.Equal(shipmentTypeRequestModel.Price, shipmentTypeDto.Price);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Message_Body_Is_Empty() {

                // Arrange
                Guid[] keys = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
                var requestKey = keys[1];
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(GetContainer(keys));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/shipmenttypes", requestKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                // Act
                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var requestModelError = modelState["requestModel"] as string[];

                // Assert
                Assert.NotNull(requestModelError);
            }

            private static IContainer GetContainer(Guid[] keys) {

                var shipmentTypes = GetDummyShipmentTypes(keys);
                var containerBuilder = GetInitialContainerBuilder();

                Mock<IShipmentService> shipmentSrvMock = new Mock<IShipmentService>();
                shipmentSrvMock.Setup(ss => ss.GetShipmentType(
                        It.Is<Guid>(
                            key => keys.Contains(key)
                        )
                    )
                ).Returns<Guid>(key =>
                    shipmentTypes.FirstOrDefault(
                        st => st.Key == key
                    )
                );

                shipmentSrvMock.Setup(ss => ss.UpdateShipmentType(
                        It.IsAny<ShipmentType>()
                    )
                ).Returns<ShipmentType>(st => st);

                containerBuilder.Register(c => shipmentSrvMock.Object)
                    .As<IShipmentService>()
                    .InstancePerApiRequest();

                return containerBuilder.Build();
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