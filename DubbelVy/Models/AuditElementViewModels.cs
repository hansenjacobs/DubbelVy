using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dubbelvy.Models
{
    public class AuditElementCreateModel
    {
        [Required]
        [StringLength(30)]
        public string Topic { get; set; }

        [Required]
        [Display(Name ="Audit Text", Description = "Text that will appear on the audit form.")]
        public string Text { get; set; }
    }
}