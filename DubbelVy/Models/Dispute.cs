using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class Dispute
    {
        public string Id { get; set; }

        public int AuditId { get; set; }
        public Audit Audit { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime SupervisorApproveDateTime { get; set; }

        public DateTime DecisionDateTime { get; set; }

        public int DecisionId { get; set; }
        public DisputeDecision Decision { get; set; }

        public int DeciderId { get; set; }
        public ApplicationUser Decider { get; set; }
    }
}