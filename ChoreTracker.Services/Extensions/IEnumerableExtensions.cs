using ChoreTracker.Data.Entities;
using ChoreTracker.Models.CompletedTaskModels;
using ChoreTracker.Models.GroupMemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Extension method that turns an IEnumerable of strings into a single string separated by spaces.
        /// </summary>
        /// <param name="enumerable">Given collection of strings that will be turned into a single continuous string.</param>
        /// <returns>A single string that holds all of the original string values.</returns>
        public static string ToSingleString(this IEnumerable<string> enumerable)
        {
            if (enumerable.Count() == 1)
                return enumerable.ElementAt(0);

            var newString = "";

            foreach (var str in enumerable)
            {
                newString += $"{str}{((str == enumerable.ElementAt(enumerable.Count() - 1)) ? "" : " ")}";
            }

            return newString;
        }

        /// <summary>
        /// Extension method that turns an IEnumerable of GroupMemberEntity objects into a List of GroupMemberDetail objects.
        /// </summary>
        /// <param name="members">Collection of GroupMemberEntities pulled from the database.</param>
        /// <returns>A converted List of GroupMemberDetail from the given Entity collection.</returns>
        public static List<GroupMemberDetail> ToGroupMemberDetailList(this IEnumerable<GroupMemberEntity> members)
        {
            var memberDetails = new List<GroupMemberDetail>();

            foreach (var member in members)
                memberDetails.Add(new GroupMemberDetail
                {
                    GroupMemberId = member.GroupMemberId,
                    IsOfficer = member.IsOfficer,
                    FirstName = member.User.FirstName,
                    LastName = member.User.LastName,
                    MemberNickname = member.MemberNickname
                });

            return memberDetails;
        }

        /// <summary>
        /// Extension method that turns an IEnumerable of CompletedTaskEntity objects into a List of CompletedTaskDetail objects.
        /// </summary>
        /// <param name="completedTasks">Collection of CompletedTaskEntities pulled from the DbContext.</param>
        /// <returns>A converted List of CompletedTaskDetail from the given Entity collection.</returns>
        public static List<CompletedTaskDetail> ToDetailList(this IEnumerable<CompletedTaskEntity> completedTasks)
        {
            var completedTaskDetails = new List<CompletedTaskDetail>();

            foreach (var task in completedTasks)
                completedTaskDetails.Add(new CompletedTaskDetail
                {
                    CompletedTaskId = task.CompletedTaskId,
                    CompletedUtc = task.CompletedUtc,
                    IsValid = task.IsValid,
                    Member = task.GroupMember.User.UserName,
                    TaskId = task.TaskId,
                });

            return completedTaskDetails;
        }
    }
}
