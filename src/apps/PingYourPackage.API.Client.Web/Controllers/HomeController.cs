using PingYourPackage.API.Client.Clients;
using PingYourPackage.API.Client.Web.Models;
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

                return RedirectToAction("Details", new { id = shipment.Key });
            }

            await GetAndSetShipmentTypesAsync();
            return View(requestModel);
        }

        [HttpGet]
        public async Task<ActionResult> Details(Guid id) {

            ShipmentDto shipment = await _shipmentsClient.GetShipmentAsync(id);
            return View(shipment);
        }

        [HttpGet]
        public async Task<ViewResult> Edit(Guid id) {

            ViewResult editViewResult = await GetEditViewResultAsync(id);
            return editViewResult;
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit_Post(
            Guid id,
            [Bind(Prefix = "ShipmentForEdit")]
            ShipmentByAffiliateUpdateRequestModel requestModel) {

            if (ModelState.IsValid) {

                ShipmentDto updatedShipment =
                    await _shipmentsClient.UpdateShipmentAsync(id, requestModel);

                return RedirectToAction("Details", new { id = updatedShipment.Key });
            }

            ViewResult editViewResult = await GetEditViewResultAsync(id);
            return editViewResult;
        }

        [HttpGet]
        public async Task<ViewResult> Delete(Guid id) {

            ShipmentDto shipment = await _shipmentsClient.GetShipmentAsync(id);
            return View(shipment);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete_Post(Guid id) {

            await _shipmentsClient.RemoveShipmentAsync(id);
            return RedirectToAction("Index");
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

        private async Task<ViewResult> GetEditViewResultAsync(Guid id) {

            ShipmentDto shipment = await _shipmentsClient.GetShipmentAsync(id);
            var shipmentForEdit = new ShipmentByAffiliateUpdateRequestModel {
                Price = shipment.Price,
                ReceiverName = shipment.ReceiverName,
                ReceiverSurname = shipment.ReceiverSurname,
                ReceiverAddress = shipment.ReceiverAddress,
                ReceiverZipCode = shipment.ReceiverZipCode,
                ReceiverCity = shipment.ReceiverCity,
                ReceiverCountry = shipment.ReceiverCountry,
                ReceiverTelephone = shipment.ReceiverTelephone,
                ReceiverEmail = shipment.ReceiverEmail
            };

            return View(new ShipmentEditViewModel { 
                Shipment = shipment,
                ShipmentForEdit = shipmentForEdit
            });
        }
    }
}