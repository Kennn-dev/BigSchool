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
    public class FollowingController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        public FollowingController()
        {
            _dbContext = new ApplicationDbContext();
        }
        [HttpDelete]
        public IHttpActionResult Attend(string id)
        {
            var userID = User.Identity.GetUserId();
            var delete = _dbContext.Followings.Where(p => p.FollowerId == id && p.FolloweeId == userID).ToList();
            for(var i = 0; i< delete.Count; i++)
            {
                _dbContext.Followings.Remove(delete[i]);
                _dbContext.SaveChanges();
            }
            return Ok();
        }
    }
}
