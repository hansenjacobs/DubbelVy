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

            return finalScore;
        }

        public ActionResult ScoreSend (Guid id)
        {
            Score(id);

            return RedirectToAction("Details", new { id = id });
        }
    }
}