namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescriptionInImageTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageFiles", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageFiles", "Description");
        }
    }
}
