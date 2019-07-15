using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TestWebAppCoolName.Helpers;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers
{
    public class CourseViewModel
    {
        public List<Person> Persons { get; set; }
        public Course Course { get; set; }
        public CourseContactForm FormModel { get; set; } = new CourseContactForm();
        public string Section { get; set; }

        public string Tagy { get; set; }
        public List<Svg> Svgs { get; set; }
    }


    public class CourseController : Controller
    {
        private ApplicationDbContext _context;
        public CourseController()
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
        // GET: Course

        public ActionResult Index(string title, string section, bool preview = false)
        {
            if (!string.IsNullOrEmpty(title))
            {
                var viewModel = new CourseViewModel();

                if (!preview)
                {
                    viewModel.Course = _context.Courses.Include(c => c.Svg).FirstOrDefault(c => c.UrlTitle == title && c.Approved && !c.Deleted);
                }
                else
                {
                    viewModel.Course = _context.Courses.Include(c => c.Svg).FirstOrDefault(c => c.UrlTitle == title);
                }

                if (viewModel.Course != null)
                {
                    viewModel.Section = section;
                    return View("Detail", viewModel);
                }
                return HttpNotFound();
            }
            return HttpNotFound();
        }


        public ActionResult Detail(int id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmail(CourseViewModel viewModel)
        {
            var course = _context.Courses.FirstOrDefault(c => c.UrlTitle == viewModel.Course.UrlTitle);
            viewModel.Course = course;

            var emailSender = new EmailSender();
            var sent = await emailSender.SendEmail(viewModel.FormModel.Email, $"Kurz: {course.Name}", $"{viewModel.FormModel.Name} {viewModel.FormModel.Surname} \n {viewModel.FormModel.Email}");
            if (!sent)
            {
                Console.WriteLine("sending email error");
            }
            else
            {
                TempData["EmailSent"] = true;
            }
            // zabrani opetovnemu odeslani formulare a presune na /kurz/{urlTitle},
            // jinak by zustal na /Course/SendEmail a po refreshi by znovu odeslal mail
            return RedirectToAction("Index", new { title = course.UrlTitle, section = "Form" });
        }

    }
}