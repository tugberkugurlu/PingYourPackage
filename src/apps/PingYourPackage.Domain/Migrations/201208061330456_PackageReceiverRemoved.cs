namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PackageReceiverRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shipments", "UserKey", "dbo.Users");
            DropForeignKey("dbo.PackageSenders", "User_Key", "dbo.Users");
            DropForeignKey("dbo.PackageReceivers", "UserKey", "dbo.Users");
            DropForeignKey("dbo.PackageReceivers", "Key", "dbo.Shipments");
            DropIndex("dbo.Shipments", new[] { "UserKey" });
            DropIndex("dbo.PackageSenders", new[] { "User_Key" });
            DropIndex("dbo.PackageReceivers", new[] { "UserKey" });
            DropIndex("dbo.PackageReceivers", new[] { "Key" });
            RenameColumn(table: "dbo.PackageSenders", name: "User_Key", newName: "UserKey");
            AddColumn("dbo.Shipments", "ReceiverName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Shipments", "ReceiverSurname", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Shipments", "ReceiverAddress", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Shipments", "ReceiverPostCode", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Shipments", "ReceiverCity", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Shipments", "ReceiverCountry", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Shipments", "ReceiverTelephone", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Shipments", "ReceiverEmail", c => c.String(nullable: false, maxLength: 320));
            AddColumn("dbo.Users", "Email", c => c.String());
            AddForeignKey("dbo.PackageSenders", "UserKey", "dbo.Users", "Key", cascadeDelete: true);
            CreateIndex("dbo.PackageSenders", "UserKey");
            DropColumn("dbo.Shipments", "PackageReceiverKey");
            DropColumn("dbo.Shipments", "UserKey");
            DropTable("dbo.PackageReceivers");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Key);
            
            AddColumn("dbo.Shipments", "UserKey", c => c.Guid(nullable: false));
            AddColumn("dbo.Shipments", "PackageReceiverKey", c => c.Guid(nullable: false));
            DropIndex("dbo.PackageSenders", new[] { "UserKey" });
            DropForeignKey("dbo.PackageSenders", "UserKey", "dbo.Users");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Shipments", "ReceiverEmail");
            DropColumn("dbo.Shipments", "ReceiverTelephone");
            DropColumn("dbo.Shipments", "ReceiverCountry");
            DropColumn("dbo.Shipments", "ReceiverCity");
            DropColumn("dbo.Shipments", "ReceiverPostCode");
            DropColumn("dbo.Shipments", "ReceiverAddress");
            DropColumn("dbo.Shipments", "ReceiverSurname");
            DropColumn("dbo.Shipments", "ReceiverName");
            RenameColumn(table: "dbo.PackageSenders", name: "UserKey", newName: "User_Key");
            CreateIndex("dbo.PackageReceivers", "Key");
            CreateIndex("dbo.PackageReceivers", "UserKey");
            CreateIndex("dbo.PackageSenders", "User_Key");
            CreateIndex("dbo.Shipments", "UserKey");
            AddForeignKey("dbo.PackageReceivers", "Key", "dbo.Shipments", "Key");
            AddForeignKey("dbo.PackageReceivers", "UserKey", "dbo.Users", "Key", cascadeDelete: true);
            AddForeignKey("dbo.PackageSenders", "User_Key", "dbo.Users", "Key");
            AddForeignKey("dbo.Shipments", "UserKey", "dbo.Users", "Key", cascadeDelete: true);
        }
    }
}
