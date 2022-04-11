using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia1.Models.Posts;
using SocialMedia1.Services.Posts;

namespace SocialMedia1.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService postService;
        private readonly UserManager<IdentityUser> userManager;

        public PostsController(IPostService postService, UserManager<IdentityUser> userManager)
        {
            this.postService = postService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(CreatePostViewModel model)
        {
            var userId = await userManager.GetUserIdAsync(await userManager.GetUserAsync(User));

            await postService.CreatePostAsync(userId, model.Content);

            return Redirect("/"); //!!!
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateGroupPost(CreatePostViewModel model)
        {
            var userId = await userManager.GetUserIdAsync(await userManager.GetUserAsync(User));

            await postService.CreateGroupPostAsync(model.Id, userId, model.Content);

            return Redirect($"/Group/{model.Id}"); //!!!
        }

        [Authorize]
        public IActionResult GroupFeed()
        {
            var userId = userManager.GetUserId(HttpContext.User);

            var model = postService.GetAllPostsInUsersGroups(userId);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            var userId = await userManager.GetUserIdAsync(await userManager.GetUserAsync(User));

            await postService.DeletePostAsync(Id, userId);

            return Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Report(string id)
        {
            var userId = await userManager.GetUserIdAsync(await userManager.GetUserAsync(User));

            await postService.ReportPostAsync(id, userId);

            return Redirect("/");
        }
    }
}