using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dubbelvy.Models
{
    public class AuditElementViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Topic { get; set; }

        [Required]
        public string Text { get; set; }

        [Display(Name = "Created")]
        public DateTime CreateDateTime { get; set; }

        [Display(Name = "Created by")]
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        [Display(Name = "Modified")]
        public DateTime ModifiedDateTime { get; set; }

        [Required]
        [Display(Name = "Modified by")]
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }

        [Display(Name = "Audit Section")]
        public int SectionId { get; set; }
        public AuditSectionViewModel Section { get; set; }

        [Display(Name = "Audit Choices")]
        public List<AuditElementChoiceViewModel> Choices { get; set; }
    }
}