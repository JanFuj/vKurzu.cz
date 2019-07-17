using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestWebAppCoolName.Models
{
    public class TutorialCategory : BaseModel
    {
      
        [Required(ErrorMessage = "Zadejte název tutoriálu")]
        [Display(Name = "Název")]
        public string Name { get; set; }
        [Display(Name = "Url")]
        [Required(ErrorMessage = "Zadejte url")]
        public string UrlTitle { get; set; }
        public List<Tag> Tags { get; set; }
      

    }
}