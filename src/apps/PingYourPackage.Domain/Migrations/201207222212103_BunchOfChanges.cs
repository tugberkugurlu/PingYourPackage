namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BunchOfChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shipments", "PackageReceiverKey", "dbo.PackageReceivers");
            DropIndex("dbo.Shipments", new[] { "PackageReceiverKey" });
            AddColumn("dbo.PackagePrices", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Shipments", "UserKey", c => c.Guid(nullable: false));
            AddColumn("dbo.Shipments", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PackageSenders", "Name", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.PackageSenders", "Surname", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.PackageSenders", "Address", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.PackageSenders", "User_Key", c => c.Guid());
            AddColumn("dbo.PackageReceivers", "UserKey", c => c.Guid(nullable: false));
            AddColumn("dbo.PackageReceivers", "Address", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.PackageReceivers", "PostCode", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.PackageReceivers", "City", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.PackageReceivers", "Country", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.PackageReceivers", "Telephone", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.PackageReceivers", "Email", c => c.String(nullable: false, maxLength: 320));
            AddColumn("dbo.PackageReceivers", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.PackageReceivers", "LastUpdatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.ShipmentStates", "ShipmentStatus", c => c.Int(nullable: false));
            AddColumn("dbo.ShipmentStates", "CreatedOn", c => c.DateTime(nullable: false));
            AddForeignKey("dbo.Shipments", "UserKey", "dbo.Users", "Key", cascadeDelete: true);
            AddForeignKey("dbo.PackageSenders", "User_Key", "dbo.Users", "Key");
            AddForeignKey("dbo.PackageReceivers", "UserKey", "dbo.Users", "Key", cascadeDelete: true);
            AddForeignKey("dbo.PackageReceivers", "Key", "dbo.Shipments", "Key");
            CreateIndex("dbo.Shipments", "UserKey");
            CreateIndex("dbo.PackageSenders", "User_Key");
            CreateIndex("dbo.PackageReceivers", "UserKey");
            CreateIndex("dbo.PackageReceivers", "Key");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PackageReceivers", new[] { "Key" });
            DropIndex("dbo.PackageReceivers", new[] { "UserKey" });
            DropIndex("dbo.PackageSenders", new[] { "User_Key" });
            DropIndex("dbo.Shipments", new[] { "UserKey" });
            DropForeignKey("dbo.PackageReceivers", "Key", "dbo.Shipments");
            DropForeignKey("dbo.PackageReceivers", "UserKey", "dbo.Users");
            DropForeignKey("dbo.PackageSenders", "User_Key", "dbo.Users");
            DropForeignKey("dbo.Shipments", "UserKey", "dbo.Users");
            DropColumn("dbo.ShipmentStates", "CreatedOn");
            DropColumn("dbo.ShipmentStates", "ShipmentStatus");
            DropColumn("dbo.PackageReceivers", "LastUpdatedOn");
            DropColumn("dbo.PackageReceivers", "CreatedOn");
            DropColumn("dbo.PackageReceivers", "Email");
            DropColumn("dbo.PackageReceivers", "Telephone");
            DropColumn("dbo.PackageReceivers", "Country");
            DropColumn("dbo.PackageReceivers", "City");
            DropColumn("dbo.PackageReceivers", "PostCode");
            DropColumn("dbo.PackageReceivers", "Address");
            DropColumn("dbo.PackageReceivers", "UserKey");
            DropColumn("dbo.PackageSenders", "User_Key");
            DropColumn("dbo.PackageSenders", "Address");
            DropColumn("dbo.PackageSenders", "Surname");
            DropColumn("dbo.PackageSenders", "Name");
            DropColumn("dbo.Shipments", "Price");
            DropColumn("dbo.Shipments", "UserKey");
            DropColumn("dbo.PackagePrices", "Price");
            CreateIndex("dbo.Shipments", "PackageReceiverKey");
            AddForeignKey("dbo.Shipments", "PackageReceiverKey", "dbo.PackageReceivers", "Key", cascadeDelete: true);
        }
    }
}
