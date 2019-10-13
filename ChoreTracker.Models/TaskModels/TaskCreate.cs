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
        public string TaskName { get; set; }

        public string Description { get; set; }

        [Required]
        public double RewardValue { get; set; }
    }
}
