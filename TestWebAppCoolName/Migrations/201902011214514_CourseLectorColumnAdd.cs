namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseLectorColumnAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Lector", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Lector");
        }
    }
}
