using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
        public async Task<IActionResult> EditUserProfile()
        {
            var model = await userProfileService.GetUserProfileDataAsync(userManager.GetUserId(HttpContext.User));

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditUserProfile(ProfileViewModel model)
        {
            await userProfileService
                .EditUserProfileAsync(userManager
                .GetUserId(HttpContext.User), model.Nickname, model.Name, model.Surname, model.IsPrivate, model.City, model.Birthday, model.EmailAddress, model.Bio);

  //          Account account = new Account(
  //"dani03",
  //"714726182434833",
  //"BuFfSQmUk6tZXXZk0tM88CZM3nM");

  //          Cloudinary cloudinary = new Cloudinary(account);

  //          var uploadParams = new ImageUploadParams()
  //          {
  //              File = new FileDescription(@"https://upload.wikimedia.org/wikipedia/commons/a/ae/Olympic_flag.jpg"),
  //              PublicId = "olympic_flag"
  //          };
  //          var uploadResult = cloudinary.Upload(uploadParams);


            return View();
        }

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var model = await userProfileService.GetUserProfileDataAsync(userManager.GetUserId(HttpContext.User));

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {
            if (id == userManager.GetUserId(HttpContext.User))
            {
                return Redirect("/UserProfiles/MyProfile");
            }

            var model = await userProfileService.GetUserProfileDataAsync(id);

            if (model is null)
            {
                return Redirect("/");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Follow(string id)
        {
            await userActionsService.FollowUserAsync(id, userManager.GetUserId(HttpContext.User));

            return Redirect($"/UserProfiles/Profile/{id}");
        }

        [Authorize]
        public async Task<IActionResult> Unfollow(string id)
        {
            await userActionsService.UnfollowUserAsync(id, userManager.GetUserId(HttpContext.User));

            return Redirect($"/UserProfiles/Profile/{id}");
        }

        [Authorize]
        public async Task<IActionResult> ApproveFollowRequest(string requesterId, string currentUserId)
        {
            await userActionsService.ApproveFollowRequestAsync(requesterId, currentUserId);

            return Redirect("/Home/FollowRequests");
        }

        [Authorize]
        public async Task<IActionResult> DeleteFollowRequest(string requesterId)
        {
            await userActionsService.DeleteRequestAsync(requesterId);

            return Redirect("/Home/FollowRequests");
        }

        [Authorize]
        public IActionResult Followers(string id)
        {
            var model = userProfileService.GetAllFollowers(id);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> RemoveFollower(string id)
        {
            var currentUser = userManager.GetUserId(HttpContext.User);

            await userActionsService.RemoveFollowerAsync(currentUser, id);

            return Redirect($"/UserProfiles/Followers/{currentUser}");
        }

        [Authorize]
        public async Task<IActionResult> Following(string id)
        {
            var model = await userProfileService.GetAllFollowingAsync(id);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> UnfollowUser(string id)
        {
            string currentUser = userManager.GetUserId(HttpContext.User);

            await userActionsService.UnfollowUserAsync(id, currentUser);

            return Redirect($"/UserProfiles/Following/{currentUser}");
        }
    }
}
