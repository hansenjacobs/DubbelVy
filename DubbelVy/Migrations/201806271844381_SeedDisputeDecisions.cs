namespace Dubbelvy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedDisputeDecisions : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO DisputeDecisions (Text, IsValidDispute) VALUES ('Valid', 1)");
            Sql("INSERT INTO DisputeDecisions (Text, IsValidDispute) VALUES ('Partially Valid', 1)");
            Sql("INSERT INTO DisputeDecisions (Text, IsValidDispute) VALUES ('Not Valid', 0)");
        }
        
        public override void Down()
        {
        }
    }
}
