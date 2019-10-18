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
        public GroupMemberEntity() { }
        public GroupMemberEntity(int groupId, string userId, bool isAccepted, bool isOfficer)
        {
            GroupId = groupId;
            UserId = userId;
            IsAccepted = isAccepted;
            IsOfficer = isOfficer;
        }
        public GroupMemberEntity(int groupId, string userId, bool isAccepted, bool isOfficer, string nickname)
            : this(groupId, userId, isAccepted, isOfficer)
        {
            MemberNickname = nickname;
        }

        [Key]
        public int GroupMemberId { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsAccepted { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsOfficer { get; set; }

        public string MemberNickname { get; set; }

        [Range(0, int.MaxValue)]
        public int EarnedPoints { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        public virtual GroupEntity Group { get; set; }

        public ICollection<ClaimedRewardEntity> Rewards { get; set; }
    }
}
