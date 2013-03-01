using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using PingYourPackage.API.Model.RequestModels;
using System;
using System.Threading.Tasks;
using WebApiDoodle.Net.Http.Client.Model;

namespace PingYourPackage.API.Client.Clients {

    public interface IShipmentsClient {

        Task<PaginatedDto<ShipmentDto>> GetShipmentsAsync(PaginatedRequestCommand paginationCmd);
        Task<ShipmentDto> GetShipmentAsync(Guid shipmentKey);
        Task<ShipmentDto> AddShipmentAsync(ShipmentByAffiliateRequestModel requestModel);
        Task<ShipmentDto> UpdateShipmentAsync(Guid shipmentKey, ShipmentByAffiliateUpdateRequestModel requestModel);
        Task RemoveShipmentAsync(Guid shipmentKey);
    }
}