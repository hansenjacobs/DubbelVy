using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dubbelvy.Models
{
    public class AuditSectionViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Audit Template")]
        public int AuditTemplateId { get; set; }
        public AuditTemplateViewModel AuditTemplate { get; set; }

        public double? Weight { get; set; }
        [Display(Name = "Weight")]
        public string WeightDisplay
        {
            get { return Weight != null ? (Weight * 100).ToString() + "%" : "Auto Fail"; }
        }

        [Display(Name = "Created")]
        public DateTime CreateDateTime { get; set; }

        [Display(Name = "Created by")]
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        [Display(Name = "Modified")]
        public DateTime ModifiedDateTime { get; set; }

        [Display(Name = "Modified by")]
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }

        [Display(Name = "Display Order")]
        public int Order { get; set; }

        public List<AuditElementViewModel> Elements { get; set; }
    }
}