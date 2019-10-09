using ChoreTracker.Models.GroupModels;
using ChoreTracker.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChoreTracker.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Group")]
    public class GroupController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAvailableGroups()
        {
            var service = GetGroupService();
            var groups = service.GetAvailableGroups();
            return Ok(groups);
        }

        [HttpPost]
        public IHttpActionResult CreateGroup(GroupCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = GetGroupService();
            var requestResponse = service.CreateGroup(model);

            if (!requestResponse.Succeeded)
                return InternalServerError(new Exception(requestResponse.Message));

            return Ok(requestResponse.Message);
        }

        private GroupService GetGroupService() => new GroupService(Guid.Parse(User.Identity.GetUserId()));
    }
}
