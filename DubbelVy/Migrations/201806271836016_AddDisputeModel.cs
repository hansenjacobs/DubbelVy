namespace Dubbelvy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisputeModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DisputeDecisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        IsValidDispute = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Disputes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AuditId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        SupervisorApproveDateTime = c.DateTime(nullable: false),
                        DecisionDateTime = c.DateTime(nullable: false),
                        DecisionId = c.Int(nullable: false),
                        DeciderId = c.Int(nullable: false),
                        Audit_Id = c.Guid(),
                        Decider_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Audits", t => t.Audit_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Decider_Id)
                .ForeignKey("dbo.DisputeDecisions", t => t.DecisionId, cascadeDelete: true)
                .Index(t => t.DecisionId)
                .Index(t => t.Audit_Id)
                .Index(t => t.Decider_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Disputes", "DecisionId", "dbo.DisputeDecisions");
            DropForeignKey("dbo.Disputes", "Decider_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Disputes", "Audit_Id", "dbo.Audits");
            DropIndex("dbo.Disputes", new[] { "Decider_Id" });
            DropIndex("dbo.Disputes", new[] { "Audit_Id" });
            DropIndex("dbo.Disputes", new[] { "DecisionId" });
            DropTable("dbo.Disputes");
            DropTable("dbo.DisputeDecisions");
        }
    }
}
