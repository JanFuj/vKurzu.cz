namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UrlTitleAddCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "UrlTitle", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "UrlTitle");
        }
    }
}
