using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestWebAppCoolName.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Zadejte název tagu bez mezer - MujNovyTag")]
        [Display(Name = "Tag zadejte ve formátu - MůjNovýTag")]
        public string Name { get; set; }
      public List<Blog> Blogs { get; set; }
        public List<Course> Courses { get; set; }
    }
}