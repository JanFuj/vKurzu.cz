namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UrlTitleAddBlogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "UrlTitle", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "UrlTitle");
        }
    }
}
