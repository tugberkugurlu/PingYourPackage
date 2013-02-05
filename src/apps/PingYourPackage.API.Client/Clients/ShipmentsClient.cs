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
            using (var apiResponse = await base.GetAsync(BaseUriTemplate, parameters)) {

                if (apiResponse.IsSuccess) {

                    return apiResponse.Model;
                }

                throw new HttpApiRequestException(
                    string.Format(ErrorMessages.HttpRequestErrorFormat, (int)apiResponse.Response.StatusCode, apiResponse.Response.ReasonPhrase),
                    apiResponse.Response.StatusCode, apiResponse.HttpError);
            }
        }

        public async Task<ShipmentDto> GetShipmentAsync(Guid shipmentKey) {

            var parameters = new { key = _affiliateKey, shipmentKey = shipmentKey };
            using (var apiResponse = await base.GetSingleAsync(BaseUriTemplateForSingle, parameters)) {

                if (apiResponse.IsSuccess) {

                    return apiResponse.Model;
                }

                throw new HttpApiRequestException(
                    string.Format(ErrorMessages.HttpRequestErrorFormat, (int)apiResponse.Response.StatusCode, apiResponse.Response.ReasonPhrase),
                    apiResponse.Response.StatusCode, apiResponse.HttpError);
            }
        }

        public async Task<ShipmentDto> AddShipmentAsync(ShipmentByAffiliateRequestModel requestModel) {

            var parameters = new { key = _affiliateKey };
            using (var apiResponse = await base.PostAsync(BaseUriTemplate, requestModel, parameters)) {

                if (apiResponse.IsSuccess) {

                    return apiResponse.Model;
                }

                throw new HttpApiRequestException(
                    string.Format(ErrorMessages.HttpRequestErrorFormat, (int)apiResponse.Response.StatusCode, apiResponse.Response.ReasonPhrase),
                    apiResponse.Response.StatusCode, apiResponse.HttpError);
            }
        }

        public async Task<ShipmentDto> UpdateShipmentAsync(Guid shipmentKey, ShipmentByAffiliateUpdateRequestModel requestModel) {

            var parameters = new { key = _affiliateKey, shipmentKey = shipmentKey };
            using (var apiResponse = await base.PutAsync(BaseUriTemplateForSingle, requestModel, parameters)) {

                if (apiResponse.IsSuccess) {

                    return apiResponse.Model;
                }

                throw new HttpApiRequestException(
                    string.Format(ErrorMessages.HttpRequestErrorFormat, (int)apiResponse.Response.StatusCode, apiResponse.Response.ReasonPhrase),
                    apiResponse.Response.StatusCode, apiResponse.HttpError);
            }
        }

        public async Task RemoveCar(Guid shipmentKey) {
            
            var parameters = new { key = _affiliateKey, shipmentKey = shipmentKey };
            using (var apiResponse = await base.DeleteAsync(BaseUriTemplateForSingle, parameters)) {

                if (!apiResponse.IsSuccess) {

                    throw new HttpApiRequestException(
                        string.Format(ErrorMessages.HttpRequestErrorFormat, (int)apiResponse.Response.StatusCode, apiResponse.Response.ReasonPhrase),
                        apiResponse.Response.StatusCode, apiResponse.HttpError);
                }
            }
        }
    }
}