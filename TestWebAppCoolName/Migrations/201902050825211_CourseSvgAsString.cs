namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseSvgAsString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Svg", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Svg");
        }
    }
}
