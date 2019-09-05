namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TutorialCategoryIdInPostTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TutorialPosts", "TutorialCategory_Id", "dbo.TutorialCategories");
            DropIndex("dbo.TutorialPosts", new[] { "TutorialCategory_Id" });
            AlterColumn("dbo.TutorialPosts", "TutorialCategory_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.TutorialPosts", "TutorialCategory_Id");
            AddForeignKey("dbo.TutorialPosts", "TutorialCategory_Id", "dbo.TutorialCategories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TutorialPosts", "TutorialCategory_Id", "dbo.TutorialCategories");
            DropIndex("dbo.TutorialPosts", new[] { "TutorialCategory_Id" });
            AlterColumn("dbo.TutorialPosts", "TutorialCategory_Id", c => c.Int());
            CreateIndex("dbo.TutorialPosts", "TutorialCategory_Id");
            AddForeignKey("dbo.TutorialPosts", "TutorialCategory_Id", "dbo.TutorialCategories", "Id");
        }
    }
}
