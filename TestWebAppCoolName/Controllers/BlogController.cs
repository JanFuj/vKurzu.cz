using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestWebAppCoolName.Helpers;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers
{
    public class BlogViewModel
    {
        public List<Person> Persons { get; set; }
        public Blog Blog { get; set; }

        public string Tagy { get; set; }
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
        public ActionResult Index(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                if (title == "index")
                {
                    RouteData.Values.Remove("title");
                    return RedirectToAction("Index");
                }


                //detail blogu
                var blog = _context.Blogs.Include(b => b.Author).FirstOrDefault(b => b.UrlTitle == title);
                return View("Article", blog);
            }
            //seznam blogu
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
        public ActionResult Admin()
        {

            return View(_context.Blogs.Include(b => b.Author).ToList());
        }



        // GET: Blog/New
        public ActionResult New()
        {
            var persons = _context.Persons.ToList();
            var viewModel = new BlogViewModel()
            {
                Persons = persons,
                Blog = new Blog(),

            };
            return View(viewModel);
        }

        // POST: Blog/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(BlogViewModel vm) {

            var tags =TagParser.ParseTags(vm.Tagy,_context); // ParseTags(vm.Tagy);
            var persons = _context.Persons.ToList();
            var viewModel = new BlogViewModel()
            {
                Persons = persons
            };
            if (!ModelState.IsValid)
            {
                viewModel.Blog = vm.Blog;
                viewModel.Blog.Tags = tags;
                return View(viewModel);
            }

            var exist = _context.Blogs.FirstOrDefault(b => b.UrlTitle == vm.Blog.UrlTitle);
            if (exist != null)
            {
                ModelState.AddModelError("blog.UrlTitle", "Zadany url titulek již existuje");
                viewModel.Blog = vm.Blog;
                viewModel.Blog.Tags = tags;
                return View(viewModel);
            }


            vm.Blog.Created = DateTime.Now;
            vm.Blog.Changed = DateTime.Now;
            vm.Blog.Tags = tags;
            _context.Blogs.Add(vm.Blog);
            _context.SaveChanges();
            ViewData["Saved"] = "Blog byl vyrvořen";
            ModelState.Clear();
            return RedirectToAction("Admin");

        }

        private List<Tag> ParseTags(string tagy)
        {

            var listTags = new List<Tag>();
            if (string.IsNullOrEmpty(tagy))
            {
                return listTags;
            }
            var tags = tagy.Split('#');
            foreach (var tag in tags)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    var existingTag = _context.Tags.FirstOrDefault(t => t.Name == tag.Trim());
                    if (existingTag != null)
                    {
                        listTags.Add(existingTag);
                    }
                }
            }

            return listTags;
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = _context.Blogs.Include(b => b.Author).Include(b => b.Tags).FirstOrDefault(b => b.Id == id);
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
        // POST: Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogViewModel vm)
        {
            var tags = TagParser.ParseTags(vm.Tagy, _context); //ParseTags(vm.Tagy);
            if (!ModelState.IsValid)
            {
                var persons = _context.Persons.ToList();
                var viewModel = new BlogViewModel()
                {
                    Persons = persons,
                    Blog = vm.Blog

                };
                return View(viewModel);
            }
            //muze editovat pouze pokud stejny url title neexistuje u jineho blogu
            var existingBlog = _context.Blogs.FirstOrDefault(b => b.UrlTitle == vm.Blog.UrlTitle);
            bool exist = existingBlog?.Id != vm.Blog.Id;
            if (exist)
            {
                ModelState.AddModelError("blog.UrlTitle", "Zadany url titulek již existuje");
                var persons = _context.Persons.ToList();
                var viewModel = new BlogViewModel()
                {
                    Persons = persons,
                    Blog = vm.Blog

                };
                return View(viewModel);
            }

            var blo = _context.Blogs.Include(b => b.Tags).FirstOrDefault(b => b.Id == vm.Blog.Id);
            if (blo != null)
            {


                blo.Tags = null;
                _context.SaveChanges();


                blo.Name = vm.Blog.Name;
                blo.Description = vm.Blog.Description;
                blo.Author_Id = vm.Blog.Author_Id;
                blo.UrlTitle = vm.Blog.UrlTitle;
                blo.Tags = tags;
                blo.Changed = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Admin");
            }
            else
            {
                return HttpNotFound();
            }
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
            return RedirectToAction("Admin");
        }
    }
}
