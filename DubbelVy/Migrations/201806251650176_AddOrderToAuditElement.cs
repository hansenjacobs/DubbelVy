namespace Dubbelvy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderToAuditElement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuditElements", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuditElements", "Order");
        }
    }
}
