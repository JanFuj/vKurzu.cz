using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestWebAppCoolName.Models
{
    public class Person
    {
        public int Id { get; set; }
        [DisplayName("Jméno")]
        [Required(ErrorMessage = "Zadejte jméno")]
        public string Name { get; set; }
        [DisplayName("Přijmení")]
        [Required(ErrorMessage = "Zadejte přijmení")]
        public string LastName { get; set; }
        [DisplayName("Celé jméno")]
        public string FullName { get; set; }

    }
}