using ChoreTracker.Models.ResponseModels;
using ChoreTracker.Models.TaskModels;
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
    [RoutePrefix("api/Task")]
    public class TaskController : ApiController
    {
        [HttpGet]
        [Route("G/{id}")]
        public IHttpActionResult GetGroupTasks(int id)
        {
            var service = GetTaskService();
            var tasks = service.GetTasksByGroupID(id);
            return Ok(tasks);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetGroupById(int id)
        {
            var service = GetTaskService();
            var response = service.GetTaskByID(id);

            return ValidateModelRequestResponse<TaskDetail>(response);
        }

        //-- Helpers

        private IHttpActionResult ValidateRequestResponse(RequestResponse response)
        {
            if (!response.Succeeded)
                return InternalServerError(new Exception(response.Message));

            return Ok(response.Message);
        }

        private IHttpActionResult ValidateModelRequestResponse<T>(RequestResponse response)
        {
            if (!response.Succeeded)
                return BadRequest(response.Message);

            var castedResponse = (ModelRequestResponse<T>)response;

            return Ok(castedResponse.Model);
        }

        private TaskService GetTaskService() => new TaskService(Guid.Parse(User.Identity.GetUserId()));
    }
}
