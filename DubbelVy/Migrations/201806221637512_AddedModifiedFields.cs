namespace Dubbelvy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedModifiedFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AuditElements", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditSections", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditTemplates", "CreatedById", "dbo.AspNetUsers");
            DropIndex("dbo.AuditElements", new[] { "CreatedById" });
            DropIndex("dbo.AuditSections", new[] { "CreatedById" });
            DropIndex("dbo.AuditTemplates", new[] { "CreatedById" });
            AddColumn("dbo.AuditElements", "ModifiedDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.AuditElements", "ModifiedById", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AuditSections", "ModifiedDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.AuditSections", "ModifiedById", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AuditTemplates", "ModifiedDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.AuditTemplates", "ModifiedById", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AuditElements", "CreatedById", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AuditSections", "CreatedById", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AuditTemplates", "CreatedById", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.AuditElements", "CreatedById");
            CreateIndex("dbo.AuditElements", "ModifiedById");
            CreateIndex("dbo.AuditSections", "CreatedById");
            CreateIndex("dbo.AuditSections", "ModifiedById");
            CreateIndex("dbo.AuditTemplates", "CreatedById");
            CreateIndex("dbo.AuditTemplates", "ModifiedById");
            AddForeignKey("dbo.AuditElements", "ModifiedById", "dbo.AspNetUsers", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AuditTemplates", "ModifiedById", "dbo.AspNetUsers", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AuditSections", "ModifiedById", "dbo.AspNetUsers", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AuditElements", "CreatedById", "dbo.AspNetUsers", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AuditSections", "CreatedById", "dbo.AspNetUsers", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AuditTemplates", "CreatedById", "dbo.AspNetUsers", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuditTemplates", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditSections", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditElements", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditSections", "ModifiedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditTemplates", "ModifiedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditElements", "ModifiedById", "dbo.AspNetUsers");
            DropIndex("dbo.AuditTemplates", new[] { "ModifiedById" });
            DropIndex("dbo.AuditTemplates", new[] { "CreatedById" });
            DropIndex("dbo.AuditSections", new[] { "ModifiedById" });
            DropIndex("dbo.AuditSections", new[] { "CreatedById" });
            DropIndex("dbo.AuditElements", new[] { "ModifiedById" });
            DropIndex("dbo.AuditElements", new[] { "CreatedById" });
            AlterColumn("dbo.AuditTemplates", "CreatedById", c => c.String(maxLength: 128));
            AlterColumn("dbo.AuditSections", "CreatedById", c => c.String(maxLength: 128));
            AlterColumn("dbo.AuditElements", "CreatedById", c => c.String(maxLength: 128));
            DropColumn("dbo.AuditTemplates", "ModifiedById");
            DropColumn("dbo.AuditTemplates", "ModifiedDateTime");
            DropColumn("dbo.AuditSections", "ModifiedById");
            DropColumn("dbo.AuditSections", "ModifiedDateTime");
            DropColumn("dbo.AuditElements", "ModifiedById");
            DropColumn("dbo.AuditElements", "ModifiedDateTime");
            CreateIndex("dbo.AuditTemplates", "CreatedById");
            CreateIndex("dbo.AuditSections", "CreatedById");
            CreateIndex("dbo.AuditElements", "CreatedById");
            AddForeignKey("dbo.AuditTemplates", "CreatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AuditSections", "CreatedById", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AuditElements", "CreatedById", "dbo.AspNetUsers", "Id");
        }
    }
}
