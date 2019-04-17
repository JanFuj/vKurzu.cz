using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWebAppCoolName.Models
{
    public class Svg
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Zadejte název")]
        [Display(Name = "Název")]
        public string Name { get; set; }
        [Display(Name = "Svg (<Path>...</Path>)")]
        [Required(ErrorMessage = "Zadejte svg (pouze <Path>...</Path>)")]
        [AllowHtml]
        public string Path { get; set; }

    }
}