namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameAndPathRequiredInSvgTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Svgs", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Svgs", "Path", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Svgs", "Path", c => c.String());
            AlterColumn("dbo.Svgs", "Name", c => c.String());
        }
    }
}
