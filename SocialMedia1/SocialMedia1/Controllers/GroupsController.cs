using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia1.Models;
using SocialMedia1.Services;

namespace SocialMedia1.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IGroupService groupService;
        private readonly UserManager<IdentityUser> userManager;

        public GroupsController(IGroupService groupService, UserManager<IdentityUser> userManager)
        {
            this.groupService = groupService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(GroupInputModel model)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            groupService.CreateGroup(model.Name, model.Description, model.IsPrivate, userId);

            return Redirect("/Home/Groups");
        }

        [HttpGet("/Group/{id}")]
        public IActionResult Group(string id)
        {
            var model = groupService.GetGroup(id);

            return View(model);
        }

        public IActionResult JoinGroup(string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            if (groupService.IsGroupPrivate(id))
            {
                if (groupService.IsJoinRequstSent(id, userId) == false)
                {
                    return SendJoinRequest(id);
                }

                return Redirect($"/Group/{id}");
            }

            groupService.JoinGroup(id, userId);

            return Redirect($"/Group/{id}");
        }

        public IActionResult SendJoinRequest(string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            groupService.SendJoinRequest(id, userId);

            return Redirect($"/Group/{id}");
        }

        public IActionResult JoinRequests(string id)
        {
            var model = groupService.GetJoinGroupRequests(id);

            return View(model);
        }

        public IActionResult LeaveGroup(string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            groupService.LeaveGroup(id, userId);

            return Redirect($"/Group/{id}");
        }


        [HttpPost]
        [Authorize]
        public IActionResult CreatePost(CreatePostViewModel model, string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            groupService.CreatePost(id, userId, model.Content);

            return Redirect($"/Group/{id}"); //!!!
        }
    }
}
