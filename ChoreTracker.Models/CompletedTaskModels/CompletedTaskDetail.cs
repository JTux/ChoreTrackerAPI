using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.CompletedTaskModels
{
    public class CompletedTaskDetail
    {
        public int CompletedTaskId { get; set; }
        public DateTimeOffset CompletedUtc { get; set; }
        public bool IsValid { get; set; }
        public string Member { get; set; }
        public int TaskId { get; set; }
    }
}
