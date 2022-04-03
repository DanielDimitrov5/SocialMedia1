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
        private readonly IGroupMemberActionsService groupMemberActionsService;
        private readonly UserManager<IdentityUser> userManager;

        public GroupsController(IGroupService groupService, IGroupMemberActionsService groupMemberActionsService, UserManager<IdentityUser> userManager)
        {
            this.groupService = groupService;
            this.groupMemberActionsService = groupMemberActionsService;
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
            if (!this.ModelState.IsValid)
            {

            }

            if (ModelState.IsValid)
            {

            }

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

            if (groupMemberActionsService.IsGroupPrivate(id))
            {
                if (groupMemberActionsService.IsJoinRequstSent(id, userId) == false)
                {
                    return SendJoinRequest(id);
                }

                return Redirect($"/Group/{id}");
            }

            groupMemberActionsService.JoinGroup(id, userId);

            return Redirect($"/Group/{id}");
        }

        [Authorize]
        public IActionResult SendJoinRequest(string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            groupMemberActionsService.SendJoinRequest(id, userId);

            return Redirect($"/Group/{id}");
        }

        [Authorize]
        public IActionResult JoinRequests(string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            if (groupMemberActionsService.IsUserGroupCreator(userId, id) == false)
            {
                return Redirect($"/Group/{id}");
            }

            var model = groupService.GetJoinGroupRequests(id);

            return View(model);
        }

        [Authorize]
        public IActionResult ApproveJoinRequest(string requesterId, string groupId)
        {
            if (groupMemberActionsService.IsGroupPrivate(groupId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            if (groupMemberActionsService.IsUserGroupMember(requesterId, groupId))
            {
                return Redirect($"/Group/{groupId}");
            }

            if (groupMemberActionsService.IsJoinRequstSent(groupId, requesterId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            groupMemberActionsService.ApproveJoinRequest(requesterId, groupId);

            return Redirect($"/Groups/JoinRequests/{groupId}");
        }

        [Authorize]
        public IActionResult DeleteJoinRequest(string requesterId, string groupId)
        {
            if (groupMemberActionsService.IsGroupPrivate(groupId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            if (groupMemberActionsService.IsUserGroupMember(requesterId, groupId))
            {
                return Redirect($"/Group/{groupId}");
            }

            if (groupMemberActionsService.IsJoinRequstSent(groupId, requesterId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            groupMemberActionsService.DeleteJoinRequest(requesterId, groupId);

            return Redirect($"/Group/{groupId}");
        }

        [Authorize]
        public IActionResult LeaveGroup(string id)
        {
            var userId = userManager.GetUserId(HttpContext.User);

            groupMemberActionsService.LeaveGroup(id, userId);

            return Redirect($"/Group/{id}");
        }

        [Authorize]
        public IActionResult KickUser(string userId, string groupId)
        {
            groupMemberActionsService.LeaveGroup(groupId, userId);

            return Redirect($"/Groups/Members/{groupId}");
        }

        [Authorize]
        public IActionResult Members(string id)
        {
            var model = groupService.GetMembers(id);

            return View(model);
        }
    }
}
