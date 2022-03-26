using SocialMedia1.Data;
using SocialMedia1.Data.Models;

namespace SocialMedia1.Services
{
    public class UserActionsService : IUserActionsService
    {
        private readonly ApplicationDbContext context;

        public UserActionsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void FollowUser(string id, string currentUserId)
        {
            var userProfile = context.UserProfiles.FirstOrDefault(x => x.Id == id);
            var currentUser = context.UserProfiles.FirstOrDefault(x => x.Id == currentUserId);

            if (CheckIfUsersAreNull(userProfile, currentUser))
            {
                return;
            }

            if (userProfile.IsPrivate)
            {
                SendFollowRequest(id, currentUserId);
                return;
            }

            currentUser.Follows.Add(userProfile);
            userProfile.FollowedBy.Add(currentUser);

            context.SaveChanges();
        }

        public void UnfollowUser(string id, string currentUserId)
        {
            var userProfile = context.UserProfiles.FirstOrDefault(x => x.Id == id);
            var currentUser = context.UserProfiles.FirstOrDefault(x => x.Id == currentUserId);

            if (CheckIfUsersAreNull(userProfile, currentUser))
            {
                return;
            }

            context.Entry(userProfile).Collection(x => x.FollowedBy).Load();
            context.Entry(currentUser).Collection(x => x.Follows).Load();

            currentUser.Follows.Remove(userProfile);
            userProfile.FollowedBy.Remove(currentUser);

            context.SaveChanges();
        }

        public void SendFollowRequest(string id, string currentUserId)
        {
            var userProfile = context.UserProfiles.FirstOrDefault(x => x.Id == id);
            var currentUser = context.UserProfiles.FirstOrDefault(x => x.Id == currentUserId);

            if (CheckIfUsersAreNull(userProfile, currentUser))
            {
                return;
            }

            FollowRequest followRequest = new FollowRequest { UserId = userProfile.Id, UserRequesterId = currentUser.Id };

            userProfile.FollowRequests.Add(followRequest);

            context.SaveChanges();
        }

        public void ApproveFollowRequest(string requesterId, string currentUserId)
        {
            var followRequester = context.UserProfiles.FirstOrDefault(x => x.Id == requesterId);

            var user = context.UserProfiles.FirstOrDefault(x => x.Id == currentUserId);

            if (CheckIfUsersAreNull(followRequester, user))
            {
                return;
            }

            var request = context.FollowRequests.FirstOrDefault(x => x.UserRequesterId == requesterId && x.UserId == currentUserId);

            context.FollowRequests.Remove(request);

            followRequester.Follows.Add(user);
            user.FollowedBy.Add(followRequester);

            context.SaveChanges();
        }

        public void DeleteRequest(string requesterId)
        {
            var followRequester = context.UserProfiles.FirstOrDefault(x => x.Id == requesterId);

            if (CheckIfUsersAreNull(followRequester, followRequester))
            {
                return;
            }

            FollowRequest request = context.FollowRequests.FirstOrDefault(x => x.UserRequesterId == requesterId);

            context.FollowRequests.Remove(request);

            context.SaveChanges();
        }

        public void RemoveFollower(string currentUserId, string followerId)
        {
            var currentUser = context.UserProfiles.Find(currentUserId);

            context.Entry(currentUser).Collection(x => x.FollowedBy).Load();

            var follower = context.UserProfiles.Find(followerId);

            currentUser.FollowedBy.Remove(follower);

            context.SaveChanges();
        }

        public bool CheckIfFollowRequestIsSent(string userId, string currentsUserId)
        {
            return context.FollowRequests.Any(x => x.UserId == userId && x.UserRequesterId == currentsUserId);
        }

        public bool IsUserFollowed(string currentUserId, string userId)
        {
            var currentUser = context.UserProfiles.FirstOrDefault(x => x.Id == currentUserId);

            var userProfile = context.UserProfiles.FirstOrDefault(x => x.Id == userId);

            if (currentUser == null || userProfile == null)
            {
                return false;
            }

            context.Entry(currentUser).Collection(x => x.Follows).Load();

            return currentUser.Follows.Contains(userProfile);
        }

        //private methods:
        private bool CheckIfUsersAreNull(UserProfile user, UserProfile currentUser)
        {
            return user is null || currentUser is null;
        }
    }
}
