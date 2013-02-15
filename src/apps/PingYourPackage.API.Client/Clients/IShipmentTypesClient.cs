using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using System;
using System.Threading.Tasks;
using WebApiDoodle.Net.Http.Client.Model;

namespace PingYourPackage.API.Client.Clients {

    public interface IShipmentTypesClient {

        Task<ShipmentTypeDto> GetShipmentTypeAsync(Guid shipmentTypeKey);
        Task<PaginatedDto<ShipmentTypeDto>> GetShipmentTypesAsync(PaginatedRequestCommand paginationCmd);
    }
}