using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model.RequestModels {

    public class ShipmentRequestModel : ShipmentBaseRequestModel {

        [Required]
        public Guid? AffiliateKey { get; set; }
    }
}