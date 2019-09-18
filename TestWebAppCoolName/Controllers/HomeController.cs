using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using TestWebAppCoolName.DAL;
using TestWebAppCoolName.Helpers;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers
{
    public class HomeViewModel
    {
        public List<Course> Courses { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<TutorialCategory> TutorialCategories { get; set; }
        public HomeContactForm FormModel { get; set; } = new HomeContactForm();
        public bool ShowAlert { get; set; } = false;
    }
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private HomeViewModel _viewModel;
        private BlogRepository _blogRepo;

        public HomeController()
        {
            _context = new ApplicationDbContext();
            _viewModel = new HomeViewModel();
            _blogRepo = new BlogRepository(_context);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _blogRepo.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult Index()
        {

            _viewModel.Courses = _context.Courses.Include(c => c.Svg).Where(c => !c.Deleted).OrderBy(c => c.Position).ToList();
            _viewModel.TutorialCategories = _context.TutorialCategory.Include(c => c.Thumbnail).Where(c => c.Approved && !c.Deleted).OrderBy(c => c.Position).ToList();
            _viewModel.Blogs = _blogRepo.GetFirst3BlogPosts();
          //  _viewModel.ShowAlert = !string.IsNullOrEmpty(TempData["EmailSent"]?.ToString());
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
            else
            {
                TempData["EmailSent"] = "sent";
            }
            // zabrani opetovnemu odeslani formulare a presune na /kurz/{urlTitle},
            // jinak by zustal na /Course/SendEmail a po refreshi by znovu odeslal mail
            return RedirectToAction("Index");
        }

    }
}