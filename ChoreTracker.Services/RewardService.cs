using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Models.ResponseModels;
using ChoreTracker.Models.RewardModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services
{
    public class RewardService : BaseService
    {
        private readonly Guid _userId;
        private readonly ApplicationDbContext _context;
        public RewardService(Guid currentUserId)
        {
            _userId = currentUserId;
            _context = new ApplicationDbContext();
        }

        public RequestResponse CreateReward(RewardCreate model)
        {
            var userMembership = GetUserMembership(model.GroupId);
            if (userMembership == null || !userMembership.IsOfficer)
                return BadResponse("Invalid permissions.");

            var rewardEntity = new RewardEntity(model.RewardName, model.Cost, model.NumberAvailable, model.GroupId);

            _context.Rewards.Add(rewardEntity);
            if (_context.SaveChanges() != 1)
                return BadResponse("Could not save reward.");

            return OkResponse("Reward created successfully.");
        }

        public RequestResponse GetRewardDetailResponseByID(int rewardId)
        {
            var reward = _context.Rewards.Find(rewardId);
            if (reward == null)
                return BadResponse("Invalid Reward ID.");

            var userMembership = reward.Group.GroupMembers.FirstOrDefault(gm => gm.UserId == _userId.ToString());
            if (userMembership == null || !userMembership.IsAccepted)
                return BadResponse("Invalid permissions.");

            var rewardDetail = new RewardDetail
            {
                RewardId = reward.RewardId,
                RewardName = reward.RewardName,
                Cost = reward.Cost,
                NumberAvailable = reward.NumberAvailable,
                GroupId = reward.GroupId
            };

            return OkModelResponse("Reward found successfully.", rewardDetail);
        }

        public IEnumerable<RewardDetail> GetRewardsByGroupID(int groupId)
        {
            var userMembership = GetUserMembership(groupId);
            if (userMembership == null || !userMembership.IsAccepted)
                return null;

            var rewards = userMembership.Group.Rewards.Select(r =>
                new RewardDetail
                {
                    RewardId = r.RewardId,
                    RewardName = r.RewardName,
                    NumberAvailable = r.NumberAvailable,
                    GroupId = r.GroupId,
                    Cost = r.Cost
                }).ToList();

            return rewards;
        }

        private GroupMemberEntity GetUserMembership(int groupId)
        {
            return _context.GroupMembers.FirstOrDefault(gm =>
                gm.UserId == _userId.ToString() &&
                gm.GroupId == groupId);
        }
    }
}
