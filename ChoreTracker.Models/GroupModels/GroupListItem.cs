using ChoreTracker.Models.GroupMemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.GroupModels
{
    public class GroupListItem
    {
        public GroupListItem() { }
        public GroupListItem(int groupId, string groupName, string inviteCode, string nickName, bool userIsOfficer, List<GroupMemberDetail> members, List<GroupMemberDetail> applicants)
        {
            GroupId = groupId;
            GroupName = groupName;
            GroupInviteCode = inviteCode;
            UserNickName = nickName;
            UserIsOfficer = userIsOfficer;
            Members = members;
            Applicants = applicants;
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupInviteCode { get; set; }
        public string UserNickName { get; set; }
        public bool UserIsOfficer { get; set; }
        public List<GroupMemberDetail> Members { get; set; }
        public List<GroupMemberDetail> Applicants { get; set; }
    }
}
