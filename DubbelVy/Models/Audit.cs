using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Audit ID")]
        public Guid Id { get; set; }

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

        [Display(Name = "Audited")]
        public DateTime AuditDateTime { get; set; }

        [Display(Name = "Modified")]
        public DateTime ModifiedDateTime { get; set; }

        [Display(Name = "Modified by")]
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }

        public double? Score { get; set; }

        [Display(Name = "Score")]
        public string ScoreDisplay
        {
            get { return Score != null ? Score.Value.ToString("0.00%") : "Not Scored"; }
        }

        public ICollection<AuditResponse> AuditResponses { get; set; }

        [StringLength(500)]
        [Display(Name = "Comments")]
        public string Comment { get; set; }

        public Dispute Dispute { get; set; }

        public void UpdateFromViewModel(AuditViewModel viewModel)
        {
            AuditeeId = viewModel.AuditeeId;
            SupervisorId = viewModel.SupervisorId;
            AuditorId = viewModel.AuditorId;
            WorkDateTime = viewModel.WorkDateTime;
            WorkIdentifier = viewModel.WorkIdentifier;
            ModifiedDateTime = viewModel.ModifiedDateTime != null ? viewModel.ModifiedDateTime.Value : DateTime.Now;
            ModifiedById = viewModel.ModifiedById;
            Score = (double)viewModel.Score;
            Comment = viewModel.Comment;
        }

        public void CreateAuditResponses(AuditTemplate template, Guid auditId)
        {
            foreach(var section in template.Sections.OrderBy(s => s.Order))
            {
                foreach(var element in section.Elements.OrderBy(e => e.Order))
                {
                    var auditResponse = new AuditResponse
                    {
                        AuditId = auditId,
                        ElementId = element.Id
                    };
                }
            }
        }
    }
}