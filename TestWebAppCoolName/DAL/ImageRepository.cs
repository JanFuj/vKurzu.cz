using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}