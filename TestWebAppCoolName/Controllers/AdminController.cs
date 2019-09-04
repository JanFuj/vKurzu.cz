using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using TestWebAppCoolName.Controllers.Admin;
using TestWebAppCoolName.Helpers;
using TestWebAppCoolName.Models;
using TestWebAppCoolName.Models.Dto;

namespace TestWebAppCoolName.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Lector)]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        // GET: Admin
        public ApplicationSignInManager SignInManager {
            get {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set {
                _userManager = value;
            }
        }
        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            Models.AdminNote adminNote = null;
            try
            {

                adminNote = _context.AdminNotes.First();
                _context.SaveChanges();
            }
            catch (Exception e)
            {

                _context.AdminNotes.Add(new AdminNote() { Note = "" });
                _context.SaveChanges();
            }

            return View(adminNote);
        }
        [HttpPost]
        public ActionResult AdminNote(AdminNote adminNote)
        {
            var aNote = _context.AdminNotes.First();
            aNote.Note = adminNote.Note;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        #region Kurz
        public ActionResult ApproveCourse(int id, bool approve)
        {
            try
            {

                var course = _context.Courses.FirstOrDefault(c => c.Id == id);
                if (course == null)
                {
                    return HttpNotFound();
                }

                course.Approved = approve;
                _context.SaveChanges();
                return RedirectToAction("Course");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [Route("admin/kurz")]
        public ActionResult Course()
        {
            var userId = User.Identity.GetUserId();
            var courses = _context.Courses.Where(c => !c.Deleted).ToList();
            if (User.IsInRole(Roles.Lector))
            {
                courses = _context.Courses.Where(c => !c.Deleted && c.OwnerId == userId).ToList();
            }

            return View(courses.OrderBy(c => c.Position));
        }

        [Route("admin/kurz/new")]
        public ActionResult NewCourse()
        {
            var newCourse = new Course();
            newCourse.Svg = _context.Svgs.First();
            newCourse.Svg_id = newCourse.Svg.ID;
            var viewModel = new CourseViewModel()
            {
                Svgs = _context.Svgs.ToList(),
                Course = newCourse

            };
            return View(viewModel);
        }

        [Route("admin/kurz/new")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCourse(CourseViewModel vm)
        {
            var tags = TagParser.ParseTags(vm.Tagy, _context);
            var viewModel = new CourseViewModel();

            if (!ModelState.IsValid)
            {
                viewModel.Course = vm.Course;
                viewModel.Course.Tags = tags;
                viewModel.Svgs = _context.Svgs.ToList();
                return View(viewModel);
            }

            var exist = _context.Courses.FirstOrDefault(c => c.UrlTitle == vm.Course.UrlTitle);

            if (exist != null)
            {
                ModelState.AddModelError("course.UrlTitle", "Zadany url titulek již existuje");
                viewModel.Course = vm.Course;
                viewModel.Course.Tags = tags;
                viewModel.Svgs = _context.Svgs.ToList();
                return View(viewModel);
            }


            if (vm.Thumbnail != null)
            {
                var path = $"Content/Images/{vm.Thumbnail.FileName}";
                var existingImage =
                    _context.ImageFiles.FirstOrDefault(x => x.Path == path);
                if (existingImage == null)
                {
                    vm.Thumbnail.SaveAs(Server.MapPath("~/Content/Images/" + vm.Thumbnail.FileName));
                    var thumbnail = new ImageFile() { Path = path, FileName = vm.Thumbnail.FileName };
                    _context.ImageFiles.Add(thumbnail);
                    _context.SaveChanges();
                    vm.Course.Thumbnail = thumbnail;
                }
                else
                {
                    vm.Course.Thumbnail = existingImage;
                }
            }

            vm.Course.OwnerId = User.Identity.GetUserId();
            vm.Course.Created = DateTime.Now;
            vm.Course.Changed = DateTime.Now;
            vm.Course.Tags = tags;
            vm.Course.Svg = _context.Svgs.First(s => s.ID == vm.Course.Svg_id);
            _context.Courses.Add(vm.Course);
            _context.SaveChanges();

            return RedirectToAction("Course");
        }

        //GET 
        [Route("admin/kurz/edit/{id?}")]
        public ActionResult EditCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = _context.Courses.Include(b => b.Tags).Include(c => c.Thumbnail).Include(s => s.Svg).FirstOrDefault(b => b.Id == id);

            if (course == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector) && course.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var viewModel = new CourseViewModel()
            {
                Course = course,
                Svgs = _context.Svgs.ToList()
            };
            return View(viewModel);
        }

        [Route("admin/kurz/edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse(CourseViewModel vm)
        {

            var tags = TagParser.ParseTags(vm.Tagy, _context);
            if (!ModelState.IsValid)
            {

                var viewModel = new CourseViewModel()
                {
                    Course = vm.Course
                };

                return View(viewModel);
            }

            var existingCourse = _context.Courses.FirstOrDefault(c => c.UrlTitle == vm.Course.UrlTitle);
            bool sameUrlInAnotherCourse = false;
            if (existingCourse != null)
            {
                sameUrlInAnotherCourse = existingCourse?.Id != vm.Course.Id;
            }

            if (sameUrlInAnotherCourse)
            {
                ModelState.AddModelError("course.UrlTitle", "Zadany url titulek již existuje");
                var viewModel = new CourseViewModel()
                {
                    Course = vm.Course
                };
                return View(viewModel);
            }

            var cour = _context.Courses.Include(c => c.Tags).Include(c => c.Thumbnail).FirstOrDefault(c => c.Id == vm.Course.Id);
            if (User.IsInRole(Roles.Lector) && cour?.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (cour != null)
            {

                if (vm.Thumbnail != null)
                {
                    var path = $"Content/Images/{vm.Thumbnail.FileName}";
                    var existingImage =
                        _context.ImageFiles.FirstOrDefault(x => x.Path == path);
                    if (existingImage == null)
                    {
                        vm.Thumbnail.SaveAs(Server.MapPath("~/Content/Images/" + vm.Thumbnail.FileName));
                        var thumbnail = new ImageFile() { Path = path };
                        _context.ImageFiles.Add(thumbnail);
                        _context.SaveChanges();
                        cour.Thumbnail = thumbnail;
                    }
                    else
                    {
                        cour.Thumbnail = existingImage;
                    }
                }

                cour.Name = vm.Course.Name;
                cour.Description = vm.Course.Description;
                cour.WillLearn = vm.Course.WillLearn;
                cour.Body = vm.Course.Body;
                cour.Modificator = vm.Course.Modificator;
                cour.Svg = vm.Course.Svg;
                cour.UrlTitle = vm.Course.UrlTitle;
                cour.Tags = tags;
                cour.Changed = DateTime.Now;
                cour.Svg = _context.Svgs.First(s => s.ID == vm.Course.Svg_id);
                _context.SaveChanges();
                return RedirectToAction("Course");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [Route("admin/kurz/delete/{id?}")]
        public ActionResult DeleteCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = _context.Courses.FirstOrDefault(b => b.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector) && course.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(course);
        }

        [Route("admin/kurz/delete/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector) && course.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            //_context.Courses.Remove(course ?? throw new InvalidOperationException());
            course.Deleted = true;
            _context.SaveChanges();
            return RedirectToAction("Course");
        }

        [HttpPost]
        public HttpStatusCode UpdateCourseOrder(List<OrderUpdateDto> orderDto)
        {
            foreach (var item in orderDto)
            {
                var course = _context.Courses.FirstOrDefault(x => x.Id == item.Id);
                if (course != null)
                {
                    course.Position = item.Position;
                }

                _context.SaveChanges();
            }

            return HttpStatusCode.OK;
        }
        #endregion

        #region Blog

        public ActionResult ApproveBlog(int id, bool approve)
        {
            try
            {

                var blog = _context.Blogs.FirstOrDefault(c => c.Id == id);
                if (blog == null)
                {
                    return HttpNotFound();
                }

                blog.Approved = approve;
                _context.SaveChanges();
                return RedirectToAction("Blog");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Route("admin/blog")]
        public ActionResult Blog()

        {
            var userId = User.Identity.GetUserId();
            var blogs = _context.Blogs.Where(b => !b.Deleted).ToList();
            if (User.IsInRole(Roles.Lector))
            {
                blogs = _context.Blogs.Where(b => !b.Deleted && b.OwnerId == userId).ToList();
            }

            return View(blogs);
        }

        [Route("admin/blog/new")]
        public ActionResult NewBlog()
        {
            var viewModel = new BlogViewModel()
            {
                Blog = new Blog(),

            };
            return View(viewModel);
        }

        [Route("admin/blog/new")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewBlog(BlogViewModel vm)
        {

            var tags = TagParser.ParseTags(vm.Tagy, _context); // ParseTags(vm.Tagy);
            var viewModel = new BlogViewModel();


            if (!ModelState.IsValid)
            {
                viewModel.Blog = vm.Blog;
                viewModel.Blog.Tags = tags;
                return View(viewModel);
            }

            var exist = _context.Blogs.FirstOrDefault(b => b.UrlTitle == vm.Blog.UrlTitle);
            if (exist != null)
            {
                ModelState.AddModelError("blog.UrlTitle", "Zadany url titulek již existuje");
                viewModel.Blog = vm.Blog;
                viewModel.Blog.Tags = tags;
                return View(viewModel);
            }

            if (vm.Thumbnail != null)
            {
                var path = $"Content/Images/{vm.Thumbnail.FileName}";
                var existingImage =
                    _context.ImageFiles.FirstOrDefault(x => x.Path == path);
                if (existingImage == null)
                {
                    vm.Thumbnail.SaveAs(Server.MapPath("~/Content/Images/" + vm.Thumbnail.FileName));
                    var thumbnail = new ImageFile() { Path = path, FileName = vm.Thumbnail.FileName };
                    _context.ImageFiles.Add(thumbnail);
                    _context.SaveChanges();
                    vm.Blog.Thumbnail = thumbnail;
                }
                else
                {
                    vm.Blog.Thumbnail = existingImage;
                }
            }
            vm.Blog.OwnerId = User.Identity.GetUserId();
            vm.Blog.Created = DateTime.Now;
            vm.Blog.Changed = DateTime.Now;
            vm.Blog.Tags = tags;
            _context.Blogs.Add(vm.Blog);
            _context.SaveChanges();
            ViewData["Saved"] = "Blog byl vyrvořen";
            ModelState.Clear();
            return RedirectToAction("Blog");

        }
        [Route("admin/blog/edit/{id?}")]
        public ActionResult EditBlog(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = _context.Blogs.Include(b => b.Tags).Include(b => b.Thumbnail).FirstOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(Roles.Lector) && blog.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            var viewModel = new BlogViewModel()
            {
                Blog = blog,
            };
            return View(viewModel);
        }

        // POST: Blog/Edit/5
        [Route("admin/blog/edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBlog(BlogViewModel vm)
        {
            var tags = TagParser.ParseTags(vm.Tagy, _context); //ParseTags(vm.Tagy);
            if (!ModelState.IsValid)
            {
                var viewModel = new BlogViewModel()
                {
                    Blog = vm.Blog

                };
                return View(viewModel);
            }

            //muze editovat pouze pokud stejny url title neexistuje u jineho blogu
            var existingBlog = _context.Blogs.FirstOrDefault(b => b.UrlTitle == vm.Blog.UrlTitle);
            bool sameUrlInAnotherBlog = false;
            if (existingBlog != null)
            {
                sameUrlInAnotherBlog = existingBlog.Id != vm.Blog.Id;
            }

            if (sameUrlInAnotherBlog)
            {
                ModelState.AddModelError("blog.UrlTitle", "Zadany url titulek již existuje");
                var viewModel = new BlogViewModel()
                {
                    Blog = vm.Blog

                };
                return View(viewModel);
            }

            var blo = _context.Blogs.Include(b => b.Tags).FirstOrDefault(b => b.Id == vm.Blog.Id);
            if (blo != null)
            {
                if (User.IsInRole(Roles.Lector) && blo.OwnerId != User.Identity.GetUserId())
                {
                    return HttpNotFound();
                }

                blo.Tags = null;
                _context.SaveChanges();
                if (vm.Thumbnail != null)
                {
                    var path = $"Content/Images/{vm.Thumbnail.FileName}";
                    var existingImage =
                        _context.ImageFiles.FirstOrDefault(x => x.Path == path);
                    if (existingImage == null)
                    {
                        vm.Thumbnail.SaveAs(Server.MapPath("~/Content/Images/" + vm.Thumbnail.FileName));
                        var thumbnail = new ImageFile() { Path = path };
                        _context.ImageFiles.Add(thumbnail);
                        _context.SaveChanges();
                        blo.Thumbnail = thumbnail;
                    }
                    else
                    {
                        blo.Thumbnail = existingImage;
                    }
                }

                blo.Name = vm.Blog.Name;
                blo.Description = vm.Blog.Description;
                blo.Body = vm.Blog.Body;
                blo.UrlTitle = vm.Blog.UrlTitle;
                blo.Tags = tags;
                blo.RelatedCourseId = vm.Blog.RelatedCourseId;
                blo.Changed = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Blog");
            }
            else
            {
                return HttpNotFound();
            }
        }
        [Route("admin/blog/delete/{id?}")]
        public ActionResult DeleteBlog(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector) && blog.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            return View(blog);
        }


        // POST: Blog/Delete/5
        [Route("admin/blog/delete/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBlogConfirmed(int id)
        {
            var blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector) && blog.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            //  _context.Blogs.Remove(blog);
            blog.Deleted = true;
            _context.SaveChanges();
            return RedirectToAction("Blog");
        }

        #endregion

        #region Tag

        [Route("admin/tag")]
        public async Task<ActionResult> Tag()
        {
            return View(await _context.Tags.ToListAsync());
        }
        [Route("admin/tag/new")]
        public ActionResult NewTag()
        {
            return View();
        }
        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("admin/tag/new")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewTag([Bind(Include = "Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction("Tag");
            }

            return View(tag);
        }

        [Route("admin/tag/edit/{id?}")]
        public async Task<ActionResult> EditTag(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("admin/tag/edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditTag([Bind(Include = "Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(tag).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Tag");
            }
            return View(tag);
        }


        // GET: Tags/Delete/5
        [Route("admin/tag/delete/{id?}")]
        public async Task<ActionResult> DeleteTag(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Delete/5
        [Route("admin/tag/delete/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTagConfirmed(int id)
        {
            Tag tag = await _context.Tags.FindAsync(id);
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction("Tag");
        }
        #endregion




        #region Lector Registration
        [Authorize(Roles = Roles.Admin)]
        [Route("admin/lector")]
        public ActionResult Person()
        {
            var admins = new List<ApplicationUser>();
            var lectors = new List<ApplicationUser>();
            var users = new List<ApplicationUser>();
            var allUsers = new List<ApplicationUser>();
            foreach (var user in _context.Users)
            {
                if (UserManager.IsInRole(user.Id, Roles.Admin))
                {
                    admins.Add(user);
                }
                if (UserManager.IsInRole(user.Id, Roles.Lector))
                {
                    lectors.Add(user);
                }
                if (UserManager.IsInRole(user.Id, Roles.User))
                {
                    users.Add(user);
                }
            }
            allUsers.AddRange(admins);
            allUsers.AddRange(lectors);
            allUsers.AddRange(users);

            return View(allUsers);
        }

        [Authorize(Roles = Roles.Admin)]
        [Route("admin/lector/register")]
        public ActionResult Register()
        {
            return View();
        }
        [Authorize(Roles = Roles.Admin)]
        [Route("admin/lector/register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, EmailConfirmed = true };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, Roles.Lector);

                    return RedirectToAction("Person", "Admin");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [Route("admin/lector/edit/{id?}")]
        public async Task<ActionResult> EditPerson(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Roles.Admin)]
        [Route("admin/lector/edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPerson(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var dbUser = await UserManager.FindByIdAsync(user.Id);
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                IdentityResult result = await UserManager.UpdateAsync(dbUser);
                if (result.Succeeded)
                    return RedirectToAction("Person");

                else
                {
                    return HttpNotFound();
                }
            }
            return View(user);
        }

        [Authorize(Roles = Roles.Admin)]
        [Route("admin/lector/delete")]
        public async Task<ActionResult> DeletePerson(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: People/Delete/5
        [Authorize(Roles = Roles.Admin)]
        [Route("admin/lector/delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePersonConfirmed(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Person");
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return RedirectToAction("Person");
        }

        #endregion
        #region People





        #endregion

        #region Svg
        [Route("admin/svg")]
        public ActionResult Svg()
        {
            return View(_context.Svgs.ToList());
        }
        [Route("admin/svg/new")]
        public ActionResult NewSvg()
        {
            return View();
        }
        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("admin/svg/new")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewSvg([Bind(Include = "Name,Path")] Svg svg)
        {

            if (!ModelState.IsValid)
            {
                return View(svg);
            }
            var exist = _context.Svgs.FirstOrDefault(c => c.Name == svg.Name);

            if (exist != null)
            {
                ModelState.AddModelError("svg.Name", "Zadany nazev již existuje");
                return View(svg);
            }
            _context.Svgs.Add(svg);
            _context.SaveChanges();
            return RedirectToAction("Svg");

        }

        [Route("admin/svg/edit/{id?}")]
        public ActionResult EditSvg(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var svg = _context.Svgs.FirstOrDefault(s => s.ID == id);

            if (svg == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector))
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(svg);
        }
        [Route("admin/svg/edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSvg(Svg svg)
        {

            if (!ModelState.IsValid)
            {
                return View(svg);
            }

            var existingSvg = _context.Svgs.FirstOrDefault(c => c.Name == svg.Name);
            bool sameUrlInAnotherCourse = false;
            if (existingSvg != null)
            {
                sameUrlInAnotherCourse = existingSvg?.ID != svg.ID;
            }

            if (sameUrlInAnotherCourse)
            {
                ModelState.AddModelError("svg.Name", "Zadany nazev již existuje");
                return View(svg);
            }

            var sv = _context.Svgs.FirstOrDefault(s => s.ID == svg.ID);

            if (sv != null)
            {
                sv.Name = svg.Name;
                sv.Path = svg.Path;
                _context.SaveChanges();
                return RedirectToAction("Svg");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [Route("admin/svg/delete/{id?}")]
        public ActionResult DeleteSvg(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var svg = _context.Svgs.FirstOrDefault(b => b.ID == id);
            if (svg == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector))
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(svg);
        }

        // POST: Blog/Delete/5
        [Route("admin/svg/delete/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSvgConfirmed(int id)
        {
            var svg = _context.Svgs.FirstOrDefault(b => b.ID == id);
            if (svg == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector))
            {
                return HttpNotFound();
            }
            //  _context.Blogs.Remove(blog);
            _context.Svgs.Remove(svg);
            _context.SaveChanges();
            return RedirectToAction("Svg");
        }

        #endregion


        #region TutorialCategories
        [Route("admin/tutorialCategories")]
        public ActionResult TutorialCategories()
        {
            var userId = User.Identity.GetUserId();
            var categories = _context.TutorialCategory.Where(c => !c.Deleted).ToList();
            if (User.IsInRole(Roles.Lector))
            {
                categories = _context.TutorialCategory.Where(c => !c.Deleted && c.OwnerId == userId).ToList();
            }
            return View("TutorialCategories", categories.OrderBy(c => c.Position));
        }
        [Route("admin/tutorialCategories/new")]
        public ActionResult NewTutorialCategory()
        {
            var viewModel = new TutorialCategoryViewModel()
            {
                TutorialCategory = new TutorialCategory(),
            };
            return View(viewModel);
        }
        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("admin/tutorialCategories/new")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewTutorialCategory(TutorialCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = _context.TutorialCategory.FirstOrDefault(b => b.UrlTitle == vm.TutorialCategory.UrlTitle);
                if (existingCategory != null)
                {
                    ModelState.AddModelError("tutorialCategory.UrlTitle", "Zadany url titulek již existuje");
                    return View(vm);
                }

                if (vm.Thumbnail != null)
                {
                    var path = $"Content/Images/{vm.Thumbnail.FileName}";
                    var existingImage =
                        _context.ImageFiles.FirstOrDefault(x => x.Path == path);
                    if (existingImage == null)
                    {
                        vm.Thumbnail.SaveAs(Server.MapPath("~/Content/Images/" + vm.Thumbnail.FileName));
                        var thumbnail = new ImageFile() { Path = path };
                        _context.ImageFiles.Add(thumbnail);
                        _context.SaveChanges();
                        vm.TutorialCategory.Thumbnail = thumbnail;
                    }
                    else
                    {
                        vm.TutorialCategory.Thumbnail = existingImage;
                    }
                }


                vm.TutorialCategory.Created = DateTime.Now;
                vm.TutorialCategory.Changed = DateTime.Now;
                vm.TutorialCategory.OwnerId = User.Identity.GetUserId();
                _context.TutorialCategory.Add(vm.TutorialCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction("TutorialCategories");
            }

            return View(vm);
        }


        [Route("admin/tutorialCategory/edit/{id?}")]
        public ActionResult EditTutorialCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tutorialCategoryFromDb = _context.TutorialCategory.Include(b => b.Tags).Include(c => c.Thumbnail).FirstOrDefault(b => b.Id == id);
            if (tutorialCategoryFromDb == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(Roles.Lector) && tutorialCategoryFromDb.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            var viewModel = new TutorialCategoryViewModel()
            {
                TutorialCategory = tutorialCategoryFromDb
            };

            return View(viewModel);
        }

        [Route("admin/tutorialCategory/edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTutorialCategory(TutorialCategoryViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var existingTutorialCategory = _context.TutorialCategory.FirstOrDefault(c => c.UrlTitle == vm.TutorialCategory.UrlTitle);
            bool sameUrlInAnotherTutorialCategory = false;
            if (existingTutorialCategory != null)
            {
                sameUrlInAnotherTutorialCategory = existingTutorialCategory?.Id != vm.TutorialCategory.Id;
            }

            if (sameUrlInAnotherTutorialCategory)
            {
                ModelState.AddModelError("UrlTitle", "Zadany url titulek již existuje");

                return View(vm);
            }

            var tutor = _context.TutorialCategory.Include(c => c.Tags).Include(c=>c.Thumbnail).FirstOrDefault(c => c.Id == vm.TutorialCategory.Id);
            if (User.IsInRole(Roles.Lector) && tutor?.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (tutor != null)
            {
                if (vm.Thumbnail != null)
                {
                    var path = $"Content/Images/{vm.Thumbnail.FileName}";
                    var existingImage =
                        _context.ImageFiles.FirstOrDefault(x => x.Path == path);
                    if (existingImage == null)
                    {
                        vm.Thumbnail.SaveAs(Server.MapPath("~/Content/Images/" + vm.Thumbnail.FileName));
                        var thumbnail = new ImageFile() { Path = path };
                        _context.ImageFiles.Add(thumbnail);
                        _context.SaveChanges();
                        tutor.Thumbnail = thumbnail;
                    }
                    else
                    {
                        tutor.Thumbnail = existingImage;
                    }
                }

                tutor.Name = vm.TutorialCategory.Name;
                tutor.Description = vm.TutorialCategory.Description;
                tutor.UrlTitle = vm.TutorialCategory.UrlTitle;
                tutor.Changed = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("TutorialCategories");
            }
            else
            {
                return HttpNotFound();
            }
        }


        [Route("admin/tutorialCategory/delete/{id?}")]
        public ActionResult DeleteTutorialCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tutorialCategory = _context.TutorialCategory.FirstOrDefault(b => b.Id == id);
            if (tutorialCategory == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector) && tutorialCategory.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(tutorialCategory);
        }

        [Route("admin/tutorialCategory/delete/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTutorialCategory(int id)
        {
            var tutorialCategory = _context.TutorialCategory.FirstOrDefault(c => c.Id == id);
            if (tutorialCategory == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector) && tutorialCategory.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            tutorialCategory.Deleted = true;
            tutorialCategory.Changed = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("TutorialCategories");
        }

        public ActionResult ApproveTutorialCategory(int id, bool approve)
        {
            try
            {

                var tutorialCategory = _context.TutorialCategory.FirstOrDefault(c => c.Id == id);
                if (tutorialCategory == null)
                {
                    return HttpNotFound();
                }

                tutorialCategory.Approved = approve;
                _context.SaveChanges();
                return RedirectToAction("TutorialCategories");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public HttpStatusCode UpdateCategoryOrder(List<OrderUpdateDto> orderDto)
        {
            foreach (var item in orderDto)
            {
                var tutorialCategory = _context.TutorialCategory.FirstOrDefault(c => c.Id == item.Id);
                if (tutorialCategory != null)
                {
                    tutorialCategory.Position = item.Position;
                }

                _context.SaveChanges();
            }

            return HttpStatusCode.OK;
        }

        #endregion








        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }
            base.Dispose(disposing);
        }



    }
}