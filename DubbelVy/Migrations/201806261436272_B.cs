namespace Dubbelvy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class B : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AuditResponses", "ChoiceId", "dbo.AuditElementChoices");
            DropIndex("dbo.AspNetUsers", new[] { "Supervisor_Id" });
            DropIndex("dbo.AuditResponses", new[] { "ChoiceId" });
            DropColumn("dbo.AspNetUsers", "SupervisorId");
            RenameColumn(table: "dbo.AspNetUsers", name: "Supervisor_Id", newName: "SupervisorId");
            AlterColumn("dbo.AspNetUsers", "SupervisorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.AuditResponses", "ChoiceId", c => c.Int());
            AlterColumn("dbo.Audits", "Score", c => c.Double());
            CreateIndex("dbo.AspNetUsers", "SupervisorId");
            CreateIndex("dbo.AuditResponses", "ChoiceId");
            AddForeignKey("dbo.AuditResponses", "ChoiceId", "dbo.AuditElementChoices", "Id");
            DropColumn("dbo.AuditResponses", "Comment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AuditResponses", "Comment", c => c.String(maxLength: 200));
            DropForeignKey("dbo.AuditResponses", "ChoiceId", "dbo.AuditElementChoices");
            DropIndex("dbo.AuditResponses", new[] { "ChoiceId" });
            DropIndex("dbo.AspNetUsers", new[] { "SupervisorId" });
            AlterColumn("dbo.Audits", "Score", c => c.Double(nullable: false));
            AlterColumn("dbo.AuditResponses", "ChoiceId", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "SupervisorId", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.AspNetUsers", name: "SupervisorId", newName: "Supervisor_Id");
            AddColumn("dbo.AspNetUsers", "SupervisorId", c => c.Guid(nullable: false));
            CreateIndex("dbo.AuditResponses", "ChoiceId");
            CreateIndex("dbo.AspNetUsers", "Supervisor_Id");
            AddForeignKey("dbo.AuditResponses", "ChoiceId", "dbo.AuditElementChoices", "Id", cascadeDelete: true);
        }
    }
}
