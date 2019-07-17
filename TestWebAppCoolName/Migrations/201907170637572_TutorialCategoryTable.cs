namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TutorialCategoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TutorialCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        UrlTitle = c.String(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        OwnerId = c.String(),
                        Created = c.DateTime(nullable: false),
                        Changed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tags", "TutorialCategory_Id", c => c.Int());
            CreateIndex("dbo.Tags", "TutorialCategory_Id");
            AddForeignKey("dbo.Tags", "TutorialCategory_Id", "dbo.TutorialCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "TutorialCategory_Id", "dbo.TutorialCategories");
            DropIndex("dbo.Tags", new[] { "TutorialCategory_Id" });
            DropColumn("dbo.Tags", "TutorialCategory_Id");
            DropTable("dbo.TutorialCategories");
        }
    }
}
