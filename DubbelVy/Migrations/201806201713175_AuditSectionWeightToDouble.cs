namespace Dubbelvy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuditSectionWeightToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AuditSections", "Weight", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AuditSections", "Weight", c => c.Int());
        }
    }
}
