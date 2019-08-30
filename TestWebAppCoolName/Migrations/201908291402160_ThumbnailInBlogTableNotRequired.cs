namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThumbnailInBlogTableNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogs", "Thumbnail", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blogs", "Thumbnail", c => c.String(nullable: false));
        }
    }
}
