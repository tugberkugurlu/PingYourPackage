namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidationAttributesChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 320));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Email", c => c.String());
        }
    }
}
