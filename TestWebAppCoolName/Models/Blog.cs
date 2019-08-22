using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebAppCoolName.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Zadejte název blogu")]
        [Display(Name = "Název")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Zadejte popis blogu")]
        [Display(Name = "Popis")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Zadejte tělo blogu")]
        [Display(Name = "Tělo blogu")]
        [AllowHtml]
        public string Body { get; set; }

        //[Display(Name = "Autor")]
        //[Required(ErrorMessage = "Zadejte autora")]
        //[ForeignKey("Author")]
        //public int Author_Id { get; set; }
        //public Person Author { get; set; }
        [Display(Name = "Url titulek")]
        [Required(ErrorMessage = "Zadejte url titulek")]
        public string UrlTitle { get; set; }
        public bool Deleted { get; set; }
        public bool Approved { get; set; }
        public string OwnerId { get; set; }
        public int Position { get; set; }
        public List<Tag> Tags { get; set; } 
        public DateTime Created { get; set; }
        public DateTime Changed { get; set; }
    }
}