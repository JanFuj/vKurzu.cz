using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.DAL
{
    public class TagRepository : IDisposable
    {
        private ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Blog> GetArticlesByTagName(string tagName)
        {
            var tag = _context.Tags.Include(a => a.Blogs).FirstOrDefault(x => x.Name == tagName);
            return tag?.Blogs.Where(b => !b.Deleted && b.Approved).ToList();
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}