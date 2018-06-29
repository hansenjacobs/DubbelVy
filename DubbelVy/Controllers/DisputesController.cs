using Dubbelvy.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public ActionResult Details(Guid id)
        {
            var dispute = _context.Disputes
                .Include(d => d.Audit.Auditee)
                .Include(d => d.Audit.AuditTemplate)
                .Include(d => d.Audit.Supervisor)
                .Single(d => d.AuditId == id);
            var viewModel = new DisputeViewModel();

            AutoMapper.Mapper.Map(dispute, viewModel);

            viewModel.DecisionOptions = _context.DisputeDecisions.ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detials(DisputeViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Users.Single(u => u.Id == userId);
            var submitDateTime = DateTime.Now;
            var dispute = _context.Disputes.Single(d => d.AuditId == model.AuditId);

            if(dispute.DecisionId != model.DecisionId)
            {
                dispute.DecisionId = model.DecisionId;
                if(dispute.DecisionId != null)
                {
                    var decision = _context.DisputeDecisions.Single(d => d.Id == dispute.DecisionId);
                    dispute.DecisionDateTime = submitDateTime;
                    dispute.Comments = $"{user.NameFLUser} {submitDateTime.ToString("MM/dd/yyyy hh:mm:ss tt")}\r\n" +
                        "Updated decision to " + decision.Text + "\r\n\r\n" + dispute.Comments;
                }
                else
                {
                    dispute.DecisionDateTime = null;
                    dispute.Comments = $"{user.NameFLUser} {submitDateTime.ToString("MM/dd/yyyy hh:mm:ss tt")}\r\n" +
                        "Removed decision.  Dispute now pending deicision" + "\r\n\r\n" + dispute.Comments;
                }
            }
            if (!string.IsNullOrWhiteSpace(model.NewComment))
            {
                dispute.Comments = $"{user.NameFLUser} {submitDateTime.ToString("MM/dd/yyyy hh:mm:ss tt")}\r\n" +
                    model.NewComment + "\r\n\r\n" + dispute.Comments;
            }

            _context.SaveChanges();

            return RedirectToAction("Details", new { id = dispute.AuditId });
        }

        public ActionResult Index()
        {
            var disputes = _context.Disputes
                .Include(d => d.Audit.Auditee)
                .Include(d => d.Audit.Auditor)
                .Include(d => d.Audit.AuditTemplate)
                .Include(d => d.Audit.Supervisor)
                .Include(d => d.Decision)
                .ToList();

            return View(disputes);
        }

        public void NotifySupervisor (Guid AuditId)
        {
            var audit = _context.Audits
                .Include(a => a.Auditee)
                .Include(a => a.AuditTemplate)
                .Include(a => a.Dispute)
                .Include(a => a.Supervisor)
                .Single(a => a.Id == AuditId);

            var message = new MailMessage();
            message.To.Add(new MailAddress(audit.Supervisor.Email));
            message.Subject = $"Audit Dispute - {audit.Auditee.NameFLUser} - {audit.WorkDateTime.ToString("MM/dd/yyyy")} {audit.WorkIdentifier}";
            message.Body = string.Format(DubbelVyEmail.DisputeSupervisorMessage, audit.Auditee.NameFirstMLast, audit.WorkIdentifier,
                audit.WorkDateTime.ToString("MM/dd/yyyy"), audit.AuditDateTime.ToString("MM/dd/yyyy"), audit.Dispute.Comments,
                audit.Id, audit.AuditTemplate.TitleVersion);

            DubbelVyEmail.Send(message);
        }

        public ActionResult Supervisor()
        {
            var userId = User.Identity.GetUserId();
            var disputes = _context.Disputes
                .Include(d => d.Audit.Auditee)
                .Include(d => d.Audit.AuditTemplate)
                .Include(d => d.Audit.Supervisor)
                .Where(d => d.Audit.SupervisorId == userId)
                .ToList();

            return View("Index", disputes);
        }

        public ActionResult SupervisorApprove(Guid id)
        {
            var submitDateTime = DateTime.Now;
            var userId = User.Identity.GetUserId();
            var user = _context.Users.Single(u => u.Id == userId);
            var dispute = _context.Disputes.Single(d => d.AuditId == id);

            dispute.SupervisorApproveDateTime = DateTime.Now;
            dispute.Comments = $"{user.NameFLUser} {submitDateTime.ToString("MM/dd/yyyy hh:mm:ss tt")}\r\nSupervisor Approved Dispute" +
                "\r\n\r\n" + dispute.Comments;
            _context.SaveChanges();

            return RedirectToAction("Supervisor", dispute.AuditId);
        }

        public ActionResult SupervisorDeny(Guid id)
        {
            var submitDateTime = DateTime.Now;
            var userId = User.Identity.GetUserId();
            var user = _context.Users.Single(u => u.Id == userId);
            var dispute = _context.Disputes.Single(d => d.AuditId == id);

            dispute.DeciderId = User.Identity.GetUserId();
            dispute.DecisionId = _context.DisputeDecisions.Single(d => d.Text == "Invalid").Id;
            dispute.DecisionDateTime = DateTime.Now;
            dispute.Comments = $"{user.NameFLUser} {submitDateTime.ToString("MM/dd/yyyy hh:mm:ss tt")}\r\nSupervisor Denied Dispute" +
                "\r\n\r\n" + dispute.Comments;
            _context.SaveChanges();

            return RedirectToAction("Supervisor", dispute.AuditId);
        }

        public ActionResult TeamDisputes()
        {
            var userId = User.Identity.GetUserId();
            var disputes = _context.Disputes
                .Include(d => d.Audit.Auditee)
                .Include(d => d.Audit.Auditor)
                .Include(d => d.Audit.AuditTemplate)
                .Include(d => d.Audit.Supervisor)
                .Include(d => d.Decision)
                .Where(d => d.Audit.SupervisorId == userId)
                .ToList();

            return View("Index", disputes);
        }
    }
}