﻿using ChoreTracker.Models.GroupMemberModels;
using ChoreTracker.Models.GroupModels;
using ChoreTracker.Models.ResponseModels;
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
    [Authorize]
    [RoutePrefix("api/Group")]
    public class GroupController : BaseController
    {
        //-- General Group Endpoints
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
            return ValidateRequestResponse(requestResponse);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetGroupInfo(int id)
        {
            var service = GetGroupService();
            var groupResponse = service.GetGroupById(id);
            return ValidateModelRequestResponse<GroupDetail>(groupResponse);
        }

        [HttpPut]
        [Route("{id}/UpdateKey")]
        public IHttpActionResult UpdateGroupInviteKey(int id)
        {
            var service = GetGroupService();
            var updateResponse = service.UpdateGroupInviteCode(id);
            return ValidateRequestResponse(updateResponse);
        }

        [HttpPost]
        [Route("Join/{key}")]
        public IHttpActionResult JoinGroupAsMember(string key)
        {
            var service = GetGroupService();
            var groupJoinResponse = service.JoinGroup(key);
            return ValidateRequestResponse(groupJoinResponse);
        }

        [HttpDelete]
        [Route("{id}/Leave")]
        public IHttpActionResult LeaveGroup(int id)
        {
            var service = GetGroupService();
            var leaveResponse = service.LeaveGroup(id);
            return ValidateRequestResponse(leaveResponse);
        }

        //-- Member Endpoints
        [HttpGet]
        [Route("M/{id}")]
        public IHttpActionResult GetMemberDetails(int id)
        {
            var service = GetMemberService();
            var memberDetail = service.GetMemberDetail(id);
            return ValidateModelRequestResponse<GroupMemberDetail>(memberDetail);
        }

        [HttpPut]
        [Route("M/{id}/Accept")]
        public IHttpActionResult AcceptApplicant(int id)
        {
            var service = GetMemberService();
            var applicantAcceptResponse = service.AcceptApplicant(id);
            return ValidateRequestResponse(applicantAcceptResponse);
        }

        [HttpPut]
        [Route("M/{id}/Decline")]
        public IHttpActionResult DeclineApplicant(int id)
        {
            var service = GetMemberService();
            var applicantDeclineResponse = service.DeclineApplicant(id);
            return ValidateRequestResponse(applicantDeclineResponse);
        }

        [HttpDelete]
        [Route("M/{id}/Remove")]
        public IHttpActionResult RemoveMember(int id)
        {
            var service = GetMemberService();
            var removalResponse = service.RemoveMember(id);
            return ValidateRequestResponse(removalResponse);
        }

        [HttpPut]
        [Route("M/{id}/UpdateNickname")]
        public IHttpActionResult UpdateUserNickname(int id, MemberNicknameUpdate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.GroupMemberId)
                return BadRequest("Group ID Mismatch.");

            var service = GetMemberService();
            var updateResponse = service.UpdateNickname(model);
            return ValidateRequestResponse(updateResponse);
        }

        [HttpPut]
        [Route("M/{id}/ToggleOfficer")]
        public IHttpActionResult ToggleOfficer(int id)
        {
            var service = GetMemberService();
            var response = service.ToggleOfficer(id);
            return ValidateRequestResponse(response);
        }
    }
}
