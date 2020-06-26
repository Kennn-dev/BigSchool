using lab.DTOs;
using lab.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace lab.Controllers
{
    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        // GET: BigSchool
        public FollowingsController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [HttpPost]
        public IHttpActionResult Follow (FollowingDto param)
        {
            var userId = User.Identity.GetUserId();
            if(_dbContext.Followings.Any(f=> f.FollowerId == userId && f.FolloweeId == param.FolloweeId))
            {
                return BadRequest("Following already exists!");
            }
            var following = new Following
            {
                FolloweeId = userId,
                FollowerId = param.FolloweeId
            };
            _dbContext.Followings.Add(following);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
