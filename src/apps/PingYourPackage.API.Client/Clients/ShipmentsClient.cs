using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using PingYourPackage.API.Model.RequestModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiDoodle.Net.Http.Client;
using WebApiDoodle.Net.Http.Client.Model;

namespace PingYourPackage.API.Client.Clients {

    public class ShipmentsClient : HttpApiClient<ShipmentDto>, IShipmentsClient {

        private const string BaseUriTemplate = "api/affiliates/{key}/shipments";
        private const string BaseUriTemplateForSingle = "api/affiliates/{key}/shipments/{shipmentKey}";
        private readonly string _affiliateKey;

        public ShipmentsClient(HttpClient httpClient, string affiliateKey)
            : base(httpClient, MediaTypeFormatterCollection.Instance) {

            if (string.IsNullOrEmpty(affiliateKey)) {

                throw new ArgumentException("The argument 'affiliateKey' is null or empty.", "affiliateKey");
            }

            _affiliateKey = affiliateKey;
        }

        public async Task<PaginatedDto<ShipmentDto>> GetShipmentsAsync(PaginatedRequestCommand paginationCmd) {

            var parameters = new { key = _affiliateKey, page = paginationCmd.Page, take = paginationCmd.Take };
            var responseTask = base.GetAsync(BaseUriTemplate, parameters);
            var shipments = await HandleResponseAsync(responseTask);
            return shipments;
        }

        public async Task<ShipmentDto> GetShipmentAsync(Guid shipmentKey) {

            var parameters = new { key = _affiliateKey, shipmentKey = shipmentKey };
            var responseTask = base.GetSingleAsync(BaseUriTemplateForSingle, parameters);
            var shipment = await HandleResponseAsync(responseTask);
            return shipment;
        }

        public async Task<ShipmentDto> AddShipmentAsync(ShipmentByAffiliateRequestModel requestModel) {

            var parameters = new { key = _affiliateKey };
            var responseTask = base.PostAsync(BaseUriTemplate, requestModel, parameters);
            var shipment = await HandleResponseAsync(responseTask);
            return shipment;
        }

        public async Task<ShipmentDto> UpdateShipmentAsync(Guid shipmentKey, ShipmentByAffiliateUpdateRequestModel requestModel) {

            var parameters = new { key = _affiliateKey, shipmentKey = shipmentKey };
            var responseTask = base.PutAsync(BaseUriTemplateForSingle, requestModel, parameters);
            var shipment = await HandleResponseAsync(responseTask);
            return shipment;
        }

        public async Task RemoveShipmentAsync(Guid shipmentKey) {
            
            var parameters = new { key = _affiliateKey, shipmentKey = shipmentKey };
            var responseTask = base.DeleteAsync(BaseUriTemplateForSingle, parameters);
            await HandleResponseAsync(responseTask);
        }

        // private helpers

        private async Task<TResult> HandleResponseAsync<TResult>(Task<HttpApiResponseMessage<TResult>> responseTask) {

            using (var apiResponse = await responseTask) {

                if (apiResponse.IsSuccess) {

                    return apiResponse.Model;
                }

                throw GetHttpApiRequestException(apiResponse);
            }
        }

        private async Task HandleResponseAsync(Task<HttpApiResponseMessage> responseTask) {

            using (var apiResponse = await responseTask) {

                if (!apiResponse.IsSuccess) {

                    throw GetHttpApiRequestException(apiResponse);
                }
            }
        }

        private HttpApiRequestException GetHttpApiRequestException(HttpApiResponseMessage apiResponse) {

            return new HttpApiRequestException(
                string.Format(ErrorMessages.HttpRequestErrorFormat, (int)apiResponse.Response.StatusCode, apiResponse.Response.ReasonPhrase),
                apiResponse.Response.StatusCode, apiResponse.HttpError);
        }
    }
}