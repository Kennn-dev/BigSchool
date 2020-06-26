using lab.Models;
using lab.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.Owin.Security.Provider;
using System.Web.Helpers;
using Newtonsoft.Json;
using System.Web.UI.WebControls;

namespace lab.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        // GET: BigSchool
        public CourseController()
        {
            _dbContext = new ApplicationDbContext();
        }
        /// <summary>
        /// Send data to AngularJs
        /// </summary>
        /// <returns>
        /// Json
        /// </returns>
        public ActionResult values()
        {
            var upcomming = _dbContext.Courses
               .Include(c => c.Lecture)
               .Include(c => c.Category)
               .Where(c => c.DateTime > DateTime.Now && c.IsCanceled == false).ToList();
            var iduser = User.Identity.GetUserId();
            var Getatendance = _dbContext.Attendances
                .Where(p => p.AttendeeId == iduser)
                .ToList();
            var GetFollowing = _dbContext.Followings
                .Where(x => x.FolloweeId == iduser)
                .Include(p => p.Follower)
                .Include(p => p.Followee)
                .ToList();
            var viewModel = new CoursesViewMode
            {
                UpcommingCourses = upcomming,
                ShowAction = User.Identity.IsAuthenticated,
                GetAttendances = Getatendance,
                GetFollowings = GetFollowing

            };
            var list = JsonConvert.SerializeObject(viewModel,
                                                    Formatting.None,
                                                    new JsonSerializerSettings()
                    {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                  });

            return Content(list, "application/json");
           
           
        }
        
        public ActionResult Home()
        {
            if (User.Identity.IsAuthenticated)
            {
                var upcomming = _dbContext.Courses
                .Include(c => c.Lecture)
                .Include(c => c.Category)
                .Where(c => c.DateTime > DateTime.Now && c.IsCanceled == false).ToList();
                var iduser = User.Identity.GetUserId();
                var Getatendance = _dbContext.Attendances
                    .Where(p => p.AttendeeId == iduser)
                    .ToList();
                var GetFollowing = _dbContext.Followings
                    .Where(x => x.FolloweeId == iduser)
                    .Include(p=>p.Follower)
                    .Include(p=>p.Followee)
                    .ToList();
                var viewModel = new CoursesViewMode
                {
                    UpcommingCourses = upcomming,
                    ShowAction = User.Identity.IsAuthenticated,
                    GetAttendances = Getatendance,
                    GetFollowings = GetFollowing
                    
                };
                return View(viewModel);
            }
            else
            {
                var upcomming = _dbContext.Courses
               .Include(c => c.Lecture)
               .Include(c => c.Category)
               .Where(c => c.DateTime > DateTime.Now && c.IsCanceled == false);
                var viewModel = new CoursesViewMode
                {
                    UpcommingCourses = upcomming,
                    ShowAction = User.Identity.IsAuthenticated,
                    

                };
                return View(viewModel);
            }
            
        }
        [Authorize]
        public ActionResult Create()
        {

            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View(viewModel);
            }
            var course = new Course
            {
                CategoryId = viewModel.Category,
                DateTime = viewModel.GetDateTime(),
                LectureId = User.Identity.GetUserId(),
                Place = viewModel.Place,
                IsCanceled = false
            };
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();
            return RedirectToAction("Home", "Course");
        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var iduser = User.Identity.GetUserId();
            var course = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include(l => l.Lecture)
                .Include(l => l.Category)
                .ToList();
            var Getatendance = _dbContext.Attendances
                  .Where(p => p.AttendeeId == iduser)
                  .ToList();
            var viewModel = new CoursesViewMode
            {
                UpcommingCourses = course,
                ShowAction = User.Identity.IsAuthenticated,
                GetAttendances = Getatendance
            };
            return View(viewModel);
        }
        [Authorize]
        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();           
            var query = from a in _dbContext.Users
                        join b in _dbContext.Followings on a.Id equals b.FollowerId
                        where b.FolloweeId == userId
                        select a;
            return View(query);
                
        }
        [Authorize]
        public ActionResult MineCourse()
        {
            var userId = User.Identity.GetUserId();
            var course = _dbContext.Courses
                .Where(c => c.LectureId == userId && c.DateTime > DateTime.Now)
                .Include(l => l.Lecture)
                .Include(c => c.Category)
                .ToList();

            return View(course);
        }
        [Authorize]
        public ActionResult EditCourse (int? id)
        {
            if(id != null)
            {
                var userId = User.Identity.GetUserId();
                var course = _dbContext.Courses.FirstOrDefault(p => p.Id == id && p.LectureId == userId);

                var viewModel = new CourseViewModel
                {
                    id = course.Id,
                    Categories = _dbContext.Categories.ToList(),
                    Date = course.DateTime.ToString("dd/MM/yyyy"),
                    Place = course.Place,
                    Time = course.DateTime.ToString("HH:mm"),
                    Category = course.CategoryId
                };
                return View(viewModel);
            }
            return RedirectToAction("Home", "Course");
        }

        [HttpPost]
        public ActionResult EditCourse(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View(viewModel);
            }
            var updatecourse =  _dbContext.Courses.FirstOrDefault(p => p.Id == viewModel.id);
            updatecourse.Place = viewModel.Place;
            updatecourse.DateTime = viewModel.GetDateTime();
            updatecourse.CategoryId = viewModel.Category;
            _dbContext.SaveChanges();
            return RedirectToAction("Home", "Course");
        }

        [Authorize]
        public ActionResult DetailLecture(string id)
        {
            if(id != null)
            {
                var upcomming = _dbContext.Courses
                .Include(c => c.Lecture)
                .Include(c => c.Category)
                .Where(c => c.DateTime > DateTime.Now && c.IsCanceled == false &&c.LectureId == id);
                var iduser = User.Identity.GetUserId();
                var Getatendance = _dbContext.Attendances
                    .Where(p => p.AttendeeId == iduser)
                    .ToList();
                var GetFollowing = _dbContext.Followings
                    .Where(x => x.FolloweeId == iduser)
                    .Include(p => p.Follower)
                    .Include(p => p.Followee)
                    .ToList();
                var viewModel = new CoursesViewMode
                {
                    UpcommingCourses = upcomming,
                    ShowAction = User.Identity.IsAuthenticated,
                    GetAttendances = Getatendance,
                    GetFollowings = GetFollowing

                };
                return View(viewModel);
            }
            return RedirectToAction("Home", "Course");
        }
        [Authorize]
        public ActionResult FollowingMe()
        {
            var id = User.Identity.GetUserId();
            var FollowingMe = _dbContext.Followings.Where(p => p.FollowerId == id)
                .Include(p=>p.Followee)
                .Include(p=> p.Follower)
                .ToList();
            return View(FollowingMe);
        }
       
        public ActionResult GoingMe(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Home", "Course");
            }
            var IDs = User.Identity.GetUserId();
            var GoingMe = (from u in _dbContext.Users
                           join a in _dbContext.Attendances on u.Id equals a.AttendeeId
                           where a.CourseId == id
                           select u).ToList();
            return View(GoingMe);

        }
       
    }
}