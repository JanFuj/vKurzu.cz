namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SvgColumnNameString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Svgs", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Svgs", "Name", c => c.Int(nullable: false));
        }
    }
}
