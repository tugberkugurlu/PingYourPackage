using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Entities;

namespace PingYourPackage.API.Model {
    
    internal static class AffiliateUpdateRequestModelExtensions {

        internal static Affiliate ToAffiliate(
            this AffiliateUpdateRequestModel requestModel,
            Affiliate existingAffiliate) {

            existingAffiliate.Address = requestModel.Address;
            existingAffiliate.CompanyName = requestModel.CompanyName;
            existingAffiliate.TelephoneNumber = requestModel.TelephoneNumber;

            return existingAffiliate;
        }
    }
}