namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogInTag : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "Blog_Id", "dbo.Blogs");
            DropIndex("dbo.Tags", new[] { "Blog_Id" });
            CreateTable(
                "dbo.TagBlogs",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Blog_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Blog_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Blogs", t => t.Blog_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Blog_Id);
            
            DropColumn("dbo.Tags", "Blog_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "Blog_Id", c => c.Int());
            DropForeignKey("dbo.TagBlogs", "Blog_Id", "dbo.Blogs");
            DropForeignKey("dbo.TagBlogs", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagBlogs", new[] { "Blog_Id" });
            DropIndex("dbo.TagBlogs", new[] { "Tag_Id" });
            DropTable("dbo.TagBlogs");
            CreateIndex("dbo.Tags", "Blog_Id");
            AddForeignKey("dbo.Tags", "Blog_Id", "dbo.Blogs", "Id");
        }
    }
}
