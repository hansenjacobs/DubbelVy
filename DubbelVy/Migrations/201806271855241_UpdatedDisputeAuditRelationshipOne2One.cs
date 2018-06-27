namespace Dubbelvy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDisputeAuditRelationshipOne2One : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Disputes");
            AlterColumn("dbo.Disputes", "AuditId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Disputes", "AuditId");
            DropColumn("dbo.Disputes", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Disputes", "Id", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.Disputes");
            AlterColumn("dbo.Disputes", "AuditId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Disputes", "Id");
        }
    }
}
