using lab.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lab.DTOs;

namespace lab.Controllers
{
    [Authorize]
    public class AttendanceController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        // GET: BigSchool
        public AttendanceController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [HttpPost]
        public IHttpActionResult Attend(AttendanceDot AttendanceDto)
        {
            var userID = User.Identity.GetUserId();
            if(_dbContext.Attendances.Any(a=> a.AttendeeId == userID && a.CourseId == AttendanceDto.CourseId))
            {
                return BadRequest("The Attendance already exist");
            }
            var attendance = new Attendance
            {
                CourseId = AttendanceDto.CourseId,
                AttendeeId = userID
            };
            _dbContext.Attendances.Add(attendance);
            _dbContext.SaveChanges();
            return Ok();
        }
     
    }
}
