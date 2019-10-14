using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
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
            var userMembership =
                _context.GroupMembers.FirstOrDefault(gm => gm.GroupId == groupId && gm.UserId == _userId.ToString());

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
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
            if (task == null)
                return BadResponse("Invalid task ID");

            var userMembership =
                task.Group.GroupMembers.FirstOrDefault(gm => gm.GroupId == task.GroupId && gm.UserId == _userId.ToString());

            if (userMembership == null)
                return BadResponse("Invalid permissions");

            var model = new TaskDetail(task.TaskId, task.TaskName, task.Description, task.CreatedUtc, task.IsComplete, task.RewardValue, task.GroupId);

            return OkModelResponse("Task found.", model);
        }

        public RequestResponse CreateTask(TaskCreate model)
        {
            var userMembership =
                _context.GroupMembers.FirstOrDefault(gm => gm.GroupId == model.GroupId && gm.UserId == _userId.ToString());

            if (userMembership == null || !userMembership.IsOfficer)
                return BadResponse("Invalid permissions.");

            var taskEntity = new TaskEntity
            {
                GroupId = model.GroupId,
                TaskName = model.TaskName,
                Description = model.Description,
                RewardValue = model.RewardValue,
                CreatedUtc = DateTimeOffset.Now
            };

            _context.Tasks.Add(taskEntity);

            if (_context.SaveChanges() != 1)
                return BadResponse("Cannot save new task.");

            return OkResponse("Task created successfully.");
        }

        public RequestResponse UpdateTask(TaskUpdate model)
        {
            var taskEntity = _context.Tasks.Find(model.TaskId);
            if (taskEntity == null)
                return BadResponse("Invalid task ID.");

            if (!taskEntity.Group.GroupMembers.FirstOrDefault(m => m.UserId == _userId.ToString()).IsOfficer)
                return BadResponse("Invalid permissions.");

            taskEntity.TaskName = model.TaskName;
            taskEntity.Description = model.Description;
            taskEntity.GroupId = model.GroupId;
            taskEntity.IsComplete = model.IsComplete;
            taskEntity.RewardValue = model.RewardValue;

            if (_context.SaveChanges() != 1)
                return BadResponse("Cannot save changes.");

            return OkResponse("Task updated");
        }
    }
}
