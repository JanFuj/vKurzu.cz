namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonColumnToCourseTableAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Lector_Id", c => c.Int());
            CreateIndex("dbo.Courses", "Lector_Id");
            AddForeignKey("dbo.Courses", "Lector_Id", "dbo.People", "Id");
            DropColumn("dbo.Courses", "Lector");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Lector", c => c.String(nullable: false));
            DropForeignKey("dbo.Courses", "Lector_Id", "dbo.People");
            DropIndex("dbo.Courses", new[] { "Lector_Id" });
            DropColumn("dbo.Courses", "Lector_Id");
        }
    }
}
