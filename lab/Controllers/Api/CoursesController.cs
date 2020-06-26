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
    public class CoursesController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        // GET: BigSchool
        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [HttpDelete]
        [Authorize]
        public IHttpActionResult Cancel(int id)
        {
            var iduser = User.Identity.GetUserId();
            var course = _dbContext.Courses.FirstOrDefault(c => c.Id == id && c.LectureId == iduser);
            if (course.IsCanceled)
            {
                return NotFound();
            }
            course.IsCanceled = true;
            _dbContext.SaveChanges();
            return Ok();
        }
       

    }
}
