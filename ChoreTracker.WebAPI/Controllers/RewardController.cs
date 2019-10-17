using ChoreTracker.Models.ResponseModels;
using ChoreTracker.Models.RewardModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChoreTracker.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Reward")]
    public class RewardController : BaseController
    {
        [HttpPost]
        public IHttpActionResult CreateReward(RewardCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = GetRewardService();
            var requestResponse = service.CreateReward(model);
            return ValidateRequestResponse(requestResponse);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetRewardByID(int id)
        {
            var service = GetRewardService();
            var requestResponse = service.GetRewardDetailResponseByID(id);
            return ValidateModelRequestResponse<RewardDetail>(requestResponse);
        }

        [HttpGet]
        [Route("G/{id}")]
        public IHttpActionResult GetRewardsByGroupID(int id)
        {
            var service = GetRewardService();
            var model = service.GetRewardsByGroupID(id);
            return Ok(model);
        }
    }
}
