using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebAppCoolName.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Net;
using Microsoft.AspNet.Identity;
using TestWebAppCoolName.DAL;
using TestWebAppCoolName.Helpers;
using TestWebAppCoolName.Models.Dto;

namespace TestWebAppCoolName.Controllers.Admin
{
    public class TutorialCategoryViewModel
    {
        public TutorialCategory TutorialCategory { get; set; }
        public List<TutorialPost> TutorialPosts { get; set; }
        public List<Person> Persons { get; set; }
        public TutorialPost TutorialPost { get; set; }
        public string Tagy { get; set; }
        [Display(Name = "Obrázek pro sdílení na sociálech")]
        public HttpPostedFileBase Thumbnail { get; set; }
    }


    //Detail kategorie - seznam clanku tvorba novych
    [Authorize(Roles = Roles.Admin + "," + Roles.Lector)]
    public class TutorialCategoryController : Controller
    {
        private readonly ITutorialCategoryPostsRepository _repo;

        public TutorialCategoryController()
        {
            _repo = new TutorialCategoryPostsRepository(new ApplicationDbContext());

        }

        //show posts in category
        // GET: TutorialCategory
        [HttpGet]
        [Route("admin/tutorialCategory/{title?}")]
        public ActionResult Index(string title)
        {
            var viewModel = new TutorialCategoryViewModel();
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


        // GET: new post in category
        [HttpGet]
        [Route("admin/tutorialCategory/{title?}/new")]
        public ActionResult NewPost()
        {
            // var persons = _context.Persons.ToList();
            var viewModel = new TutorialCategoryViewModel()
            {
                TutorialPost = new TutorialPost(),
            };
            return View("NewPost", viewModel);
        }

        // POST: new post in category
        [HttpPost]
        [Route("admin/tutorialCategory/{title?}/new")]
        public ActionResult Create(TutorialCategoryViewModel vm, string title)
        {

            var tags = _repo.ParseTags(vm.Tagy);// TagParser.ParseTags(vm.Tagy, _context); // ParseTags(vm.Tagy);
            var persons = _repo.GetPeople();
            var viewModel = new TutorialCategoryViewModel()
            {
                Persons = persons
            };
            if (!ModelState.IsValid)
            {
                viewModel.TutorialPost = vm.TutorialPost;
                viewModel.TutorialPost.Tags = tags;
                return View("NewPost", viewModel);
            }

            var exist = _repo.GetPostByUrl(title, vm.TutorialPost.UrlTitle);
            if (exist != null)
            {
                ModelState.AddModelError("tutorialPost.UrlTitle", "Zadany url titulek již existuje");
                viewModel.TutorialPost = vm.TutorialPost;
                viewModel.TutorialPost.Tags = tags;
                return View("NewPost", viewModel);
            }
            vm.TutorialPost.OwnerId = User.Identity.GetUserId();
            vm.TutorialPost.Created = DateTime.Now;
            vm.TutorialPost.Changed = DateTime.Now;
            vm.TutorialPost.Tags = tags;
            _repo.AddPostInCategory(title, vm.TutorialPost);
            _repo.Save();
            return RedirectToAction("Index", new { title = title });

        }


        // GET: TutorialCategory/Edit/5
        [HttpGet]
        [Route("admin/tutorialCategory/{title}/edit/{id}")]
        public ActionResult Edit(string title, int id)
        {
            if (title == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = _repo.GetTutorialCategory(title);
            if (category == null)
            {
                return HttpNotFound();
            }

            var post = _repo.GetPostById(title, id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var viewModel = new TutorialCategoryViewModel()
            {

                Persons = _repo.GetPeople(),
                TutorialPost = post,

            };

            return View("EditPost", viewModel);
        }

        // POST: TutorialCategory/Edit/5
        [HttpPost]
        [Route("admin/tutorialCategory/{title}/edit/{id}")]
        public ActionResult Edit(TutorialCategoryViewModel vm, string title)
        {
            var category = _repo.GetTutorialCategory(title);
            if (category == null)
            {
                return HttpNotFound();
            }

            var tags = _repo.ParseTags(vm.Tagy);
            var persons = _repo.GetPeople();
            var viewModel = new TutorialCategoryViewModel();
            viewModel.Persons = persons;
            viewModel.TutorialPost = vm.TutorialPost;
            viewModel.TutorialPost.Tags = tags;
            if (!ModelState.IsValid)
            {
                return View("EditPost", viewModel);
            }
            //muze editovat pouze pokud stejny url title neexistuje u jineho clanku
            var existingPost = _repo.GetPostByUrl(title, vm.TutorialPost.UrlTitle);
            var postFromDb = _repo.GetPostById(title, vm.TutorialPost.Id);
            bool sameUrlInAnotherPost = false;
            if (existingPost != null)
            {
                sameUrlInAnotherPost = existingPost.Id != vm.TutorialPost.Id;
            }

            if (sameUrlInAnotherPost)
            {
                ModelState.AddModelError("tutorialPost.UrlTitle", "Zadany url titulek již existuje");
                return View("EditPost", viewModel);
            }

            try
            {

                if (postFromDb != null)
                {
                    if (User.IsInRole(Roles.Lector) && postFromDb.OwnerId != User.Identity.GetUserId())
                    {
                        return HttpNotFound();
                    }

                    postFromDb.Tags = null;
                    _repo.Save();
                    postFromDb.Name = vm.TutorialPost.Name;
                    postFromDb.Description = vm.TutorialPost.Description;
                    postFromDb.Body = vm.TutorialPost.Body;
                    postFromDb.RelatedCourseId = vm.TutorialPost.RelatedCourseId;
                    postFromDb.UrlTitle = vm.TutorialPost.UrlTitle;
                    postFromDb.HeaderImage = vm.TutorialPost.HeaderImage;
                    postFromDb.SocialSharingImage = vm.TutorialPost.SocialSharingImage;
                    postFromDb.Tags = tags;
                    postFromDb.Changed = DateTime.Now;
                    _repo.Save();
                    return RedirectToAction("Index", new { title = title });
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception e)
            {
                return HttpNotFound();
            }

        }
        [HttpGet]
        [Route("admin/tutorialCategory/{title}/delete/{id}")]
        public ActionResult Delete(string title, int id)
        {
            if (title == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = _repo.GetTutorialCategory(title);
            if (category == null)
            {
                return HttpNotFound();
            }

            var post = _repo.GetPostById(title, id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var viewModel = new TutorialCategoryViewModel()
            {

                TutorialPost = post,
                TutorialCategory = category,

            };
            return View("DeletePost", viewModel);
        }

        [Route("admin/tutorialCategory/{title}/delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(string title, int id)
        {
            var post = _repo.GetPostById(title, id);
            if (post == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector) && post.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            post.Deleted = true;
            _repo.Save();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("admin/tutorialCategory/{title}/ApproveTutorialPost/{id}")]
        public ActionResult ApproveTutorialPost(string title, int id, bool approve)
        {
            try
            {
                var post = _repo.GetPostById(title, id);
                if (post == null)
                {
                    return HttpNotFound();
                }

                post.Approved = approve;
                _repo.Save();
                return RedirectToAction("Index", new { title = title });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public HttpStatusCode UpdatePostOrder(List<OrderUpdateDto> orderDto, string title)
        {
            foreach (var item in orderDto)
            {
                var post = _repo.GetPostById(title, item.Id);
                if (post != null)
                {
                    post.Position = item.Position;
                }

                _repo.Save();
            }

            return HttpStatusCode.OK;
        }

    }
}
