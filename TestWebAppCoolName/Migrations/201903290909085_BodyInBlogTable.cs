namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BodyInBlogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Body", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "Body");
        }
    }
}
