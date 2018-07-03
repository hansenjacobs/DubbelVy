using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class AuditViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Template")]
        public int AuditTemplateId { get; set; }
        public AuditTemplate AuditTemplate { get; set; }

        [Display(Name = "Auditee")]
        public string AuditeeId { get; set; }
        public ApplicationUser Auditee { get; set; }

        [Display(Name = "Supervisor")]
        public string SupervisorId { get; set; }
        public ApplicationUser Supervisor { get; set; }

        [Display(Name = "Auditor")]
        public string AuditorId { get; set; }
        public ApplicationUser Auditor { get; set; }

        [Display(Name = "Work Date/Time")]
        public DateTime WorkDateTime { get; set; }

        [Display(Name = "Work Identifier")]
        public string WorkIdentifier { get; set; }

        [Display(Name = "Audit Date/Time")]
        public DateTime? AuditDateTime { get; set; }

        [Display(Name = "Modified")]
        public DateTime? ModifiedDateTime { get; set; }

        [Display(Name = "Modified by")]
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }

        [Display(Name = "Score")]
        public double? Score { get; set; }

        public string ScoreDisplay
        {
            get { return Score != null ? Score.Value.ToString("0.00%") : "Not Scored"; }
        }

        [StringLength(500)]
        public string Comment { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public List<AuditTemplate> AuditTemplates { get; set; }
    }
}