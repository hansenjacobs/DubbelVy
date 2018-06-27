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
    public class DisputesController : Controller
    {
        private ApplicationDbContext _context;

        public DisputesController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Create(Guid id)
        {
            var dispute = new Dispute
            {
                AuditId = id,
                Audit = _context.Audits
                .Include(a => a.Auditee)
                .Include(a => a.AuditTemplate)
                .Include(a => a.Supervisor)
                .Single(a => a.Id == id)
            };

            return View(dispute);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dispute model)
        {
            var submitDateTime = DateTime.Now;
            var userId = User.Identity.GetUserId();
            var user = _context.Users.Single(u => u.Id == userId);

            if (!string.IsNullOrWhiteSpace(model.Comments))
            {
                var dispute = new Dispute
                {
                    AuditId = model.AuditId,
                    Comments = $"{user.NameFLUser} {submitDateTime.ToString("MM/dd/yyyy hh:mm:ss tt")}\r\n" + model.Comments,
                    CreateDateTime = submitDateTime
                };

                _context.Disputes.Add(dispute);
                _context.SaveChanges();

                return RedirectToAction("Details", "Audits", new { id = dispute.AuditId });
            }
            model.Audit = _context.Audits
                .Include(a => a.Auditee)
                .Include(a => a.AuditTemplate)
                .Include(a => a.Supervisor)
                .Single(a => a.Id == model.AuditId);

            return View(model);
        }

        public ActionResult Index()
        {
            var disputes = _context.Disputes
                .Include(d => d.Audit.Auditee)
                .Include(d => d.Audit.Auditor)
                .Include(d => d.Audit.AuditTemplate)
                .Include(d => d.Audit.Supervisor)
                .Include(d => d.Decider)
                .Include(d => d.Decision)
                .ToList();

            return View(disputes);
        }
    }
}