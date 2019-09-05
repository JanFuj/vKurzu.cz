using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Helpers
{
    public class DataService : IDisposable
    {
        private readonly ApplicationDbContext _context;

        public DataService()
        {
            _context = new ApplicationDbContext();
        }
        public List<Blog> GetRelatedArticles(Blog blog)
        {
            //TODO some logic to pick up 3 related articles
            return _context.Blogs.Where(b => b.UrlTitle != blog.UrlTitle && b.Approved && !b.Deleted).Take(3).ToList();
        }
        public Tuple<TutorialPost, TutorialPost> GetRelatedArticles(TutorialPost tutorialPost)
        {
            //  tutorialPost
            var allCategoryPosts = tutorialPost.Category.Posts;
            var postBefore = allCategoryPosts.FirstOrDefault(p =>!p.Deleted && p.Position == (tutorialPost.Position - 1));
            var postAfter = allCategoryPosts.FirstOrDefault(p => !p.Deleted && p.Position == (tutorialPost.Position + 1));
            return new Tuple<TutorialPost, TutorialPost>(postBefore, postAfter);
        }
        public List<Tag> GetTags()
        {
            return _context.Tags.ToList();
        }

        public Course GetRelatedCourse(Blog blog)
        {

            return _context.Courses.Include(x => x.Svg)
                .FirstOrDefault(c => !c.Deleted && c.Approved && c.Id == blog.RelatedCourseId);
        }

        public List<Course> GetAllCourses()
        {
            return _context.Courses.Where(c => !c.Deleted).ToList();
        }


        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}