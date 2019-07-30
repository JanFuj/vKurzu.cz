namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PositionInBlogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "Position");
        }
    }
}
