namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelatedCourseInBlogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "RelatedCourseId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "RelatedCourseId");
        }
    }
}
