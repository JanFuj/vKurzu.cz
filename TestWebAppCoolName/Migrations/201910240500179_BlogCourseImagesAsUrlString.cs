namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogCourseImagesAsUrlString : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "Thumbnail_Id", "dbo.ImageFiles");
            DropForeignKey("dbo.TutorialCategories", "Thumbnail_Id", "dbo.ImageFiles");
            DropForeignKey("dbo.TutorialPosts", "Thumbnail_Id", "dbo.ImageFiles");
            DropForeignKey("dbo.Blogs", "Thumbnail_Id", "dbo.ImageFiles");
            DropIndex("dbo.Blogs", new[] { "Thumbnail_Id" });
            DropIndex("dbo.Courses", new[] { "Thumbnail_Id" });
            DropIndex("dbo.TutorialPosts", new[] { "Thumbnail_Id" });
            DropIndex("dbo.TutorialCategories", new[] { "Thumbnail_Id" });
            AddColumn("dbo.Blogs", "HeaderImage", c => c.String());
            AddColumn("dbo.Blogs", "SocialSharingImage", c => c.String());
            AddColumn("dbo.Courses", "HeaderImage", c => c.String());
            AddColumn("dbo.Courses", "SocialSharingImage", c => c.String());
            AddColumn("dbo.TutorialPosts", "HeaderImage", c => c.String());
            AddColumn("dbo.TutorialPosts", "SocialSharingImage", c => c.String());
            AddColumn("dbo.TutorialCategories", "HeaderImage", c => c.String());
            AddColumn("dbo.TutorialCategories", "SocialSharingImage", c => c.String());
            DropColumn("dbo.Blogs", "Thumbnail_Id");
            DropColumn("dbo.Courses", "Thumbnail_Id");
            DropColumn("dbo.TutorialPosts", "Thumbnail_Id");
            DropColumn("dbo.TutorialCategories", "Thumbnail_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TutorialCategories", "Thumbnail_Id", c => c.Int());
            AddColumn("dbo.TutorialPosts", "Thumbnail_Id", c => c.Int());
            AddColumn("dbo.Courses", "Thumbnail_Id", c => c.Int());
            AddColumn("dbo.Blogs", "Thumbnail_Id", c => c.Int());
            DropColumn("dbo.TutorialCategories", "SocialSharingImage");
            DropColumn("dbo.TutorialCategories", "HeaderImage");
            DropColumn("dbo.TutorialPosts", "SocialSharingImage");
            DropColumn("dbo.TutorialPosts", "HeaderImage");
            DropColumn("dbo.Courses", "SocialSharingImage");
            DropColumn("dbo.Courses", "HeaderImage");
            DropColumn("dbo.Blogs", "SocialSharingImage");
            DropColumn("dbo.Blogs", "HeaderImage");
            CreateIndex("dbo.TutorialCategories", "Thumbnail_Id");
            CreateIndex("dbo.TutorialPosts", "Thumbnail_Id");
            CreateIndex("dbo.Courses", "Thumbnail_Id");
            CreateIndex("dbo.Blogs", "Thumbnail_Id");
            AddForeignKey("dbo.Blogs", "Thumbnail_Id", "dbo.ImageFiles", "Id");
            AddForeignKey("dbo.TutorialPosts", "Thumbnail_Id", "dbo.ImageFiles", "Id");
            AddForeignKey("dbo.TutorialCategories", "Thumbnail_Id", "dbo.ImageFiles", "Id");
            AddForeignKey("dbo.Courses", "Thumbnail_Id", "dbo.ImageFiles", "Id");
        }
    }
}
