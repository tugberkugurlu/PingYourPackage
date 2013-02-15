using PingYourPackage.API.Client.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.Client {

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ApiClientContextExtensions {

        public static IShipmentsClient GetShipmentsClient(this ApiClientContext apiClientContext) {

            return apiClientContext.GetClient<IShipmentsClient>(() => new ShipmentsClient(apiClientContext.HttpClient, apiClientContext.AffiliateKey));
        }

        public static IShipmentTypesClient GetShipmentTypesClient(this ApiClientContext apiClientContext) {

            return apiClientContext.GetClient<IShipmentTypesClient>(() => new ShipmentTypesClient(apiClientContext.HttpClient));
        }

        internal static TClient GetClient<TClient>(this ApiClientContext apiClientContext, Func<TClient> valueFactory) {

            return (TClient)apiClientContext.Clients.GetOrAdd(typeof(TClient), k => valueFactory());
        }
    }
}