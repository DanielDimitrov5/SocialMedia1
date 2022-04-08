using SocialMedia1.Data;
using SocialMedia1.Data.Models;

namespace SocialMedia1.Services.Groups
{
    public class GroupMemberActionsService : IGroupMemberActionsService
    {
        private readonly ApplicationDbContext context;

        public GroupMemberActionsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task ApproveJoinRequestAsync(string requesterId, string groupId)
        {
            UserProfile requester = await context.UserProfiles.FindAsync(requesterId);
            Group group = await context.Groups.FindAsync(groupId);
            
            if (requester == null || group == null)
            {
                return;
            }

            await JoinGroupAsync(groupId, requesterId);

            UserGroupRequest followRequest = context.JoinGroupRequest.FirstOrDefault(x => x.UserProfileId == requesterId && x.GroupId == groupId);

            context.JoinGroupRequest.Remove(followRequest);

            await context.SaveChangesAsync();
        }

        public async Task DeleteJoinRequestAsync(string requesterId, string groupId)
        {
            var request = context.JoinGroupRequest.FirstOrDefault(x => x.UserProfileId == requesterId && x.GroupId == groupId);

            if (request == null)
            {
                return;
            }

            context.JoinGroupRequest.Remove(request);

            await context.SaveChangesAsync();
        }

        public bool IsGroupPrivate(string groupId)
        {
            return context.Groups.FirstOrDefault(x => x.Id == groupId).IsPrivate;
        }

        public bool IsJoinRequstSent(string groupId, string userId)
        {
            return context.JoinGroupRequest.Any(x => x.UserProfileId == userId && x.GroupId == groupId);
        }

        public bool IsUserGroupCreator(string userId, string groupId)
        {
            return context.Groups.Any(x => x.Id == groupId && x.CreaterId == userId);
        }

        public bool IsUserGroupMember(string userId, string groupId)
        {
            return context.UserProfilesGroups.Any(x => x.GroupId == groupId && x.UserProfileId == userId);
        }

        public async Task JoinGroupAsync(string groupId, string userId)
        {
            Group group = await context.Groups.FindAsync(groupId);

            UserProfile user = await context.UserProfiles.FindAsync(userId);

            if (group == null || user == null)
            {
                return;
            }

            await context.UserProfilesGroups.AddAsync(new UserProfileGroup { GroupId = groupId, UserProfileId = userId });

            group.MembersCount++;

            await context.SaveChangesAsync();
        }

        public async Task LeaveGroupAsync(string groupId, string userId)
        {
            Group group = await context.Groups.FindAsync(groupId);

            UserProfileGroup user = context.UserProfilesGroups.FirstOrDefault(x => x.UserProfileId == userId && x.GroupId == groupId);

            if (group == null || user == null || group.CreaterId == userId)
            {
                return;
            }

            group.MembersCount--;

            group.Users.Remove(user);

            await context.SaveChangesAsync();
        }

        public async Task SendJoinRequestAsync(string groupId, string userId)
        {
            UserGroupRequest userGroupRequest = new UserGroupRequest
            {
                UserProfileId = userId,
                GroupId = groupId,
            };

            await context.JoinGroupRequest.AddAsync(userGroupRequest);

            await context.SaveChangesAsync();
        }
    }
}
