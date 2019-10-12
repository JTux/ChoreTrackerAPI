using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.GroupMemberModels
{
    public class GroupMemberDetail
    {
        public GroupMemberDetail() { }
        public GroupMemberDetail(int groupMemberId, bool isOfficer, string nickname, string firstName, string lastName)
        {
            GroupMemberId = groupMemberId;
            IsOfficer = isOfficer;
            MemberNickname = nickname;
            FirstName = firstName;
            LastName = lastName;
        }

        public int GroupMemberId { get; set; }

        public bool IsOfficer { get; set; }

        public string MemberNickname { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
