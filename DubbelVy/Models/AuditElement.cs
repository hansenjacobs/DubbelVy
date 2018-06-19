using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dubbelvy.Models
{
    public class AuditElement
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Topic { get; set; }

        [Required]
        public string Text { get; set; }

        public int AuditSectionId { get; set; }
        public AuditSection AuditSection { get; set; }

        public DateTime CreateDateTime { get; set; }

        public Guid CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        public int Order { get; set; }
    }
}