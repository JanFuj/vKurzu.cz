using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Init
{
    public class AppInitializer : System.Data.Entity.CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context) {
        //    base.Seed(context);
            CreateRolesAndUsers(context);
        }

        private void CreateRolesAndUsers(ApplicationDbContext context) {
         
        }
    }
}