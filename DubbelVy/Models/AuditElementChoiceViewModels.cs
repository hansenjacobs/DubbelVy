using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dubbelvy.Models
{
    public class AuditElementChoiceViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Audit Element")]
        public int ElementId { get; set; }
        public AuditElement Element { get; set; }

        [Display(Name = "Text")]
        public string Text { get; set; }

        [Display(Name = "Point Value")]
        public double Score { get; set; }

        [Display(Name = "Display Order")]
        public int Order { get; set; }
    }
}