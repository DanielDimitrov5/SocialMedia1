using SocialMedia1.Data;

namespace SocialMedia1.Services.Common
{
    public class NavBarService : INavBarService
    {
        private readonly ApplicationDbContext context;

        public NavBarService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public int FollowRequestsCount(string userId)
        {
            var user = context.UserProfiles.Find(userId);

            int count = context.Entry(user).Collection(x => x.FollowRequests).Query().Count();

            return count;
        }
    }
}
