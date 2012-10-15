using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PingYourPackage.API.Test.Integration.Controllers {
    
    public class ShipmentTypesControllerIntegrationTest {

        public class GetShipmentTypes {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_ShipmentTypes_If_Request_Authorized() {

                throw new NotImplementedException();
            }
        }

        public class GetShipmentType {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_ShipmentType_If_Request_Authorized_And_ShipmentType_Exists() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_ShipmentType_Does_Not_Exist() { 

                throw new NotImplementedException();
            }
        }

        public class PostShipmentType {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_201_And_ShipmentType_If_Request_Authorized_And_Success() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_409_If_Request_Authorized_But_Conflicted() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Invalid() {

                throw new NotImplementedException();
            }
        }

        public class PutShipmentType {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_ShipmentType_Does_Not_Exist() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Invalid() {

                throw new NotImplementedException();
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_ShipmentType_If_Request_Authorized_But_Request_Is_Valid() {

                throw new NotImplementedException();
            }
        }
    }
}