﻿using System;
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

        public double Score { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }
    }
}