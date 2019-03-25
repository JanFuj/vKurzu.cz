namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagsAddedToCourse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseTags",
                c => new
                    {
                        Course_Id = c.Int(nullable: false),
                        Tag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Course_Id, t.Tag_Id })
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .Index(t => t.Course_Id)
                .Index(t => t.Tag_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseTags", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.CourseTags", "Course_Id", "dbo.Courses");
            DropIndex("dbo.CourseTags", new[] { "Tag_Id" });
            DropIndex("dbo.CourseTags", new[] { "Course_Id" });
            DropTable("dbo.CourseTags");
        }
    }
}
