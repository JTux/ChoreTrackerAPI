using ChoreTracker.Models.GroupMemberModels;
using ChoreTracker.Models.TaskModels;
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
        public GroupListItem(int groupId, string groupName, string inviteCode, string nickname, bool userIsOfficer, List<GroupMemberDetail> members, List<GroupMemberDetail> applicants)
        {
            GroupId = groupId;
            GroupName = groupName;
            GroupInviteCode = inviteCode;
            UserNickname = nickname;
            UserIsOfficer = userIsOfficer;
            Members = members;
            Applicants = applicants;
        }
        public GroupListItem(int groupId, string groupName, string inviteCode, string nickname, bool userIsOfficer, List<GroupMemberDetail> members, List<GroupMemberDetail> applicants, List<TaskDetail> tasks)
            : this(groupId, groupName, inviteCode, nickname, userIsOfficer, members, applicants)
        {
            Tasks = tasks;
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupInviteCode { get; set; }
        public string UserNickname { get; set; }
        public bool UserIsOfficer { get; set; }
        public List<GroupMemberDetail> Members { get; set; }
        public List<GroupMemberDetail> Applicants { get; set; }

        public List<TaskDetail> Tasks { get; set; }
    }
}
