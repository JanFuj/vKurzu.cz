using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestWebAppCoolName.Helpers;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.DAL
{

    public class TutorialCategoryPostsRepository : ITutorialCategoryPostsRepository, IDisposable
    {
        private ApplicationDbContext _context;
        private bool disposed = false;

        public TutorialCategoryPostsRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public TutorialCategory GetTutorialCategory(string title)
        {
            return _context.TutorialCategory.Include(x => x.Posts.Select(y => y.Tags)).Include(x => x.Tags).FirstOrDefault(x => x.UrlTitle == title);
        }

        public List<TutorialPost> GetPosts(string tutorialCategoryTitle)
        {
            var category = GetTutorialCategory(tutorialCategoryTitle);
            return category?.Posts.Where(x => !x.Deleted).OrderBy(c => c.Position).ToList();
        }
        public List<TutorialPost> GetPostsByOwner(string tutorialCategoryTitle, string ownerId)
        {
            var category = GetTutorialCategory(tutorialCategoryTitle);
            return category?.Posts.Where(x => !x.Deleted && x.OwnerId == ownerId).OrderBy(c => c.Position).ToList();
        }

        public TutorialPost GetPostById(string categoryTitle, int postId)
        {
            var category = GetTutorialCategory(categoryTitle);

            var post = category.Posts.FirstOrDefault(x => x.Id == postId);
            return category.Posts.FirstOrDefault(x => x.Id == postId);
        }


        public TutorialPost GetBlogPostById(int postId)
        {
            throw new NotImplementedException();
        }

        public void NewTutorialPost(TutorialPost tutorialPost)
        {
            throw new NotImplementedException();
        }

        public void DeleteTutorialPost(int postId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTutorialPost(TutorialPost tutorialPost)
        {

        }

        public void Save()
        {
            _context.SaveChanges();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public List<Person> GetPeople()
        {
            return _context.Persons.ToList();
        }

        public TutorialPost GetPostByUrl(string tutorialCategoryUrl, string tutorialPostUrlTitle)
        {
            var category = GetTutorialCategory(tutorialCategoryUrl);
            return category?.Posts.FirstOrDefault(x => x.UrlTitle == tutorialPostUrlTitle);
        }

        public void AddPostInCategory(string title, TutorialPost vmTutorialPost)
        {
            var category = GetTutorialCategory(title);
            category.Posts.Add(vmTutorialPost);

        }

        public List<Tag> ParseTags(string vmTagy)
        {
            return TagParser.ParseTags(vmTagy, _context);
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

        public Person GetAuthorById(int id)
        {
            return _context.Persons.FirstOrDefault(x => x.Id == id);
        }
    }
}