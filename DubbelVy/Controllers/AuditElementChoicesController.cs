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
    public class AuditElementChoicesController : Controller
    {
        private ApplicationDbContext _context;

        public AuditElementChoicesController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Add(int id)
        {
            var auditElementId = id;
            var auditElement = _context.AuditElements
                .Include(a => a.Section.AuditTemplate)
                .Single(a => a.Id == auditElementId);

            if(auditElement.Section.AuditTemplate.GetCompletedAuditCount(_context) == 0)
            {
                var viewModel = new AuditElementChoiceViewModel
                {
                    ElementId = auditElementId,
                    Element = new AuditElementViewModel
                    {
                        Section = new AuditSectionViewModel { AuditTemplate = new AuditTemplateViewModel() }
                    }
                };
                AutoMapper.Mapper.Map(auditElement, viewModel.Element);

                return View("Form", viewModel);
            }

            ViewBag.Message = "Unable to add choice.  Audits have been completed using the audit template this audit template.";
            return RedirectToAction("Details", "AuditTemplates", new { id = auditElement.Section.AuditTemplateId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AuditElementChoiceViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;

            var auditElement = _context.AuditElements.Single(a => a.Id == model.ElementId);
            var auditSection = _context.AuditSections.Single(a => a.Id == auditElement.SectionId);
            var auditTemplate = _context.AuditTemplates.Single(a => a.Id == auditSection.AuditTemplateId);

            if(auditTemplate.GetCompletedAuditCount(_context) == 0)
            {
                if (ModelState.IsValid)
                {
                    var auditElementChoice = new AuditElementChoice();

                    AutoMapper.Mapper.Map(model, auditElementChoice);
                    _context.AuditElementChoices.Add(auditElementChoice);

                    auditElement.ModifiedById = userId;
                    auditElement.ModifiedDateTime = submitDateTime;
                    auditSection.ModifiedById = userId;
                    auditSection.ModifiedDateTime = submitDateTime;
                    auditTemplate.ModifiedById = userId;
                    auditTemplate.ModifiedDateTime = submitDateTime;

                    _context.SaveChanges();

                    return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplate.Id });
                }

                model.Element = new AuditElementViewModel
                {
                    Section = new AuditSectionViewModel { AuditTemplate = new AuditTemplateViewModel() }
                };
                AutoMapper.Mapper.Map(auditElement, model.Element);
                AutoMapper.Mapper.Map(auditSection, model.Element.Section);
                AutoMapper.Mapper.Map(auditTemplate, model.Element.Section.AuditTemplate);
                return View("Form", model);
            }

            ViewBag.Message = "Unable to add new choice. Audits have been completed using the audit template.";
            return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplate.Id });
        }

        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;

            var auditElementChoice = _context.AuditElementChoices.Single(a => a.Id == id);
            var auditElement = _context.AuditElements.Single(a => a.Id == auditElementChoice.ElementId);
            var auditSection = _context.AuditSections.Single(a => a.Id == auditElement.SectionId);
            var auditTemplate = _context.AuditTemplates.Single(a => a.Id == auditSection.AuditTemplateId);

            if (auditTemplate.GetCompletedAuditCount(_context) == 0)
            {
                _context.AuditElementChoices.Remove(auditElementChoice);
                auditElement.ModifiedById = userId;
                auditElement.ModifiedDateTime = submitDateTime;
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

        public ActionResult Edit(int id)
        {
            var auditElementChoice = _context.AuditElementChoices
                .Include(a => a.Element.Section.AuditTemplate)
                .Single(a => a.Id == id);

            if (auditElementChoice.Element.Section.AuditTemplate.GetCompletedAuditCount(_context) == 0)
            {
                var viewModel = new AuditElementChoiceViewModel
                {
                    Element = new AuditElementViewModel
                    {
                        Section = new AuditSectionViewModel { AuditTemplate = new AuditTemplateViewModel() }
                    }
                    
                };

                AutoMapper.Mapper.Map(auditElementChoice, viewModel);

                return View("Form", viewModel);
            }

            ViewBag.Message = "Unable to edit element choice.  Audits have already been completed using the audit template that it belongs to.";
            return RedirectToAction("Details", "AuditTemplates", new { id = auditElementChoice.Element.Section.AuditTemplateId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuditElementChoiceViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var submitDateTime = DateTime.Now;

            var auditElementChoice = _context.AuditElementChoices.Single(a => a.Id == model.Id);
            var auditElement = _context.AuditElements.Single(a => a.Id == auditElementChoice.ElementId);
            var auditSection = _context.AuditSections.Single(a => a.Id == auditElement.SectionId);
            var auditTemplate = _context.AuditTemplates.Single(a => a.Id == auditSection.AuditTemplateId);

            if (ModelState.IsValid)
            {
                auditElementChoice.UpdateFromViewModel(model);
                auditElement.ModifiedById = userId;
                auditElement.ModifiedDateTime = submitDateTime;
                auditSection.ModifiedById = userId;
                auditSection.ModifiedDateTime = submitDateTime;
                auditTemplate.ModifiedById = userId;
                auditTemplate.ModifiedDateTime = submitDateTime;
                _context.SaveChanges();

                return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplate.Id });
            }

            model.Element = new AuditElementViewModel
            {
                Section = new AuditSectionViewModel { AuditTemplate = new AuditTemplateViewModel() }
            };
            AutoMapper.Mapper.Map(auditElement, model.Element);
            AutoMapper.Mapper.Map(auditSection, model.Element.Section);
            AutoMapper.Mapper.Map(auditTemplate, model.Element.Section.AuditTemplate);
            return View("Form", model);
        }
    }
}