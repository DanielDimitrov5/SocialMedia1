﻿namespace SocialMedia1.Services
{
    public interface IUserActionsService
    {
        Task FollowUserAsync(string id, string currentUserId);

        Task UnfollowUserAsync(string id, string currentUserId);

        Task SendFollowRequest(string id, string currentUserId);

        Task<bool> IsUserFollowedAsync(string currentUserId, string userId);

        Task ApproveFollowRequestAsync(string requesterId, string currentUser);

        Task DeleteRequestAsync(string requesterId);

        bool CheckIfFollowRequestIsSent(string userId, string currentsUserId);

        Task RemoveFollowerAsync(string currentUserId, string followerId);
    }
}
