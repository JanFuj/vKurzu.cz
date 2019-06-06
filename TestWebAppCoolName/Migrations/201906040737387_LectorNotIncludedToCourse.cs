namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LectorNotIncludedToCourse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "Lector_Id", "dbo.People");
            DropIndex("dbo.Courses", new[] { "Lector_Id" });
            DropColumn("dbo.Courses", "Lector_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Lector_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "Lector_Id");
            AddForeignKey("dbo.Courses", "Lector_Id", "dbo.People", "Id", cascadeDelete: true);
        }
    }
}
