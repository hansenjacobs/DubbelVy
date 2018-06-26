using AutoMapper;
using Dubbelvy.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dubbelvy.Controllers
{
    public class AuditsController : Controller
    {
        private ApplicationDbContext _context;

        public AuditsController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Add()
        {
            var viewModel = new AuditViewModel
            {
                Users = _context.Users
                .OrderBy(u => u.NameLast)
                .ThenBy(u => u.NameFirst)
                .ThenBy(u => u.NameMiddle)
                .ThenBy(u => u.UserName)
                .ToList(),
                AuditTemplates = _context.AuditTemplates
                .Where(a => a.DeployDateTime.HasValue && a.DeployDateTime <= DateTime.Now && a.DepreciateDateTime == null || a.DepreciateDateTime > DateTime.Now).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AuditViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                model.AuditDateTime = submitDateTime;
                model.AuditorId = userId;
                model.ModifiedById = userId;
                model.ModifiedDateTime = submitDateTime;
                model.SupervisorId = _context.Users.Single(u => u.Id == model.AuditeeId).SupervisorId;

                var audit = new Audit();
                AutoMapper.Mapper.Map(model, audit);

                _context.Audits.Add(audit);
                _context.SaveChanges();

                var auditTemplate = _context.AuditTemplates
                    .Include(a => a.Sections)
                    .Single(a => a.Id == audit.AuditTemplateId);
                var auditResponses = new List<AuditResponse>();

                foreach(var section in auditTemplate.Sections)
                {
                    section.Elements = _context.AuditElements.Where(a => a.SectionId == section.Id).ToList();
                    foreach(var element in section.Elements)
                    {
                        var auditResponse = new AuditResponse
                        {
                            AuditId = audit.Id,
                            ElementId = element.Id
                        };
                        auditResponses.Add(auditResponse);
                    }
                }

                _context.AuditResponses.AddRange(auditResponses);
                _context.SaveChanges();

                return RedirectToAction("Edit", "AuditResponses", new { id = audit.Id });
            }

            model.Users = _context.Users.OrderBy(u => u.NameLastFirstMiddle).ToList();
            return View("Form", model);
        }

        public ActionResult Index()
        {
            var audits = _context.Audits
                .Include(a => a.Auditee)
                .Include(a => a.Auditor)
                .Include(a => a.AuditTemplate)
                .Include(a => a.ModifiedBy)
                .Include(a => a.Supervisor)
                .ToList();
            var viewModel = new List<AuditViewModel>();

            AutoMapper.Mapper.Map(audits, viewModel);

            return View(viewModel);
        }
    }
}