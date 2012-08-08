namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FromPackageTypeToShipmentType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shipments", "PackageTypeKey", "dbo.PackageTypes");
            DropIndex("dbo.Shipments", new[] { "PackageTypeKey" });
            CreateTable(
                "dbo.ShipmentTypes",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            AddColumn("dbo.Shipments", "ShipmentTypeKey", c => c.Guid(nullable: false));
            AddForeignKey("dbo.Shipments", "ShipmentTypeKey", "dbo.ShipmentTypes", "Key", cascadeDelete: true);
            CreateIndex("dbo.Shipments", "ShipmentTypeKey");
            DropColumn("dbo.Shipments", "PackageTypeKey");
            DropTable("dbo.PackageTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PackageTypes",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            AddColumn("dbo.Shipments", "PackageTypeKey", c => c.Guid(nullable: false));
            DropIndex("dbo.Shipments", new[] { "ShipmentTypeKey" });
            DropForeignKey("dbo.Shipments", "ShipmentTypeKey", "dbo.ShipmentTypes");
            DropColumn("dbo.Shipments", "ShipmentTypeKey");
            DropTable("dbo.ShipmentTypes");
            CreateIndex("dbo.Shipments", "PackageTypeKey");
            AddForeignKey("dbo.Shipments", "PackageTypeKey", "dbo.PackageTypes", "Key", cascadeDelete: true);
        }
    }
}
