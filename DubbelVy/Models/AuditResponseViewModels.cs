using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class AuditResponseViewModel
    {
        public Guid AuditId { get; set; }
        public Audit Audit { get; set; }

        public Dictionary<int, AuditResponse> AuditResponses { get; set; }
    }
}