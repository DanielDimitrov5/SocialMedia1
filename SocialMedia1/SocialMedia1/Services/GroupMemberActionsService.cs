using SocialMedia1.Data;
using SocialMedia1.Data.Models;

namespace SocialMedia1.Services
{
    public class GroupMemberActionsService : IGroupMemberActionsService
    {
        private readonly ApplicationDbContext context;

        public GroupMemberActionsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void ApproveJoinRequest(string requesterId, string groupId)
        {
            UserProfile requester = context.UserProfiles.Find(requesterId);
            Group group = context.Groups.Find(groupId);

            if (requester == null || group == null)
            {
                return;
            }

            JoinGroup(groupId, requesterId);

            UserGroupRequest followRequest = context.JoinGroupRequest.FirstOrDefault(x => x.UserProfileId == requesterId && x.GroupId == groupId);

            context.JoinGroupRequest.Remove(followRequest);

            context.SaveChanges();
        }

        public void DeleteJoinRequest(string requesterId, string groupId)
        {
            var request = context.JoinGroupRequest.FirstOrDefault(x => x.UserProfileId == requesterId && x.GroupId == groupId);

            if (request == null)
            {
                return;
            }

            context.JoinGroupRequest.Remove(request);

            context.SaveChanges();
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

        public void JoinGroup(string groupId, string userId)
        {
            Group group = context.Groups.Find(groupId);

            UserProfile user = context.UserProfiles.Find(userId);

            if (group == null || user == null)
            {
                return;
            }

            context.UserProfilesGroups.Add(new UserProfileGroup { GroupId = groupId, UserProfileId = userId });

            group.MembersCount++;

            context.SaveChanges();
        }

        public void LeaveGroup(string groupId, string userId)
        {
            Group group = context.Groups.Find(groupId);

            UserProfileGroup user = context.UserProfilesGroups.FirstOrDefault(x => x.UserProfileId == userId && x.GroupId == groupId);

            if (group == null || user == null)
            {
                return;
            }

            group.MembersCount--;

            group.Users.Remove(user);

            context.SaveChanges();
        }

        public void SendJoinRequest(string groupId, string userId)
        {
            UserGroupRequest userGroupRequest = new UserGroupRequest
            {
                UserProfileId = userId,
                GroupId = groupId,
            };

            context.JoinGroupRequest.Add(userGroupRequest);

            context.SaveChanges();
        }
    }
}
