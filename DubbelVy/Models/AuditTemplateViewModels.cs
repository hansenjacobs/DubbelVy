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

        [Display(Name = "Deployed")]
        public DateTime? DeployDateTime { get; set; }

        [Display(Name = "Depreciated")]
        public DateTime? DepreciateDateTime { get; set; }

        [Display(Name = "Created")]
        public DateTime? CreateDateTime { get; set; }

        [Display(Name = "Created by")]
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        [Display(Name = "Modified")]
        public DateTime? ModifiedDateTime { get; set; }

        [Display(Name = "Modified by")]
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }

        public List<AuditSectionViewModel> Sections { get; set; }

        public int? AuditsCompleted { get; set; }
    }
}