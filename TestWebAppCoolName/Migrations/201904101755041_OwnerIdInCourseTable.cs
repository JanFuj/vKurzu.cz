namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OwnerIdInCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "OwnerId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "OwnerId");
        }
    }
}
