namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorRemovedFromTutorialPost : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TutorialPosts", "Author_Id", "dbo.People");
            DropIndex("dbo.TutorialPosts", new[] { "Author_Id" });
            DropColumn("dbo.TutorialPosts", "Author_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TutorialPosts", "Author_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.TutorialPosts", "Author_Id");
            AddForeignKey("dbo.TutorialPosts", "Author_Id", "dbo.People", "Id", cascadeDelete: true);
        }
    }
}
