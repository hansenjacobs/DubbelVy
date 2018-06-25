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
    public class AuditSectionsController : Controller
    {
        private ApplicationDbContext _context;

        public AuditSectionsController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Add(int id)
        {
            var auditTemplateId = id;
            var auditTemplate = _context.AuditTemplates.Single(a => a.Id == auditTemplateId);

            if(auditTemplate.GetCompletedAuditCount(_context) == 0)
            {
                var viewModel = new AuditSectionViewModel
                {
                    AuditTemplate = new AuditTemplateViewModel(),
                    AuditTemplateId = auditTemplateId
                };

                AutoMapper.Mapper.Map(auditTemplate, viewModel.AuditTemplate);

                return View("Form", viewModel);
            }

            ViewBag.Message = "Unable to add section to aduit template.  Audits have already been completed using this audit template.";
            return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplateId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AuditSectionViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;
            model.CreateDateTime = submitDateTime;
            model.ModifiedDateTime = submitDateTime;
            model.CreatedById = userId;
            model.ModifiedById = userId;

            var auditTemplate = _context.AuditTemplates.Single(a => a.Id == model.AuditTemplateId);

            if (ModelState.IsValid)
            {
                var newAuditSection = new AuditSection();

                AutoMapper.Mapper.Map(model, newAuditSection);
                auditTemplate.ModifiedById = userId;
                auditTemplate.ModifiedDateTime = submitDateTime;

                _context.AuditSections.Add(newAuditSection);
                _context.SaveChanges();

                return RedirectToAction("Details", "AuditTemplates", new { id = newAuditSection.AuditTemplateId });
            }

            model.AuditTemplate = new AuditTemplateViewModel();
            AutoMapper.Mapper.Map(auditTemplate, model.AuditTemplate);
            return View("Form", model);
        }

        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;
            var auditSection = _context.AuditSections.Single(a => a.Id == id);
            var auditTemplate = _context.AuditTemplates.Single(a => a.Id == auditSection.AuditTemplateId);

            if(auditTemplate.GetCompletedAuditCount(_context) == 0)
            {
                _context.AuditSections.Remove(auditSection);
                auditTemplate.ModifiedById = userId;
                auditTemplate.ModifiedDateTime = submitDateTime;

                _context.SaveChanges();

                ViewBag.Message = "Audit section deleted successfully.";

            }
            else
            {
                ViewBag.Message = "Unable to delete audit section.  Audits have been completed using the audit template that the section belongs to.";
            }

            return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplate.Id });
        }

        public ActionResult Details(int id)
        {
            var auditSection = _context.AuditSections
                .Include(a => a.AuditTemplate)
                .Include(a => a.CreatedBy)
                .Include(a => a.ModifiedBy)
                .Single(a => a.Id == id);

            var viewModel = new AuditSectionViewModel();

            AutoMapper.Mapper.Map(auditSection, viewModel);

            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var auditSection = _context.AuditSections
                .Include(a => a.AuditTemplate)
                .Single(a => a.Id == id);

            if(auditSection.AuditTemplate.GetCompletedAuditCount(_context) == 0)
            {
                var viewModel = new AuditSectionViewModel();

                AutoMapper.Mapper.Map(auditSection, viewModel);

                return View("Form", viewModel);
            }

            ViewBag.Message = "Unable to edit audit section. Audits have been completed using the audit template that the section belongs to.";
            return RedirectToAction("Details", "AuditTemplates", new { id = auditSection.AuditTemplateId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuditSectionViewModel model)
        {
            var auditSection = _context.AuditSections.Single(a => a.Id == model.Id);

            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;
            model.CreateDateTime = auditSection.CreateDateTime;
            model.ModifiedDateTime = submitDateTime;
            model.CreatedById = auditSection.CreatedById;
            model.ModifiedById = userId;

            var auditTemplate = _context.AuditTemplates.Single(a => a.Id == model.AuditTemplateId);

            if (ModelState.IsValid)
            {
                auditSection.UpdateFromViewModel(model);

                auditTemplate.ModifiedById = userId;
                auditTemplate.ModifiedDateTime = submitDateTime;
                _context.SaveChanges();

                return RedirectToAction("Details", "AuditTemplates", new { id = auditSection.AuditTemplateId });
            }

            model.AuditTemplate = new AuditTemplateViewModel();
            AutoMapper.Mapper.Map(auditTemplate, model.AuditTemplate);
            return View("Form", model);
        }
    }
}