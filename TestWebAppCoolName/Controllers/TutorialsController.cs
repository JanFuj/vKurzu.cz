using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TestWebAppCoolName.DAL;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers
{
    public class TutorialsViewModel
    {
        public List<Person> Persons { get; set; }
        public TutorialPost TutorialPost { get; set; }
        public List<TutorialPost> Posts { get; set; }
        public TutorialCategory TutorialCategory { get; set; }
        public Course RelatedCourse { get; set; }
        public string Tagy { get; set; }
    }

    public class TutorialsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private TutorialCategoryPostsRepository _repo;

        public TutorialsController()
        {
            _context = new ApplicationDbContext();
            _repo = new TutorialCategoryPostsRepository(_context);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: Tutorials
        [HttpGet]
        [Route("tutorialy")]
        public ActionResult Index()
        {
            //seznam kategorií

            var categories = _context.TutorialCategory.Where(c => !c.Deleted && c.Approved).OrderBy(c => c.Position).ToList();

            return View(categories);
        }
        [HttpGet]
        [Route("tutorialy/{categoryTitle}")]
        public ActionResult TutorialCategoryPosts(string categoryTitle)
        {
            //seznam kategorií

            var category = _repo.GetTutorialCategory(categoryTitle);
            if (category == null)
            {
                return HttpNotFound();
            }

            var posts = category.Posts.Where(p => p.Approved && !p.Deleted).OrderBy(p=>p.Position).ToList();

            var viewModel = new TutorialsViewModel()
            {
                TutorialCategory = category,
                Posts = posts
            };
            return View(viewModel);
        }
        [HttpGet]
        [Route("tutorialy/{categoryTitle}/{postTitle}")]
        public ActionResult TutorialPost(string categoryTitle, string postTitle, bool preview = false)
        {
            var post = _repo.GetPostByUrl(categoryTitle, postTitle);
            if (post == null)
            {
                return HttpNotFound();
            }
            if (!preview && !post.Approved)
            {
                return HttpNotFound();
            }

            var vm = new TutorialsViewModel { TutorialPost = post };

            return View("TutorialPost", vm);
        }


    }
}