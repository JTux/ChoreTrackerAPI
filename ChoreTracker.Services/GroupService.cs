using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Models.GroupMemberModels;
using ChoreTracker.Models.GroupModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services
{
    public class GroupService
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
                var groupMembersQuery = context.GroupMembers.Where(gm => gm.UserId == _userId.ToString()).ToArray();

                var groups = new List<GroupListItem>();
                foreach (var groupMember in groupMembersQuery)
                {
                    var members = new List<GroupMemberDetail>();

                    foreach (var member in groupMember.Group.GroupMembers)
                        members.Add(new GroupMemberDetail
                        {
                            GroupMemberId = member.GroupMemberId,
                            IsOfficer = member.IsOfficer,
                            FirstName = member.FirstName,
                            LastName = member.LastName,
                            MemberNickName =
                                (string.IsNullOrEmpty(member.MemberNickName))
                                    ? member.MemberNickName
                                    : $"{member.User.FirstName} {member.User.LastName}"
                        });

                    groups.Add(new GroupListItem { GroupName = groupMember.Group.GroupName, Members = members });
                }

                return groups;
            }
        }

        public bool CreateGroup(GroupCreate model)
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
                    return false;

                var user = ctx.Users.FirstOrDefault(u => u.Id == _userId.ToString());
                if (user == null)
                    return false;

                var memberEntity = new GroupMemberEntity
                {
                    GroupId = groupEntity.GroupId,
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsOfficer = true
                };

                ctx.GroupMembers.Add(memberEntity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
