namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PackagePriceTableDroped : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PackagePrices", "PackageTypeKey", "dbo.PackageTypes");
            DropIndex("dbo.PackagePrices", new[] { "PackageTypeKey" });
            AddColumn("dbo.PackageTypes", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropTable("dbo.PackagePrices");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Key);
            
            DropColumn("dbo.PackageTypes", "Price");
            CreateIndex("dbo.PackagePrices", "PackageTypeKey");
            AddForeignKey("dbo.PackagePrices", "PackageTypeKey", "dbo.PackageTypes", "Key", cascadeDelete: true);
        }
    }
}
