namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WillLearnInCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "WillLearn", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "WillLearn");
        }
    }
}
