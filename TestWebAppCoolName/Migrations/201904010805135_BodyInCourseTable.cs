namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BodyInCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Body", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Body");
        }
    }
}
