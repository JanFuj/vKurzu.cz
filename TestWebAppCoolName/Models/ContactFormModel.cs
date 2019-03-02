using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestWebAppCoolName.Models
{
    public class ContactFormModel
    {


        [Required(ErrorMessage = "Zadejte email")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Nesprávný email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Co máte na srdci?")]
        [Display(Name = "Body")]
        public string Body { get; set; }
    }
}