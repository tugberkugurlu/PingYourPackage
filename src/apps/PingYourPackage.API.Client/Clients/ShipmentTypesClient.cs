using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiDoodle.Net.Http.Client;
using WebApiDoodle.Net.Http.Client.Model;

namespace PingYourPackage.API.Client.Clients {

    public class ShipmentTypesClient : HttpApiClient<ShipmentTypeDto>, IShipmentTypesClient {

        private const string BaseUriTemplate = "api/shipmenttypes";
        private const string BaseUriTemplateForSingle = "api/shipmenttypes/{key}";

        public ShipmentTypesClient(HttpClient httpClient)
            : base(httpClient, MediaTypeFormatterCollection.Instance) {
        }

        public async Task<PaginatedDto<ShipmentTypeDto>> GetShipmentTypesAsync(PaginatedRequestCommand paginationCmd) {

            using (var apiResponse = await base.GetAsync(BaseUriTemplate, paginationCmd)) {

                if (apiResponse.IsSuccess) {

                    return apiResponse.Model;
                }

                throw new HttpApiRequestException(
                    string.Format(ErrorMessages.HttpRequestErrorFormat, (int)apiResponse.Response.StatusCode, apiResponse.Response.ReasonPhrase),
                    apiResponse.Response.StatusCode, apiResponse.HttpError);
            }
        }

        public async Task<ShipmentTypeDto> GetShipmentTypeAsync(Guid shipmentTypeKey) { 

            var parameters = new { key = shipmentTypeKey };
            using (var apiResponse = await base.GetSingleAsync(BaseUriTemplateForSingle, parameters)) {

                if (apiResponse.IsSuccess) {

                    return apiResponse.Model;
                }

                throw new HttpApiRequestException(
                    string.Format(ErrorMessages.HttpRequestErrorFormat, (int)apiResponse.Response.StatusCode, apiResponse.Response.ReasonPhrase),
                    apiResponse.Response.StatusCode, apiResponse.HttpError);
            }
        }
    }
}