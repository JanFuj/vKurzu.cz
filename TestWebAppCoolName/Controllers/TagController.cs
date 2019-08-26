using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebAppCoolName.DAL;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers
{
    public class TagController : Controller
    {
        private TagRepository _repo;

        public TagController()
        {
            _repo = new TagRepository(new ApplicationDbContext());
        }

        protected override void Dispose(bool disposing)
        {
            _repo.Dispose();
            base.Dispose(disposing);
        }

        // GET: Tag
        [HttpGet]
        [Route("tag/{tagName}")]
        public ActionResult Index(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return HttpNotFound();
            }

            var articles = _repo.GetArticlesByTagName(tagName);
            if (articles == null)
            {
                return HttpNotFound();
            }
            return View(articles);
        }
    }
}