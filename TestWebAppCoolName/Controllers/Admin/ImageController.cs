using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using TestWebAppCoolName.DAL;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Controllers.Admin
{

    public class ImageViewModel
    {
        [Display(Name = "Vyber obrázek/y")]
        public HttpPostedFileBase[] Thumbnails { get; set; }

    }

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
        [Route("admin/image")]
        public ActionResult Index()
        {
            return View(_repo.GetImages());
        }
        [HttpGet]
        [Route("admin/image/create")]
        public ActionResult Create()
        {
            return View(new ImageViewModel());
        }


        [Route("admin/image/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ImageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    foreach (var image in viewModel.Thumbnails)
                    {
                        _repo.SaveImage(image);
                        _repo.Save();
                    }

                   return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("","Something Failed");
                }
            }
            //failed    
            return View(viewModel);
        }



        [HttpGet]
        [Route("admin/image/edit/{id}")]
        public ActionResult Edit(int id)
        {
            var imageFile = _repo.GetImageById(id);
            if (imageFile == null)
            {
                HttpNotFound();
            }
            return View(imageFile);
        }

        [Route("admin/image/edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ImageFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(imageFile);
            }

            var imageFileFromDb = _repo.GetImageById(imageFile.Id);
            if (imageFileFromDb == null)
            {
                return HttpNotFound();
            }
            imageFileFromDb.Description = imageFile.Description;
            _repo.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("admin/image/delete/{id}")]
        public ActionResult Delete(int id)
        {
            var imageFile = _repo.GetImageById(id);
            if (imageFile == null)
            {
                HttpNotFound();
            }

            return View(imageFile);
        }
        [Route("admin/image/delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var imageFile = _repo.GetImageById(id);
            if (imageFile == null)
            {
                HttpNotFound();
            }

            try
            {
                _repo.DeleteImage(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Obrázek nemůže být smazán protože je přiřazen k nějakému článku");

            }
            //somehing failed
            return View(imageFile);
        }
        protected override void Dispose(bool disposing)
        {
            _repo.Dispose();
            base.Dispose(disposing);
        }
    }
}