using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebAppCoolName.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace TestWebAppCoolName.Controllers.Admin
{
    [Authorize]
    public class TutorialCategoryController : Controller
    {
        private ApplicationDbContext _context;

        public TutorialCategoryController()
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

        // GET: TutorialCategory
        [HttpGet]
        [Route("admin/tutorialCategory/{title?}")]
        public ActionResult Index(string title)
        {
            //  var userId = User.Identity.GetUserId();
            var category = _context.TutorialCategory.Include(x => x.Posts).Include(x => x.Tags)
                .FirstOrDefault(x => x.UrlTitle == title);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // GET: TutorialCategory/Details/5
       

        // GET: TutorialCategory/Create
        [HttpGet]
        [Route("admin/tutorialCategory/{title?}/new")]
        public ActionResult NewPost()
        {
            var persons = _context.Persons.ToList();
            var viewModel = new BlogViewModel()
            {
                Persons = persons,
                Blog = new Blog(),

            };
            return View("NewPost", viewModel);
        }

        // POST: TutorialCategory/Create
        [HttpPost]
        [Route("admin/tutorialCategory/{title?}/new")]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [Route("admin/tutorialCategory/{title}/detail/{id}")]
        public ActionResult Details(string title, int id)
        {

            var category = _context.TutorialCategory.Include(x => x.Posts).Include(x => x.Tags)
                .FirstOrDefault(x => x.UrlTitle == title);
            var post = category?.Posts.FirstOrDefault(x => x.Id == id);
            if (category == null || post == null)
            {
                return HttpNotFound();
            }


            return View();
        }
        // GET: TutorialCategory/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TutorialCategory/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TutorialCategory/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TutorialCategory/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
