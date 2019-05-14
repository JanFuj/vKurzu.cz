using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers
{
    public class HomeViewModel
    {
        public List<Course> Courses { get; set; }
        public List<Blog> Blogs { get; set; }
    }
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private HomeViewModel _viewModel;

        public HomeController()
        {
            _context = new ApplicationDbContext();
            _viewModel = new HomeViewModel();

        }
        public ActionResult Index() {
       
            _viewModel.Courses = _context.Courses.Include(b => b.Lector).Include(c=>c.Svg).Where(c=>!c.Deleted).OrderBy(c=>c.Position).ToList();
            _viewModel.Blogs = _context.Blogs.Include(b=>b.Author).ToList();
            return View(_viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

      
    }
}