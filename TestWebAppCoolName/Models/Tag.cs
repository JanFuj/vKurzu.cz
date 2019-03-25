using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebAppCoolName.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
      public List<Blog> Blogs { get; set; }
        public List<Course> Courses { get; set; }
    }
}