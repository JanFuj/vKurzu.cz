namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LectorInCourseRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "Lector_Id", "dbo.People");
            DropIndex("dbo.Courses", new[] { "Lector_Id" });
            AlterColumn("dbo.Courses", "Lector_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "Lector_Id");
            AddForeignKey("dbo.Courses", "Lector_Id", "dbo.People", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "Lector_Id", "dbo.People");
            DropIndex("dbo.Courses", new[] { "Lector_Id" });
            AlterColumn("dbo.Courses", "Lector_Id", c => c.Int());
            CreateIndex("dbo.Courses", "Lector_Id");
            AddForeignKey("dbo.Courses", "Lector_Id", "dbo.People", "Id");
        }
    }
}
