using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dubbelvy.Models;
using Microsoft.AspNet.Identity;
using AutoMapper;
using System.Data.Entity;

namespace Dubbelvy.Controllers
{
    public class AuditTemplatesController : Controller
    {
        private ApplicationDbContext _context { get; set; }

        public AuditTemplatesController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var auditTemplates = _context.AuditTemplates
                .Include(a => a.CreatedBy)
                .Include(a => a.ModifiedBy)
                .ToList();

            var viewModel = new List<AuditTemplateViewModel>();

            AutoMapper.Mapper.Map(auditTemplates, viewModel);

            foreach(var template in viewModel)
            {
                template.AuditsCompleted = _context.Audits.Count(a => a.AuditTemplateId == template.Id);
            }

            return View(viewModel);
        }

        public ActionResult Add()
        {
            var viewModel = new AuditTemplateViewModel();
            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AuditTemplateViewModel model)
        {
            model.CreateDateTime = DateTime.Now;
            model.ModifiedDateTime = DateTime.Now;
            model.CreatedById = User.Identity.GetUserId();
            model.ModifiedById = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                var auditTemaplate = new AuditTemplate();

                AutoMapper.Mapper.Map(model, auditTemaplate);

                _context.AuditTemplates.Add(auditTemaplate);
                _context.SaveChanges();

                return RedirectToAction("Details", new { id = auditTemaplate.Id });
            }

            return View("Form", model);
        }

        public ActionResult Delete(int id)
        {
            if(_context.Audits.Count(a => a.AuditTemplateId == id) == 0)
            {
                var auditTemplate = _context.AuditTemplates.Single(a => a.Id == id);
                var auditSections = _context.AuditSections.Where(a => a.AuditTemplateId == id).ToList();
                var auditElements = new List<AuditElement>();
                var auditElementChoices = new List<AuditElementChoice>();

                foreach (var section in auditSections)
                {
                    var results = _context.AuditElements.Where(a => a.SectionId == section.Id).ToList();
                    if (results.Count > 0)
                        auditElements.AddRange(results);
                }

                foreach (var element in auditElements)
                {
                    var results = _context.AuditElementChoices.Where(a => a.ElementId == element.Id).ToList();
                    if (results.Count > 0)
                        auditElementChoices.AddRange(results);
                }

                if (auditElementChoices.Count > 0)
                    _context.AuditElementChoices.RemoveRange(auditElementChoices);

                if (auditElements.Count > 0)
                    _context.AuditElements.RemoveRange(auditElements);

                if (auditSections.Count > 0)
                    _context.AuditSections.RemoveRange(auditSections);

                _context.AuditTemplates.Remove(auditTemplate);
                _context.SaveChanges();

                ViewBag.Message = "Audit template deleted.";
            }
            else
            {
                ViewBag.Message = "Unable to delete audit template.  Audit templates cannot be deleted once audits have been completed using the template.";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var auditTemplate = _context.AuditTemplates
                .Include(a => a.CreatedBy)
                .Include(a => a.ModifiedBy)
                .Single(a => a.Id == id);

            var auditSections = _context.AuditSections
                .Where(a => a.AuditTemplateId == id)
                .ToList();


            var viewModel = new AuditTemplateViewModel
            {
                Sections = new List<AuditSectionViewModel>()
            };

            AutoMapper.Mapper.Map(auditTemplate, viewModel);
            AutoMapper.Mapper.Map(auditSections, viewModel.Sections);

            foreach(var section in viewModel.Sections)
            {
                var elements = _context.AuditElements
                    .Where(a => a.SectionId == section.Id)
                    .ToList();

                section.Elements = new List<AuditElementViewModel>();

                AutoMapper.Mapper.Map(elements, section.Elements);

                foreach(var element in section.Elements)
                {
                    var choices = _context.AuditElementChoices
                        .Where(a => a.ElementId == element.Id)
                        .ToList();

                    element.Choices = new List<AuditElementChoiceViewModel>();

                    AutoMapper.Mapper.Map(choices, element.Choices);
                }
            }

            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var viewModel = new AuditTemplateViewModel
            {
                AuditsCompleted = _context.Audits.Count(a => a.AuditTemplateId == id)
            };

            if(viewModel.AuditsCompleted == 0)
            {
                var auditTemplate = _context.AuditTemplates.Single(a => a.Id == id);

                AutoMapper.Mapper.Map(auditTemplate, viewModel);

                return View("Form", viewModel);
            }

            ViewBag.Message = "You cannot edit an audit template once audits have already been completed using it.";
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuditTemplateViewModel model)
        {
            var auditTemplate = _context.AuditTemplates.Single(a => a.Id == model.Id);
            model.CreateDateTime = auditTemplate.CreateDateTime;
            model.CreatedById = auditTemplate.CreatedById;
            model.ModifiedDateTime = DateTime.Now;
            model.ModifiedById = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                AutoMapper.Mapper.Map(model, auditTemplate);
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = auditTemplate.Id });
            }
            return View("Form", model);
        }
    }
}