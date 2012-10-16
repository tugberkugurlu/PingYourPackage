using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Entities;

namespace PingYourPackage.API.Model {
    
    internal static class AffiliateRequestModelExtensions {

        internal static Affiliate ToAffiliate(this AffiliateRequestModel requestModel) {

            return new Affiliate {
                CompanyName = requestModel.CompanyName,
                Address = requestModel.Address,
                TelephoneNumber = requestModel.TelephoneNumber
            };
        }
    }
}