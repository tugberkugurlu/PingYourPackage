namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
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
                        ValidFrom = c.DateTime(nullable: false, storeType: "date"),
                        ValidTill = c.DateTime(nullable: false, storeType: "date"),
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
                        UserKey = c.Guid(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.PackageSenders", t => t.PackageSenderKey, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserKey, cascadeDelete: true)
                .ForeignKey("dbo.PackageTypes", t => t.PackageTypeKey, cascadeDelete: true)
                .Index(t => t.PackageSenderKey)
                .Index(t => t.UserKey)
                .Index(t => t.PackageTypeKey);
            
            CreateTable(
                "dbo.PackageSenders",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Surname = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 50),
                        User_Key = c.Guid(),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Users", t => t.User_Key)
                .Index(t => t.User_Key);
            
            CreateTable(
                "dbo.PackageReceivers",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        UserKey = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Surname = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 50),
                        PostCode = c.String(nullable: false, maxLength: 50),
                        City = c.String(nullable: false, maxLength: 50),
                        Country = c.String(nullable: false, maxLength: 50),
                        Telephone = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 320),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Users", t => t.UserKey, cascadeDelete: true)
                .ForeignKey("dbo.Shipments", t => t.Key)
                .Index(t => t.UserKey)
                .Index(t => t.Key);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
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
            DropIndex("dbo.PackageReceivers", new[] { "Key" });
            DropIndex("dbo.PackageReceivers", new[] { "UserKey" });
            DropIndex("dbo.PackageSenders", new[] { "User_Key" });
            DropIndex("dbo.Shipments", new[] { "PackageTypeKey" });
            DropIndex("dbo.Shipments", new[] { "UserKey" });
            DropIndex("dbo.Shipments", new[] { "PackageSenderKey" });
            DropIndex("dbo.PackagePrices", new[] { "PackageTypeKey" });
            DropForeignKey("dbo.ShipmentStates", "ShipmentKey", "dbo.Shipments");
            DropForeignKey("dbo.UserInRoles", "RoleKey", "dbo.Roles");
            DropForeignKey("dbo.UserInRoles", "UserKey", "dbo.Users");
            DropForeignKey("dbo.PackageReceivers", "Key", "dbo.Shipments");
            DropForeignKey("dbo.PackageReceivers", "UserKey", "dbo.Users");
            DropForeignKey("dbo.PackageSenders", "User_Key", "dbo.Users");
            DropForeignKey("dbo.Shipments", "PackageTypeKey", "dbo.PackageTypes");
            DropForeignKey("dbo.Shipments", "UserKey", "dbo.Users");
            DropForeignKey("dbo.Shipments", "PackageSenderKey", "dbo.PackageSenders");
            DropForeignKey("dbo.PackagePrices", "PackageTypeKey", "dbo.PackageTypes");
            DropTable("dbo.ShipmentStates");
            DropTable("dbo.Roles");
            DropTable("dbo.UserInRoles");
            DropTable("dbo.Users");
            DropTable("dbo.PackageReceivers");
            DropTable("dbo.PackageSenders");
            DropTable("dbo.Shipments");
            DropTable("dbo.PackagePrices");
            DropTable("dbo.PackageTypes");
        }
    }
}
