using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Models.ResponseModels;
using ChoreTracker.Models.TaskModels;
using ChoreTracker.Services.Extensions;
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
            var userMembership = GetUserMembership(groupId);

            if (userMembership == null || !userMembership.IsAccepted)
                return null;

            var tasks = userMembership.Group.Tasks.Select(t =>
                    new TaskDetail
                    {
                        TaskId = t.TaskId,
                        TaskName = t.TaskName,
                        Description = t.Description,
                        RewardValue = t.RewardValue,
                        IsComplete = t.Completions.Where(c => c.IsValid == true).Count() > 0,
                        GroupId = t.GroupId,
                        GroupName = t.Group.GroupName,
                        CreatedUtc = t.CreatedUtc,
                        Completions = (userMembership.IsOfficer) ? t.Completions.ToDetailList() : null
                    }).ToList();

            return tasks;
        }

        public RequestResponse GetTaskByID(int taskId)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
            if (task == null)
                return BadResponse("Invalid task ID");

            var userMembership =
                task.Group.GroupMembers.FirstOrDefault(gm => gm.GroupId == task.GroupId && gm.UserId == _userId.ToString() && gm.IsAccepted);

            if (userMembership == null)
                return BadResponse("Invalid permissions");

            var isComplete = task.Completions.Where(c => c.IsValid == true).Count() > 0;

            var model = new TaskDetail(task.TaskId, task.TaskName, task.Description, task.CreatedUtc, isComplete, task.RewardValue, task.GroupId)
            {
                Completions = task.Completions.ToDetailList()
            };

            return OkModelResponse("Task found.", model);
        }

        public RequestResponse CreateTask(TaskCreate model)
        {
            if (model == null)
                return BadResponse("Request Body was empty.");

            var userMembership = GetUserMembership(model.GroupId);

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
            if (model == null)
                return BadResponse("Request Body was empty.");

            var taskEntity = _context.Tasks.Find(model.TaskId);
            if (taskEntity == null)
                return BadResponse("Invalid task ID.");

            if (!taskEntity.Group.GroupMembers.FirstOrDefault(m => m.UserId == _userId.ToString()).IsOfficer)
                return BadResponse("Invalid permissions.");

            taskEntity.TaskName = model.TaskName;
            taskEntity.Description = model.Description;
            taskEntity.GroupId = model.GroupId;
            taskEntity.RewardValue = model.RewardValue;

            if (_context.SaveChanges() != 1)
                return BadResponse("Cannot save changes.");

            return OkResponse("Task updated");
        }

        public RequestResponse CompleteTask(int taskId)
        {
            var task = _context.Tasks.Find(taskId);
            if (task == null)
                return BadResponse("Invalid task ID.");

            var userMembership = task.Group.GroupMembers.FirstOrDefault(gm => gm.UserId == _userId.ToString() && gm.IsAccepted);
            if (userMembership == null)
                return BadResponse("Invalid permissions.");

            if (task.Completions.FirstOrDefault(c => c.UserId == _userId.ToString()) != null)
                return BadResponse("Already completed this task.");

            var completedTaskEntity = new CompletedTaskEntity
            {
                TaskId = taskId,
                UserId = _userId.ToString(),
                IsValid = false,
                CompletedUtc = DateTimeOffset.Now
            };

            _context.CompletedTasks.Add(completedTaskEntity);
            if (_context.SaveChanges() != 1)
                return BadResponse("Cannot complete task.");

            return OkResponse("Task completed successfully.");
        }

        public RequestResponse ValidateCompletedTask(int completedTaskId)
        {
            var completedTask = _context.CompletedTasks.Find(completedTaskId);
            if (completedTask == null)
                return BadResponse("Invalid completed task ID.");

            var userMembership = completedTask.Task.Group.GroupMembers.FirstOrDefault(m => m.UserId == _userId.ToString() && m.IsAccepted);
            if (userMembership == null || !userMembership.IsOfficer)
                return BadResponse("Invalid permissions.");

            if (completedTask.IsValid)
                return BadResponse("Completed task already marked valid.");

            completedTask.IsValid = true;
            userMembership.EarnedPoints += Convert.ToInt32(completedTask.Task.RewardValue);
            if (_context.SaveChanges() != 2)
                return BadResponse("Cannot save completed task.");

            return OkResponse("Completed task validated successfully.");
        }

        public RequestResponse DeleteTaskByID(int taskId)
        {
            var task = _context.Tasks.Find(taskId);
            if (task == null)
                return BadResponse("Invalid task ID.");

            var userMembership = task.Group.GroupMembers.FirstOrDefault(gm => gm.UserId == _userId.ToString() && gm.IsAccepted);
            if (userMembership == null || !userMembership.IsOfficer)
                return BadResponse("Invalid permissions");

            _context.Tasks.Remove(task);
            if (_context.SaveChanges() != 1)
                return BadResponse("Cannot delete task.");

            return OkResponse("Task deleted successfully.");
        }

        private GroupMemberEntity GetUserMembership(int groupId)
        {
            return _context.GroupMembers.FirstOrDefault(gm =>
                gm.UserId == _userId.ToString() &&
                gm.GroupId == groupId);
        }
    }
}
