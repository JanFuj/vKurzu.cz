using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Helpers
{
    public  class DataService
    {
        private readonly ApplicationDbContext _context;

        public DataService()
        {
            _context = new ApplicationDbContext();
        }
        public  List<Blog> GetRelatedArticles(Blog blog)
        {
            //TODO some logic to pick up 3 related articles
            return _context.Blogs.Include(b=>b.Author).Where(b=>b.UrlTitle != blog.UrlTitle).Take(3).ToList();
        }
    }
}