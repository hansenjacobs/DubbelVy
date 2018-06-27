namespace Dubbelvy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDisputeModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Disputes", "DecisionId", "dbo.DisputeDecisions");
            DropIndex("dbo.Disputes", new[] { "DecisionId" });
            AddColumn("dbo.Disputes", "Comments", c => c.String());
            AlterColumn("dbo.Disputes", "SupervisorApproveDateTime", c => c.DateTime());
            AlterColumn("dbo.Disputes", "DecisionDateTime", c => c.DateTime());
            AlterColumn("dbo.Disputes", "DecisionId", c => c.Int());
            AlterColumn("dbo.Disputes", "DeciderId", c => c.Int());
            CreateIndex("dbo.Disputes", "DecisionId");
            AddForeignKey("dbo.Disputes", "DecisionId", "dbo.DisputeDecisions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Disputes", "DecisionId", "dbo.DisputeDecisions");
            DropIndex("dbo.Disputes", new[] { "DecisionId" });
            AlterColumn("dbo.Disputes", "DeciderId", c => c.Int(nullable: false));
            AlterColumn("dbo.Disputes", "DecisionId", c => c.Int(nullable: false));
            AlterColumn("dbo.Disputes", "DecisionDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Disputes", "SupervisorApproveDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Disputes", "Comments");
            CreateIndex("dbo.Disputes", "DecisionId");
            AddForeignKey("dbo.Disputes", "DecisionId", "dbo.DisputeDecisions", "Id", cascadeDelete: true);
        }
    }
}
