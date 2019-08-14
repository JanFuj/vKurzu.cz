namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TutorialPostTag : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "TutorialPost_Id", "dbo.TutorialPosts");
            DropIndex("dbo.Tags", new[] { "TutorialPost_Id" });
            CreateTable(
                "dbo.TutorialPostTags",
                c => new
                    {
                        TutorialPost_Id = c.Int(nullable: false),
                        Tag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TutorialPost_Id, t.Tag_Id })
                .ForeignKey("dbo.TutorialPosts", t => t.TutorialPost_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .Index(t => t.TutorialPost_Id)
                .Index(t => t.Tag_Id);
            
            DropColumn("dbo.Tags", "TutorialPost_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "TutorialPost_Id", c => c.Int());
            DropForeignKey("dbo.TutorialPostTags", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.TutorialPostTags", "TutorialPost_Id", "dbo.TutorialPosts");
            DropIndex("dbo.TutorialPostTags", new[] { "Tag_Id" });
            DropIndex("dbo.TutorialPostTags", new[] { "TutorialPost_Id" });
            DropTable("dbo.TutorialPostTags");
            CreateIndex("dbo.Tags", "TutorialPost_Id");
            AddForeignKey("dbo.Tags", "TutorialPost_Id", "dbo.TutorialPosts", "Id");
        }
    }
}
