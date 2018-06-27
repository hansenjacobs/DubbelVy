using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class Dispute
    {
        [Key]
        [ForeignKey("Audit")]
        public Guid AuditId { get; set; }
        public Audit Audit { get; set; }

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