namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonLastNameAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "LastName");
        }
    }
}
