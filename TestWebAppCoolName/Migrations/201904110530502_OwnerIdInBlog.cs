namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OwnerIdInBlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "OwnerId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "OwnerId");
        }
    }
}
