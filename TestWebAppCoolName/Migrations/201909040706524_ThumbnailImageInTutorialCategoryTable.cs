namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThumbnailImageInTutorialCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TutorialCategories", "Thumbnail_Id", c => c.Int());
            CreateIndex("dbo.TutorialCategories", "Thumbnail_Id");
            AddForeignKey("dbo.TutorialCategories", "Thumbnail_Id", "dbo.ImageFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TutorialCategories", "Thumbnail_Id", "dbo.ImageFiles");
            DropIndex("dbo.TutorialCategories", new[] { "Thumbnail_Id" });
            DropColumn("dbo.TutorialCategories", "Thumbnail_Id");
        }
    }
}
