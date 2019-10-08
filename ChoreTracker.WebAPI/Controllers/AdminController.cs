using ChoreTracker.Data;
using ChoreTracker.Models.AdminModels;
using ChoreTracker.Services.Extensions;
using ChoreTracker.WebAPI.Data;
using ChoreTracker.WebAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChoreTracker.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        [HttpPost]
        [Route("Register")]
        public IHttpActionResult CreateAdminAccount(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newAdmin = new ApplicationUser() { UserName = model.Username, Email = model.Email };

            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var userCreationResult = userManager.Create(newAdmin, model.Password);

            if (userCreationResult.Succeeded)
            {
                var roleSetResult = userManager.AddToRole(newAdmin.Id, RoleNames.Admin);

                if (!roleSetResult.Succeeded)
                    return InternalServerError(new Exception("Role could not be assigned."));
            }
            else
                return BadRequest(userCreationResult.Errors.ToSingleString());

            return Ok();
        }

        [HttpPut]
        [Route("ToggleAdmin")]
        public IHttpActionResult ToggleAdminRole(UserInfo userInfo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                var user = userManager.FindByEmail(userInfo.Email);

                if (User.Identity.GetUserId() == user.Id)
                    return BadRequest("Cannot demote yourself you big dummy.");

                bool userIsAdmin = userManager.IsInRole(user.Id, RoleNames.Admin);

                string currentRole = (userIsAdmin) ? RoleNames.Admin : RoleNames.User;
                string newRole = (userIsAdmin) ? RoleNames.User : RoleNames.Admin;

                var roleRemoveResult = userManager.RemoveFromRole(user.Id, currentRole);
                if (!roleRemoveResult.Succeeded)
                    return InternalServerError(new Exception($"Could not remove from {currentRole} role."));

                var roleAddResult = userManager.AddToRole(user.Id, newRole);
                if (!roleAddResult.Succeeded)
                    return InternalServerError(new Exception($"Could not add to {newRole} role."));

                return Ok($"User added to {newRole} role.");
            }
        }
    }
}
