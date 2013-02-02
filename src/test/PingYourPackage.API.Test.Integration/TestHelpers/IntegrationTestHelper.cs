using Autofac;
using PingYourPackage.API.Config;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiDoodle.Net.Http.Client.Model;
using Xunit;

namespace PingYourPackage.API.Test.Integration {
    
    internal static class IntegrationTestHelper {

        internal static Guid[] GetKeys(int count) {

            var array = new Guid[count];
            for (int i = 0; i < count; i++) {
                array[i] = Guid.NewGuid();
            }

            return array;
        }

        internal static HttpConfiguration GetInitialIntegrationTestConfig() {

            var config = new HttpConfiguration();
            RouteConfig.RegisterRoutes(config);
            WebAPIConfig.Configure(config);
            
            return config;
        }

        internal static HttpConfiguration GetInitialIntegrationTestConfig(IContainer container) {

            var config = GetInitialIntegrationTestConfig();
            AutofacWebAPI.Initialize(config, container);

            return config;
        }

        internal static ContainerBuilder GetEmptyContainerBuilder() {

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(
                Assembly.GetAssembly(typeof(WebAPIConfig)));

            return builder;
        }

        // Tests

        internal static async Task<PaginatedDto<TDto>> TestForPaginatedDtoAsync<TDto>(
            HttpConfiguration config,
            HttpRequestMessage request,
            int expectedPageIndex,
            int expectedTotalPageCount,
            int expectedCurrentItemsCount,
            int expectedTotalItemsCount,
            bool expectedHasNextPageResult,
            bool expectedHasPreviousPageResult) where TDto : IDto {

            // Act
            var userPaginatedDto = await 
                GetResponseMessageBodyAsync<PaginatedDto<TDto>>(
                    config, request, HttpStatusCode.OK);

            // Assert
            Assert.Equal(expectedPageIndex, userPaginatedDto.PageIndex);
            Assert.Equal(expectedTotalPageCount, userPaginatedDto.TotalPageCount);
            Assert.Equal(expectedCurrentItemsCount, userPaginatedDto.Items.Count());
            Assert.Equal(expectedTotalItemsCount, userPaginatedDto.TotalCount);
            Assert.Equal(expectedHasNextPageResult, userPaginatedDto.HasNextPage);
            Assert.Equal(expectedHasPreviousPageResult, userPaginatedDto.HasPreviousPage);

            return userPaginatedDto;
        }

        internal static async Task<TResult> GetResponseMessageBodyAsync<TResult>(
            HttpConfiguration config, 
            HttpRequestMessage request, 
            HttpStatusCode expectedHttpStatusCode) {

            var response = await GetResponseAsync(config, request);
            Assert.Equal(expectedHttpStatusCode, response.StatusCode);
            var result = await response.Content.ReadAsAsync<TResult>();

            return result;
        }

        internal static async Task<HttpResponseMessage> GetResponseAsync(
            HttpConfiguration config, HttpRequestMessage request) {

            using (var httpServer = new HttpServer(config))
            using (var client = HttpClientFactory.Create(innerHandler: httpServer)) {

                return await client.SendAsync(request);
            }
        }
    }
}