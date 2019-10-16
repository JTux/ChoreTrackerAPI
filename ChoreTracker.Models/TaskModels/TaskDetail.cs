using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoreTracker.Models.CompletedTaskModels;

namespace ChoreTracker.Models.TaskModels
{
    public class TaskDetail
    {
        public TaskDetail() { }

        public TaskDetail(int taskId, string taskName, string description, DateTimeOffset createdUtc, bool isComplete, double rewardValue, int groupId)
        {
            TaskId = taskId;
            TaskName = taskName;
            Description = description;
            CreatedUtc = createdUtc;
            IsComplete = isComplete;
            RewardValue = rewardValue;
            GroupId = groupId;
        }

        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
        public double RewardValue { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<CompletedTaskDetail> Completions { get; set; }
    }
}
