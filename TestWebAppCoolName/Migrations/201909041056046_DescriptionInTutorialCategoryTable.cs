namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescriptionInTutorialCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TutorialCategories", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TutorialCategories", "Description");
        }
    }
}
