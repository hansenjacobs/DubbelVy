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
                        CreatedById = c.String(maxLength: 128),
                        SectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.AuditSections", t => t.SectionId, cascadeDelete: true)
                .Index(t => t.CreatedById)
                .Index(t => t.SectionId);
            
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
                        Weight = c.Double(),
                        CreateDateTime = c.DateTime(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuditTemplates", t => t.AuditTemplateId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.AuditTemplateId)
                .Index(t => t.CreatedById);
            
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
                        CreatedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.CreatedById);
            
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
                        AuditeeId = c.String(maxLength: 128),
                        SupervisorId = c.String(maxLength: 128),
                        AuditorId = c.String(maxLength: 128),
                        WorkDateTime = c.DateTime(nullable: false),
                        WorkIdentifier = c.String(),
                        AuditDateTime = c.DateTime(nullable: false),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        ModifiedById = c.String(maxLength: 128),
                        Score = c.Double(nullable: false),
                        Comment = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuditeeId)
                .ForeignKey("dbo.AspNetUsers", t => t.AuditorId)
                .ForeignKey("dbo.AuditTemplates", t => t.AuditTemplateId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifiedById)
                .ForeignKey("dbo.AspNetUsers", t => t.SupervisorId)
                .Index(t => t.AuditTemplateId)
                .Index(t => t.AuditeeId)
                .Index(t => t.SupervisorId)
                .Index(t => t.AuditorId)
                .Index(t => t.ModifiedById);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            Sql("INSERT INTO AspNetUsers (Id, NameFirst, NameLast, SupervisorId, ServiceDateTime, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, UserName) VALUES ('e292f2bc-5c26-4bf2-8e4a-cb0c65f5d41e', 'Admin', 'Admin', '00000000-0000-0000-0000-000000000000', '2000-01-01', 'admin@admin.com', 0, 'AMBJIrkm8CcqcD1M6gf/+krH1bG1RJSJwbd1bE8GMWXievNx6N2WJce2EqXux7S5lA==', 'ecb708d2-03af-4a6f-9f67-86ca5d3f1a82', 0, 0, 1, 0, 'admin')");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AuditResponses", "ElementId", "dbo.AuditElements");
            DropForeignKey("dbo.AuditResponses", "ChoiceId", "dbo.AuditElementChoices");
            DropForeignKey("dbo.AuditResponses", "AuditId", "dbo.Audits");
            DropForeignKey("dbo.Audits", "SupervisorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Audits", "ModifiedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Audits", "AuditTemplateId", "dbo.AuditTemplates");
            DropForeignKey("dbo.Audits", "AuditorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Audits", "AuditeeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditElements", "SectionId", "dbo.AuditSections");
            DropForeignKey("dbo.AuditSections", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditSections", "AuditTemplateId", "dbo.AuditTemplates");
            DropForeignKey("dbo.AuditTemplates", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditElements", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Supervisor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuditElementChoices", "ElementId", "dbo.AuditElements");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Audits", new[] { "ModifiedById" });
            DropIndex("dbo.Audits", new[] { "AuditorId" });
            DropIndex("dbo.Audits", new[] { "SupervisorId" });
            DropIndex("dbo.Audits", new[] { "AuditeeId" });
            DropIndex("dbo.Audits", new[] { "AuditTemplateId" });
            DropIndex("dbo.AuditResponses", new[] { "ChoiceId" });
            DropIndex("dbo.AuditResponses", new[] { "ElementId" });
            DropIndex("dbo.AuditResponses", new[] { "AuditId" });
            DropIndex("dbo.AuditTemplates", new[] { "CreatedById" });
            DropIndex("dbo.AuditSections", new[] { "CreatedById" });
            DropIndex("dbo.AuditSections", new[] { "AuditTemplateId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Supervisor_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AuditElements", new[] { "SectionId" });
            DropIndex("dbo.AuditElements", new[] { "CreatedById" });
            DropIndex("dbo.AuditElementChoices", new[] { "ElementId" });
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
