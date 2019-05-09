namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PositionInCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Position");
        }
    }
}
