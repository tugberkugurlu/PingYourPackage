namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PackageTypes",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.PackagePrices",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.PackageSenders",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.PackageReceivers",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.ShipmentStatus",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
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
                        Key = c.Int(nullable: false, identity: true),
                        UserKey = c.Int(nullable: false),
                        RoleKey = c.Int(nullable: false),
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
                        Key = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Key);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserInRoles", new[] { "RoleKey" });
            DropIndex("dbo.UserInRoles", new[] { "UserKey" });
            DropForeignKey("dbo.UserInRoles", "RoleKey", "dbo.Roles");
            DropForeignKey("dbo.UserInRoles", "UserKey", "dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.UserInRoles");
            DropTable("dbo.Users");
            DropTable("dbo.ShipmentStatus");
            DropTable("dbo.Shipments");
            DropTable("dbo.PackageReceivers");
            DropTable("dbo.PackageSenders");
            DropTable("dbo.PackagePrices");
            DropTable("dbo.PackageTypes");
        }
    }
}
