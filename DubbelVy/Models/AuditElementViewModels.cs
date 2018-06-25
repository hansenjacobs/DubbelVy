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

        [Display(Name = "Created on")]
        public DateTime CreateDateTime { get; set; }

        [Display(Name = "Created by")]
        public string CreatedById { get; set; }
        [Display(Name = "Created by")]
        public ApplicationUser CreatedBy { get; set; }

        [Display(Name = "Audit Section")]
        public int SectionId { get; set; }
        public AuditSection Section { get; set; }

        [Display(Name = "Audit Choices")]
        public List<AuditElementChoiceViewModel> Choices { get; set; }
    }
}