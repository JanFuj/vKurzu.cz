using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using TestWebAppCoolName.ActionResults;
using TestWebAppCoolName.DAL;
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
        [Display(Name = "Obrázek pro sdílení na sociálech")]
     //   [Required(ErrorMessage = "Nahrajte obrzázek")]
       // [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Pouze png, jpg, gif")]
        public HttpPostedFileBase Thumbnail { get; set; }
    }

    public class BlogController : Controller
    {
        private ApplicationDbContext _context;
        private IBlogRepository _repo;

        public BlogController() {
            _context = new ApplicationDbContext();
            _repo = new BlogRepository(_context);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _repo.Dispose();
            base.Dispose(disposing);
        }
        [AllowAnonymous]
        // GET: Blog
        public ActionResult Index(string title, bool preview = false)
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
                if (!preview)
                {
                    vm.Blog = _context.Blogs.Include(b => b.Tags).Include(b=>b.Thumbnail).FirstOrDefault(b => b.UrlTitle == title && b.Approved);
                }
                else {
                    vm.Blog = _context.Blogs.Include(b => b.Tags).Include(b => b.Thumbnail).FirstOrDefault(b => b.UrlTitle == title);
                }
                if (vm.Blog != null)
                {
                    vm.RelatedCourse = new DataService().GetRelatedCourse(vm.Blog);
                    ViewBag.Thumbnail = vm.Blog.Thumbnail?.Path;
                    ViewBag.Description = vm.Blog.Description;
                    return View("Article", vm);
                }

                return HttpNotFound();
            }
            //seznam blogu
            var blogs = _repo.GetBlogPosts();
            return View(blogs);
        }

        [AllowAnonymous]
        // GET: Blog/Article/5
        public ActionResult Article(int? id)
        {
            return View("Article");
        }


        [Route("blog/rss/new")]
        public ActionResult Rss()
        {
            IEnumerable<Blog> blogs = _context.Blogs.Where(b => b.Approved);
            var feed =
                new SyndicationFeed("CoolName", "Coolname",
                    new Uri("http://coolname.aspfree.cz/"),
                    Guid.NewGuid().ToString(),
                    DateTime.Now);
            var items = new List<SyndicationItem>();
            foreach (Blog bp in blogs)
            {
                string postUrl = $"blog/{bp.UrlTitle}";
                var item = new SyndicationItem(bp.Name,
                        bp.Description,
                        new Uri(postUrl, UriKind.Relative),
                        bp.Id.ToString(),
                        bp.Created);
                items.Add(item);

            }
            feed.Items = items;
            return new RssActionResult { Feed = feed };
        }
    }
}
