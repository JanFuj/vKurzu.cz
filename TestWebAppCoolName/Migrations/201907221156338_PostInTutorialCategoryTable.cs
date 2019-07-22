namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostInTutorialCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "TutorialCategory_Id", c => c.Int());
            CreateIndex("dbo.Blogs", "TutorialCategory_Id");
            AddForeignKey("dbo.Blogs", "TutorialCategory_Id", "dbo.TutorialCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blogs", "TutorialCategory_Id", "dbo.TutorialCategories");
            DropIndex("dbo.Blogs", new[] { "TutorialCategory_Id" });
            DropColumn("dbo.Blogs", "TutorialCategory_Id");
        }
    }
}
