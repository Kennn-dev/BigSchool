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
    public class DeleteCourseController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        // GET: BigSchool
        public DeleteCourseController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteCourse(int id)
        {
            var iduser = User.Identity.GetUserId();
            var course = _dbContext.Courses.FirstOrDefault(c => c.Id == id && c.LectureId == iduser);
            
            _dbContext.Courses.Remove(course);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
