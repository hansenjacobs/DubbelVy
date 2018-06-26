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
    public class AuditResponsesController : Controller
    {
        private ApplicationDbContext _context;

        public AuditResponsesController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Edit(Guid id)
        {
            var auditId = id;

            var viewModel = new AuditResponseViewModel
            {
                AuditId = auditId,
                Audit = _context.Audits
                .Include(a => a.Auditee)
                .Include(a => a.Auditor)
                .Include(a => a.AuditTemplate.Sections.Select(s => s.Elements.Select(e => e.Choices)))
                .Include(a => a.ModifiedBy)
                .Include(a => a.Supervisor)
                .Single(a => a.Id == auditId)
            };

            var auditResponses = _context.AuditResponses.Include(a => a.Element.Choices).Where(a => a.AuditId == auditId);
            viewModel.AuditResponses = new Dictionary<int, AuditResponse>();
            foreach(var auditResponse in auditResponses)
            {
                viewModel.AuditResponses.Add(auditResponse.ElementId, auditResponse);
            }

            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuditResponseViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                var auditResponseUpdates = new List<AuditResponse>();
                foreach(var auditResponse in model.AuditResponses)
                {
                    var auditResponseInDb = _context.AuditResponses.Single(a => a.Id == auditResponse.Value.Id);
                    auditResponseInDb.ChoiceId = auditResponse.Value.ChoiceId;
                    auditResponseUpdates.Add(auditResponseInDb);
                }

                var audit = _context.Audits.Single(a => a.Id == model.Audit.Id);
                audit.ModifiedById = userId;
                audit.ModifiedDateTime = submitDateTime;

                _context.SaveChanges();
            }

            model.Audit = _context.Audits
                .Include(a => a.Auditee)
                .Include(a => a.Auditor)
                .Include(a => a.AuditTemplate.Sections.Select(s => s.Elements.Select(e => e.Choices)))
                .Include(a => a.ModifiedBy)
                .Include(a => a.Supervisor)
                .Single(a => a.Id == model.AuditId);

            return View("Form", model);
        }
    }
}