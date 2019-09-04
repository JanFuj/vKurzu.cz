namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThumbNailInTutorialPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TutorialPosts", "Thumbnail_Id", c => c.Int());
            CreateIndex("dbo.TutorialPosts", "Thumbnail_Id");
            AddForeignKey("dbo.TutorialPosts", "Thumbnail_Id", "dbo.ImageFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TutorialPosts", "Thumbnail_Id", "dbo.ImageFiles");
            DropIndex("dbo.TutorialPosts", new[] { "Thumbnail_Id" });
            DropColumn("dbo.TutorialPosts", "Thumbnail_Id");
        }
    }
}
