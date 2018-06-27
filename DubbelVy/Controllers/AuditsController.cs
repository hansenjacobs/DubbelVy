using AutoMapper;
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
                    section.Elements = _context.AuditElements.Include(a => a.Choices).Where(a => a.SectionId == section.Id).ToList();
                    foreach(var element in section.Elements)
                    {
                        var auditResponse = new AuditResponse
                        {
                            AuditId = audit.Id,
                            ElementId = element.Id,
                            ChoiceId = element.Choices.OrderBy(e => e.Order).ToList()[0].Id
                        };
                        auditResponses.Add(auditResponse);
                    }
                }

                _context.AuditResponses.AddRange(auditResponses);
                _context.SaveChanges();

                return RedirectToAction("Details", "Audits", new { id = audit.Id });
            }

            model.Users = _context.Users.OrderBy(u => u.NameLastFirstMiddle).ToList();
            return View("Form", model);
        }

        public ActionResult Delete(Guid id)
        {
            var audit = _context.Audits.Single(a => a.Id == id);
            _context.Audits.Remove(audit);
            TempData["message"] = "Audit successfully deleted.";
            return RedirectToAction("Index");
        }

        public ActionResult Details(Guid id)
        {
            var audit = _context.Audits
                .Include(a => a.Auditee)
                .Include(a => a.Auditor)
                .Include(a => a.AuditResponses.Select(r => r.Choice))
                .Include(a => a.AuditTemplate.Sections.Select(s => s.Elements.Select(e => e.Choices)))
                .Include(a => a.ModifiedBy)
                .Include(a => a.Supervisor)
                .Single(a => a.Id == id);

            return View(audit);
        }

        public ActionResult EditComment(Guid id)
        {
            var audit = _context.Audits
                .Include(a => a.Auditee)
                .Include(a => a.Auditor)
                .Include(a => a.AuditResponses.Select(r => r.Choice))
                .Include(a => a.AuditTemplate.Sections.Select(s => s.Elements.Select(e => e.Choices)))
                .Include(a => a.ModifiedBy)
                .Include(a => a.Supervisor)
                .Single(a => a.Id == id);

            return View(audit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment(Audit model)
        {
            var audit = _context.Audits.Single(a => a.Id == model.Id);
            audit.Comment = model.Comment;
            audit.ModifiedDateTime = DateTime.Now;
            audit.ModifiedById = User.Identity.GetUserId();
            audit.Score = null;
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = audit.Id });
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

        public double? Score(Guid id)
        {
            var audit = _context.Audits
                .Include(a => a.AuditResponses.Select(r => r.Choice))
                .Include(a => a.AuditTemplate.Sections)
                .Include(a => a.AuditResponses.Select(r => r.Choice))
                .Single(a => a.Id == id);

            var _auditTotalPossiblePoints = audit.AuditTemplate.GetTotalPossiblePoints(_context);
            if (!_auditTotalPossiblePoints.HasValue) { return null; }
            var auditTotalPossiblePoints = _auditTotalPossiblePoints.Value;

            var finalScore = 0.0;

            foreach(var section in audit.AuditTemplate.Sections)
            {
                var sectionScore = 0.0;
                var sectionTotalPossiblePoints = section.GetTotalPossiblePoints(_context);
                foreach(var response in audit.AuditResponses.Where(r => r.Element.SectionId == section.Id))
                {
                    sectionScore += response.Choice.Score;
                }

                if(section.Weight == null)
                {
                    if(sectionScore < sectionTotalPossiblePoints)
                    {
                        finalScore = 0;
                        break;
                    }
                }
                else
                {
                    finalScore += (sectionScore / sectionTotalPossiblePoints.Value * section.Weight.Value);
                }
            }

            audit.Score = finalScore;
            audit.ModifiedById = User.Identity.GetUserId();
            audit.ModifiedDateTime = DateTime.Now;
            _context.SaveChanges();

            return finalScore;
        }

        public ActionResult ScoreSend (Guid id)
        {
            if(Score(id) != null)
            {
                Send(id);
                return RedirectToAction("Details", new { id = id });
            }

            TempData.Add("message", "Unable to score audit. The audit template it is based on has a total possible point value of zero.");
            return RedirectToAction("Details", new { id = id });
        }

        public void Send(Guid id)
        {
            var scoreThresholdBelowEmailSupervisor = 0.80;

            var audit = _context.Audits
                .Include(a => a.Auditee.Supervisor)
                .Include(a => a.AuditTemplate)
                .Single(a => a.Id == id);

            var message = new MailMessage();
            message.To.Add(new MailAddress(audit.Auditee.Email));
            if(audit.Score < scoreThresholdBelowEmailSupervisor) { message.To.Add(new MailAddress(audit.Auditee.Supervisor.Email)); }
            message.Subject = $"Audit Completed: {audit.WorkIdentifier} on {audit.WorkDateTime.ToString("MM/dd/yy")} - {audit.Auditee.NameFirstLast}";
            message.Body = string.Format(DubbelVyEmail.AuditCompletedMessage, audit.AuditTemplate.TitleVersion, audit.Auditee.NameFLUser, audit.Auditee.Supervisor.NameFLUser, audit.WorkIdentifier, audit.WorkDateTime.ToString("MM/dd/yyyy hh:m:ss tt"), audit.ScoreDisplay, audit.Comment, audit.Id);
            message.IsBodyHtml = true;

            DubbelVyEmail.Send(message);
        }
    }
}