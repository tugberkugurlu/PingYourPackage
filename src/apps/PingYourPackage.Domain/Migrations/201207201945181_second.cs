namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        Key = c.Int(nullable: false),
                        ShipmentKey = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Shipments", t => t.Key)
                .Index(t => t.Key);
            
            AddColumn("dbo.PackageTypes", "Name", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.PackageTypes", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.PackagePrices", "PackageTypeKey", c => c.Int(nullable: false));
            AddColumn("dbo.PackagePrices", "ValidFrom", c => c.DateTime(nullable: false));
            AddColumn("dbo.PackagePrices", "ValidTill", c => c.DateTime(nullable: false));
            AddColumn("dbo.PackagePrices", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Shipments", "PackageSenderKey", c => c.Int(nullable: false));
            AddColumn("dbo.Shipments", "PackageReceiverKey", c => c.Int(nullable: false));
            AddColumn("dbo.Shipments", "PackageType_Key", c => c.Int());
            AddColumn("dbo.ShipmentStatus", "ShipmentKey", c => c.Int(nullable: false));
            AddForeignKey("dbo.PackagePrices", "PackageTypeKey", "dbo.PackageTypes", "Key", cascadeDelete: true);
            AddForeignKey("dbo.Shipments", "PackageSenderKey", "dbo.PackageSenders", "Key", cascadeDelete: true);
            AddForeignKey("dbo.Shipments", "PackageReceiverKey", "dbo.PackageReceivers", "Key", cascadeDelete: true);
            AddForeignKey("dbo.Shipments", "PackageType_Key", "dbo.PackageTypes", "Key");
            AddForeignKey("dbo.ShipmentStatus", "ShipmentKey", "dbo.Shipments", "Key", cascadeDelete: true);
            CreateIndex("dbo.PackagePrices", "PackageTypeKey");
            CreateIndex("dbo.Shipments", "PackageSenderKey");
            CreateIndex("dbo.Shipments", "PackageReceiverKey");
            CreateIndex("dbo.Shipments", "PackageType_Key");
            CreateIndex("dbo.ShipmentStatus", "ShipmentKey");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Deliveries", new[] { "Key" });
            DropIndex("dbo.ShipmentStatus", new[] { "ShipmentKey" });
            DropIndex("dbo.Shipments", new[] { "PackageType_Key" });
            DropIndex("dbo.Shipments", new[] { "PackageReceiverKey" });
            DropIndex("dbo.Shipments", new[] { "PackageSenderKey" });
            DropIndex("dbo.PackagePrices", new[] { "PackageTypeKey" });
            DropForeignKey("dbo.Deliveries", "Key", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentStatus", "ShipmentKey", "dbo.Shipments");
            DropForeignKey("dbo.Shipments", "PackageType_Key", "dbo.PackageTypes");
            DropForeignKey("dbo.Shipments", "PackageReceiverKey", "dbo.PackageReceivers");
            DropForeignKey("dbo.Shipments", "PackageSenderKey", "dbo.PackageSenders");
            DropForeignKey("dbo.PackagePrices", "PackageTypeKey", "dbo.PackageTypes");
            DropColumn("dbo.ShipmentStatus", "ShipmentKey");
            DropColumn("dbo.Shipments", "PackageType_Key");
            DropColumn("dbo.Shipments", "PackageReceiverKey");
            DropColumn("dbo.Shipments", "PackageSenderKey");
            DropColumn("dbo.PackagePrices", "CreatedOn");
            DropColumn("dbo.PackagePrices", "ValidTill");
            DropColumn("dbo.PackagePrices", "ValidFrom");
            DropColumn("dbo.PackagePrices", "PackageTypeKey");
            DropColumn("dbo.PackageTypes", "CreatedOn");
            DropColumn("dbo.PackageTypes", "Name");
            DropTable("dbo.Deliveries");
        }
    }
}
