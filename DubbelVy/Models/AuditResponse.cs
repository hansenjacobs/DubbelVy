using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dubbelvy.Models
{
    public class AuditResponse
    {
        public int Id { get; set; }

        public Guid AuditId { get; set; }
        public Audit Audit { get; set; }

        public int ElementId { get; set; }
        public AuditElement Element { get; set; }

        public int? ChoiceId { get; set; }
        public AuditElementChoice Choice { get; set; }

    }
}