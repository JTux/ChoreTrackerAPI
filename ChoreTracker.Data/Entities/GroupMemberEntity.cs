using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Data.Entities
{
    public class GroupMemberEntity
    {
        [Key]
        public int GroupMemberId { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsOfficer { get; set; }

        public string MemberNickName { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        public virtual GroupEntity Group { get; set; }
    }
}
