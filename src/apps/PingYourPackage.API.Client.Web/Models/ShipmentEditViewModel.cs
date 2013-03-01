using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestModels;

namespace PingYourPackage.API.Client.Web.Models {
    
    public class ShipmentEditViewModel {

        public ShipmentDto Shipment { get; set; }
        public ShipmentByAffiliateUpdateRequestModel ShipmentForEdit { get; set; }
    }
}