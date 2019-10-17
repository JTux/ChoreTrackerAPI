using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Models.GroupMemberModels;
using ChoreTracker.Models.GroupModels;
using ChoreTracker.Models.ResponseModels;
using ChoreTracker.Models.TaskModels;
using ChoreTracker.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services
{
    public class GroupService : BaseService
    {
        private readonly Guid _userId;
        private readonly ApplicationDbContext _context;
        public GroupService(Guid currentUserId)
        {
            _userId = currentUserId;
            _context = new ApplicationDbContext();
        }

        public IEnumerable<GroupListItem> GetAvailableGroups()
        {
            var groupsAsMember = _context.GroupMembers.Where(gm => gm.UserId == _userId.ToString() && gm.IsAccepted).ToArray();

            var groups = new List<GroupListItem>();
            foreach (var membership in groupsAsMember)
            {
                var members = GetMemberDetailList(membership.Group.GroupMembers.Where(m => m.IsAccepted));

                var applicants = (membership.IsOfficer) ? GetMemberDetailList(membership.Group.GroupMembers.Where(m => !m.IsAccepted)) : null;

                var tasks = new TaskService(_userId).GetTasksByGroupID(membership.GroupId).ToList();

                var rewards = new RewardService(_userId).GetRewardsByGroupID(membership.GroupId).ToList();

                groups.Add(new GroupListItem(membership.GroupId, membership.Group.GroupName, membership.Group.GroupInviteCode, membership.MemberNickname, membership.IsOfficer, members, applicants, tasks, rewards));
            }

            return groups;
        }

        private List<GroupMemberDetail> GetMemberDetailList(IEnumerable<GroupMemberEntity> entities)
        {
            var memberDetails = new List<GroupMemberDetail>();
            foreach (var entity in entities)
            {
                var nickname = (!string.IsNullOrEmpty(entity.MemberNickname))
                                ? entity.MemberNickname
                                : $"{entity.User.FirstName} {entity.User.LastName}";

                memberDetails.Add(new GroupMemberDetail(entity.GroupMemberId, entity.IsAccepted, entity.IsOfficer, nickname, entity.User.FirstName, entity.User.LastName));
            }
            return memberDetails;
        }

        public RequestResponse LeaveGroup(int groupId)
        {
            var groupMember = GetUserMembership(groupId);

            if (groupMember == null)
                return BadResponse("Cannot access group.");

            if (groupMember.Group.OwnerId == _userId && groupMember.Group.GroupMembers.Count() > 1)
                return BadResponse("Owner cannot leave group with existing members.");

            _context.GroupMembers.Remove(groupMember);

            int changeCount = 1;
            if (groupMember.Group.GroupMembers.Count == 0)
            {
                _context.Groups.Remove(groupMember.Group);
                changeCount++;
            }

            if (_context.SaveChanges() != changeCount)
                return BadResponse("Could not leave group.");

            return OkResponse("Successfully left group.");
        }

        public RequestResponse GetGroupById(int groupId)
        {
            var userMembership = GetUserMembership(groupId);
            if (userMembership == null || !userMembership.IsAccepted)
                return BadResponse("Invalid permissions.");

            var groupDetail = new GroupDetail(
                userMembership.Group.GroupId,
                userMembership.Group.GroupName,
                userMembership.Group.GroupInviteCode,
                userMembership.IsOfficer,
                userMembership.MemberNickname,
                userMembership.Group.GroupMembers.ToGroupMemberDetailList());

            return OkModelResponse("Group found.", groupDetail);
        }

        public RequestResponse CreateGroup(GroupCreate model)
        {
            if (model == null)
                return BadResponse("Request Body was empty.");

            var groupEntity = new GroupEntity(model.GroupName, _userId, GetNewRandomKey(8));

            _context.Groups.Add(groupEntity);

            if (_context.SaveChanges() != 1)
                return BadResponse("Could not create group");

            var memberEntity = new GroupMemberEntity(groupEntity.GroupId, _userId.ToString(), true, true);

            _context.GroupMembers.Add(memberEntity);
            if (_context.SaveChanges() != 1)
                return BadResponse("Could not create group member.");

            return OkResponse("Group created successfully.");
        }

        public RequestResponse UpdateGroupInviteCode(int groupId)
        {
            var userMembership = GetUserMembership(groupId);

            if (userMembership == null || !userMembership.IsOfficer)
                return BadResponse("Unable to edit key.");

            userMembership.Group.GroupInviteCode = GetNewRandomKey(8);

            if (_context.SaveChanges() != 1)
                return BadResponse("Unable to save changes.");

            return OkResponse("Invite key updated.");
        }

        public RequestResponse JoinGroup(string groupKey)
        {
            var groupEntity = _context.Groups.FirstOrDefault(g => g.GroupInviteCode == groupKey);

            if (groupEntity == null)
                return BadResponse("Invalid code.");

            if (CheckUserGroupAccess(groupEntity.GroupId))
                return BadResponse("Already in group.");

            var memberEntity = new GroupMemberEntity(groupEntity.GroupId, _userId.ToString(), false, false);

            _context.GroupMembers.Add(memberEntity);

            if (_context.SaveChanges() != 1)
                return BadResponse("Could not join group.");

            return OkResponse("Joined group successfully.");
        }

        /// <summary>
        /// Takes in a group Id to check if the user is in the corresponding group
        /// </summary>
        /// <param name="groupId">Id of the expected group.</param>
        /// <returns></returns>
        private bool CheckUserGroupAccess(int groupId)
        {
            var groupMember = GetUserMembership(groupId);

            return groupMember != null ? true : false;
        }

        private string GetNewRandomKey(int size)
        {
            string key = "";
            while (true)
            {
                for (int i = 0; i < size; i++)
                    key += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * RandomGenerator.NextDouble() + 65)));

                if (_context.Groups.Where(g => g.GroupInviteCode == key).Count() == 0)
                    break;
            }

            return key;
        }
        
        private GroupMemberEntity GetUserMembership(int groupId)
        {
            return _context.GroupMembers.FirstOrDefault(gm =>
                gm.UserId == _userId.ToString() &&
                gm.GroupId == groupId);
        }
    }
}
