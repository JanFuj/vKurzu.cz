namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedColumnInBlogAndCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Courses", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Deleted");
            DropColumn("dbo.Blogs", "Deleted");
        }
    }
}
