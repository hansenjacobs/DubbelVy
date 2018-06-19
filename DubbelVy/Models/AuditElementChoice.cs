using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dubbelvy.Models
{
    public class AuditElementChoice
    {
        public int Id { get; set; }

        public int ElementId { get; set; }
        public AuditElement Element { get; set; }

        public string Text { get; set; }

        public double Score { get; set; }
    }
}