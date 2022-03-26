using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia1.Models;
using SocialMedia1.Services;

namespace SocialMedia1.Controllers
{
    public class UserProfilesController : Controller
    {

        private readonly IUserProfileService userProfileService;
        private readonly IUserActionsService userActionsService;
        private readonly UserManager<IdentityUser> userManager;

        public UserProfilesController(IUserProfileService userProfileService, IUserActionsService userActionsService, UserManager<IdentityUser> userManager)
        {
            this.userProfileService = userProfileService;
            this.userActionsService = userActionsService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult EditUserProfile()
        {
            var model = userProfileService.GetUserProfileData(userManager.GetUserId(HttpContext.User));

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult EditUserProfile(ProfileViewModel model)
        {
            userProfileService
                .EditUserProfileAsync(userManager
                .GetUserId(HttpContext.User), model.Nickname, model.Name, model.Surname, model.IsPrivate, model.City, model.Birthday, model.EmailAddress, model.Bio);

            return View();
        }

        [Authorize]
        public IActionResult MyProfile()
        {
            var model = userProfileService.GetUserProfileData(userManager.GetUserId(HttpContext.User));

            return View(model);
        }

        [Authorize]
        public IActionResult Profile(string id)
        {
            if (id == userManager.GetUserId(HttpContext.User))
            {
                return Redirect("/UserProfiles/MyProfile");
            }

            var model = userProfileService.GetUserProfileData(id);

            if (model is null)
            {
                return Redirect("/");
            }

            return View(model);
        }

        [Authorize]
        public IActionResult Follow(string id)
        {
            userActionsService.FollowUser(id, userManager.GetUserId(HttpContext.User));

            return Redirect($"/UserProfiles/Profile/{id}");
        }

        [Authorize]
        public IActionResult Unfollow(string id)
        {
            userActionsService.UnfollowUser(id, userManager.GetUserId(HttpContext.User));

            return Redirect($"/UserProfiles/Profile/{id}");
        }

        [Authorize]
        public IActionResult ApproveFollowRequest(string requesterId, string currentUserId)
        {
            userActionsService.ApproveFollowRequest(requesterId, currentUserId);

            return Redirect("/Home/FollowRequests");
        }

        [Authorize]
        public IActionResult DeleteFollowRequest(string requesterId)
        {
            userActionsService.DeleteRequest(requesterId);

            return Redirect("/Home/FollowRequests");
        }

        [Authorize]
        public IActionResult Followers(string id)
        {
            var model = userProfileService.GetAllFollowers(id);

            return View(model);
        }

        [Authorize]
        public IActionResult RemoveFollower(string id)
        {
            var currentUser = userManager.GetUserId(HttpContext.User);

            userActionsService.RemoveFollower(currentUser, id);

            return Redirect($"/UserProfiles/Followers/{currentUser}");
        }

        [Authorize]
        public IActionResult Following(string id)
        {
            var model = userProfileService.GetAllFollowing(id);

            return View(model);
        }

        [Authorize]
        public IActionResult UnfollowUser(string id)
         {
            string currentUser = userManager.GetUserId(HttpContext.User);

            userActionsService.UnfollowUser(id, currentUser);

            return Redirect($"/UserProfiles/Following/{currentUser}");
        }
    }
}
