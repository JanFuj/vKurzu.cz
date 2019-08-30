namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageFileInBlogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Thumbnail_Id", c => c.Int());
            CreateIndex("dbo.Blogs", "Thumbnail_Id");
            AddForeignKey("dbo.Blogs", "Thumbnail_Id", "dbo.ImageFiles", "Id");
            DropColumn("dbo.Blogs", "Thumbnail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Blogs", "Thumbnail", c => c.String());
            DropForeignKey("dbo.Blogs", "Thumbnail_Id", "dbo.ImageFiles");
            DropIndex("dbo.Blogs", new[] { "Thumbnail_Id" });
            DropColumn("dbo.Blogs", "Thumbnail_Id");
        }
    }
}
