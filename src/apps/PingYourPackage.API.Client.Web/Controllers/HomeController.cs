using PingYourPackage.API.Client.Clients;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestCommands;
using PingYourPackage.API.Model.RequestModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApiDoodle.Net.Http.Client.Model;

namespace PingYourPackage.API.Client.Web.Controllers {

    public class HomeController : Controller {

        private const int DefaultPageSize = 2;
        private readonly IShipmentsClient _shipmentsClient;
        private readonly IShipmentTypesClient _shipmentTypesClient;

        public HomeController(IShipmentsClient shipmentsClient, IShipmentTypesClient shipmentTypesClient) {

            _shipmentsClient = shipmentsClient;
            _shipmentTypesClient = shipmentTypesClient;
        }

        public async Task<ViewResult> Index(int page = 1) {

            PaginatedDto<ShipmentDto> shipments = 
                await _shipmentsClient.GetShipmentsAsync(
                    new PaginatedRequestCommand(page, DefaultPageSize));

            return View(shipments);
        }

        [HttpGet]
        public async Task<ViewResult> Create() {

            await GetAndSetShipmentTypesAsync();
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create_Post(ShipmentByAffiliateRequestModel requestModel) {

            if (ModelState.IsValid) {

                ShipmentDto shipment =
                    await _shipmentsClient.AddShipmentAsync(requestModel);

                return RedirectToAction("Index", new { id = shipment.Key });
            }

            await GetAndSetShipmentTypesAsync();
            return View(requestModel);
        }

        [HttpGet]
        public async Task<ActionResult> Details(Guid id) {

            ShipmentDto shipment = await _shipmentsClient.GetShipmentAsync(id);
            return View(shipment);
        }

        // private helpers
        private async Task GetAndSetShipmentTypesAsync() {

            PaginatedDto<ShipmentTypeDto> shipmentTypes = 
                await _shipmentTypesClient.GetShipmentTypesAsync(new PaginatedRequestCommand(1, 50));

            ViewBag.ShipmentTypes = shipmentTypes.Items.Select(x => new SelectListItem() { 
                Text = x.Name,
                Value = x.Key.ToString()
            });
        }
    }
}