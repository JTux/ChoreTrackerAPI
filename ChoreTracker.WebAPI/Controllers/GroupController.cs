using ChoreTracker.Models.GroupMemberModels;
using ChoreTracker.Models.GroupModels;
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

            return GetResponseReturn(requestResponse);
        }

        [HttpGet]
        [Route("{id}")]
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

            return GetResponseReturn(groupJoinResponse);
        }

        [HttpDelete]
        [Route("Leave/{id}")]
        public IHttpActionResult LeaveGroup(int id)
        {
            var service = GetGroupService();

            var leaveResponse = service.LeaveGroup(id);

            return GetResponseReturn(leaveResponse);
        }

        [HttpPut]
        [Route("M/{id}/Accept")]
        public IHttpActionResult AcceptApplicant(int id)
        {
            var service = GetMemberService();

            var applicantAcceptResponse = service.AcceptApplicant(id);

            return GetResponseReturn(applicantAcceptResponse);
        }

        [HttpPut]
        [Route("M/{id}/Decline")]
        public IHttpActionResult DeclineApplicant(int id)
        {
            var service = GetMemberService();

            var applicantDeclineResponse = service.DeclineApplicant(id);

            return GetResponseReturn(applicantDeclineResponse);
        }

        [HttpDelete]
        [Route("M/Remove/{id}")]
        public IHttpActionResult RemoveMember(int id)
        {
            var service = GetMemberService();

            var removalResponse = service.RemoveMember(id);

            return GetResponseReturn(removalResponse);
        }

        [HttpPut]
        [Route("{id}/UpdateNickname")]
        public IHttpActionResult UpdateUserNickname(int id, MemberNicknameUpdate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.GroupId)
                return BadRequest("Group Id Mismatch.");

            var service = GetMemberService();

            var updateResponse = service.UpdateNickname(model);

            return GetResponseReturn(updateResponse);
        }

        private IHttpActionResult GetResponseReturn(RequestResponse response)
        {
            if (!response.Succeeded)
                return InternalServerError(new Exception(response.Message));

            return Ok(response.Message);
        }

        private GroupService GetGroupService() => new GroupService(Guid.Parse(User.Identity.GetUserId()));
        private GroupMemberService GetMemberService() => new GroupMemberService(Guid.Parse(User.Identity.GetUserId()));
    }
}
