using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dubbelvy.Models;
using Microsoft.AspNet.Identity;

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
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AuditElementCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var auditElement = new AuditElement()
                {
                    Topic = model.Topic,
                    Text = model.Text,
                    CreateDateTime = DateTime.Now,
                    CreatedById = new Guid(User.Identity.GetUserId())
                };

                _context.AuditElements.Add(auditElement);
                _context.SaveChanges();

                return RedirectToAction("Details", new { id = auditElement.Id });
            }

            return View(model);
        }

    }
}