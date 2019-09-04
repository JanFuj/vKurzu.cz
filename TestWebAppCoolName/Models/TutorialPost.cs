using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebAppCoolName.Models
{
    public class TutorialPost : BaseModel
    {

        [Required(ErrorMessage = "Zadejte název článku")]
        [Display(Name = "Název")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Zadejte popis článku")]
        [Display(Name = "Popis")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Zadejte tělo článku")]
        [Display(Name = "Tělo článku")]
        [AllowHtml]
        public string Body { get; set; }

        [Display(Name = "Obrázek pro sdílení na sociálech")]
        public ImageFile Thumbnail { get; set; }
        [Display(Name = "Url titulek")]
        [Required(ErrorMessage = "Zadejte url titulek")]
        public string UrlTitle { get; set; }
        public List<Tag> Tags { get; set; }
    }
}