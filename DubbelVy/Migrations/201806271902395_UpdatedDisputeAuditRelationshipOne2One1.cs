namespace Dubbelvy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDisputeAuditRelationshipOne2One1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Disputes");
            DropIndex("dbo.Disputes", new[] { "Audit_Id" });
            DropIndex("dbo.Disputes", new[] { "Decider_Id" });
            DropColumn("dbo.Disputes", "AuditId");
            DropColumn("dbo.Disputes", "DeciderId");
            RenameColumn(table: "dbo.Disputes", name: "Audit_Id", newName: "AuditId");
            RenameColumn(table: "dbo.Disputes", name: "Decider_Id", newName: "DeciderId");
            AlterColumn("dbo.Disputes", "AuditId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Disputes", "DeciderId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Disputes", "AuditId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Disputes", "AuditId");
            CreateIndex("dbo.Disputes", "AuditId");
            CreateIndex("dbo.Disputes", "DeciderId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Disputes", new[] { "DeciderId" });
            DropIndex("dbo.Disputes", new[] { "AuditId" });
            DropPrimaryKey("dbo.Disputes");
            AlterColumn("dbo.Disputes", "AuditId", c => c.Guid());
            AlterColumn("dbo.Disputes", "DeciderId", c => c.Int());
            AlterColumn("dbo.Disputes", "AuditId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Disputes", "AuditId");
            RenameColumn(table: "dbo.Disputes", name: "DeciderId", newName: "Decider_Id");
            RenameColumn(table: "dbo.Disputes", name: "AuditId", newName: "Audit_Id");
            AddColumn("dbo.Disputes", "DeciderId", c => c.Int());
            AddColumn("dbo.Disputes", "AuditId", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.Disputes", "Decider_Id");
            CreateIndex("dbo.Disputes", "Audit_Id");
        }
    }
}
