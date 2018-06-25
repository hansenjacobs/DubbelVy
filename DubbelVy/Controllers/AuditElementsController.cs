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
    public class AuditElementsController : Controller
    {
        private ApplicationDbContext _context;

        public AuditElementsController()
        {
            _context = new ApplicationDbContext();
        }
        
        public ActionResult Add(int id)
        {
            var auditSectionId = id;

            var viewModel = new AuditElementViewModel
            {
                SectionId = id,
                Section = new AuditSectionViewModel
                {
                    AuditTemplate = new AuditTemplateViewModel()
                }
            };

            var auditSection = _context.AuditSections.Single(a => a.Id == auditSectionId);
            var auditTemaplate = _context.AuditTemplates.Single(a => a.Id == auditSection.AuditTemplateId);

            AutoMapper.Mapper.Map(auditSection, viewModel.Section);
            AutoMapper.Mapper.Map(auditTemaplate, viewModel.Section.AuditTemplate);

            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AuditElementViewModel model)
        {
            model.CreateDateTime = DateTime.Now;
            model.ModifiedDateTime = DateTime.Now;
            model.CreatedById = User.Identity.GetUserId();
            model.ModifiedById = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                var auditElement = new AuditElement();

                AutoMapper.Mapper.Map(model, auditElement);

                _context.AuditElements.Add(auditElement);
                _context.SaveChanges();

                var auditTemplateId = _context.AuditSections.Single(a => a.Id == auditElement.SectionId).AuditTemplateId;

                return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplateId });
            }

            var auditSection = _context.AuditSections.Single(a => a.Id == model.SectionId);
            var auditTemaplate = _context.AuditTemplates.Single(a => a.Id == auditSection.AuditTemplateId);

            model.Section = new AuditSectionViewModel { AuditTemplate = new AuditTemplateViewModel() };

            AutoMapper.Mapper.Map(auditSection, model.Section);
            AutoMapper.Mapper.Map(auditTemaplate, model.Section.AuditTemplate);

            return View("Form", model);
        }

        public ActionResult Delete (int id)
        {
            var auditElement = _context.AuditElements.Single(a => a.Id == id);

            var auditTemplateId = _context.AuditSections.Single(a => a.Id == auditElement.SectionId).AuditTemplateId;
            var auditsCompleted = _context.AuditTemplates.Count(a => a.Id == auditTemplateId);

            if(auditsCompleted == 0)
            {
                var auditElementChoices = _context.AuditElementChoices.Where(a => a.ElementId == id).ToList();

                if (auditElementChoices.Count > 0)
                    _context.AuditElementChoices.RemoveRange(auditElementChoices);

                _context.AuditElements.Remove(auditElement);
                _context.SaveChanges();

                ViewBag.Message = "Audit element has been successfully deleted.";
            }
            else
            {
                ViewBag.Message = "Unable to delete audit element.  Audits have been completed using the audit template that the audit element belongs to.";
            }

            return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplateId });
        }

        public ActionResult Details(int id)
        {
            var auditElement = _context.AuditElements
                .Include(a => a.Section.AuditTemplate)
                .Include(a => a.CreatedBy)
                .Include(a => a.ModifiedBy)
                .Single(a => a.Id == id);

            var viewModel = new AuditElementViewModel
            {
                Section = new AuditSectionViewModel { AuditTemplate = new AuditTemplateViewModel() }
            };

            AutoMapper.Mapper.Map(auditElement, viewModel);

            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var auditElement = _context.AuditElements
                .Include(a => a.Section.AuditTemplate)
                .Single(a => a.Id == id);

            var viewModel = new AuditElementViewModel
            {
                Section = new AuditSectionViewModel { AuditTemplate = new AuditTemplateViewModel() }
            };

            AutoMapper.Mapper.Map(auditElement, viewModel);

            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuditElementViewModel model)
        {
            var auditElement = _context.AuditElements.Single(a => a.Id == model.Id);

            model.CreateDateTime = auditElement.CreateDateTime;
            model.CreatedById = auditElement.CreatedById;
            model.ModifiedDateTime = DateTime.Now;
            model.ModifiedById = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                AutoMapper.Mapper.Map(model, auditElement);

                _context.SaveChanges();

                var auditTemplateId = _context.AuditSections.Single(a => a.Id == auditElement.SectionId).AuditTemplateId;

                return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplateId });
            }

            model.Section = new AuditSectionViewModel { AuditTemplate = new AuditTemplateViewModel() };
            AutoMapper.Mapper.Map(_context.AuditSections.Include(a => a.AuditTemplate).Single(a => a.Id == model.SectionId), model.Section);
            return View("Form", model);
        }
    }
}