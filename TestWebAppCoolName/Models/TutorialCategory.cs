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
        [Required(ErrorMessage = "Zadejte popis kurzu")]
        [Display(Name = "Popis")]
        public string Description { get; set; }
        [Display(Name = "Url")]
        [Required(ErrorMessage = "Zadejte url")]
        public string UrlTitle { get; set; }
        [Display(Name = "Url obrazku v hlavnim lazoutu")]
        public string HeaderImage { get; set; }
        [Display(Name = "Url obrazku pro sdílení na socíalech")]
        public string SocialSharingImage { get; set; }
        public List<Tag> Tags { get; set; }
        public List<TutorialPost> Posts { get; set; }

      
    }
}