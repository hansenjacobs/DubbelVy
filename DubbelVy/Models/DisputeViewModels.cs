using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class DisputeViewModel
    {
        public Guid AuditId { get; set; }
        public Audit Audit { get; set; }

        [Display(Name = "Comment")]
        public string NewComment { get; set; }

        public string Comments { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? SupervisorApproveDateTime { get; set; }

        public DateTime? DecisionDateTime { get; set; }

        public int? DecisionId { get; set; }
        public DisputeDecision Decision { get; set; }

        public string DeciderId { get; set; }
        public ApplicationUser Decider { get; set; }
    }
}