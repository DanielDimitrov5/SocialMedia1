using SocialMedia1.Data;
using SocialMedia1.Data.Models;

namespace SocialMedia1.Services.Users
{
    public class UserActionsService : IUserActionsService
    {
        private readonly ApplicationDbContext context;

        public UserActionsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task FollowUserAsync(string id, string currentUserId)
        {
            var userProfile = context.UserProfiles.FirstOrDefault(x => x.Id == id);
            var currentUser = context.UserProfiles.FirstOrDefault(x => x.Id == currentUserId);

            if (CheckIfUsersAreNull(userProfile, currentUser))
            {
                return;
            }

            if (userProfile.IsPrivate)
            {
                await SendFollowRequest(id, currentUserId);
                return;
            }

            currentUser.Follows.Add(userProfile);
            userProfile.FollowedBy.Add(currentUser);

            await context.SaveChangesAsync();
        }

        public async Task UnfollowUserAsync(string id, string currentUserId)
        {
            var userProfile = context.UserProfiles.FirstOrDefault(x => x.Id == id);
            var currentUser = context.UserProfiles.FirstOrDefault(x => x.Id == currentUserId);

            if (CheckIfUsersAreNull(userProfile, currentUser))
            {
                return;
            }

            await context.Entry(userProfile).Collection(x => x.FollowedBy).LoadAsync();
            await context.Entry(currentUser).Collection(x => x.Follows).LoadAsync();

            currentUser.Follows.Remove(userProfile);
            userProfile.FollowedBy.Remove(currentUser);

            await context.SaveChangesAsync();
        }

        

        public async Task ApproveFollowRequestAsync(string requesterId, string currentUserId)
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

            await context.SaveChangesAsync();
        }

        public async Task DeleteRequestAsync(string requesterId, string currentUserId)
        {
            var followRequester = context.UserProfiles.FirstOrDefault(x => x.Id == requesterId);
            var currentUser = context.UserProfiles.FirstOrDefault(x => x.Id == currentUserId);

            if (CheckIfUsersAreNull(followRequester, currentUser))
            {
                return;
            }

            FollowRequest request = context.FollowRequests.FirstOrDefault(x => x.UserRequesterId == requesterId && x.UserId == currentUserId);

            context.FollowRequests.Remove(request);

            await context.SaveChangesAsync();
        }

        public async Task RemoveFollowerAsync(string currentUserId, string followerId)
        {
            var currentUser = await context.UserProfiles.FindAsync(currentUserId);
            var follower = await context.UserProfiles.FindAsync(followerId);

            if (CheckIfUsersAreNull(currentUser, follower))
            {
                return;
            }

            await context.Entry(currentUser).Collection(x => x.FollowedBy).LoadAsync();

            currentUser.FollowedBy.Remove(follower);

            await context.SaveChangesAsync();
        }

        public bool CheckIfFollowRequestIsSent(string userId, string currentsUserId)
        {
            return context.FollowRequests.Any(x => x.UserId == userId && x.UserRequesterId == currentsUserId);
        }

        public async Task<bool> IsUserFollowingAsync(string currentUserId, string followedUserId)
        {
            var currentUser = context.UserProfiles.FirstOrDefault(x => x.Id == currentUserId);

            var userProfile = context.UserProfiles.FirstOrDefault(x => x.Id == followedUserId);

            if (currentUser == null || userProfile == null)
            {
                return false;
            }

            await context.Entry(currentUser).Collection(x => x.Follows).LoadAsync();

            return currentUser.Follows.Contains(userProfile);
        }

        //private methods:
        private bool CheckIfUsersAreNull(UserProfile user, UserProfile currentUser)
        {
            return user is null || currentUser is null;
        }

        private async Task SendFollowRequest(string id, string currentUserId)
        {
            var userProfile = context.UserProfiles.FirstOrDefault(x => x.Id == id);
            var currentUser = context.UserProfiles.FirstOrDefault(x => x.Id == currentUserId);

            if (CheckIfUsersAreNull(userProfile, currentUser))
            {
                return;
            }

            FollowRequest followRequest = new FollowRequest { UserId = userProfile.Id, UserRequesterId = currentUser.Id };

            userProfile.FollowRequests.Add(followRequest);

            await context.SaveChangesAsync();
        }
    }
}
