namespace SocialMedia1.Services.Groups
{
    public interface IGroupMemberActionsService : IService
    {
        Task JoinGroupAsync(string groupId, string userId);

        Task SendJoinRequestAsync(string groupId, string userId);

        Task ApproveJoinRequestAsync(string requesterId, string groupId);

        Task DeleteJoinRequestAsync(string requesterId, string groupId);

        Task LeaveGroupAsync(string groupId, string userId);

        bool IsUserGroupMember(string userId, string groupId);

        Task<bool> IsGroupPrivateAsync(string groupId);

        Task<bool> IsJoinRequstSentAsync(string groupId, string userId);

        bool IsUserGroupCreator(string userId, string groupId);
    }
}
