namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentsTableCreatedOnColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "CreatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "CreatedOn");
        }
    }
}
