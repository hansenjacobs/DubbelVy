using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dubbelvy.Models;

namespace Dubbelvy.Controllers
{
    public class AuditTemplateController : Controller
    {
        // GET: AuditTemplate
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            var model = new AuditTemplateViewModel()
            {
                Sections = new List<AuditSectionViewModel>()
            };
            return View("AuditTemplateForm", model);
        }

        public ActionResult AddSection(int index)
        {
            var newSection = new AuditSectionViewModel { Index = index };
            ViewData.TemplateInfo.HtmlFieldPrefix = $"Sections[{index}]";
            return PartialView("~/Views/Shared/EditorTemplates/AuditSectionViewModel.cshtml");
        }
    }
}