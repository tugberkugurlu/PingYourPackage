using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Model.RequestModels {
    
    public class ShipmentTypeRequestModel {

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public decimal? Price { get; set; }
    }
}