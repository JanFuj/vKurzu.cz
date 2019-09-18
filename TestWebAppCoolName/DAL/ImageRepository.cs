using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.DAL
{
    public class ImageRepository : IDisposable
    {
        private ApplicationDbContext _context;
        private bool _disposed = false;

        public ImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ImageFile> GetImages()
        {
            return _context.ImageFiles.ToList();
        }

        public ImageFile GetImageById(int id)
        {
            return _context.ImageFiles.FirstOrDefault(i => i.Id == id);
        }

        public void DeleteImage(int id)
        {
            try
            {
                var imageFile = _context.ImageFiles.FirstOrDefault(i => i.Id == id);
                if (imageFile != null)
                {
                    string fullPath = HostingEnvironment.MapPath($"~/Content/Images/{imageFile.FileName}");
                    if (fullPath != null && System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    _context.ImageFiles.Remove(imageFile);
                    _context.SaveChanges();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Image associated with some article");
            }

        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public ImageFile GetImageFileByName(string fileName)
        {
            return _context.ImageFiles.FirstOrDefault(x => x.FileName == fileName);
        }
        public void SaveImage(HttpPostedFileBase image)
        {

            if (GetImageFileByName(image.FileName) == null)
            {
                //file not exist    
                var path = $"Content/Images/{image.FileName}";
                image.SaveAs(HostingEnvironment.MapPath("~/Content/Images/" + image.FileName) ?? throw new InvalidOperationException());
                var imageToSave = new ImageFile() { Path = path, FileName = image.FileName };
                _context.ImageFiles.Add(imageToSave);
            }
            else
            {
                //file already exists on server
                //  throw new Exception("file already exists on server");
            }
        }
    }
}