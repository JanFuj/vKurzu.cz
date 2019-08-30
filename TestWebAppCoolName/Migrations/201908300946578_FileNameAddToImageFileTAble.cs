namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileNameAddToImageFileTAble : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageFiles", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageFiles", "FileName");
        }
    }
}
