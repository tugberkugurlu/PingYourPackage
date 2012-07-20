namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GuidKeys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PackageTypes",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.PackagePrices",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        PackageTypeKey = c.Guid(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTill = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.PackageTypes", t => t.PackageTypeKey, cascadeDelete: true)
                .Index(t => t.PackageTypeKey);
            
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
                "dbo.PackageSenders",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.PackageReceivers",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.ShipmentStatus",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        ShipmentKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Shipments", t => t.ShipmentKey, cascadeDelete: true)
                .Index(t => t.ShipmentKey);
            
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        ShipmentKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Shipments", t => t.Key)
                .Index(t => t.Key);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        HashedPassword = c.String(nullable: false),
                        Salt = c.String(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.UserInRoles",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        UserKey = c.Guid(nullable: false),
                        RoleKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Users", t => t.UserKey, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleKey, cascadeDelete: true)
                .Index(t => t.UserKey)
                .Index(t => t.RoleKey);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Key);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserInRoles", new[] { "RoleKey" });
            DropIndex("dbo.UserInRoles", new[] { "UserKey" });
            DropIndex("dbo.Deliveries", new[] { "Key" });
            DropIndex("dbo.ShipmentStatus", new[] { "ShipmentKey" });
            DropIndex("dbo.Shipments", new[] { "PackageTypeKey" });
            DropIndex("dbo.Shipments", new[] { "PackageReceiverKey" });
            DropIndex("dbo.Shipments", new[] { "PackageSenderKey" });
            DropIndex("dbo.PackagePrices", new[] { "PackageTypeKey" });
            DropForeignKey("dbo.UserInRoles", "RoleKey", "dbo.Roles");
            DropForeignKey("dbo.UserInRoles", "UserKey", "dbo.Users");
            DropForeignKey("dbo.Deliveries", "Key", "dbo.Shipments");
            DropForeignKey("dbo.ShipmentStatus", "ShipmentKey", "dbo.Shipments");
            DropForeignKey("dbo.Shipments", "PackageTypeKey", "dbo.PackageTypes");
            DropForeignKey("dbo.Shipments", "PackageReceiverKey", "dbo.PackageReceivers");
            DropForeignKey("dbo.Shipments", "PackageSenderKey", "dbo.PackageSenders");
            DropForeignKey("dbo.PackagePrices", "PackageTypeKey", "dbo.PackageTypes");
            DropTable("dbo.Roles");
            DropTable("dbo.UserInRoles");
            DropTable("dbo.Users");
            DropTable("dbo.Deliveries");
            DropTable("dbo.ShipmentStatus");
            DropTable("dbo.PackageReceivers");
            DropTable("dbo.PackageSenders");
            DropTable("dbo.Shipments");
            DropTable("dbo.PackagePrices");
            DropTable("dbo.PackageTypes");
        }
    }
}
