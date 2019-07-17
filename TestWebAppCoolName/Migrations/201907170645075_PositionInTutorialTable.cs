namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PositionInTutorialTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TutorialCategories", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TutorialCategories", "Position");
        }
    }
}
