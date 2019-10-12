using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Models.GroupMemberModels;
using ChoreTracker.Models.GroupModels;
using ChoreTracker.Models.ResponseModels;
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
            var groupsAsMember = _context.GroupMembers.Where(gm => gm.UserId == _userId.ToString()).ToArray();

            var groups = new List<GroupListItem>();
            foreach (var membership in groupsAsMember)
            {
                if (!membership.IsAccepted)
                    continue;

                var members = GetMemberDetailList(membership.Group.GroupMembers.Where(m => m.IsAccepted));

                var applicants = new List<GroupMemberDetail>();
                if (membership.IsOfficer)
                    applicants = GetMemberDetailList(membership.Group.GroupMembers.Where(m => !m.IsAccepted));

                groups.Add(new GroupListItem(membership.GroupId, membership.Group.GroupName, membership.Group.GroupInviteCode, membership.MemberNickname, membership.IsOfficer, members, applicants));
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

                memberDetails.Add(new GroupMemberDetail(entity.GroupMemberId, entity.IsOfficer, nickname, entity.User.FirstName, entity.User.LastName));
            }
            return memberDetails;
        }

        public RequestResponse LeaveGroup(int groupId)
        {
            var groupMember =
                    _context.GroupMembers.FirstOrDefault(gm => gm.UserId == _userId.ToString() && gm.GroupId == groupId);
            var group = _context.Groups.Find(groupId);
            if (groupMember == null)
                return BadResponse("Cannot access group.");

            if (groupMember.Group.OwnerId == _userId && groupMember.Group.GroupMembers.Count() > 1)
                return BadResponse("Owner cannot leave group with existing members.");

            _context.GroupMembers.Remove(groupMember);

            int changeCount = 1;
            if (group.GroupMembers.Count == 0)
            {
                _context.Groups.Remove(group);
                changeCount++;
            }

            if (_context.SaveChanges() != changeCount)
                return BadResponse("Could not leave group.");

            return OkResponse("Successfully left group.");
        }

        public GroupDetail GetGroupById(int id)
        {
            if (!CheckUserGroupAccess(id))
                return null;

            var group = _context.Groups.Find(id);
            var userGroupMember = group.GroupMembers.FirstOrDefault(m => m.UserId == _userId.ToString());

            var groupDetail =
                new GroupDetail(group.GroupId, group.GroupName, group.GroupInviteCode, userGroupMember.IsOfficer, userGroupMember.MemberNickname, group.GroupMembers.ToGroupMemberDetailList());

            return groupDetail;
        }

        public RequestResponse CreateGroup(GroupCreate model)
        {
            var groupEntity = new GroupEntity(model.GroupName, _userId, "Ayy");

            _context.Groups.Add(groupEntity);

            if (_context.SaveChanges() != 1)
                return BadResponse("Could not create group");

            var memberEntity = new GroupMemberEntity(groupEntity.GroupId, _userId.ToString(), true, true);

            _context.GroupMembers.Add(memberEntity);
            if (_context.SaveChanges() != 1)
                return BadResponse("Could not create group member.");

            return OkResponse("Group was created!");
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
            var groupMember =
                    _context.GroupMembers.FirstOrDefault(gm => gm.UserId == _userId.ToString() && gm.GroupId == groupId);

            return groupMember != null ? true : false;
        }
    }
}
