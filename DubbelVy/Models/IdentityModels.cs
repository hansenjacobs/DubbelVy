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

        public string SupervisorId { get; set; }
        public ApplicationUser Supervisor { get; set; }

        public DateTime ServiceDateTime { get; set; }
        public DateTime? TerminationDateTime { get; set; }

        public string NameFirstMiddleLast
        {
            get
            {
                var result = !(string.IsNullOrWhiteSpace(NameFirst)) ? NameFirst + " " : "";
                result += !(string.IsNullOrWhiteSpace(NameMiddle)) ? NameMiddle + " " : "";
                result += !(string.IsNullOrWhiteSpace(NameLast)) ? NameLast + " " : "";

                return result.Trim();
            }
        }

        public string NameFirstMLast
        {
            get
            {
                var result = !(string.IsNullOrWhiteSpace(NameFirst)) ? NameFirst + " " : "";
                result += !(string.IsNullOrWhiteSpace(NameMiddle)) ? NameMiddle.Substring(0, 1) + " " : "";
                result += !(string.IsNullOrWhiteSpace(NameLast)) ? NameLast + " " : "";

                return result.Trim();
            }
        }

        public string NameFirstLast
        {
            get
            {
                var result = !(string.IsNullOrWhiteSpace(NameFirst)) ? NameFirst + " " : "";
                result += !(string.IsNullOrWhiteSpace(NameLast)) ? NameLast + " " : "";

                return result.Trim();
            }
        }

        public string NameFLUser
        {
            get
            {
                return NameFirstLast + $" <{UserName}>";
            }
        }

        public string NameLastFirst
        {
            get
            {
                var result = !(string.IsNullOrWhiteSpace(NameLast)) ? NameLast + ", " : "";
                result += !(string.IsNullOrWhiteSpace(NameFirst)) ? NameFirst + " " : "";

                return result.Trim();
            }
        }

        public string NameLastFirstM
        {
            get
            {
                var result = !(string.IsNullOrWhiteSpace(NameLast)) ? NameLast + ", " : "";
                result += !(string.IsNullOrWhiteSpace(NameFirst)) ? NameFirst + " " : "";
                result += !(string.IsNullOrWhiteSpace(NameMiddle)) ? NameMiddle.Substring(0, 1) + " " : "";

                return result.Trim();
            }
        }

        public string NameLastFirstMiddle
        {
            get
            {
                var result = !(string.IsNullOrWhiteSpace(NameLast)) ? NameLast + ", " : "";
                result += !(string.IsNullOrWhiteSpace(NameFirst)) ? NameFirst + " " : "";
                result += !(string.IsNullOrWhiteSpace(NameMiddle)) ? NameMiddle + " " : "";

                return result.Trim();
            }
        }

        public string NameLFUser
        {
            get
            {
                return NameLastFirst + $" <{UserName}>";
            }
        }


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
        public DbSet<Dispute> Disputes { get; set; }
        public DbSet<DisputeDecision> DisputeDecisions { get; set; }


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