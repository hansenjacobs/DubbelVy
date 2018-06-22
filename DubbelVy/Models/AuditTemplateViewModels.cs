using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dubbelvy.Models
{
    public class AuditTemplateViewModel
    {
        [Display(Name = "Audit ID")]
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Version { get; set; }

        public DateTime? DeployDateTime { get; set; }

        public DateTime? DepreciateDateTime { get; set; }

        [Display(Name = "Created")]
        public DateTime CreateDateTime { get; set; }

        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        public List<AuditSectionViewModel> Sections { get; set; }
    }
}