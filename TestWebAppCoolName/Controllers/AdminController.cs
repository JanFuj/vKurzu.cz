using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TestWebAppCoolName.Helpers;
using TestWebAppCoolName.Models;
using TestWebAppCoolName.Models.Dto;

namespace TestWebAppCoolName.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;

        // GET: Admin
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
            var persons = _context.Persons.ToList();
            var newCourse = new Course();
            newCourse.Svg = _context.Svgs.First();
            newCourse.Svg_id = newCourse.Svg.ID;
            var viewModel = new CourseViewModel()
            {
                Persons = persons,
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
            var persons = _context.Persons.ToList();
            var viewModel = new CourseViewModel()
            {
                Persons = persons,
            };

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

            var course = _context.Courses.Include(b => b.Tags).Include(s => s.Svg).FirstOrDefault(b => b.Id == id);

            if (course == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Lector) && course.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var persons = _context.Persons.ToList();
            var viewModel = new CourseViewModel()
            {
                Persons = persons,
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

                var persons = _context.Persons.ToList();
                var viewModel = new CourseViewModel()
                {
                    Persons = persons,
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
                var persons = _context.Persons.ToList();
                var viewModel = new CourseViewModel()
                {
                    Persons = persons,
                    Course = vm.Course
                };
                return View(viewModel);
            }

            var cour = _context.Courses.Include(c => c.Tags).FirstOrDefault(c => c.Id == vm.Course.Id);
            if (User.IsInRole(Roles.Lector) && cour?.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (cour != null)
            {
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
        public HttpStatusCode UpdateCourseOrder(List<CourseOrderUpdateDto> orderDto)
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
            var blogs = _context.Blogs.Include(b => b.Author).Where(b => !b.Deleted).ToList();
            if (User.IsInRole(Roles.Lector))
            {
                blogs = _context.Blogs.Include(b => b.Author).Where(b => !b.Deleted && b.OwnerId == userId).ToList();
            }

            return View(blogs);
        }

        [Route("admin/blog/new")]
        public ActionResult NewBlog()
        {
            var persons = _context.Persons.ToList();
            var viewModel = new BlogViewModel()
            {
                Persons = persons,
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
            var persons = _context.Persons.ToList();
            var viewModel = new BlogViewModel()
            {
                Persons = persons
            };
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

            Blog blog = _context.Blogs.Include(b => b.Author).Include(b => b.Tags).FirstOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(Roles.Lector) && blog.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            var persons = _context.Persons.ToList();
            var viewModel = new BlogViewModel()
            {
                Persons = persons,
                Blog = blog

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
                var persons = _context.Persons.ToList();
                var viewModel = new BlogViewModel()
                {
                    Persons = persons,
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
                var persons = _context.Persons.ToList();
                var viewModel = new BlogViewModel()
                {
                    Persons = persons,
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


                blo.Name = vm.Blog.Name;
                blo.Description = vm.Blog.Description;
                blo.Body = vm.Blog.Body;
                blo.Author_Id = vm.Blog.Author_Id;
                blo.UrlTitle = vm.Blog.UrlTitle;
                blo.Tags = tags;
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

        #region People
        [Route("admin/person")]
        public ActionResult Person()
        {
            return View(_context.Persons.ToList());
        }
        [Route("admin/person/new")]
        public ActionResult NewPerson()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("admin/person/new")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPerson([Bind(Include = "Name,LastName")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.FullName = person.Name + " " + person.LastName;
                _context.Persons.Add(person);
                _context.SaveChanges();
                return RedirectToAction("Person");
            }

            return View(person);
        }
        [Route("admin/person/edit/{id?}")]
        public ActionResult EditPerson(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = _context.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("admin/person/edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPerson([Bind(Include = "Id,Name,LastName")] Person person)
        {
            if (ModelState.IsValid)
            {
                // db.Entry(person).State = EntityState.Modified;
                var per = _context.Persons.FirstOrDefault(p => p.Id == person.Id);
                if (per != null)
                {
                    per.Name = person.Name;
                    per.LastName = person.LastName;
                    per.FullName = person.Name + " " + person.LastName;
                    _context.SaveChanges();
                    return RedirectToAction("Person");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            return View(person);
        }

        // GET: People/Delete/5
        [Route("admin/person/delete")]
        public ActionResult DeletePerson(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = _context.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [Route("admin/person/delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePersonConfirmed(int id)
        {
            Person person = _context.Persons.Find(id);
            _context.Persons.Remove(person);
            _context.SaveChanges();
            return RedirectToAction("Person");
        }

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


        #region Tutorials
        [Route("admin/tutorialCategory")]
        public ActionResult TutorialCategory()
        {
            var userId = User.Identity.GetUserId();
            var categories = _context.TutorialCategory.Where(c => !c.Deleted).ToList();
            if (User.IsInRole(Roles.Lector))
            {
                categories = _context.TutorialCategory.Where(c => !c.Deleted && c.OwnerId == userId).ToList();
            }
            return View("TutorialCategory",categories.OrderBy(c => c.Position));
        }
        [Route("admin/tutorialCategory/new")]
        public ActionResult NewTutorialCategory()
        {
            return View();
        }
        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("admin/tutorialCategory/new")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewTutorialCategory([Bind(Include = "Id,Name,UrlTitle")] TutorialCategory tutorialCategory)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = _context.TutorialCategory.FirstOrDefault(b => b.UrlTitle == tutorialCategory.UrlTitle);
                if (existingCategory != null)
                {
                    ModelState.AddModelError("UrlTitle", "Zadany url titulek již existuje");
                    return View(tutorialCategory);
                }
                tutorialCategory.Created = DateTime.Now;
                tutorialCategory.Changed = DateTime.Now;
                _context.TutorialCategory.Add(tutorialCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction("TutorialCategory");
            }

            return View(tutorialCategory);
        }


        [Route("admin/tutorialCategory/edit/{id?}")]
        public ActionResult EditTutorialCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tutorialCategory = _context.TutorialCategory.Include(b => b.Tags).FirstOrDefault(b => b.Id == id);
            if (tutorialCategory == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole(Roles.Lector) && tutorialCategory.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

        
            return View(tutorialCategory);
        }

        [Route("admin/tutorialCategory/edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTutorialCategory(TutorialCategory tutorialCategory)
        {

            if (!ModelState.IsValid)
            {
                return View(tutorialCategory);
            }

            var existingTutorialCategory = _context.TutorialCategory.FirstOrDefault(c => c.UrlTitle == tutorialCategory.UrlTitle);
            bool sameUrlInAnotherTutorialCategory = false;
            if (existingTutorialCategory != null)
            {
                sameUrlInAnotherTutorialCategory = existingTutorialCategory?.Id != tutorialCategory.Id;
            }

            if (sameUrlInAnotherTutorialCategory)
            {
                ModelState.AddModelError("UrlTitle", "Zadany url titulek již existuje");
                
                return View(tutorialCategory);
            }

            var tutor = _context.TutorialCategory.Include(c => c.Tags).FirstOrDefault(c => c.Id == tutorialCategory.Id);
            if (User.IsInRole(Roles.Lector) && tutor?.OwnerId != User.Identity.GetUserId())
            {
                return HttpNotFound();// return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (tutor != null)
            {
                tutor.Name = tutorialCategory.Name;
                tutor.UrlTitle = tutorialCategory.UrlTitle;
                tutor.Changed = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("TutorialCategory");
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
        [HttpPost, ActionName("Delete")]
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
            _context.SaveChanges();
            return RedirectToAction("TutorialCategory");
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
                return RedirectToAction("TutorialCategory");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }





        //[Route("admin/blog")]
        //public ActionResult Blog()

        //{
        //    var userId = User.Identity.GetUserId();
        //    var blogs = _context.Blogs.Include(b => b.Author).Where(b => !b.Deleted).ToList();
        //    if (User.IsInRole(Roles.Lector))
        //    {
        //        blogs = _context.Blogs.Include(b => b.Author).Where(b => !b.Deleted && b.OwnerId == userId).ToList();
        //    }

        //    return View(blogs);
        //}

        //[Route("admin/blog/new")]
        //public ActionResult NewBlog()
        //{
        //    var persons = _context.Persons.ToList();
        //    var viewModel = new BlogViewModel()
        //    {
        //        Persons = persons,
        //        Blog = new Blog(),

        //    };
        //    return View(viewModel);
        //}

        //[Route("admin/blog/new")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult NewBlog(BlogViewModel vm)
        //{

        //    var tags = TagParser.ParseTags(vm.Tagy, _context); // ParseTags(vm.Tagy);
        //    var persons = _context.Persons.ToList();
        //    var viewModel = new BlogViewModel()
        //    {
        //        Persons = persons
        //    };
        //    if (!ModelState.IsValid)
        //    {
        //        viewModel.Blog = vm.Blog;
        //        viewModel.Blog.Tags = tags;
        //        return View(viewModel);
        //    }

        //    var exist = _context.Blogs.FirstOrDefault(b => b.UrlTitle == vm.Blog.UrlTitle);
        //    if (exist != null)
        //    {
        //        ModelState.AddModelError("blog.UrlTitle", "Zadany url titulek již existuje");
        //        viewModel.Blog = vm.Blog;
        //        viewModel.Blog.Tags = tags;
        //        return View(viewModel);
        //    }

        //    vm.Blog.OwnerId = User.Identity.GetUserId();
        //    vm.Blog.Created = DateTime.Now;
        //    vm.Blog.Changed = DateTime.Now;
        //    vm.Blog.Tags = tags;
        //    _context.Blogs.Add(vm.Blog);
        //    _context.SaveChanges();
        //    ViewData["Saved"] = "Blog byl vyrvořen";
        //    ModelState.Clear();
        //    return RedirectToAction("Blog");

        //}
        //[Route("admin/blog/edit/{id?}")]
        //public ActionResult EditBlog(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Blog blog = _context.Blogs.Include(b => b.Author).Include(b => b.Tags).FirstOrDefault(b => b.Id == id);
        //    if (blog == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    if (User.IsInRole(Roles.Lector) && blog.OwnerId != User.Identity.GetUserId())
        //    {
        //        return HttpNotFound();
        //    }

        //    var persons = _context.Persons.ToList();
        //    var viewModel = new BlogViewModel()
        //    {
        //        Persons = persons,
        //        Blog = blog

        //    };
        //    return View(viewModel);
        //}

        //// POST: Blog/Edit/5
        //[Route("admin/blog/edit/{id?}")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditBlog(BlogViewModel vm)
        //{
        //    var tags = TagParser.ParseTags(vm.Tagy, _context); //ParseTags(vm.Tagy);
        //    if (!ModelState.IsValid)
        //    {
        //        var persons = _context.Persons.ToList();
        //        var viewModel = new BlogViewModel()
        //        {
        //            Persons = persons,
        //            Blog = vm.Blog

        //        };
        //        return View(viewModel);
        //    }

        //    //muze editovat pouze pokud stejny url title neexistuje u jineho blogu
        //    var existingBlog = _context.Blogs.FirstOrDefault(b => b.UrlTitle == vm.Blog.UrlTitle);
        //    bool sameUrlInAnotherBlog = false;
        //    if (existingBlog != null)
        //    {
        //        sameUrlInAnotherBlog = existingBlog.Id != vm.Blog.Id;
        //    }

        //    if (sameUrlInAnotherBlog)
        //    {
        //        ModelState.AddModelError("blog.UrlTitle", "Zadany url titulek již existuje");
        //        var persons = _context.Persons.ToList();
        //        var viewModel = new BlogViewModel()
        //        {
        //            Persons = persons,
        //            Blog = vm.Blog

        //        };
        //        return View(viewModel);
        //    }

        //    var blo = _context.Blogs.Include(b => b.Tags).FirstOrDefault(b => b.Id == vm.Blog.Id);
        //    if (blo != null)
        //    {
        //        if (User.IsInRole(Roles.Lector) && blo.OwnerId != User.Identity.GetUserId())
        //        {
        //            return HttpNotFound();
        //        }

        //        blo.Tags = null;
        //        _context.SaveChanges();


        //        blo.Name = vm.Blog.Name;
        //        blo.Description = vm.Blog.Description;
        //        blo.Body = vm.Blog.Body;
        //        blo.Author_Id = vm.Blog.Author_Id;
        //        blo.UrlTitle = vm.Blog.UrlTitle;
        //        blo.Tags = tags;
        //        blo.Changed = DateTime.Now;
        //        _context.SaveChanges();
        //        return RedirectToAction("Blog");
        //    }
        //    else
        //    {
        //        return HttpNotFound();
        //    }
        //}
        //[Route("admin/blog/delete/{id?}")]
        //public ActionResult DeleteBlog(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
        //    if (blog == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    if (User.IsInRole(Roles.Lector) && blog.OwnerId != User.Identity.GetUserId())
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(blog);
        //}


        //// POST: Blog/Delete/5
        //[Route("admin/blog/delete/{id?}")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteBlogConfirmed(int id)
        //{
        //    var blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
        //    if (blog == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    if (User.IsInRole(Roles.Lector) && blog.OwnerId != User.Identity.GetUserId())
        //    {
        //        return HttpNotFound();
        //    }
        //    //  _context.Blogs.Remove(blog);
        //    blog.Deleted = true;
        //    _context.SaveChanges();
        //    return RedirectToAction("Blog");
        //}

        #endregion


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}