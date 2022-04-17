using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia1.Models;
using SocialMedia1.Models.Groups;
using SocialMedia1.Services.Groups;

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
        public async Task<IActionResult> Create(GroupInputModel model)
        {
            var userId = await userManager.GetUserIdAsync(await userManager.GetUserAsync(User));

            await groupService.CreateGroupAsync(model.Name, model.Description, model.IsPrivate, userId);

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
        public async Task<IActionResult> JoinGroup(string id)
        {
            var userId = await userManager.GetUserIdAsync(await userManager.GetUserAsync(User));

            if (await groupMemberActionsService.IsGroupPrivateAsync(id))
            {
                if (await groupMemberActionsService.IsJoinRequstSentAsync(id, userId) == false)
                {
                    await groupMemberActionsService.SendJoinRequestAsync(id, userId);

                    return Redirect($"/Group/{id}");
                }

                return Redirect($"/Group/{id}");
            }

            await groupMemberActionsService.JoinGroupAsync(id, userId);

            return Redirect($"/Group/{id}");
        }

        [Authorize]
        public async Task<IActionResult> SendJoinRequest(string id)
        {
            var userId = await userManager.GetUserIdAsync(await userManager.GetUserAsync(User));

            await groupMemberActionsService.SendJoinRequestAsync(id, userId);

            return Redirect($"/Group/{id}");
        }

        [Authorize]
        public async Task<IActionResult> JoinRequests(string id)
        {
            var userId = await userManager.GetUserIdAsync(await userManager.GetUserAsync(User));

            if (groupMemberActionsService.IsUserGroupCreatorAsync(userId, id) == false)
            {
                return Redirect($"/Group/{id}");
            }

            var model = await groupService.GetJoinGroupRequestsAsync(id);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> ApproveJoinRequest(string requesterId, string groupId)
        {
            if (await groupMemberActionsService.IsGroupPrivateAsync(groupId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            if (await groupMemberActionsService.IsUserGroupMemberAsync(requesterId, groupId))
            {
                return Redirect($"/Group/{groupId}");
            }

            if (await groupMemberActionsService.IsJoinRequstSentAsync(groupId, requesterId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            await groupMemberActionsService.ApproveJoinRequestAsync(requesterId, groupId);

            return Redirect($"/Groups/JoinRequests/{groupId}");
        }

        [Authorize]
        public async Task<IActionResult> DeleteJoinRequest(string requesterId, string groupId)
        {
            if (await groupMemberActionsService.IsGroupPrivateAsync(groupId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            if (await groupMemberActionsService.IsUserGroupMemberAsync(requesterId, groupId))
            {
                return Redirect($"/Group/{groupId}");
            }

            if (await groupMemberActionsService.IsJoinRequstSentAsync(groupId, requesterId) == false)
            {
                return Redirect($"/Group/{groupId}");
            }

            await groupMemberActionsService.DeleteJoinRequestAsync(requesterId, groupId);

            return Redirect($"/Group/{groupId}");
        }

        [Authorize]
        public async Task<IActionResult> LeaveGroup(string id)
        {
            var userId = await userManager.GetUserIdAsync(await userManager.GetUserAsync(User));

            await groupMemberActionsService.LeaveGroupAsync(id, userId);

            return Redirect($"/Group/{id}");
        }

        [Authorize]
        public async Task<IActionResult> KickUser(string userId, string groupId)
        {
            await groupMemberActionsService.LeaveGroupAsync(groupId, userId);

            return Redirect($"/Groups/Members/{groupId}");
        }

        [Authorize]
        public async Task<IActionResult> Members(string id)
        {
            var model = await groupService.GetMembersAsync(id);

            return View(model);
        }
    }
}
