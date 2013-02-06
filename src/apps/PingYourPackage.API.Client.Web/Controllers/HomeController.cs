using PingYourPackage.API.Client.Clients;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApiDoodle.Net.Http.Client.Model;

namespace PingYourPackage.API.Client.Web.Controllers {

    public class HomeController : Controller {

        private const int DefaultPageSize = 5;
        private readonly IShipmentsClient _shipmentsClient;

        public HomeController(IShipmentsClient shipmentsClient) {

            _shipmentsClient = shipmentsClient;
        }

        public async Task<ViewResult> Index(int page = 1) {

            PaginatedDto<ShipmentDto> shipments = 
                await _shipmentsClient.GetShipmentsAsync(
                    new PaginatedRequestCommand(page, DefaultPageSize));

            return View(shipments);
        }
    }
}