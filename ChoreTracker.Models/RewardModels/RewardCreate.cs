using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.RewardModels
{
    public class RewardCreate
    {
        [Required]
        [Display(Name = "Reward Name")]
        public string RewardName { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Cost")]
        public int Cost { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Number Available")]
        public int NumberAvailable { get; set; }

        [Required]
        [Display(Name = "Group ID")]
        public int GroupId { get; set; }
    }
}
