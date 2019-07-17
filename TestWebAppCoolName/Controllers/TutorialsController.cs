using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers
{
    public class TutorialsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TutorialsController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: Tutorials
        public ActionResult Index()
        {
            //seznam kategorií

            var userId = User.Identity.GetUserId();
            var categories = _context.TutorialCategory.Where(c => !c.Deleted).ToList();
            if (User.IsInRole(Roles.Lector))
            {
                categories = _context.TutorialCategory.Where(c => !c.Deleted && c.OwnerId == userId).ToList();
            }
            return View(categories.OrderBy(c => c.Position));
        }


    }
}