using ChoreTracker.Models.ResponseModels;
using ChoreTracker.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ChoreTracker.WebAPI.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected IHttpActionResult ValidateRequestResponse(RequestResponse response)
        {
            if (!response.Succeeded)
                return InternalServerError(new Exception(response.Message));

            return Ok(response.Message);
        }

        protected IHttpActionResult ValidateModelRequestResponse<T>(RequestResponse response)
        {
            if (!response.Succeeded)
                return BadRequest(response.Message);

            var castedResponse = (ModelRequestResponse<T>)response;

            return Ok(castedResponse.Model);
        }

        protected GroupService GetGroupService() => new GroupService(Guid.Parse(User.Identity.GetUserId()));
        protected GroupMemberService GetMemberService() => new GroupMemberService(Guid.Parse(User.Identity.GetUserId()));
        protected TaskService GetTaskService() => new TaskService(Guid.Parse(User.Identity.GetUserId()));
    }
}