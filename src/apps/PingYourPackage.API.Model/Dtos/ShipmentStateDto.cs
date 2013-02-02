using System;
using WebApiDoodle.Net.Http.Client.Model;

namespace PingYourPackage.API.Model.Dtos {
    
    public class ShipmentStateDto : IDto {

        public Guid Key { get; set; }
        public Guid ShipmentKey { get; set; }
        public string ShipmentStatus { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}