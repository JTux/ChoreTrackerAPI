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

                var members = new List<GroupMemberDetail>();
                var applicants = new List<GroupMemberDetail>();

                foreach (var member in membership.Group.GroupMembers.Where(m => m.IsAccepted))
                    members.Add(new GroupMemberDetail
                    {
                        GroupMemberId = member.GroupMemberId,
                        IsOfficer = member.IsOfficer,
                        FirstName = member.User.FirstName,
                        LastName = member.User.LastName,
                        MemberNickName =
                            (!string.IsNullOrEmpty(member.MemberNickName))
                                ? member.MemberNickName
                                : $"{member.User.FirstName} {member.User.LastName}"
                    });

                if (membership.IsOfficer)
                {
                    foreach (var applicant in membership.Group.GroupMembers.Where(m => !m.IsAccepted))
                    {
                        applicants.Add(new GroupMemberDetail
                        {
                            GroupMemberId = applicant.GroupMemberId,
                            IsOfficer = applicant.IsOfficer,
                            FirstName = applicant.User.FirstName,
                            LastName = applicant.User.LastName,
                            MemberNickName =
                                (!string.IsNullOrEmpty(applicant.MemberNickName))
                                    ? applicant.MemberNickName
                                    : $"{applicant.User.FirstName} {applicant.User.LastName}"
                        });
                    }
                }

                groups.Add(
                    new GroupListItem
                    {
                        GroupId = membership.GroupId,
                        GroupName = membership.Group.GroupName,
                        GroupInviteCode = membership.Group.GroupInviteCode,
                        UserNickName = membership.MemberNickName,
                        UserIsOfficer = membership.IsOfficer,
                        Members = members,
                        Applicants = applicants
                    });
            }

            return groups;
        }

        public GroupDetail GetGroupById(int id)
        {
            if (!CheckUserGroupAccess(id))
                return null;

            var group = _context.Groups.Find(id);
            var userGroupMember = group.GroupMembers.FirstOrDefault(m => m.UserId == _userId.ToString());

            var groupDetail = new GroupDetail
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                GroupInviteCode = group.GroupInviteCode,
                Members = group.GroupMembers.ToGroupMemberDetailList(),
                UserIsOfficer = userGroupMember.IsOfficer,
                UserNickName = userGroupMember.MemberNickName
            };

            return groupDetail;
        }

        public RequestResponse CreateGroup(GroupCreate model)
        {
            var groupEntity = new GroupEntity
            {
                DateFounded = DateTimeOffset.Now,
                GroupName = model.GroupName,
                OwnerId = _userId,
                GroupInviteCode = "Ayy"
            };

            _context.Groups.Add(groupEntity);

            if (_context.SaveChanges() != 1)
                return BadResponse("Could not create group");

            var memberEntity = new GroupMemberEntity
            {
                GroupId = groupEntity.GroupId,
                UserId = _userId.ToString(),
                IsOfficer = true,
                IsAccepted = true
            };

            _context.GroupMembers.Add(memberEntity);
            if (_context.SaveChanges() != 1)
                return BadResponse("Could not create group member.");

            string[] x = new string[] { "Hey", "There", "bud" };
            x.ToSingleString();

            return OkResponse("Group was created!");
        }

        public RequestResponse JoinGroup(string groupKey)
        {
            var groupEntity = _context.Groups.FirstOrDefault(g => g.GroupInviteCode == groupKey);

            if (groupEntity == null)
                return BadResponse("Invalid code.");

            if (CheckUserGroupAccess(groupEntity.GroupId))
                return BadResponse("Already in group.");

            var memberEntity = new GroupMemberEntity
            {
                GroupId = groupEntity.GroupId,
                UserId = _userId.ToString(),
                IsOfficer = false,
                IsAccepted = false
            };

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
