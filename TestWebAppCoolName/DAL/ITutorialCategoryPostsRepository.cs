using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.DAL
{
    interface ITutorialCategoryPostsRepository {
        TutorialCategory GetTutorialCategory(string title);
        List<TutorialPost> GetPosts(string tutorialCategoryTitle);
        List<TutorialPost> GetPostsByOwner(string tutorialCategoryTitle, string ownerId);
        TutorialPost GetPostById(int postId);
        void NewTutorialPost(TutorialPost tutorialPost);
        void DeleteTutorialPost(int postId);
        void UpdateTutorialPost(TutorialPost tutorialPost);
        void Save();
        void Dispose();
        List<Person> GetPeople();
    }
}
