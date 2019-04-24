namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SvgAddedToCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Svg_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "Svg_id");
            AddForeignKey("dbo.Courses", "Svg_id", "dbo.Svgs", "ID", cascadeDelete: true);
            DropColumn("dbo.Courses", "Svg");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Svg", c => c.String(nullable: false));
            DropForeignKey("dbo.Courses", "Svg_id", "dbo.Svgs");
            DropIndex("dbo.Courses", new[] { "Svg_id" });
            DropColumn("dbo.Courses", "Svg_id");
        }
    }
}
