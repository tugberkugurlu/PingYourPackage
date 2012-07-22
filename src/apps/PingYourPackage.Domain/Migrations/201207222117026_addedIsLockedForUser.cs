namespace PingYourPackage.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedIsLockedForUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsLocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsLocked");
        }
    }
}
