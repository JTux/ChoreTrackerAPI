﻿using ChoreTracker.Models.GroupModels;
using ChoreTracker.Models.ResponseModels;
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

        [HttpGet]
        public IHttpActionResult GetGroupInfo(int id)
        {
            var service = GetGroupService();

            var group = service.GetGroupById(id);
            if (group == null)
                return BadRequest("Group not found.");

            return Ok(group);
        }

        [HttpPost]
        [Route("Join/{key}")]
        public IHttpActionResult JoinGroupAsMember(string key)
        {
            var service = GetGroupService();

            var groupJoinResponse = service.JoinGroup(key);

            if (!groupJoinResponse.Succeeded)
                return InternalServerError(new Exception(groupJoinResponse.Message));

            return Ok(groupJoinResponse.Message);
        }

        [HttpPut]
        [Route("M/{id}/Accept")]
        public IHttpActionResult AcceptApplicant(int id)
        {
            var service = GetMemberService();

            RequestResponse applicantAcceptResponse = service.AcceptApplicant(id);

            if (!applicantAcceptResponse.Succeeded)
                return InternalServerError(new Exception(applicantAcceptResponse.Message));

            return Ok(applicantAcceptResponse.Message);
        }

        [HttpPut]
        [Route("M/{id}/Decline")]
        public IHttpActionResult DeclineApplicant(int id)
        {
            var service = GetMemberService();

            RequestResponse applicantDeclineResponse = service.DeclineApplicant(id);

            if (!applicantDeclineResponse.Succeeded)
                return InternalServerError(new Exception(applicantDeclineResponse.Message));

            return Ok(applicantDeclineResponse.Message);
        }

        private GroupService GetGroupService() => new GroupService(Guid.Parse(User.Identity.GetUserId()));
        private GroupMemberService GetMemberService() => new GroupMemberService(Guid.Parse(User.Identity.GetUserId()));
    }
}
