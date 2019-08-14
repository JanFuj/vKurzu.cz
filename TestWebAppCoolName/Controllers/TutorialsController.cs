using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            //seznam kategorií

            var userId = User.Identity.GetUserId();
            var categories = _context.TutorialCategory.Where(c => !c.Deleted).ToList();
            if (User.IsInRole(Roles.Lector))
            {
                categories = _context.TutorialCategory.Where(c => !c.Deleted && c.OwnerId == userId).ToList();
            }
            return View(categories.OrderBy(c => c.Position));
        }
        [HttpGet]
        [Route("tutorialCategory/{categoryTitle}/{postTitle}")]
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

            post.Author = _repo.GetAuthorById(post.Author_Id);
            var vm = new TutorialsViewModel {TutorialPost = post};

            return View("TutorialPost", vm);
        }


    }
}