﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dubbelvy.Models
{
    public class AuditSection
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }


        public int AuditTemplateId { get; set; }
        public AuditTemplate AuditTemplate { get; set; }

        public double? Weight { get; set; }
        public string WeightDisplay
        {
            get { return Weight != null ? (Weight * 100).ToString() + "%" : "Auto Fail"; }
        }

        public DateTime CreateDateTime { get; set; }

        [Required]
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        [Required]
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }

        public int Order { get; set; }

        public ICollection<AuditElement> Elements { get; set; }

        public void UpdateFromViewModel (AuditSectionViewModel viewModel)
        {
            Description = viewModel.Description;
            Weight = viewModel.Weight;
            ModifiedById = viewModel.ModifiedById;
            ModifiedDateTime = viewModel.ModifiedDateTime != null ? viewModel.ModifiedDateTime.Value : DateTime.Now;
            Order = viewModel.Order;
        }

        public double? GetTotalPossiblePoints(ApplicationDbContext _context)
        {
            Elements = _context.AuditElements
                .Where(e => e.SectionId == Id)
                .ToList();

            if(Elements.Count > 0)
            {
                double result = 0;
                foreach (var element in Elements)
                {
                    var elementPoints = element.GetTotalPossiblePoints(_context);
                    if(elementPoints.HasValue) { result += elementPoints.Value; }
                }
                return result;
            }

            return null;
        }
    }
}