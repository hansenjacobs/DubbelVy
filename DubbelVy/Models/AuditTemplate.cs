using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dubbelvy.Models
{
    public class AuditTemplate
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Version { get; set; }

        public string TitleVersion
        {
            get { return $"{Title} V{Version}"; }
        }

        public DateTime? DeployDateTime { get; set; }

        public DateTime? DepreciateDateTime { get; set; }

        public DateTime CreateDateTime { get; set; }

        [Required]
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        [Required]
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }

        public ICollection<AuditSection> Sections { get; set; }

        public int GetCompletedAuditCount(ApplicationDbContext _context)
        {
            return _context.Audits.Count(a => a.AuditTemplateId == Id);
        }

        public double? GetTotalPossiblePoints(ApplicationDbContext _context)
        {
            Sections = _context.AuditSections.Where(a => a.AuditTemplateId == Id).ToList();

            if(Sections.Count > 0)
            {
                double result = 0;
                foreach(var section in Sections)
                {
                    var sectionPoints = section.GetTotalPossiblePoints(_context);
                    if (sectionPoints.HasValue) { result += sectionPoints.Value; }
                }
                return result;
            }

            return null;
        }

        public void UpdateFromViewModel(AuditTemplateViewModel viewModel)
        {
            Title = viewModel.Title;
            Version = viewModel.Version;
            DeployDateTime = viewModel.DeployDateTime;
            DepreciateDateTime = viewModel.DepreciateDateTime;
            ModifiedById = viewModel.ModifiedById;
            ModifiedDateTime = viewModel.ModifiedDateTime != null ? viewModel.ModifiedDateTime.Value : DateTime.Now;
        }
    }
}