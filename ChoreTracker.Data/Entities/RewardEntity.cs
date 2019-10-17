using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Data.Entities
{
    public class RewardEntity
    {
        public RewardEntity() { }
        public RewardEntity(string rewardName, int cost, int numberAvailable, int groupId)
        {
            RewardName = rewardName;
            Cost = cost;
            NumberAvailable = numberAvailable;
            GroupId = groupId;
        }

        [Key]
        public int RewardId { get; set; }

        [Required]
        public string RewardName { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Cost { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int NumberAvailable { get; set; }

        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        public virtual GroupEntity Group { get; set; }
    }
}
