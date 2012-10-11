using PingYourPackage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PingYourPackage.API.Test.Integration.Controllers {
    
    public class RolesControllerIntegrationTest : IDisposable {

        public RolesControllerIntegrationTest() {
        }

        [Fact]
        public void Foo() {

            var efMigrationSettings = new PingYourPackage.Domain.Migrations.Configuration();
            var efMigrator = new DbMigrator(efMigrationSettings);

            efMigrator.Update();

            using (EntitiesContext ctx = new EntitiesContext()) {

                //ctx.Users.Add(new User { 
                //    Key = Guid.NewGuid(),
                //    Name = "tugberk",
                //    Salt = "foo",
                //    HashedPassword = "foobar",
                //    Email = "tugberk@foo.com",
                //    IsLocked = false,
                //    CreatedOn = DateTime.Now
                //});

                //ctx.SaveChanges();

                Assert.True(ctx.Users.FirstOrDefault().Name == "tugberk");
            }
        }

        public void Dispose() {
        }
    }
}