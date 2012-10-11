namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
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
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                        AffiliateKey = c.Guid(nullable: false),
                        PackageTypeKey = c.Guid(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceiverName = c.String(nullable: false, maxLength: 50),
                        ReceiverSurname = c.String(nullable: false, maxLength: 50),
                        ReceiverAddress = c.String(nullable: false, maxLength: 50),
                        ReceiverZipCode = c.String(nullable: false, maxLength: 50),
                        ReceiverCity = c.String(nullable: false, maxLength: 50),
                        ReceiverCountry = c.String(nullable: false, maxLength: 50),
                        ReceiverTelephone = c.String(nullable: false, maxLength: 50),
                        ReceiverEmail = c.String(nullable: false, maxLength: 320),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Affiliates", t => t.AffiliateKey, cascadeDelete: true)
                .ForeignKey("dbo.PackageTypes", t => t.PackageTypeKey, cascadeDelete: true)
                .Index(t => t.AffiliateKey)
                .Index(t => t.PackageTypeKey);
            
            CreateTable(
                "dbo.Affiliates",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        CompanyName = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 50),
                        TelephoneNumber = c.String(maxLength: 50),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Users", t => t.Key)
                .Index(t => t.Key);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(),
                        HashedPassword = c.String(nullable: false),
                        Salt = c.String(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
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
            
            CreateTable(
                "dbo.ShipmentStates",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        ShipmentKey = c.Guid(nullable: false),
                        ShipmentStatus = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Shipments", t => t.ShipmentKey, cascadeDelete: true)
                .Index(t => t.ShipmentKey);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ShipmentStates", new[] { "ShipmentKey" });
            DropIndex("dbo.UserInRoles", new[] { "RoleKey" });
            DropIndex("dbo.UserInRoles", new[] { "UserKey" });
            DropIndex("dbo.Affiliates", new[] { "Key" });
            DropIndex("dbo.Shipments", new[] { "PackageTypeKey" });
            DropIndex("dbo.Shipments", new[] { "AffiliateKey" });
            DropIndex("dbo.PackagePrices", new[] { "PackageTypeKey" });
            DropForeignKey("dbo.ShipmentStates", "ShipmentKey", "dbo.Shipments");
            DropForeignKey("dbo.UserInRoles", "RoleKey", "dbo.Roles");
            DropForeignKey("dbo.UserInRoles", "UserKey", "dbo.Users");
            DropForeignKey("dbo.Affiliates", "Key", "dbo.Users");
            DropForeignKey("dbo.Shipments", "PackageTypeKey", "dbo.PackageTypes");
            DropForeignKey("dbo.Shipments", "AffiliateKey", "dbo.Affiliates");
            DropForeignKey("dbo.PackagePrices", "PackageTypeKey", "dbo.PackageTypes");
            DropTable("dbo.ShipmentStates");
            DropTable("dbo.Roles");
            DropTable("dbo.UserInRoles");
            DropTable("dbo.Users");
            DropTable("dbo.Affiliates");
            DropTable("dbo.Shipments");
            DropTable("dbo.PackagePrices");
            DropTable("dbo.PackageTypes");
        }
    }
}
