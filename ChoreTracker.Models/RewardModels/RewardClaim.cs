using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.RewardModels
{
    public class RewardClaim
    {
        [Required]
        public int RewardId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
    }
}
