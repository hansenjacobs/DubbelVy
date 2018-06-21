using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Dubbelvy.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Dubbelvy.Controllers
{
    public class AuditElementsController : Controller
    {
        private ApplicationDbContext _context;

        public AuditElementsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: AuditElements
        public ActionResult Index()
        {
            var elements = _context.AuditElements
                .Include(a => a.CreatedBy)
                .OrderBy(a => a.Topic);
            return View(elements);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AuditElement element, List<AuditElementChoice> choices)
        {
            element.CreateDateTime = DateTime.Now;
            element.CreatedById = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                _context.AuditElements.Add(element);
                _context.SaveChanges();

                foreach (var choice in choices)
                {
                    choice.ElementId = element.Id;
                }

                _context.AuditElementChoices.AddRange(choices);
                _context.SaveChanges();

                return RedirectToAction("Details", new { id = element.Id });
            }

            return View(element);
        }

        public ActionResult Details(int id)
        {
            var auditElement = _context.AuditElements
                .Include(a => a.Sections.Select(s => s.Section.AuditTemplate))
                .Include(a => a.Choices)
                .Include(a => a.CreatedBy)
                .Single(a => a.Id == id);

            return View(auditElement);
        }

        public ActionResult Edit(int id)
        {
            var element = _context.AuditElements
                .Include(a => a.Sections.Select(s => s.Section.AuditTemplate))
                .Single(a => a.Id == id);

            var isEditable = true;
            foreach(var section in element.Sections)
            {
                if (section.Section.AuditTemplate.DeployDateTime != null)
                {
                    isEditable = false;
                    break;
                }
            }

            if (!isEditable)
            {
                TempData.Add("Editable", isEditable);
            }

            return View(element);
        }

        public ActionResult _CreateAuditElementChoice(int? i)
        {
            ViewBag.i = i;
            return PartialView();
        }
    }
}