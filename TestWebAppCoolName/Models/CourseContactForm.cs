using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestWebAppCoolName.Models
{
    public class CourseContactForm
    {
        [Required(ErrorMessage = "Zadejte jméno")]
        [Display(Name = "Jméno")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Zadejte přijmení")]
        [Display(Name = "Přijmení")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Zadejte email")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Nesprávný email")]
        public string Email { get; set; }
    }
}