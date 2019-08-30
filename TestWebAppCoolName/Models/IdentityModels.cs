using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestWebAppCoolName.Init;

namespace TestWebAppCoolName.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        [DisplayName("Jméno")]
        [Required(ErrorMessage = "Zadejte jméno")]
        public string FirstName { get; set; }
        [DisplayName("Přijmení")]
        [Required(ErrorMessage = "Zadejte přijmení")]
        public string LastName { get; set; }

        public string Fullname => FirstName + " " + LastName;
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Svg> Svgs { get; set; }
        public DbSet<AdminNote> AdminNotes { get; set; }
        public DbSet<TutorialCategory> TutorialCategory { get; set; }
        public DbSet<TutorialPost> TutorialPosts { get; set; }
        public DbSet<ImageFile> ImageFiles { get; set; }
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
            // Database.SetInitializer(new AppInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }




    }
}