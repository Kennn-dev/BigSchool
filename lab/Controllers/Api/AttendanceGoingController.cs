using lab.DTOs;
using lab.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Data.Entity;

namespace lab.Controllers.Api
{
    public class AttendanceGoingController : ApiController
    {
        // GET: BigSchool
        private readonly ApplicationDbContext _dbContext;
        public AttendanceGoingController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [HttpDelete]
        public IHttpActionResult Attend(int id)
        {
            var userID = User.Identity.GetUserId();
           
            var delete = _dbContext.Attendances.FirstOrDefault(p => p.CourseId == id);
            _dbContext.Attendances.Remove(delete);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
