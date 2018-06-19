using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class AuditSectionAuditElement
    {
        public int ElementId { get; set; }
        public AuditElement Element { get; set; }

        public int ResponseId { get; set; }
        public AuditResponse Response { get; set; }

        public int ResponseOrder { get; set; }
    }
}