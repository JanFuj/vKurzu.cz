namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileColumnAddedToCourseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "ImagePath", c => c.String());
            DropColumn("dbo.Courses", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Image", c => c.String());
            DropColumn("dbo.Courses", "ImagePath");
        }
    }
}
