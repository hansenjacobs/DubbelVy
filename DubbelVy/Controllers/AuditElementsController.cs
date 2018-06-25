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

            var auditSection = _context.AuditSections.Single(a => a.Id == auditSectionId);
            var auditTemaplate = _context.AuditTemplates.Single(a => a.Id == auditSection.AuditTemplateId);

            if(auditTemaplate.GetCompletedAuditCount(_context) == 0)
            {
                var viewModel = new AuditElementViewModel
                {
                    SectionId = id,
                    Section = new AuditSectionViewModel
                    {
                        AuditTemplate = new AuditTemplateViewModel()
                    }
                };

                AutoMapper.Mapper.Map(auditSection, viewModel.Section);
                AutoMapper.Mapper.Map(auditTemaplate, viewModel.Section.AuditTemplate);

                return View("Form", viewModel);
            }

            ViewBag.Message = "Unable to add element. Audits have already been completed using this audit template.";
            return RedirectToAction("Details", "AuditTemplates", new { id = auditTemaplate.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AuditElementViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;
            model.CreateDateTime = submitDateTime;
            model.ModifiedDateTime = submitDateTime;
            model.CreatedById = userId;
            model.ModifiedById = userId;

            var auditSection = _context.AuditSections.Single(a => a.Id == model.SectionId);
            var auditTemaplate = _context.AuditTemplates.Single(a => a.Id == auditSection.AuditTemplateId);

            if (ModelState.IsValid)
            {
                var auditElement = new AuditElement();

                AutoMapper.Mapper.Map(model, auditElement);

                auditSection.ModifiedById = userId;
                auditSection.ModifiedDateTime = submitDateTime;
                auditTemaplate.ModifiedById = userId;
                auditTemaplate.ModifiedDateTime = submitDateTime;

                _context.AuditElements.Add(auditElement);
                _context.SaveChanges();

                var auditTemplateId = _context.AuditSections.Single(a => a.Id == auditElement.SectionId).AuditTemplateId;

                return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplateId });
            }

            

            model.Section = new AuditSectionViewModel { AuditTemplate = new AuditTemplateViewModel() };

            AutoMapper.Mapper.Map(auditSection, model.Section);
            AutoMapper.Mapper.Map(auditTemaplate, model.Section.AuditTemplate);

            return View("Form", model);
        }

        public ActionResult Delete (int id)
        {
            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;

            var auditElement = _context.AuditElements.Single(a => a.Id == id);
            var auditSection = _context.AuditSections.Single(a => a.Id == auditElement.SectionId);
            var auditTemplate = _context.AuditTemplates.Single(a => a.Id == auditSection.AuditTemplateId);

            if(auditTemplate.GetCompletedAuditCount(_context) == 0)
            {
                var auditElementChoices = _context.AuditElementChoices.Where(a => a.ElementId == id).ToList();

                if (auditElementChoices.Count > 0)
                    _context.AuditElementChoices.RemoveRange(auditElementChoices);

                _context.AuditElements.Remove(auditElement);
                auditSection.ModifiedById = userId;
                auditSection.ModifiedDateTime = submitDateTime;
                auditTemplate.ModifiedById = userId;
                auditTemplate.ModifiedDateTime = submitDateTime;
                _context.SaveChanges();

                ViewBag.Message = "Audit element has been successfully deleted.";
            }
            else
            {
                ViewBag.Message = "Unable to delete audit element.  Audits have been completed using the audit template that the audit element belongs to.";
            }

            return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplate.Id });
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

            if(auditElement.Section.AuditTemplate.GetCompletedAuditCount(_context) == 0)
            {
                var viewModel = new AuditElementViewModel
                {
                    Section = new AuditSectionViewModel { AuditTemplate = new AuditTemplateViewModel() }
                };

                AutoMapper.Mapper.Map(auditElement, viewModel);

                return View("Form", viewModel);
            }

            ViewBag.Message = "Unable to edit element.  Audits have already been completed using the audit template that it belongs to.";
            return RedirectToAction("Details", "AuditTemplates", new { id = auditElement.Section.AuditTemplateId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuditElementViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;

            var auditElement = _context.AuditElements.Single(a => a.Id == model.Id);
            var auditSection = _context.AuditSections.Single(a => a.Id == auditElement.SectionId);
            var auditTemplate = _context.AuditTemplates.Single(a => a.Id == auditSection.AuditTemplateId);

            model.CreateDateTime = auditElement.CreateDateTime;
            model.CreatedById = auditElement.CreatedById;
            model.ModifiedDateTime = submitDateTime;
            model.ModifiedById = userId;

            if (ModelState.IsValid)
            {
                auditElement.UpdateFromViewModel(model);
                auditSection.ModifiedById = userId;
                auditSection.ModifiedDateTime = submitDateTime;
                auditTemplate.ModifiedById = userId;
                auditTemplate.ModifiedDateTime = submitDateTime;
                _context.SaveChanges();

                return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplate.Id });
            }

            model.Section = new AuditSectionViewModel { AuditTemplate = new AuditTemplateViewModel() };
            AutoMapper.Mapper.Map(auditSection, model.Section);
            AutoMapper.Mapper.Map(auditTemplate, model.Section.AuditTemplate);
            return View("Form", model);
        }
    }
}