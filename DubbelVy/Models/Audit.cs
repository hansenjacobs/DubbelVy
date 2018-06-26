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
        public Guid Id { get; set; }

        public int AuditTemplateId { get; set; }
        public AuditTemplate AuditTemplate { get; set; }

        public string AuditeeId { get; set; }
        public ApplicationUser Auditee { get; set; }

        public string SupervisorId { get; set; }
        public ApplicationUser Supervisor { get; set; }

        public string AuditorId { get; set; }
        public ApplicationUser Auditor { get; set; }

        public DateTime WorkDateTime { get; set; }

        public string WorkIdentifier { get; set; }

        public DateTime AuditDateTime { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }

        public double? Score { get; set; }

        public string ScoreDisplay
        {
            get { return Score != null ? Score.Value.ToString("0.00%") : "Not Scored"; }
        }

        [StringLength(500)]
        public string Comment { get; set; }

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