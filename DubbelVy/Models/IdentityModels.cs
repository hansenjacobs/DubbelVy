using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dubbelvy.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string NameFirst { get; set; }

        [StringLength(50)]
        public string NameMiddle { get; set; }

        [Required]
        [StringLength(100)]
        public string NameLast { get; set; }

        public Guid SupervisorId { get; set; }
        public ApplicationUser Supervisor { get; set; }

        public DateTime ServiceDateTime { get; set; }
        public DateTime? TerminationDateTime { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditElement> AuditElements { get; set; }
        public DbSet<AuditElementChoice> AuditElementChoices { get; set; }
        public DbSet<AuditResponse> AuditResponses { get; set; }
        public DbSet<AuditSection> AuditSections { get; set; }
        public DbSet<AuditTemplate> AuditTemplates { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}