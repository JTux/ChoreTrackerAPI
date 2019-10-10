using ChoreTracker.Models.GroupMemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.GroupModels
{
    public class GroupDetail
    {
        public GroupDetail() { }
        public GroupDetail(int groupId, string groupName, string inviteCode, bool userIsOfficer, string userNickName, List<GroupMemberDetail> members)
        {
            GroupId = groupId;
            GroupName = groupName;
            GroupInviteCode = inviteCode;
            UserIsOfficer = userIsOfficer;
            UserNickName = userNickName;
            Members = members;
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupInviteCode { get; set; }
        public bool UserIsOfficer { get; set; }
        public string UserNickName { get; set; }
        public List<GroupMemberDetail> Members { get; set; }
    }
}
