using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dubbelvy.Models
{
    public class AuditSectionAuditElement
    {
        [Key]
        [Column(Order = 1)]
        public int SectionId { get; set; }
        public AuditSection Section { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ElementId { get; set; }
        public AuditElement Element { get; set; }

        public int ResponseOrder { get; set; }
    }
}