using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PingYourPackage.API;
using System.Net.Http;

namespace PingYourPackage.API {
    
    internal static class HttpRequestMessageExtensions {

        internal static IShipmentService GetShipmentService(this HttpRequestMessage request) {

            var dependencyScope = request.GetDependencyScope();
            var shipmentService = (IShipmentService)dependencyScope.GetService(typeof(IShipmentService));

            return shipmentService;
        }

        internal static IMembershipService GetMembershipService(this HttpRequestMessage request) {

            var dependencyScope = request.GetDependencyScope();
            var membershipService = (IMembershipService)dependencyScope.GetService(typeof(IMembershipService));

            return membershipService;
        }
    }
}