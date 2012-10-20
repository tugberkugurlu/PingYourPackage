using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using PingYourPackage.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using PingYourPackage.Domain.Services;
using System.Net;
using System.Net.Http;
using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.API.Filters;

namespace PingYourPackage.API.Controllers {

    [Authorize(Roles = "Admin,Employee")]
    public class AffiliatesController : ApiController {

        private readonly IShipmentService _shipmentService;

        public AffiliatesController(IShipmentService shipmentService) {

            _shipmentService = shipmentService;
        }

        public PaginatedDto<AffiliateDto> GetAffiliates(PaginatedRequestCommand cmd) {

            var affiliates = _shipmentService.GetAffiliates(
                cmd.Page, cmd.Take);

            return affiliates.ToPaginatedDto(
                affiliates.Select(af => af.ToAffiliateDto()));
        }

        public AffiliateDto GetAffiliate(Guid key) {

            var affiliate = _shipmentService.GetAffiliate(key);
            if (affiliate == null) {

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return affiliate.ToAffiliateDto();
        }

        public HttpResponseMessage PostAffiliate(AffiliateRequestModel requestModel) {

            var createdAffiliateResult = _shipmentService
                .AddAffiliate(
                    requestModel.UserKey.Value, 
                    requestModel.ToAffiliate());

            if (!createdAffiliateResult.IsSuccess) {

                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }

            var response = Request.CreateResponse(HttpStatusCode.Created,
                createdAffiliateResult.Entity.ToAffiliateDto());

            response.Headers.Location = new Uri(Url.Link("DefaultHttpRoute",
                    new { key = createdAffiliateResult.Entity.Key }));

            return response;
        }

        public AffiliateDto PutAffiliate(Guid key, AffiliateUpdateRequestModel requestModel) {

            var affiliate = _shipmentService.GetAffiliate(key);
            if (affiliate == null) {

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var updatedAffiliate = _shipmentService.UpdateAffiliate(
                requestModel.ToAffiliate(affiliate));

            return updatedAffiliate.ToAffiliateDto();
        }
    }
}