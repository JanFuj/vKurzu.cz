using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebAppCoolName.Models;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using TestWebAppCoolName.DAL;

namespace TestWebAppCoolName.Controllers.Admin
{
    public class TutorialCategoryViewModel
    {
        public TutorialCategory TutorialCategory { get; set; }
        public List<TutorialPost> TutorialPosts { get; set; }
        public List<Person> Persons { get; set; }
        public TutorialPost TutorialPost { get; set; }
        public string Tagy { get; set; }
    }


    //Detail kategorie - seznam clanku tvorba novych
    [Authorize]
    public class TutorialCategoryController : Controller
    {
        private readonly ITutorialCategoryPostsRepository _repo;

        public TutorialCategoryController()
        {
            _repo = new TutorialCategoryPostsRepository(new ApplicationDbContext());

        }


        // GET: TutorialCategory
        [HttpGet]
        [Route("admin/tutorialCategory/{title?}")]
        public ActionResult Index(string title)
        {
            var viewModel = new TutorialCategoryViewModel();
            //  var userId = User.Identity.GetUserId();
            var category = _repo.GetTutorialCategory(title);
            if (category == null)
            {
                return HttpNotFound();
            }

            List<TutorialPost> posts;
            if (User.IsInRole(Roles.Lector))
            {
                posts = _repo.GetPostsByOwner(category.UrlTitle, User.Identity.GetUserId());
            }
            else
            {
                posts = _repo.GetPosts(category.UrlTitle);
            }

            viewModel.TutorialCategory = category;
            viewModel.TutorialPosts = posts;
            viewModel.Persons = _repo.GetPeople();

            return View(viewModel);
        }

        // GET: TutorialCategory/Details/5


        // GET: TutorialCategory/Create
        [HttpGet]
        [Route("admin/tutorialCategory/{title?}/new")]
        public ActionResult NewPost()
        {
            // var persons = _context.Persons.ToList();
            var viewModel = new TutorialCategoryViewModel()
            {
                Persons = _repo.GetPeople(),
                TutorialPost = new TutorialPost(),

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

            //var category = _context.TutorialCategory.Include(x => x.Posts).Include(x => x.Tags)
            //    .FirstOrDefault(x => x.UrlTitle == title);
            //var post = category?.Posts.FirstOrDefault(x => x.Id == id);
            //if (category == null || post == null)
            //{
            //    return HttpNotFound();
            //}


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
