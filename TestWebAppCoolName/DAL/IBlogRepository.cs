using System.Collections.Generic;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.DAL
{
  public  interface IBlogRepository
    {
        List<Blog> GetBlogPosts();
        Blog GetBlogPostById(int blogId);
        void NewBlogPost(Blog blog);
        void DeleteBlogPost(int blogId);
        void UpdateBlogPost(Blog blog);
        void Save();
        void Dispose();

    }
}
