using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace PingYourPackage.API.WebHost {

    public class EFConfig {

        public static void Initialize() {

            RunMigrations();
        }

        private static void RunMigrations() {

            //PingYourPackage connectionstring
            var connectionString = ConfigurationManager.ConnectionStrings["PingYourPackage"];

            var efMigrationSettings = new PingYourPackage.Domain.Migrations.Configuration();
            var efMigrator = new DbMigrator(efMigrationSettings);

            efMigrator.Update();
        }
    }
}