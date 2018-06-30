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
    public class UsersController : Controller
    {
        private ApplicationDbContext _context;

        public UsersController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Add()
        {
            var viewModel = new RegisterViewModel
            {
                Roles = _context.Roles.Include(r => r.Users).ToList(),
                Supervisors = new List<ApplicationUser>()
            };

            var results = _context.Roles.Where(r => r.Name == "Admin" || r.Name == "Audit Manager" || r.Name == "Supervisor").Select(r => r.Users.Select(u => u.UserId)).ToList();
            foreach (var list in results)
            {
                var supervisors = _context.Users.Where(u => list.Contains(u.Id)).ToList();
                viewModel.Supervisors.AddRange(supervisors);
            }

            return View(viewModel);
        }

        public ActionResult Index()
        {
            var users = _context.Users.Include(u => u.Supervisor).Include(u => u.Roles).ToList();

            var roles = _context.Roles.ToList();
            var roleDictionary = new Dictionary<string, string>();
            foreach(var role in roles)
            {
                roleDictionary.Add(role.Id, role.Name);
            }

            ViewBag.Roles = roleDictionary;
            return View(users);
        }
    }
}