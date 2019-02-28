using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers
{
    public class CourseViewModel
    {
        public List<Person> Persons { get; set; }
        public Course Course { get; set; }
    }

    public class CourseController : Controller
    {
        private ApplicationDbContext _context;
        public CourseController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Course

        public ActionResult Index(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                var course = _context.Courses.FirstOrDefault(c => c.UrlTitle == title);
                if (course != null)
                {
                    return View("Detail", course);
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
            if (ModelState.IsValid)
            {
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
            var persons = _context.Persons.ToList();
            var viewModel = new CourseViewModel()
            {
                Persons = persons,
                Course = course
            };
            return View(viewModel);
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


    }
}