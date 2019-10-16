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
    public class CompletedTaskEntity
    {
        [Key]
        public int CompletedTaskId { get; set; }

        [Required]
        public DateTimeOffset CompletedUtc { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsValid { get; set; }

        [ForeignKey(nameof(Task))]
        public int TaskId { get; set; }
        public virtual TaskEntity Task { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
