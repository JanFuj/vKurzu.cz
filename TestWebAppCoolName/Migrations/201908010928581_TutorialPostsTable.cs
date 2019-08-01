namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TutorialPostsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TutorialPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        Author_Id = c.Int(nullable: false),
                        UrlTitle = c.String(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        OwnerId = c.String(),
                        Position = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Changed = c.DateTime(nullable: false),
                        TutorialCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Author_Id, cascadeDelete: true)
                .ForeignKey("dbo.TutorialCategories", t => t.TutorialCategory_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.TutorialCategory_Id);
            
            AddColumn("dbo.Tags", "TutorialPost_Id", c => c.Int());
            CreateIndex("dbo.Tags", "TutorialPost_Id");
            AddForeignKey("dbo.Tags", "TutorialPost_Id", "dbo.TutorialPosts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TutorialPosts", "TutorialCategory_Id", "dbo.TutorialCategories");
            DropForeignKey("dbo.Tags", "TutorialPost_Id", "dbo.TutorialPosts");
            DropForeignKey("dbo.TutorialPosts", "Author_Id", "dbo.People");
            DropIndex("dbo.TutorialPosts", new[] { "TutorialCategory_Id" });
            DropIndex("dbo.TutorialPosts", new[] { "Author_Id" });
            DropIndex("dbo.Tags", new[] { "TutorialPost_Id" });
            DropColumn("dbo.Tags", "TutorialPost_Id");
            DropTable("dbo.TutorialPosts");
        }
    }
}
