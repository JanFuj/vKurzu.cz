using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.DAL
{
    public class BlogRepository : IBlogRepository, IDisposable
    {
        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Blog> GetBlogPosts()
        {
            return _context.Blogs.Include(b => b.Author).Include(b => b.Tags).Where(b => b.Approved).OrderByDescending(x=>x.Created).ToList();
        }
        public List<Blog> GetFirst3BlogPosts()
        {
            return _context.Blogs.Include(b => b.Author).Include(b => b.Tags).Where(b => b.Approved).OrderByDescending(x => x.Created).Take(3).ToList();
        }

        public Blog GetBlogPostById(int blogId)
        {
            return _context.Blogs.Find(blogId);
        }

        public void NewBlogPost(Blog blog)
        {
            _context.Blogs.Add(blog);
        }

        public void DeleteBlogPost(int blogId)
        {
            var blogPost = _context.Blogs.Find(blogId);
            if (blogPost != null) _context.Blogs.Remove(blogPost);
        }

        public void UpdateBlogPost(Blog blog)
        {
            _context.Entry(blog).State = EntityState.Modified;
        }

        public void Save() {
            _context.SaveChanges();
        }
        private bool disposed = false;
        private ApplicationDbContext _context;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}