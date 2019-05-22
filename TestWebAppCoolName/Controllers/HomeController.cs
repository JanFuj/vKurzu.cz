using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using TestWebAppCoolName.Helpers;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers
{
    public class HomeViewModel
    {
        public List<Course> Courses { get; set; }
        public List<Blog> Blogs { get; set; }
        public HomeContactForm FormModel { get; set; } = new HomeContactForm();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmail(HomeViewModel viewModel)
        {
        

            var emailSender = new EmailSender();
            var sent = await emailSender.SendEmail(viewModel.FormModel.Email, "Dotaz", $"{viewModel.FormModel.Message} \n {viewModel.FormModel.Email}");
            if (!sent)
            {
                Console.WriteLine("sending email error");
            }
            // zabrani opetovnemu odeslani formulare a presune na /kurz/{urlTitle},
            // jinak by zustal na /Course/SendEmail a po refreshi by znovu odeslal mail
            return RedirectToAction("Index");
        }

    }
}