namespace SocialMedia1.Services
{
    public interface IUserActionsService
    {
        void FollowUser(string id, string currentUserId);

        void UnfollowUser(string id, string currentUserId);

        bool IsUserFollowed(string currentUserId, string userId);

        void ApproveFollowRequest(string requesterId, string currentUser);

        void DeleteRequest(string requesterId);

        bool CheckIfFollowRequestIsSent(string userId, string currentsUserId);

        public void RemoveFollower(string currentUserId, string followerId);
    }
}
