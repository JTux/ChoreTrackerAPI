using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.GroupMemberModels
{
    public class MemberNicknameUpdate
    {
        [Required]
        public int GroupMemberId { get; set; }

        [Required]
        public string NewNickname { get; set; }
    }
}
