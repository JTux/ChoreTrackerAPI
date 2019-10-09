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
        public GroupService(Guid currentUserId)
        {
            _userId = currentUserId;
        }

        public IEnumerable<GroupListItem> GetAvailableGroups()
        {
            using (var context = new ApplicationDbContext())
            {
                var groupsAsMember = context.GroupMembers.Where(gm => gm.UserId == _userId.ToString()).ToArray();

                var groups = new List<GroupListItem>();
                foreach (var membership in groupsAsMember)
                {
                    var members = new List<GroupMemberDetail>();

                    foreach (var member in membership.Group.GroupMembers)
                        members.Add(new GroupMemberDetail
                        {
                            GroupMemberId = member.GroupMemberId,
                            IsOfficer = member.IsOfficer,
                            FirstName = member.User.FirstName,
                            LastName = member.User.LastName,
                            MemberNickName =
                                (string.IsNullOrEmpty(member.MemberNickName))
                                    ? member.MemberNickName
                                    : $"{member.User.FirstName} {member.User.LastName}"
                        });

                    groups.Add(
                        new GroupListItem
                        {
                            GroupId = membership.GroupId,
                            GroupName = membership.Group.GroupName,
                            UserNickName = membership.MemberNickName,
                            UserIsOfficer = membership.IsOfficer,
                            Members = members
                        });
                }

                return groups;
            }
        }

        public GroupDetail GetGroupById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                if (!CheckUserGroupAccess(id, context))
                    return null;

                var group = context.Groups.Find(id);
                var userGroupMember = group.GroupMembers.FirstOrDefault(m => m.UserId == _userId.ToString());

                var groupDetail = new GroupDetail
                {
                    GroupId = group.GroupId,
                    GroupName = group.GroupName,
                    Members = group.GroupMembers.ToGroupMemberDetailList(),
                    UserIsOfficer = userGroupMember.IsOfficer,
                    UserNickName = userGroupMember.MemberNickName
                };

                return groupDetail;
            }
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

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Groups.Add(groupEntity);

                if (ctx.SaveChanges() != 1)
                    return BadResponse("Could not create group");

                var user = ctx.Users.FirstOrDefault(u => u.Id == _userId.ToString());

                var memberEntity = new GroupMemberEntity
                {
                    GroupId = groupEntity.GroupId,
                    UserId = user.Id,
                    IsOfficer = true
                };

                ctx.GroupMembers.Add(memberEntity);
                if (ctx.SaveChanges() != 1)
                    return BadResponse("Could not create group member.");

                return OkResponse("Group was created!");
            }
        }

        /// <summary>
        /// Takes in a group Id and an existing ApplicationDbContext to check if the user is in the corresponding group
        /// </summary>
        /// <param name="groupId">Id of the expected group.</param>
        /// <param name="context">Existing ApplicationDbContext given as to avoid creating multiple instances.</param>
        /// <returns></returns>
        private bool CheckUserGroupAccess(int groupId, ApplicationDbContext context)
        {
            var groupMember =
                    context.GroupMembers.FirstOrDefault(gm => gm.UserId == _userId.ToString() && gm.GroupId == groupId);

            return groupMember != null ? true : false;
        }
    }
}
