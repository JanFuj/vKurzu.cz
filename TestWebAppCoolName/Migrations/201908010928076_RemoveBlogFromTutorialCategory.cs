namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveBlogFromTutorialCategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Blogs", "TutorialCategory_Id", "dbo.TutorialCategories");
            DropIndex("dbo.Blogs", new[] { "TutorialCategory_Id" });
            DropColumn("dbo.Blogs", "TutorialCategory_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Blogs", "TutorialCategory_Id", c => c.Int());
            CreateIndex("dbo.Blogs", "TutorialCategory_Id");
            AddForeignKey("dbo.Blogs", "TutorialCategory_Id", "dbo.TutorialCategories", "Id");
        }
    }
}
