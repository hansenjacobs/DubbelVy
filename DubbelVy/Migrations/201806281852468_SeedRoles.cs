namespace Dubbelvy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedRoles : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO AspNetRoles VALUES (1, 'Admin')");
            Sql("INSERT INTO AspNetRoles VALUES (2, 'Audit Manager')");
            Sql("INSERT INTO AspNetRoles VALUES (3, 'Auditor')");
            Sql("INSERT INTO AspNetRoles VALUES (4, 'Supervisor')");
            Sql("INSERT INTO AspNetRoles VALUES (5, 'Auditee')");
            Sql("INSERT INTO AspNetUserRoles VALUES ('e292f2bc-5c26-4bf2-8e4a-cb0c65f5d41e', 1)");
        }
        
        public override void Down()
        {
        }
    }
}
