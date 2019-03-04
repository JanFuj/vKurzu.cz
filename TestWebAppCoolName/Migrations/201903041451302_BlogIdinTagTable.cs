namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogIdinTagTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "Blog_Id", c => c.Int());
            CreateIndex("dbo.Tags", "Blog_Id");
            AddForeignKey("dbo.Tags", "Blog_Id", "dbo.Blogs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "Blog_Id", "dbo.Blogs");
            DropIndex("dbo.Tags", new[] { "Blog_Id" });
            DropColumn("dbo.Tags", "Blog_Id");
        }
    }
}
