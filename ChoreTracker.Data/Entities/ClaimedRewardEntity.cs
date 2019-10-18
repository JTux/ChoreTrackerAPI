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
    public class ClaimedRewardEntity
    {
        [Key]
        public int ClaimedRewardId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTimeOffset ClaimedUtc { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Awarded { get; set; }

        [ForeignKey(nameof(GroupMember))]
        public int GroupMemberId { get; set; }
        public virtual GroupMemberEntity GroupMember { get; set; }

        [ForeignKey(nameof(Reward))]
        public int RewardId { get; set; }
        public virtual RewardEntity Reward { get; set; }
    }
}
