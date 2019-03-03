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
    }

    public class CourseController : Controller
    {
        private ApplicationDbContext _context;
        public CourseController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Course

        public ActionResult Index(string title, string section)
        {
            if (!string.IsNullOrEmpty(title))
            {
                var course = _context.Courses.FirstOrDefault(c => c.UrlTitle == title);
                if (course != null)
                {
                    var viewModel = new CourseViewModel();
                    viewModel.Course = course;
                    viewModel.Section = section;
                    return View("Detail", viewModel);
                }
                return HttpNotFound();
            }
            return HttpNotFound();
        }
        [AllowAnonymous]
        public ActionResult Detail(int id)
        {
            return View();
        }

        #region Admin

        // GET: Course/CourseAdmin
        public ActionResult CourseAdmin()
        {
            var courses = _context.Courses.Include(b => b.Lector).ToList();
            return View(courses);
        }

        public ActionResult New()
        {
            var persons = _context.Persons.ToList();
            var viewModel = new CourseViewModel()
            {
                Persons = persons
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(Course course)
        {
            var persons = _context.Persons.ToList();
            var viewModel = new CourseViewModel()
            {
                Persons = persons,
            };

            if (!ModelState.IsValid)
            {
                viewModel.Course = course;
                return View(viewModel);
            }

            var exist = _context.Courses.FirstOrDefault(c => c.UrlTitle == course.UrlTitle);

            if (exist != null)
            {
                ModelState.AddModelError("course.UrlTitle", "Zadany url titulek již existuje");
                viewModel.Course = course;
                return View(viewModel);
            }


            course.Created = DateTime.Now;
            course.Changed = DateTime.Now;
            _context.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction("CourseAdmin");
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            var persons = _context.Persons.ToList();
            var viewModel = new CourseViewModel()
            {
                Persons = persons,
                Course = course
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {


            if (!ModelState.IsValid)
            {

                var persons = _context.Persons.ToList();
                var viewModel = new CourseViewModel()
                {
                    Persons = persons,
                    Course = course
                };

                return View(viewModel);
            }

            var existingCourse = _context.Courses.FirstOrDefault(c => c.UrlTitle == course.UrlTitle);
            bool exist = existingCourse?.Id != course.Id;
            if (exist)
            {
                ModelState.AddModelError("course.UrlTitle", "Zadany url titulek již existuje");
                var persons = _context.Persons.ToList();
                var viewModel = new CourseViewModel()
                {
                    Persons = persons,
                    Course = course
                };
                return View(viewModel);
            }

            var cour = _context.Courses.FirstOrDefault(c => c.Id == course.Id);
            if (cour != null)
            {
                cour.Name = course.Name;
                cour.Description = course.Description;
                cour.Lector_Id = course.Lector_Id;
                cour.Modificator = course.Modificator;
                cour.Svg = course.Svg;
                cour.UrlTitle = course.UrlTitle;
                cour.Changed = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("CourseAdmin");
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = _context.Courses.FirstOrDefault(b => b.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == id);
            _context.Courses.Remove(course ?? throw new InvalidOperationException());
            _context.SaveChanges();
            return RedirectToAction("CourseAdmin");
        }



        #endregion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmail(CourseViewModel viewModel)
        {
            var course = _context.Courses.FirstOrDefault(c => c.UrlTitle == viewModel.Course.UrlTitle);
            viewModel.Course = course;

            var emailSender = new EmailSender();
            var sent = await emailSender.SendEmail(viewModel.FormModel.Email, course.Name, $"{viewModel.FormModel.Name} {viewModel.FormModel.Surname} \n {viewModel.FormModel.Email}");
            if (!sent)
            {
                Console.WriteLine("sending email error");
            }
            // zabrani opetovnemu odeslani formulare a presune na /kurz/{urlTitle},
            // jinak by zustal na /Course/SendEmail a po refreshi by znovu odeslal mail
            return RedirectToAction("Index", new {title = course.UrlTitle, section = "Form"});
        }

    }
}