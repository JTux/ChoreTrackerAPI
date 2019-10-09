using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.GroupMemberModels
{
    public class GroupMemberDetail
    {
        public int GroupMemberId { get; set; }

        public bool IsOfficer { get; set; }

        public string MemberNickName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
