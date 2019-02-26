using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers
{
    public class BlogViewModel
    {
        public List<Person> Persons { get; set; }
        public Blog Blog { get; set; }
    }

    public class BlogController : Controller
    {
        private ApplicationDbContext _context;

        public BlogController()
        {
            _context = new ApplicationDbContext();
        }
        [AllowAnonymous]
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        // GET: Blog/Article/5
        public ActionResult Article(int? id)
        {


            return View("Article");

        }

        #region Admin
        // GET: Blog/BlogAdmin
        public ActionResult BlogAdmin()
        {
            return View(_context.Blogs.Include(b => b.Author).ToList());
        }



        // GET: Blog/New
        public ActionResult New()
        {
            var persons = _context.Persons.ToList();
            var viewModel = new BlogViewModel()
            {
                Persons = persons
            };
            return View(viewModel);
        }

        // POST: Blog/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(Blog blog)
        {

            var persons = _context.Persons.ToList();
            var viewModel = new BlogViewModel()
            {
                Persons = persons
            };
            if (!ModelState.IsValid)
            {
                viewModel.Blog = blog;
                return View(viewModel);
            }
            blog.Created = DateTime.Now;
            blog.Changed = DateTime.Now;
            _context.Blogs.Add(blog);
            _context.SaveChanges();
            ViewData["Saved"] = "Blog byl vyrvořen";
            ModelState.Clear();
            return RedirectToAction("BlogAdmin");

        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            var persons = _context.Persons.ToList();
            var viewModel = new BlogViewModel()
            {
                Persons = persons,
                Blog = blog

            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Blog blog)
        {
            if (ModelState.IsValid)
            {
                var blo = _context.Blogs.FirstOrDefault(b => b.Id == blog.Id);
                if (blo != null)
                {
                    blo.Name = blog.Name;
                    blo.Description = blog.Description;
                    blo.Author_Id = blog.Author_Id;
                    blo.Changed = DateTime.Now;
                    _context.SaveChanges();
                    return RedirectToAction("BlogAdmin");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            var persons = _context.Persons.ToList();
            var viewModel = new BlogViewModel()
            {
                Persons = persons,
                Blog = blog

            };
            return View(viewModel);
        }



        // GET: Blog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }
        #endregion

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction("BlogAdmin");
        }
    }
}
