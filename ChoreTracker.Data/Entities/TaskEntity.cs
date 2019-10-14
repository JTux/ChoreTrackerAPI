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
    public class TaskEntity
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string TaskName { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTimeOffset CreatedUtc { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsComplete { get; set; }

        [Required]
        [Range(0, 9000.01)]
        public double RewardValue { get; set; }

        [ForeignKey(nameof(Group))]
        public int GroupId { get; set; }
        public virtual GroupEntity Group { get; set; }

        public virtual ICollection<CompletedTaskEntity> Completions { get; set; }
    }
}
