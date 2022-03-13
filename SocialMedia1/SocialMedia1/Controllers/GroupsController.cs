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
        [Authorize]
        public IActionResult Group(string id)
        {
            var model = groupService.GetGroup(id);

            return View(model);
        }

        [Authorize]
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

        [Authorize]
        public IActionResult SendJoinRequest(string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            groupService.SendJoinRequest(id, userId);

            return Redirect($"/Group/{id}");
        }

        [Authorize]
        public IActionResult JoinRequests(string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            if (groupService.IsUserGroupCreator(userId, id) == false)
            {
                return Redirect($"/Group/{id}");
            }

            var model = groupService.GetJoinGroupRequests(id);

            return View(model);
        }

        [Authorize]
        public IActionResult ApproveJoinRequest(string requesterId, string groupId)
        {
            if (groupService.IsGroupPrivate(groupId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            if (groupService.IsUserGroupMember(requesterId, groupId))
            {
                return Redirect($"/Group/{groupId}");
            }

            if (groupService.IsJoinRequstSent(groupId, requesterId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            groupService.ApproveJoinRequest(requesterId, groupId);

            return Redirect($"/Groups/JoinRequests/{groupId}");
        }

        [Authorize]
        public IActionResult DeleteJoinRequest(string requesterId, string groupId)
        {
            if (groupService.IsGroupPrivate(groupId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            if (groupService.IsUserGroupMember(requesterId, groupId))
            {
                return Redirect($"/Group/{groupId}");
            }

            if (groupService.IsJoinRequstSent(groupId, requesterId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            groupService.DeleteJoinRequest(requesterId, groupId);

            return Redirect($"/Group/{groupId}");
        }

        [Authorize]
        public IActionResult LeaveGroup(string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            groupService.LeaveGroup(id, userId);

            return Redirect($"/Group/{id}");
        }

        [Authorize]
        public IActionResult KickUser(string userId, string groupId)
        {
            groupService.LeaveGroup(groupId, userId);

            return Redirect($"/Groups/Members/{groupId}");
        }


        [HttpPost]
        [Authorize]
        public IActionResult CreatePost(CreatePostViewModel model, string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            groupService.CreatePost(id, userId, model.Content);

            return Redirect($"/Group/{id}"); //!!!
        }

        [Authorize]
        public IActionResult Members(string id)
        {
            var model = groupService.GetMembers(id);

            return View(model);
        }
    }
}
