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


        [Required]
        [Display(Name = "Audit Template")]
        public int AuditTemplateId { get; set; }
        public AuditTemplate AuditTemplate { get; set; }

        public double? Weight { get; set; }
        public string WeightDisplay
        {
            get { return Weight != null ? (Weight * 100).ToString() + "%" : "Auto Fail"; }
        }

        [Display(Name = "Created")]
        public DateTime CreateDateTime { get; set; }

        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        [Display(Name = "Display Order")]
        public int Order { get; set; }

        public ICollection<AuditElementViewModel> Elements { get; set; }

        public bool IsDeleted { get; set; }

        public int Index { get; set; }
    }
}