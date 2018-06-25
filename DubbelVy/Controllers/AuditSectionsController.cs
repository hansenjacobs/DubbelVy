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

            var viewModel = new AuditSectionViewModel
            {
                AuditTemplate = new AuditTemplateViewModel(),
                AuditTemplateId = auditTemplateId
            };

            AutoMapper.Mapper.Map(_context.AuditTemplates.Single(a => a.Id == auditTemplateId), viewModel.AuditTemplate);

            return View("Form",viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AuditSectionViewModel model)
        {
            model.CreateDateTime = DateTime.Now;
            model.ModifiedDateTime = DateTime.Now;
            model.CreatedById = User.Identity.GetUserId();
            model.ModifiedById = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                var newAuditSection = new AuditSection();

                AutoMapper.Mapper.Map(model, newAuditSection);
                _context.AuditSections.Add(newAuditSection);
                _context.SaveChanges();

                return RedirectToAction("Details", "AuditTemplates", new { id = newAuditSection.AuditTemplateId });
            }

            model.AuditTemplate = new AuditTemplateViewModel();
            AutoMapper.Mapper.Map(_context.AuditTemplates.Single(a => a.Id == model.AuditTemplateId), model.AuditTemplate);
            return View("Form", model);
        }

        public ActionResult Delete(int id)
        {
            var auditSection = _context.AuditSections.Single(a => a.Id == id);
            var auditTemplateId = auditSection.AuditTemplateId;
            _context.AuditSections.Remove(auditSection);
            _context.SaveChanges();

            return RedirectToAction("Details", "AuditTemplates", new { id = auditTemplateId });
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

            var viewModel = new AuditSectionViewModel();

            AutoMapper.Mapper.Map(auditSection, viewModel);

            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AuditSectionViewModel model)
        {
            var auditSection = _context.AuditSections.Single(a => a.Id == model.Id);

            model.CreateDateTime = auditSection.CreateDateTime;
            model.ModifiedDateTime = DateTime.Now;
            model.CreatedById = auditSection.CreatedById;
            model.ModifiedById = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                AutoMapper.Mapper.Map(model, auditSection);

                _context.SaveChanges();

                return RedirectToAction("Details", "AuditTemplates", new { id = auditSection.AuditTemplateId });
            }

            model.AuditTemplate = new AuditTemplateViewModel();
            AutoMapper.Mapper.Map(_context.AuditTemplates.Single(a => a.Id == model.AuditTemplateId), model.AuditTemplate);
            return View("Form", model);
        }
    }
}