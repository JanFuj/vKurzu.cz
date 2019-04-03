namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApproveBlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Approved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "Approved");
        }
    }
}
