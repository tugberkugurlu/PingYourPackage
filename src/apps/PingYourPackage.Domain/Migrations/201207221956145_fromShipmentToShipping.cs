namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fromShipmentToShipping : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shipments", "PackageSenderKey", "dbo.PackageSenders");
            DropForeignKey("dbo.Shipments", "PackageReceiverKey", "dbo.PackageReceivers");
            DropForeignKey("dbo.Shipments", "PackageTypeKey", "dbo.PackageTypes");
            DropForeignKey("dbo.ShipmentStatus", "ShipmentKey", "dbo.Shipments");
            DropIndex("dbo.Shipments", new[] { "PackageSenderKey" });
            DropIndex("dbo.Shipments", new[] { "PackageReceiverKey" });
            DropIndex("dbo.Shipments", new[] { "PackageTypeKey" });
            DropIndex("dbo.ShipmentStatus", new[] { "ShipmentKey" });
            CreateTable(
                "dbo.Shippings",
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
                "dbo.ShippingStatus",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        ShippingKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Shippings", t => t.ShippingKey, cascadeDelete: true)
                .Index(t => t.ShippingKey);
            
            DropTable("dbo.Shipments");
            DropTable("dbo.ShipmentStatus");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ShipmentStatus",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        ShipmentKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        PackageSenderKey = c.Guid(nullable: false),
                        PackageReceiverKey = c.Guid(nullable: false),
                        PackageTypeKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            DropIndex("dbo.ShippingStatus", new[] { "ShippingKey" });
            DropIndex("dbo.Shippings", new[] { "PackageTypeKey" });
            DropIndex("dbo.Shippings", new[] { "PackageReceiverKey" });
            DropIndex("dbo.Shippings", new[] { "PackageSenderKey" });
            DropForeignKey("dbo.ShippingStatus", "ShippingKey", "dbo.Shippings");
            DropForeignKey("dbo.Shippings", "PackageTypeKey", "dbo.PackageTypes");
            DropForeignKey("dbo.Shippings", "PackageReceiverKey", "dbo.PackageReceivers");
            DropForeignKey("dbo.Shippings", "PackageSenderKey", "dbo.PackageSenders");
            DropTable("dbo.ShippingStatus");
            DropTable("dbo.Shippings");
            CreateIndex("dbo.ShipmentStatus", "ShipmentKey");
            CreateIndex("dbo.Shipments", "PackageTypeKey");
            CreateIndex("dbo.Shipments", "PackageReceiverKey");
            CreateIndex("dbo.Shipments", "PackageSenderKey");
            AddForeignKey("dbo.ShipmentStatus", "ShipmentKey", "dbo.Shipments", "Key", cascadeDelete: true);
            AddForeignKey("dbo.Shipments", "PackageTypeKey", "dbo.PackageTypes", "Key", cascadeDelete: true);
            AddForeignKey("dbo.Shipments", "PackageReceiverKey", "dbo.PackageReceivers", "Key", cascadeDelete: true);
            AddForeignKey("dbo.Shipments", "PackageSenderKey", "dbo.PackageSenders", "Key", cascadeDelete: true);
        }
    }
}
