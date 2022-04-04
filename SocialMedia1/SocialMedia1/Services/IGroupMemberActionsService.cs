namespace SocialMedia1.Services
{
    public interface IGroupMemberActionsService
    {
        Task JoinGroupAsync(string groupId, string userId);

        Task SendJoinRequestAsync(string groupId, string userId);

        Task ApproveJoinRequestAsync(string requesterId, string groupId);

        Task DeleteJoinRequestAsync(string requesterId, string groupId);

        Task LeaveGroupAsync(string groupId, string userId);

        bool IsUserGroupMember(string userId, string groupId);

        bool IsGroupPrivate(string groupId);

        bool IsJoinRequstSent(string groupId, string userId);

        bool IsUserGroupCreator(string userId, string groupId);
    }
}
