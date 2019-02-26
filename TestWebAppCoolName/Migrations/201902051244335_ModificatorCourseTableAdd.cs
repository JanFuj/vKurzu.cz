namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificatorCourseTableAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Modificator", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Modificator");
        }
    }
}
