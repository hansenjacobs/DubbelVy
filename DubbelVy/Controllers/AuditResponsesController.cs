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

        public ActionResult Edit(int id)
        {
            var auditResponse = _context.AuditResponses
                .Include(a => a.Audit.Auditee)
                .Include(a => a.Audit.AuditTemplate)
                .Include(a => a.Audit.Auditor)
                .Include(a => a.Audit.Supervisor)
                .Include(a => a.Element.Choices)
                .Single(a => a.Id == id);

            return View("Form", auditResponse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuditResponse model)
        {
            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;

            if(model.ChoiceId != null)
            {
                var auditResponse = _context.AuditResponses.Single(a => a.Id == model.Id);
                var audit = _context.Audits.Single(a => a.Id == model.AuditId);

                auditResponse.ChoiceId = model.ChoiceId;

                audit.ModifiedById = userId;
                audit.ModifiedDateTime = submitDateTime;
                audit.Score = null;

                _context.SaveChanges();

                return RedirectToAction("Details", "Audits", new { id = audit.Id });
            }

            model = _context.AuditResponses
                .Include(a => a.Audit.Auditee)
                .Include(a => a.Audit.AuditTemplate)
                .Include(a => a.Audit.Supervisor)
                .Include(a => a.Choice.Element.Section)
                .Single(a => a.Id == model.Id);

            return View("Form", model);
        }
    }
}