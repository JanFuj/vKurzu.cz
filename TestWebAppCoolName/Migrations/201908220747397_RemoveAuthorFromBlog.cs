namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAuthorFromBlog : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Blogs", "Author_Id", "dbo.People");
            DropIndex("dbo.Blogs", new[] { "Author_Id" });
            DropColumn("dbo.Blogs", "Author_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Blogs", "Author_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Blogs", "Author_Id");
            AddForeignKey("dbo.Blogs", "Author_Id", "dbo.People", "Id", cascadeDelete: true);
        }
    }
}
