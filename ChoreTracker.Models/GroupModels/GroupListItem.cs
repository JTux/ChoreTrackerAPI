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
        public string GroupName { get; set; }
        public List<GroupMemberDetail> Members { get; set; }
    }
}
