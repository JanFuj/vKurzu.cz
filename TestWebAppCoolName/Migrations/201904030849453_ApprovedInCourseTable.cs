namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApprovedInCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Approved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Approved");
        }
    }
}
