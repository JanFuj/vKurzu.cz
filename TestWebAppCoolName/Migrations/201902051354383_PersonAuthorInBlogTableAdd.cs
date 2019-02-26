namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonAuthorInBlogTableAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Author_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Blogs", "Author_Id");
            AddForeignKey("dbo.Blogs", "Author_Id", "dbo.People", "Id", cascadeDelete: true);
            DropColumn("dbo.Blogs", "Author");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Blogs", "Author", c => c.String(nullable: false));
            DropForeignKey("dbo.Blogs", "Author_Id", "dbo.People");
            DropIndex("dbo.Blogs", new[] { "Author_Id" });
            DropColumn("dbo.Blogs", "Author_Id");
        }
    }
}
