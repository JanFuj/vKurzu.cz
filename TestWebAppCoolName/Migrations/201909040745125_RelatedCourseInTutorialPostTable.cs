namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelatedCourseInTutorialPostTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TutorialPosts", "RelatedCourseId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TutorialPosts", "RelatedCourseId");
        }
    }
}
