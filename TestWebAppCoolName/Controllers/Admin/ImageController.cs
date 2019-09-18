using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebAppCoolName.DAL;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers.Admin
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Lector)]
    public class ImageController : Controller
    {
        private ImageRepository _repo;

        public ImageController()
        {
            _repo = new ImageRepository(new ApplicationDbContext());
        }
        // GET: Image
        [HttpGet]
        [Route("admin/image/{title?}")]
        public ActionResult Index()
        {
            return View(_repo.GetImages());
        }

        protected override void Dispose(bool disposing)
        {
            _repo.Dispose();
            base.Dispose(disposing);
        }
    }
}