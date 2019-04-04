using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebAppCoolName.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Zadejte název kurzu")]
        [Display(Name = "Název")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Zadejte popis kurzu")]
        [Display(Name = "Popis")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Zadejte co se v kurzu naučí")]
        [Display(Name = "Naučíte se - pouze odentrovat body")]
        public string WillLearn { get; set; }
        [Required(ErrorMessage = "Zadejte tělo kurzu")]
        [Display(Name = "Tělo kurzu - Mělo by obsahovat nadpisy - Proč se zůčastnit, Osnova, Kdy a kde")]
        public string Body { get; set; }

        [Display(Name = "Svg (<Path>...</Path>)")]
        [Required(ErrorMessage = "Zadejte svg (pouze <Path>...</Path>)")]
        [AllowHtml]
        public string Svg { get; set; }

        [Display(Name = "Modificator")]
        [Required(ErrorMessage = "Zadejte modifikator")]
        public string Modificator { get; set; }

        [Display(Name = "Lektor")]
        [Required(ErrorMessage = "Vyberte lektora")]
        [ForeignKey("Lector")]
        public int Lector_Id { get; set; }

        public Person Lector { get; set; }
        [Display(Name = "Url titulek")]
        [Required(ErrorMessage = "Zadejte url titulek")]
        public string UrlTitle { get; set; }
        public bool Deleted { get; set; }
        public bool Approved { get; set; }
        public List<Tag> Tags { get; set; }
        public DateTime Created { get; set; }
        public DateTime Changed { get; set; }
    }
}