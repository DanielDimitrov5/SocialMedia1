namespace SocialMedia1.Services.Users
{
    public interface IUserActionsService : IService
    {
        Task FollowUserAsync(string id, string currentUserId);

        Task UnfollowUserAsync(string id, string currentUserId);

        Task<bool> IsUserFollowingAsync(string currentUserId, string userId);

        Task ApproveFollowRequestAsync(string requesterId, string currentUser);

        Task DeleteRequestAsync(string requesterId, string currentUser);

        bool CheckIfFollowRequestIsSent(string userId, string currentsUserId);

        Task RemoveFollowerAsync(string currentUserId, string followerId);
    }
}
