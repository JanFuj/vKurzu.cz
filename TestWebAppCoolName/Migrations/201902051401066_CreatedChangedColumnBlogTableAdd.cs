namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedChangedColumnBlogTableAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Blogs", "Changed", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "Changed");
            DropColumn("dbo.Blogs", "Created");
        }
    }
}
