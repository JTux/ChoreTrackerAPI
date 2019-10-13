using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Models.GroupMemberModels;
using ChoreTracker.Models.ResponseModels;

namespace ChoreTracker.Services
{
    public class GroupMemberService : BaseService
    {
        private readonly Guid _userId;
        private readonly ApplicationDbContext _context;

        public GroupMemberService(Guid userId)
        {
            _userId = userId;
            _context = new ApplicationDbContext();
        }

        public RequestResponse GetMemberDetail(int memberId)
        {
            var memberEntity = _context.GroupMembers.Find(memberId);
            if (memberEntity == null)
                return BadResponse("Invalid member ID.");

            var userMembership = memberEntity.Group.GroupMembers.FirstOrDefault(m => m.GroupId == memberEntity.GroupId && _userId.ToString() == m.UserId);

            if (userMembership == null)
                return BadResponse("Invalid permissions.");

            var nickname = (!string.IsNullOrEmpty(memberEntity.MemberNickname))
                                ? memberEntity.MemberNickname
                                : $"{memberEntity.User.FirstName} {memberEntity.User.LastName}";

            var memberDetail = new GroupMemberDetail
            {
                GroupMemberId = memberEntity.GroupMemberId,
                IsAccepted = memberEntity.IsAccepted,
                IsOfficer = memberEntity.IsOfficer,
                MemberNickname = nickname,
                FirstName = memberEntity.User.FirstName,
                LastName = memberEntity.User.LastName
            };

            return OkModelResponse("Member detail found.", memberDetail);
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

        public RequestResponse ToggleOfficer(int memberId)
        {
            var member = _context.GroupMembers.Find(memberId);

            if (member == null)
                return BadResponse("Invalid member information.");

            if (member.Group.OwnerId != _userId)
                return BadResponse("Invalid permissions.");

            return (member.IsOfficer) ? DemoteOfficer(member) : PromoteOfficer(member);
        }

        private RequestResponse PromoteOfficer(GroupMemberEntity member)
        {
            member.IsOfficer = true;

            if (_context.SaveChanges() != 1)
                return BadResponse("Unable to save changes.");

            return OkResponse("Member promoted.");
        }

        private RequestResponse DemoteOfficer(GroupMemberEntity member)
        {
            if (member.UserId == _userId.ToString())
                return BadResponse("Cannot demote yourself.");

            member.IsOfficer = false;

            if (_context.SaveChanges() != 1)
                return BadResponse("Unable to save changes.");

            return OkResponse("Member demoted.");
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
