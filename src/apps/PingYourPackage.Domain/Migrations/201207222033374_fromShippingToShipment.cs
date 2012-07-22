namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fromShippingToShipment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shippings", "PackageSenderKey", "dbo.PackageSenders");
            DropForeignKey("dbo.Shippings", "PackageReceiverKey", "dbo.PackageReceivers");
            DropForeignKey("dbo.Shippings", "PackageTypeKey", "dbo.PackageTypes");
            DropForeignKey("dbo.ShippingStatus", "ShippingKey", "dbo.Shippings");
            DropIndex("dbo.Shippings", new[] { "PackageSenderKey" });
            DropIndex("dbo.Shippings", new[] { "PackageReceiverKey" });
            DropIndex("dbo.Shippings", new[] { "PackageTypeKey" });
            DropIndex("dbo.ShippingStatus", new[] { "ShippingKey" });
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        PackageSenderKey = c.Guid(nullable: false),
                        PackageReceiverKey = c.Guid(nullable: false),
                        PackageTypeKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.PackageSenders", t => t.PackageSenderKey, cascadeDelete: true)
                .ForeignKey("dbo.PackageReceivers", t => t.PackageReceiverKey, cascadeDelete: true)
                .ForeignKey("dbo.PackageTypes", t => t.PackageTypeKey, cascadeDelete: true)
                .Index(t => t.PackageSenderKey)
                .Index(t => t.PackageReceiverKey)
                .Index(t => t.PackageTypeKey);
            
            CreateTable(
                "dbo.ShipmentStates",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        ShipmentKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Shipments", t => t.ShipmentKey, cascadeDelete: true)
                .Index(t => t.ShipmentKey);
            
            DropTable("dbo.Shippings");
            DropTable("dbo.ShippingStatus");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ShippingStatus",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        ShippingKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.Shippings",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        PackageSenderKey = c.Guid(nullable: false),
                        PackageReceiverKey = c.Guid(nullable: false),
                        PackageTypeKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            DropIndex("dbo.ShipmentStates", new[] { "ShipmentKey" });
            DropIndex("dbo.Shipments", new[] { "PackageTypeKey" });
            DropIndex("dbo.Shipments", new[] { "PackageReceiverKey" });
            DropIndex("dbo.Shipments", new[] { "PackageSenderKey" });
            DropForeignKey("dbo.ShipmentStates", "ShipmentKey", "dbo.Shipments");
            DropForeignKey("dbo.Shipments", "PackageTypeKey", "dbo.PackageTypes");
            DropForeignKey("dbo.Shipments", "PackageReceiverKey", "dbo.PackageReceivers");
            DropForeignKey("dbo.Shipments", "PackageSenderKey", "dbo.PackageSenders");
            DropTable("dbo.ShipmentStates");
            DropTable("dbo.Shipments");
            CreateIndex("dbo.ShippingStatus", "ShippingKey");
            CreateIndex("dbo.Shippings", "PackageTypeKey");
            CreateIndex("dbo.Shippings", "PackageReceiverKey");
            CreateIndex("dbo.Shippings", "PackageSenderKey");
            AddForeignKey("dbo.ShippingStatus", "ShippingKey", "dbo.Shippings", "Key", cascadeDelete: true);
            AddForeignKey("dbo.Shippings", "PackageTypeKey", "dbo.PackageTypes", "Key", cascadeDelete: true);
            AddForeignKey("dbo.Shippings", "PackageReceiverKey", "dbo.PackageReceivers", "Key", cascadeDelete: true);
            AddForeignKey("dbo.Shippings", "PackageSenderKey", "dbo.PackageSenders", "Key", cascadeDelete: true);
        }
    }
}
