using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.TaskModels
{
    public class TaskCreate
    {
        [Required]
        public int GroupId { get; set; }

        [Required]
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Reward Value")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}.")]
        public int RewardValue { get; set; }
    }
}
