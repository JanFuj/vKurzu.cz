namespace TestWebAppCoolName.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulatePersonsTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO PEOPLE(Name) VALUES ('Dan')");
            Sql("INSERT INTO PEOPLE(Name) VALUES ('Honza')");
        }
        
        public override void Down()
        {
         
        }
    }
}
