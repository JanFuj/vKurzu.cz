namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedChangedColumnCourseTableAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Courses", "Changed", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Changed");
            DropColumn("dbo.Courses", "Created");
        }
    }
}
