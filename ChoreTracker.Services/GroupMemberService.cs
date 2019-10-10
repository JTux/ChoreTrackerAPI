using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoreTracker.Data;
using ChoreTracker.Models.ResponseModels;

namespace ChoreTracker.Services
{
    public class GroupMemberService : BaseService
    {
        private Guid _userId;

        public GroupMemberService(Guid userId)
        {
            _userId = userId;
        }

        public RequestResponse AcceptApplicant(int applicantId)
        {
            using (var context = new ApplicationDbContext())
            {
                var applicant = context.GroupMembers.Find(applicantId);
                if (applicant == null)
                    return BadResponse("Applicant does not exist.");

                if (!UserIsOfficer(applicant.GroupId, context))
                    return BadResponse("Invalid permissions.");

                applicant.IsAccepted = true;
                if (context.SaveChanges() != 1)
                    return BadResponse("Could not accept applicant.");

                return OkResponse("Member successfully added.");
            }
        }

        public RequestResponse DeclineApplicant(int applicantId)
        {
            using (var context = new ApplicationDbContext())
            {
                var applicant = context.GroupMembers.Find(applicantId);
                if (applicant == null || applicant.IsAccepted)
                    return BadResponse("Applicant does not exist.");

                if (!UserIsOfficer(applicant.GroupId, context))
                    return BadResponse("Invalid permissions.");

                context.GroupMembers.Remove(applicant);
                if (context.SaveChanges() != 1)
                    return BadResponse("Could not remove applicant.");

                return OkResponse("Applicant successfully removed.");
            }
        }

        private bool UserIsOfficer(int groupId, ApplicationDbContext context)
        {
            var userMembership =
                    context.GroupMembers.FirstOrDefault(m => m.UserId == _userId.ToString() && m.GroupId == groupId);

            if (userMembership == null)
                return false;

            return userMembership.IsOfficer;
        }
    }
}
