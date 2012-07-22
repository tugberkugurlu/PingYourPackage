namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PackagePrices", "ValidFrom", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.PackagePrices", "ValidTill", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PackagePrices", "ValidTill", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PackagePrices", "ValidFrom", c => c.DateTime(nullable: false));
        }
    }
}
