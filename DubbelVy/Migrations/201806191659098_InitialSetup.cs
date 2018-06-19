namespace Dubbelvy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditElementChoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ElementId = c.Int(nullable: false),
                        Text = c.String(),
                        Score = c.Double(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuditElements", t => t.ElementId, cascadeDelete: true)
                .Index(t => t.ElementId);
            
            CreateTable(
                "dbo.AuditElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Topic = c.String(nullable: false, maxLength: 30),
                        Text = c.String(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        CreatedById = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        NameFirst = c.String(nullable: false, maxLength: 50),
                        NameMiddle = c.String(maxLength: 50),
                        NameLast = c.String(nullable: false, maxLength: 100),
                        SupervisorId = c.Guid(nullable: false),
                        ServiceDateTime = c.DateTime(nullable: false),
                        TerminationDateTime = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Supervisor_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Supervisor_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Supervisor_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AuditSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        AuditTemplateId = c.Int(nullable: false),
                        Weight = c.Int(),
                        CreateDateTime = c.DateTime(nullable: false),
                        CreatedById = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuditTemplates", t => t.AuditTemplateId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.AuditTemplateId)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.AuditTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Version = c.String(nullable: false),
                        DeployDateTime = c.DateTime(),
                        DepreciateDateTime = c.DateTime(),
                        CreateDateTime = c.DateTime(nullable: false),
                        CreatedById = c.Guid(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.AuditResponses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuditId = c.Guid(nullable: false),
                        ElementId = c.Int(nullable: false),
                        ChoiceId = c.Int(nullable: false),
                        Comment = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Audits", t => t.AuditId, cascadeDelete: true)
                .ForeignKey("dbo.AuditElementChoices", t => t.ChoiceId, cascadeDelete: false)
                .ForeignKey("dbo.AuditElements", t => t.ElementId, cascadeDelete: false)
                .Index(t => t.AuditId)
                .Index(t => t.ElementId)
                .Index(t => t.ChoiceId);
            
            CreateTable(
                "dbo.Audits",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AuditTemplateId = c.Int(nullable: false),
                        AuditeeId = c.Guid(nullable: false),
                        SupervisorId = c.Guid(nullable: false),
                        AuditorId = c.Guid(nullable: false),
                        WorkDateTime = c.DateTime(nullable: false),
                        WorkIdentifier = c.String(),
                        AuditDateTime = c.DateTime(nullable: false),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        ModifiedById = c.Guid(nullable: false),
                        Score = c.Double(nullable: false),
                        Comment = c.String(maxLength: 500),
                        Auditee_Id = c.String(maxLength: 128),
                        Auditor_Id = c.String(maxLength: 128),
                        ModifiedBy_Id = c.String(maxLength: 128),
                        Supervisor_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Auditee_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Auditor_Id)
                .ForeignKey("dbo.AuditTemplates", t => t.AuditTemplateId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Supervisor_Id)
                .Index(t => t.AuditTemplateId)
                .Index(t => t.Auditee_Id)
                .Index(t => t.Auditor_Id)
                .Index(t => t.ModifiedBy_Id)
                .Index(t => t.Supervisor_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AuditSectionAuditElements",
                c => new
                    {
                        AuditSection_Id = c.Int(nullable: false),
                        AuditElement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AuditSection_Id, t.AuditElement_Id })
                .ForeignKey("dbo.AuditSections", t => t.AuditSection_Id, cascadeDelete: true)
                .ForeignKey("dbo.AuditElements", t => t.AuditElement_Id, cascadeDelete: true)
                .Index(t => t.AuditSection_Id)
                .Index(t => t.AuditElement_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AuditResponses", "ElementId", "dbo.AuditElements");
            DropForeignKey("dbo.AuditResponses", "ChoiceId", "dbo.AuditElementChoices");
            DropForeignKey("dbo.AuditResponses", "AuditId", "dbo.Audits");
            DropForeignKey("dbo.Audits", "Supervisor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Audits", "ModifiedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Audits", "AuditTemplateId", "dbo.AuditTemplates");
            DropForeignKey("dbo.Audits", "Auditor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Audits", "Auditee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditSectionAuditElements", "AuditElement_Id", "dbo.AuditElements");
            DropForeignKey("dbo.AuditSectionAuditElements", "AuditSection_Id", "dbo.AuditSections");
            DropForeignKey("dbo.AuditSections", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditSections", "AuditTemplateId", "dbo.AuditTemplates");
            DropForeignKey("dbo.AuditTemplates", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditElements", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Supervisor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditElementChoices", "ElementId", "dbo.AuditElements");
            DropIndex("dbo.AuditSectionAuditElements", new[] { "AuditElement_Id" });
            DropIndex("dbo.AuditSectionAuditElements", new[] { "AuditSection_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Audits", new[] { "Supervisor_Id" });
            DropIndex("dbo.Audits", new[] { "ModifiedBy_Id" });
            DropIndex("dbo.Audits", new[] { "Auditor_Id" });
            DropIndex("dbo.Audits", new[] { "Auditee_Id" });
            DropIndex("dbo.Audits", new[] { "AuditTemplateId" });
            DropIndex("dbo.AuditResponses", new[] { "ChoiceId" });
            DropIndex("dbo.AuditResponses", new[] { "ElementId" });
            DropIndex("dbo.AuditResponses", new[] { "AuditId" });
            DropIndex("dbo.AuditTemplates", new[] { "CreatedBy_Id" });
            DropIndex("dbo.AuditSections", new[] { "CreatedBy_Id" });
            DropIndex("dbo.AuditSections", new[] { "AuditTemplateId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Supervisor_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AuditElements", new[] { "CreatedBy_Id" });
            DropIndex("dbo.AuditElementChoices", new[] { "ElementId" });
            DropTable("dbo.AuditSectionAuditElements");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Audits");
            DropTable("dbo.AuditResponses");
            DropTable("dbo.AuditTemplates");
            DropTable("dbo.AuditSections");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AuditElements");
            DropTable("dbo.AuditElementChoices");
        }
    }
}
