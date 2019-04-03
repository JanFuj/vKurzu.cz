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
        public Course RelatedCourse { get; set; }
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

                var vm = new BlogViewModel();
                //detail blogu
                vm.Blog = _context.Blogs.Include(b => b.Author).Include(b => b.Tags).FirstOrDefault(b => b.UrlTitle == title);
                vm.RelatedCourse = new DataService().GetRelatedCourse(vm.Blog);
                return View("Article", vm);
            }
            //seznam blogu
            var blogs = _context.Blogs.Include(b => b.Author).Include(b => b.Tags).Where(b=>b.Approved).ToList();
            return View(blogs);
        }

        [AllowAnonymous]
        // GET: Blog/Article/5
        public ActionResult Article(int? id)
        {
            return View("Article");
        }



   
    }
}
