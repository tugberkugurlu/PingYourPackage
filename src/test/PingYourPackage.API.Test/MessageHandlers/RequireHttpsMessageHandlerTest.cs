using PingYourPackage.API.MessageHandlers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PingYourPackage.API.Test.MessageHandlers {
    
    public class RequireHttpsMessageHandlerTest {

        [Fact]
        public async Task Returns_Forbidden_If_Request_Is_Not_Over_HTTPS() {
            
            //Arange
            var request = new HttpRequestMessage(
                HttpMethod.Get, "http://localhost:8080");

            //Act
            var response = await TestHelper.InvokeMessageHandler(request,
                new RequireHttpsMessageHandler());

            Assert.Equal(
                HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Returns_Delegated_StatusCode_When_Request_Is_Over_HTTPS() {

            //Arange
            var request = new HttpRequestMessage(
                HttpMethod.Get, "https://localhost:8080");

            //Act
            var response = await TestHelper.InvokeMessageHandler(request, 
                new RequireHttpsMessageHandler());

            Assert.Equal(
                HttpStatusCode.OK, response.StatusCode);
        }
    }
}