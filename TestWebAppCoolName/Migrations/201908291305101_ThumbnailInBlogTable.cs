namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThumbnailInBlogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Thumbnail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "Thumbnail");
        }
    }
}
