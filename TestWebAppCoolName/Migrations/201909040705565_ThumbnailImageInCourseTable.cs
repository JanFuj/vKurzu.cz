namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThumbnailImageInCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Thumbnail_Id", c => c.Int());
            CreateIndex("dbo.Courses", "Thumbnail_Id");
            AddForeignKey("dbo.Courses", "Thumbnail_Id", "dbo.ImageFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "Thumbnail_Id", "dbo.ImageFiles");
            DropIndex("dbo.Courses", new[] { "Thumbnail_Id" });
            DropColumn("dbo.Courses", "Thumbnail_Id");
        }
    }
}
