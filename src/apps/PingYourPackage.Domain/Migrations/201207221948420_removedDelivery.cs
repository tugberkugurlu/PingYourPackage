namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedDelivery : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Deliveries", "Key", "dbo.Shipments");
            DropIndex("dbo.Deliveries", new[] { "Key" });
            AddColumn("dbo.PackageReceivers", "Name", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.PackageReceivers", "Surname", c => c.String(nullable: false, maxLength: 50));
            DropTable("dbo.Deliveries");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        ShipmentKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            DropColumn("dbo.PackageReceivers", "Surname");
            DropColumn("dbo.PackageReceivers", "Name");
            CreateIndex("dbo.Deliveries", "Key");
            AddForeignKey("dbo.Deliveries", "Key", "dbo.Shipments", "Key");
        }
    }
}
