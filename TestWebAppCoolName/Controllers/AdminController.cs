using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestWebAppCoolName.Helpers;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers
{

    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        // GET: Admin
        public AdminController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            return View();
        }

        #region Kurz
        [Route("admin/kurz")]
        public ActionResult Course()
        {
            var courses = _context.Courses.Include(b => b.Lector).ToList();
            return View(courses);
        }
        [Route("admin/kurz/new")]
        public ActionResult NewCourse()
        {
            var persons = _context.Persons.ToList();
            var viewModel = new CourseViewModel()
            {
                Persons = persons
            };
            return View(viewModel);
        }
        [Route("admin/kurz/new")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCourse(CourseViewModel vm)
        {
            var tags = TagParser.ParseTags(vm.Tagy, _context);
            var persons = _context.Persons.ToList();
            var viewModel = new CourseViewModel()
            {
                Persons = persons,
            };

            if (!ModelState.IsValid)
            {
                viewModel.Course = vm.Course;
                viewModel.Course.Tags = tags;
                return View(viewModel);
            }

            var exist = _context.Courses.FirstOrDefault(c => c.UrlTitle == vm.Course.UrlTitle);

            if (exist != null)
            {
                ModelState.AddModelError("course.UrlTitle", "Zadany url titulek již existuje");
                viewModel.Course = vm.Course;
                viewModel.Course.Tags = tags;
                return View(viewModel);
            }


            vm.Course.Created = DateTime.Now;
            vm.Course.Changed = DateTime.Now;
            vm.Course.Tags = tags;
            _context.Courses.Add(vm.Course);
            _context.SaveChanges();

            return RedirectToAction("Course");
        }
        [Route("admin/kurz/edit/{id?}")]
        public ActionResult EditCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = _context.Courses.Include(b => b.Lector).Include(b => b.Tags).FirstOrDefault(b => b.Id == id);
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
        [Route("admin/kurz/edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse(CourseViewModel vm)
        {

            var tags = TagParser.ParseTags(vm.Tagy, _context);
            if (!ModelState.IsValid)
            {

                var persons = _context.Persons.ToList();
                var viewModel = new CourseViewModel()
                {
                    Persons = persons,
                    Course = vm.Course
                };

                return View(viewModel);
            }

            var existingCourse = _context.Courses.FirstOrDefault(c => c.UrlTitle == vm.Course.UrlTitle);
            bool sameUrlInAnotherCourse = false;
            if (existingCourse != null)
            {
                sameUrlInAnotherCourse = existingCourse?.Id != vm.Course.Id;
            }
            if (sameUrlInAnotherCourse)
            {
                ModelState.AddModelError("course.UrlTitle", "Zadany url titulek již existuje");
                var persons = _context.Persons.ToList();
                var viewModel = new CourseViewModel()
                {
                    Persons = persons,
                    Course = vm.Course
                };
                return View(viewModel);
            }

            var cour = _context.Courses.Include(c => c.Tags).FirstOrDefault(c => c.Id == vm.Course.Id);
            if (cour != null)
            {
                cour.Name = vm.Course.Name;
                cour.Description = vm.Course.Description;
                cour.WillLearn = vm.Course.WillLearn;
                cour.Body = vm.Course.Body;
                cour.Lector_Id = vm.Course.Lector_Id;
                cour.Modificator = vm.Course.Modificator;
                cour.Svg = vm.Course.Svg;
                cour.UrlTitle = vm.Course.UrlTitle;
                cour.Tags = tags;
                cour.Changed = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Course");
            }
            else
            {
                return HttpNotFound();
            }
        }
        [Route("admin/kurz/delete/{id?}")]
        public ActionResult DeleteCourse(int? id)
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
        [Route("admin/kurz/delete/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == id);
            _context.Courses.Remove(course ?? throw new InvalidOperationException());
            _context.SaveChanges();
            return RedirectToAction("Course");
        }
        #endregion

        #region Blog

        [Route("Admin/Blog")]
        public ActionResult Blog()
        {

            return View();
        }

        #endregion


        #region Tag

        [Route("Admin/Tag")]
        public ActionResult Tag()
        {

            return View();
        }

        #endregion


    }
}