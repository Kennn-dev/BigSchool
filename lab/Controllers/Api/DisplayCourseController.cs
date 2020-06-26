using lab.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace lab.Controllers.Api
{
    public class DisplayCourseController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        // GET: BigSchool
        public DisplayCourseController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [HttpPost]
        [Authorize]
        public IHttpActionResult Put(int id)
        {
            var iduser = User.Identity.GetUserId();
            var course = _dbContext.Courses.FirstOrDefault(c => c.Id == id && c.LectureId == iduser);
            if (course.IsCanceled==false)
            {
                return NotFound();
            }
            course.IsCanceled = false;
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
