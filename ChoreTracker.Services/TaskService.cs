using ChoreTracker.Data;
using ChoreTracker.Models.ResponseModels;
using ChoreTracker.Models.TaskModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services
{
    public class TaskService : BaseService
    {
        private readonly Guid _userId;
        private readonly ApplicationDbContext _context;
        public TaskService(Guid userId)
        {
            _userId = userId;
            _context = new ApplicationDbContext();
        }

        public IEnumerable<TaskDetail> GetTasksByGroupID(int groupId)
        {
            var userMembership = _context.GroupMembers.FirstOrDefault(gm => gm.GroupId == groupId && gm.UserId == _userId.ToString());
            if (userMembership == null)
                return null;

            var tasks = userMembership.Group.Tasks.Select(t =>
                    new TaskDetail
                    {
                        TaskId = t.TaskId,
                        TaskName = t.TaskName,
                        Description = t.Description,
                        RewardValue = t.RewardValue,
                        IsComplete = t.IsComplete,
                        GroupId = t.GroupId,
                        GroupName = t.Group.GroupName,
                        CreatedUtc = t.CreatedUtc
                    }).ToList();

            return tasks;
        }

        public RequestResponse GetTaskByID(int taskId)
        {
            var userMembership = _context.GroupMembers.FirstOrDefault(gm => gm.GroupId == taskId && gm.UserId == _userId.ToString());
            if (userMembership == null)
                return BadResponse("Invalid permissions");

            var task = userMembership.Group.Tasks.FirstOrDefault(t => t.TaskId == taskId);
            if (task == null)
                return BadResponse("Invalid task ID");

            var model = new TaskDetail(task.TaskId, task.TaskName, task.Description, task.CreatedUtc, task.IsComplete, task.RewardValue, task.GroupId);

            return OkModelResponse("Task found.", model);
        }
    }
}
