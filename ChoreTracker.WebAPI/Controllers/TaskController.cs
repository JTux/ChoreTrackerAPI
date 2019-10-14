using ChoreTracker.Models.ResponseModels;
using ChoreTracker.Models.TaskModels;
using ChoreTracker.Services;
using ChoreTracker.WebAPI.Models;
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
    public class TaskController : BaseController
    {
        [HttpPost]
        public IHttpActionResult CreateNewTask(TaskCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = GetTaskService();

            var createResponse = service.CreateTask(model);

            return ValidateRequestResponse(createResponse);
        }

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

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateTask(int id, TaskUpdate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.TaskId)
                return BadRequest("Task ID mismatch.");

            var service = GetTaskService();

            var updateResponse = service.UpdateTask(model);

            return ValidateRequestResponse(updateResponse);
        }
    }
}
