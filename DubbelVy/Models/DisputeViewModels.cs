using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class DisputeViewModel
    {
        [Display(Name = "Audit ID")]
        public Guid AuditId { get; set; }
        public Audit Audit { get; set; }

        [Display(Name = "Comment")]
        public string NewComment { get; set; }

        public string Comments { get; set; }

        [Display(Name = "Created")]
        public DateTime CreateDateTime { get; set; }

        [Display(Name = "Supervisor Approved")]
        public DateTime? SupervisorApproveDateTime { get; set; }

        [Display(Name = "Decision Date")]
        public DateTime? DecisionDateTime { get; set; }

        [Display(Name = "Decision")]
        public int? DecisionId { get; set; }
        public DisputeDecision Decision { get; set; }

        [Display(Name = "Decided by")]
        public string DeciderId { get; set; }
        public ApplicationUser Decider { get; set; }

        public List<DisputeDecision> DecisionOptions { get; set; }
    }
}