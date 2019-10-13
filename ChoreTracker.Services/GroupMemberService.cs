using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoreTracker.Data;
using ChoreTracker.Models.GroupMemberModels;
using ChoreTracker.Models.ResponseModels;

namespace ChoreTracker.Services
{
    public class GroupMemberService : BaseService
    {
        private Guid _userId;
        private readonly ApplicationDbContext _context;

        public GroupMemberService(Guid userId)
        {
            _userId = userId;
            _context = new ApplicationDbContext();
        }

        public GroupMemberDetail GetMemberDetail(int memberId)
        {
            var memberEntity = _context.GroupMembers.Find(memberId);
            if (memberEntity == null)
                return null;

            var userMembership =
                _context.GroupMembers.FirstOrDefault(m => m.GroupId == memberEntity.GroupId && _userId.ToString() == m.UserId);
            if (userMembership == null)
                return null;

            var memberDetail = new GroupMemberDetail
            {
                GroupMemberId = memberEntity.GroupMemberId,
                IsAccepted = memberEntity.IsAccepted,
                IsOfficer = memberEntity.IsOfficer,
                MemberNickname = memberEntity.MemberNickname,
                FirstName = memberEntity.User.FirstName,
                LastName = memberEntity.User.LastName
            };

            return memberDetail;
        }

        public RequestResponse AcceptApplicant(int applicantId)
        {
            var applicant = _context.GroupMembers.Find(applicantId);
            if (applicant == null)
                return BadResponse("Applicant does not exist.");

            if (!UserIsOfficer(applicant.GroupId))
                return BadResponse("Invalid permissions.");

            applicant.IsAccepted = true;
            if (_context.SaveChanges() != 1)
                return BadResponse("Could not accept applicant.");

            return OkResponse("Member successfully added.");
        }

        public RequestResponse DeclineApplicant(int applicantId)
        {
            var applicant = _context.GroupMembers.Find(applicantId);
            if (applicant == null || applicant.IsAccepted)
                return BadResponse("Applicant does not exist.");

            if (!UserIsOfficer(applicant.GroupId))
                return BadResponse("Invalid permissions.");

            _context.GroupMembers.Remove(applicant);
            if (_context.SaveChanges() != 1)
                return BadResponse("Could not remove applicant.");

            return OkResponse("Applicant successfully removed.");
        }

        public RequestResponse RemoveMember(int memberId)
        {
            var member = _context.GroupMembers.Find(memberId);
            if (member == null || !member.IsAccepted)
                return BadResponse("Member does not exist");

            var userMembership =
                _context.GroupMembers.FirstOrDefault(m => m.UserId == _userId.ToString() && m.GroupId == member.GroupId);

            if ((member.IsOfficer && userMembership.Group.OwnerId != _userId) || !UserIsOfficer(member.GroupId))
                return BadResponse("Insufficient permissions.");

            _context.GroupMembers.Remove(member);
            if (_context.SaveChanges() != 1)
                return BadResponse("Could not remove member.");

            return OkResponse("Member successfully removed.");
        }

        public RequestResponse UpdateNickname(MemberNicknameUpdate model)
        {
            var member = _context.GroupMembers.FirstOrDefault(gm => gm.GroupMemberId == model.GroupMemberId);

            if (member == null)
                return BadResponse("Invalid member information.");

            if (member.UserId != _userId.ToString() && !UserIsOfficer(member.GroupId))
                return BadResponse("Invalid permissions.");

            member.MemberNickname = model.NewNickname;
            if (_context.SaveChanges() != 1)
                return BadResponse("Could not update nickname.");

            return OkResponse("Nickname updated.");
        }

        private bool UserIsOfficer(int groupId)
        {
            var userMembership =
                    _context.GroupMembers.FirstOrDefault(m => m.UserId == _userId.ToString() && m.GroupId == groupId);

            if (userMembership == null)
                return false;

            return userMembership.IsOfficer;
        }
    }
}
